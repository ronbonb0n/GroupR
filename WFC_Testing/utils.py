from PIL import Image
import random
import numpy as np
import colorama
from scipy.ndimage import measurements

def get_pixel_dec(img, coords: tuple, precision: int):
    # Get intial RGB values (0 - 255)
    r, g, b = img[coords[0], coords[1]] # (x,y)
    # Convert to 1dp decimals - important and it accounts for compression errors in colour spectrum
    r, g, b = round(r/255, precision), round(g/255, precision), round(b/255, precision)
    return r, g, b

# record holds the data on each cell - its number (for debugging purposes), whether it has been collapsed and the possible options it could be based on surrounding tiles 
def build_grid_records(dims, pixel_types):
    grid = [0] * int(dims ** 2)
    for i in range(dims**2):
        grid[i] = {
            "cell_number" : i,
            "collapsed": False,
            # pixel_types designated in main by taking keys from the swatch made in class below
            "options": pixel_types 
        }
    return grid

# Used to initialise the output matrix
def build_output_matrix(dims, verbose):
    out = np.zeros(shape = (dims, dims))
    out[:] = np.nan

    # Pad perimeter of out matrix with zeros and update grid record accordingly
    out[:, -1] = 0
    out[:, 0] = 0
    out[0, :] = 0
    out[-1, :] = 0

    if verbose:
        print_output(output = out)
    return out

# After we have assessed the entropy if the cell was deemed as collapseable and was collapsed then we populate it with the only possible option
def update_output(out_array, dims, grid):
    # Update output
    for i in range(dims):
        for j in range(dims):
            cell = grid[int(i + j * dims)]
            if cell["collapsed"]:
                out_array[i,j] = cell["options"]
            else:
                out_array[i,j] = np.nan
    return out_array

def print_output(output: list):
    with np.printoptions(precision=1, suppress=True, formatter={'float': '{:0}'.format}, linewidth=150):
        for row in output:
            print(row)
                 
class build_lookup(object):
    def __init__(self, img_dir: str, dims: int) -> None:
        img = Image.open(img_dir)
        self.width, self.height = img.size
        self.pixels = img.load()
        self.string_swatch = {}
        self.int_swatch = {}
        # string types for debugging only
        self.pixel_types  = [
            # General
            "EmptySpace", "OpenArea", 
            # Outer Corners
            "OuterTopRightCorner", "OuterTopLeftCorner", "OuterBottomRightCorner", "OuterBottomLeftCorner",
            # Inner Corners
            "InnerTopRightCorner", "InnerTopLeftCorner", "InnerBottomRightCorner", "InnerBottomLeftCorner",
            # Walls
            "LeftWall", "RightWall", "TopWall", "BottomWall",
            # Corridors
            "UpDownCorridor", "AcrossCorridor",
            # Doorways
            "LeftDoorway", "RightDoorway", "TopDoorway", "BottomDoorway",
            # Deadends
            #"LeftDeadend", "RightDeadend", "TopDeadend", "BottomDeadend"
        ]
        self.total_tiles = len(self.pixel_types)
        self.dims = dims

    def build_swatches(self, precision, verbose = False):
        # Iterate over the provided dungeon-map-generator-swatch and get the RGB values from each pixel
        i = 0
        for y in range(self.height):     
            for x in range(self.width):   
                r, g, b = get_pixel_dec(img = self.pixels, coords = (x, y), precision = precision)
                self.string_swatch[self.pixel_types[i]] = (r, g, b)
                self.int_swatch[i] = (r, g, b)
                i += 1
                if i == self.total_tiles:
                    break

        if verbose:
            for string, integer in zip(self.string_swatch, self.int_swatch):
                print((string, integer, self.int_swatch[integer]))
            print("-----------------------------")
        return self.string_swatch, self.int_swatch

class populate_cell(object):
    def __init__(self, grid_in) -> None:
        self.grid_copy = grid_in

    # sorts the cells to have lowest entropy first whilst ignoring cells that are collapsed as their entropy will always be lowest 
    # but upon collapsing them we want to leave these tiles alone
    def sort_grid(self):
        # for dic in grid_copy:
        #    print((dic["collapsed"], dic["options"]))
        
        '''
        remove collapsed cells from grid copy
        their entropy will always be 1 and will result in breaking the loop
        '''
        self.grid_copy = [i for i in self.grid_copy if not (i['collapsed'] == True)]
        
        # Sort by option list length - shortest first
        self.grid_copy.sort(key=lambda d: len(d.get('options', [])), reverse=False)
        
        return self
    
    def select_minimal_entropy(self):
        '''
            By this point grid copy has been sorted and the first element
            will be the shortest and we select for the tiles with the lowest entropy at the top of the list
        '''
        target_len = len(self.grid_copy[0]['options'])
        stop_index = 0
        for i, cell in enumerate(self.grid_copy):
            if len(cell["options"]) > target_len:
                stop_index = i
                break
        if stop_index > 0:
            self.grid_copy = self.grid_copy[:stop_index]
        # for dic in grid_copy:
        #    print((dic["collapsed"], dic["options"]))
        return self
    # Important to select a random tile if there are multiple options of the same entropy (normally applies at the early stages of the main code loop)
    def select_and_collapse(self):
        cell = random.choice(self.grid_copy)
        cell['collapsed'] = True
        cell['options'] = random.choice(cell['options'])
        return cell

