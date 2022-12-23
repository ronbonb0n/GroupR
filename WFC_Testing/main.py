# ---------------------------------------------------------------------- #
# An interpretation of the wave function collapse algorithm              #
# Written by James Joslin - MRes Computer Science Student                # 
# For Group R coursework project in multi platform game development      #
# Queen Mary University of London                                        #
# ---------------------------------------------------------------------- #
# When writing this code personal understanding was of greater import    #
# I don't mind that it takes a few seconds to build the level bitmap     #
# as it doesn't have to run in real time - this is a tool for production #
# ---------------------------------------------------------------------- #

# My libraries
import utils
import build_rules
# Public Libraries
import numpy as np
from tqdm import tqdm
from PIL import Image


def main(DIMS:int, EXPORT_PATH:str):
    '''
    DIMS - length and width of output
    Always outputs a square image - not able to run non square outputs
    '''
    lookup_image = utils.build_lookup(
        img_dir = 'Dungeon-Map-Generator-Swatch.jpg',
        dims = DIMS
    )

    # Swatch dictionaries with names or ints and their corresponding r,g,b decimals
    string_swatch, int_swatch = lookup_image.build_swatches(precision = 5, verbose = False)

    # initialise records grid - records whether a cell is collapsed and its options
    grid = utils.build_grid_records(
        dims = DIMS, pixel_types=list(int_swatch.keys())
    )

    # Initialised 2D output
    out = utils.build_output_matrix(dims = DIMS, verbose = False)

    # Update Grid
    for i in range(DIMS):
        for j in range(DIMS):
            tile = out[i,j]
            if np.isnan(tile):
                pass
            else:
                grid[i + j * DIMS]["collapsed"] = True
                grid[i + j * DIMS]["options"] = int(tile)
    
    '''
    We need to build our tile relationships - i.e. how they connect together
    '''
    # Define rules for entropy assessment
    tile_definitions = build_rules.define_tiles()
    '''
    Rules are made up of a dictionary
    each element holds a nested list
    sublists hold tile numbers that can go to the right, left, below and above each dict key
    '''
    tile_rules = tile_definitions.define_rules(verbose = False)

    # Begin main functionality 
    for a in tqdm(range(int(np.count_nonzero(out)))):
        # Assess and update entropy now that new cell has been populated
        for i in range(DIMS):
            for j in range(DIMS):
                cell = out[i,j]
                if np.isnan(cell):
                    # Initialise empty rule set for the current cell
                    cell_rules = []
                    '''
                    When we look in a direction at a cell next to the cell of the current iteratipon
                    we need to consider what the target cell (the cell the iteration cell in next to)
                    can be next to in the opposite direction. For example, when looking right we
                    need to see what cells can go to the left of this cell that is on the right hand side
                    of the cell of the current iteration
                    '''
                    # Look right
                    try:
                        right_cell = out[i,int(j + 1)]
                        if np.isnan(right_cell):
                            cell_rules.append(list(int_swatch.keys()))
                        else:
                            # get cells that can to the left of the cell that is to the right of the current cell within the loop
                            rules = tile_rules[int(right_cell)][1] 
                            cell_rules.append(rules) # append these cells to the cell rules of the current cell
                    except IndexError:
                        right_cell = np.nan
                        cell_rules.append(list(int_swatch.keys()))
                    # Look Left
                    try:
                        left_cell = out[i,int(j - 1)]
                        if np.isnan(left_cell):
                            cell_rules.append(list(int_swatch.keys()))
                        else:
                            # get cells that can to the right of the cell that is to the left of the current cell within the loop
                            rules = tile_rules[int(left_cell)][0]
                            cell_rules.append(rules)
                    except IndexError:
                        left_cell = np.nan
                        cell_rules.append(list(int_swatch.keys()))
                    # Look Down
                    try:
                        below_cell = out[int(i + 1),j]
                        if np.isnan(below_cell):
                            cell_rules.append(list(int_swatch.keys()))
                        else:
                            # get cells that can be above the cell that is below the current cell within the loop
                            rules = tile_rules[int(below_cell)][3]
                            cell_rules.append(rules)
                    except IndexError:
                        below_cell = np.nan
                        cell_rules.append(list(int_swatch.keys()))
                    # Look Up
                    try:
                        above_cell = out[int(i - 1),j]
                        if np.isnan(above_cell):
                            cell_rules.append(list(int_swatch.keys()))
                        else:
                            # get cells that can be above the cell that is below the current cell within the loop
                            rules = tile_rules[int(above_cell)][2]
                            cell_rules.append(rules)
                    except IndexError:
                        above_cell = np.nan
                        cell_rules.append(list(int_swatch.keys()))
            
                    final_rules = list(set.intersection(*[set(sublist) for sublist in cell_rules]))
                    grid[i + j * DIMS]["options"] = final_rules

                else:
                    pass

        # Sort a copy of the record grid - lowest entropy first
        sorted_grid = utils.populate_cell(grid_in = grid.copy()).sort_grid()
        # select the cells with lowest entropy
        minimal_cells = sorted_grid.select_minimal_entropy()
        '''
        choose random cell - collapse and choose random option
        for some reason this also feeds it back into the original grid
        even though we were working with a copy... I don't know why
        '''
        minimal_cells.select_and_collapse()

        out = utils.update_output(out_array = out, dims = DIMS, grid = grid)
    
    # Clean up undesirable cell combos
    cleaned_output = utils.cleanup_output(output = out, dims = DIMS)
    cleaned_output = cleaned_output.fix_bad_combos()
    cleaned_output = cleaned_output.remove_disconnected_clusters(
        pixel_matrices = tile_definitions.tiles_as_matrices,
        verbose = True
    ) 
    cleaned_output = cleaned_output.clusters_to_int()

    # Encode bitmap
    encoder = utils.encode(out_array = cleaned_output, integer_swatch = int_swatch, dims = DIMS)
    rgb_array = encoder.int2rgb(verbose = False).out.astype("uint8")
    dungeon_bitmap = Image.fromarray(rgb_array)
    #dungeon_bitmap.show()
    dungeon_bitmap.save(EXPORT_PATH)
    print(f'Dungeon bitmap exported to: {EXPORT_PATH}')

if __name__ == "__main__":
    INCOMPLETE = True
    print("Output is a square image therefore enter one number for dimensions")
    input_value = int(input("Enter dimension size: "))
    print("Provide file directory and name as one string - save as a .png file!")
    save_path = str(input("Export path: "))
    while INCOMPLETE:
        try:
            main(DIMS = input_value, EXPORT_PATH = save_path)
            INCOMPLETE = False
        except IndexError: # Index error arises if the algorithm doesn't converge onto an ideal and runs out of options
                print(" Attempt failed - Reattempting")