class cleanup_output(object):
    def __init__(self, output, dims) -> None:
        self.out = output
        self.bad_combos = [
            np.array(
                [[8,19,9],
                 [6,18,7]]),
            np.array(
                [[8,9],
                 [17,16],
                 [6,7]]),
        ]
        self.dims = dims

    def fix_bad_combos(self):
        # Removes random doors appearing and replaces them with open space
        # This is due to unlimited pairing options
        # When the algorithm compares tile relationships these tiles do correctly go together
        # And this is where we need to consider a balance between procedural and "human" production
        # Sometimes a small amount of intervention is required

        for i in range(self.dims):
            for j in range(self.dims):
                tile = self.out[i,j]
                if tile == 8:
                    # Check for bad combo
                    cross_combo = self.out.copy()[i:i+2, j:j+3]
                    if np.array_equal(cross_combo, self.bad_combos[0]):
                        self.out[i:i+2, j:j+3] = 1

                    down_combo = self.out.copy()[i:i+3, j:j+2]
                    if np.array_equal(down_combo, self.bad_combos[1]):
                        self.out[i:i+3, j:j+2] = 1

                else:
                    pass
        return self

    def remove_disconnected_clusters(self, pixel_matrices, verbose = False):
        self.pixel_matrices = pixel_matrices
        # build a 4d matric from the integer values that represent each tile type in the final iteration of the output
        # do this by looking up the 2d binary matrix in the look up table created in build rules
        four_d_visual = np.zeros((self.dims,self.dims,3,3))
        for i in range(self.dims):
            for j in range(self.dims):
                tile = self.out[i,j]
                tile_matrix = self.pixel_matrices[tile]
                four_d_visual[i,j] = tile_matrix
        
        # convert this to a 2d matrix by selecting the ith of each 2d matrix nested within each row of the 4d matrix
        two_d_visual = []
        for row in four_d_visual:
            for i in range(3):
                values = row[0:, i]
                two_d_visual.append(values.flatten().tolist())
        two_d_visual = np.array(two_d_visual)
        
        # invert the 2d matrix and use scipy to determine clusters within the inverted 2d matrix
        lw, num = measurements.label(1-two_d_visual)
        # determine the most common value (making the largest cluster that isn't empty space)
        counter_dict = {}
        for i in range(lw.shape[0]):
            for j in range(lw.shape[1]):
                val = lw[i,j]
                if val in counter_dict:
                    counter_dict[val] += 1
                else:
                    counter_dict[val] = 1
        counter_dict = {k: v for k, v in sorted(counter_dict.items(), key=lambda item: item[1], reverse = True)}
        most_common = list(counter_dict.keys())[0]
        if most_common == 0:
            most_common = list(counter_dict.keys())[1]
        # select for this cluster
        for i in range(lw.shape[0]):
            for j in range(lw.shape[1]):
                lw_val = lw[i,j]
                if lw_val == most_common:
                    two_d_visual[i,j] = 0
                else:
                    two_d_visual[i,j] = 1
        #output to self
        self.cluster_array = two_d_visual
        if verbose:
            def color_sign(x):
                c = colorama.Fore.GREEN if x != 0 else colorama.Fore.RED
                return f'{c}{x}'
            np.set_printoptions(formatter={'float': color_sign}, linewidth=750)
            for row in self.cluster_array:
                print(row)
                
        return self
    # reverse lookup converting the tiles represented as 2d matrices back to their single integers by effectively useing a cookie cutter approach and slicing 3x3 arrays
    # from cluster array
    def clusters_to_int(self):
        cleaned_output = np.empty((self.out.shape[0], self.out.shape[1]))
        for i in range(cleaned_output.shape[0]):
            for j in range(cleaned_output.shape[1]):
                selected_cluster = self.cluster_array[int(i * 3) : int((i * 3) + 3), int(j * 3) : int((j * 3) + 3)]
                for key in self.pixel_matrices:
                    test_cluster = self.pixel_matrices[key]
                    if np.array_equal(selected_cluster,test_cluster):
                        pixel_int = key
                        cleaned_output[i,j] = int(pixel_int)
                        break
        return cleaned_output
                

class encode(object):
    def __init__(self, out_array: np.array, integer_swatch: dict, dims: int) -> None:
        self.out = out_array
        self.swatch = integer_swatch
        self.dims = dims
    # convert integer to rgb by using the RGB lookup generated at the start with the dungeon-map-generator-swatch.jpg
    # for a future implementation it would be worth hardcoding this lookup to not require the image so the tool can act as a standalone 
    # piece of software without any outside requirements
    # compiling with cython would be an interesting endeavour or perhaps taking this prototype and building it in full with c++
    def int2rgb(self, verbose = False):
        self.out = self.out.tolist()
        for sublist in self.out:
            for i, element in enumerate(sublist):
                colour = self.swatch[element]
                sublist[i] = colour
        self.out = np.rint(np.array(self.out) * 255)
        if verbose:
            for row in self.out:
                print(row.round(1).tolist())
        return self

    # Hex and byte conversion not needed in the end - was able to use pillow instead
    def rgb2hex(self, verbose):
        self.hex_array = np.empty((self.dims,self.dims), dtype="S7")
        for i in range(self.dims):
            for j in range(self.dims):
                pixel = self.out[i,j]
                pixel = np.rint(pixel * 255).astype("int")
                hex_string = '%02x%02x%02x' % (pixel[0], pixel[1], pixel[2])
                self.hex_array[i,j] = hex_string
        if verbose:
            for row in self.hex_array:
                print(row.tolist())
        return self

    def hex2bytes(self):
        byte_array = self.hex_array.tobytes()
        print(byte_array)
        print("_")

if __name__ == "__main__":
    pass

# Select random cell from grid record and collapse - assign random option
#random_index = random.choice(range(DIMS ** 2))
#grid[random_index]['collapsed'] = True
#grid[random_index]["options"] = random.choice(list(int_swatch.keys()))
## Assign to output matrix
#out = utils.update_output(out_array = out, dims = DIMS, grid = grid)
#utils.print_output(output = out)