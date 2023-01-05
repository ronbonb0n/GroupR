import utils
import numpy as np
from PIL import Image

class define_tiles(object):
    def __init__(self) -> None:
        '''
        tiles broken down into 3 x 3 np arrays
        within the arrays 1s represent areas where a player can't go
        0s represent areas where players can walk
        we just need to match each tile by each facing side
        '''
        self.tiles_as_matrices = {
            0: np.array(
                [[1,1,1],
                 [1,1,1],
                 [1,1,1]]
            ), # Empty Space
            1: np.array(
                [[0,0,0],
                 [0,0,0],
                 [0,0,0]]
            ), # Open Area
            2: np.array(
                [[1,1,1],
                 [0,0,1],
                 [0,0,1]]
            ), # Outer Top Right Corner
            3: np.array(
                [[1,1,1],
                 [1,0,0],
                 [1,0,0]]
            ), # Outer Top Left Corner
            4: np.array(
                [[0,0,1],
                 [0,0,1],
                 [1,1,1]]
            ), # Outer Bottom Right Corner
            5: np.array(
                [[1,0,0],
                 [1,0,0],
                 [1,1,1]]
            ), # Outer Bottom Left Corner
            6: np.array(
                [[0,0,1],
                 [0,0,0],
                 [0,0,0]]
            ), # InnerTopRightCorner
            7: np.array(
                [[1,0,0],
                 [0,0,0],
                 [0,0,0]]
            ), # InnerTopLeftCorner
            8: np.array(
                [[0,0,0],
                 [0,0,0],
                 [0,0,1]]
            ), # Inner Bottom Right Corner
            9: np.array(
                [[0,0,0],
                 [0,0,0],
                 [1,0,0]]
            ), # Inner Bottom Left Corner
            10: np.array(
                [[1,0,0],
                 [1,0,0],
                 [1,0,0]]
            ), # Left Wall
            11: np.array(
                [[0,0,1],
                 [0,0,1],
                 [0,0,1]]
            ), # Right Wall
            12: np.array(
                [[1,1,1],
                 [0,0,0],
                 [0,0,0]]
            ), # Top Wall
            13: np.array(
                [[0,0,0],
                 [0,0,0],
                 [1,1,1]]
            ), # Bottom Wall
            14: np.array(
                [[1,0,1],
                 [1,0,1],
                 [1,0,1]]
            ), # Up Down Corridor
            15: np.array(
                [[1,1,1],
                 [0,0,0],
                 [1,1,1]]
            ), # Across Corridor
            16: np.array(
                [[1,0,0],
                 [0,0,0],
                 [1,0,0]]
            ), # Left Doorway
            17: np.array(
                [[0,0,1],
                 [0,0,0],
                 [0,0,1]]
            ), # Right Doorway
            18: np.array(
                [[1,0,1],
                 [0,0,0],
                 [0,0,0]]
            ), # Top Doorway
            19: np.array(
                [[0,0,0],
                 [0,0,0],
                 [1,0,1]]
            ), # Bottom Doorway
            #20: np.array(
            #    [[1,1,1],
            #     [1,0,0],
            #     [1,1,1]]
            #), # Left Deadend
            #21: np.array(
            #    [[1,1,1],
            #     [0,0,1],
            #     [1,1,1]]
            #), # Right Deadend
            #22: np.array(
            #    [[1,1,1],
            #     [1,0,1],
            #     [1,0,1]]
            #), # Top Deadend
            #23: np.array(
            #    [[1,0,1],
            #     [1,0,1],
            #     [1,1,1]]
            #), # Bottom Deadend
        }
        # Rules order: Right, Left, Down, Up
        self.rules = {
            0: [[],[],[],[]],
            1: [[],[],[],[]],
            2: [[],[],[],[]],
            3: [[],[],[],[]],
            4: [[],[],[],[]],
            5: [[],[],[],[]],
            6: [[],[],[],[]],
            7: [[],[],[],[]],
            8: [[],[],[],[]],
            9: [[],[],[],[]],
            10: [[],[],[],[]],
            11: [[],[],[],[]],
            12: [[],[],[],[]],
            13: [[],[],[],[]],
            14: [[],[],[],[]],
            15: [[],[],[],[]],
            16: [[],[],[],[]],
            17: [[],[],[],[]],
            18: [[],[],[],[]],
            19: [[],[],[],[]],
            #20: [[],[],[],[]],
            #21: [[],[],[],[]],
            #22: [[],[],[],[]],
            #23: [[],[],[],[]]
        }

    def define_rules(self, verbose = False):
        for i in range(len(self.tiles_as_matrices)):
            selected_tile = self.tiles_as_matrices[i]
            # Right, Left, Down, Up

            # Right hand side - check left hand side of each other tile (including self)
            selected_right = selected_tile[:, -1]

            for j in range(len(self.tiles_as_matrices)):
                target_tile = self.tiles_as_matrices[j]
                target_left = target_tile[:, 0]
                # If the compared sides match then in this combination the tiles can fit together
                if np.array_equal(selected_right, target_left):
                    self.rules[i][0].append(j)

            # Left hand side - check right hand side of each other tile (including self)
            selected_left = selected_tile[:, 0]

            for j in range(len(self.tiles_as_matrices)):
                target_tile = self.tiles_as_matrices[j]
                target_right = target_tile[:, -1]
                if np.array_equal(selected_left, target_right):
                    self.rules[i][1].append(j)

            # Down side - Check up side of each other tile (including self)
            selected_down = selected_tile[-1, :]

            for j in range(len(self.tiles_as_matrices)):
                target_tile = self.tiles_as_matrices[j]
                target_up = target_tile[0, :]
                if np.array_equal(selected_down, target_up):
                    self.rules[i][2].append(j)

            # Up side - Check down side of each other tile (including self)
            selected_up = selected_tile[0, :]

            for j in range(len(self.tiles_as_matrices)):
                target_tile = self.tiles_as_matrices[j]
                target_down = target_tile[-1, :]
                if np.array_equal(selected_up, target_down):
                    self.rules[i][3].append(j)
        
        if verbose:
            for item in self.rules:
                print((item, self.rules[item]))

        return self.rules

# The code from here on out was not used but has been left into highlight different thought processes and experimentations that went into this project
    # No manual adjustments used in the end - better results arose from using a clean up parse
    def manual_adjustments(self, verbose = False):
        # Remove option for left doorway to lead directly into right doorway
        self.rules[16][1].remove(17)
        # Remove option for right doorway to lead directly into left doorway
        self.rules[17][0].remove(16)
        # Remove option for top doorway to lead directly into down doorway
        self.rules[18][3].remove(19)
        # Remove option for down doorway to lead directly into up doorway
        self.rules[19][2].remove(18)

        if verbose:
            for item in self.rules:
                print((item, self.rules[item]))

        return self.rules

# Not used in the end - decided to work fully procedurally
class load_template(object):
    def __init__(self, template_path: str, dims: int, integer_swatch: dict) -> None:
        img = Image.open(template_path)
        self.width, self.height = img.size
        self.pixels = img.load()
        self.swatch = integer_swatch
        for key in self.swatch:
            item = self.swatch[key]
            item = (round(item[0], 1), round(item[1], 1), round(item[2], 1))
            self.swatch[key] = item
        self.dims = dims
        self.rules = {
            0: [[],[],[],[]],
            1: [[],[],[],[]],
            2: [[],[],[],[]],
            3: [[],[],[],[]],
            4: [[],[],[],[]],
            5: [[],[],[],[]],
            6: [[],[],[],[]],
            7: [[],[],[],[]],
            8: [[],[],[],[]],
            9: [[],[],[],[]],
            10: [[],[],[],[]],
            11: [[],[],[],[]],
            12: [[],[],[],[]],
            13: [[],[],[],[]],
            14: [[],[],[],[]],
            15: [[],[],[],[]],
            16: [[],[],[],[]],
            17: [[],[],[],[]],
            18: [[],[],[],[]],
            19: [[],[],[],[]],
            #20: [[],[],[],[]],
            #21: [[],[],[],[]],
            #22: [[],[],[],[]],
            #23: [[],[],[],[]]
        }

    def rules_from_template(self, verbose = False):
        for y in range(self.height):     
            for x in range(self.width):
                r, g, b = utils.get_pixel_dec(img = self.pixels, coords = (x, y), precision = 1)
                pixel_int = list(self.swatch.keys())[list(self.swatch.values()).index((r,g,b))]

                # Try right
                try:
                    r, g, b = utils.get_pixel_dec(img = self.pixels, coords = (x+1, y), precision = 1)
                    right_pixel_int = list(self.swatch.keys())[list(self.swatch.values()).index((r,g,b))]
                    self.rules[pixel_int][0].append(right_pixel_int)
                except:
                    pass
                # Try left
                try:
                    r, g, b = utils.get_pixel_dec(img = self.pixels, coords = (x-1, y), precision = 1)
                    left_pixel_int = list(self.swatch.keys())[list(self.swatch.values()).index((r,g,b))]
                    self.rules[pixel_int][1].append(left_pixel_int)
                except:
                    pass
                # Try down
                try:
                    r, g, b = utils.get_pixel_dec(img = self.pixels, coords = (x, y+1), precision = 1)
                    down_pixel_int = list(self.swatch.keys())[list(self.swatch.values()).index((r,g,b))]
                    self.rules[pixel_int][2].append(down_pixel_int)
                except:
                    pass
                # Try up
                try:
                    r, g, b = utils.get_pixel_dec(img = self.pixels, coords = (x, y-1), precision = 1)
                    up_pixel_int = list(self.swatch.keys())[list(self.swatch.values()).index((r,g,b))]
                    self.rules[pixel_int][3].append(up_pixel_int)
                except:
                    pass

        for key in self.swatch:
            item = self.rules[key]
            for i, sublist in enumerate(item):
                item[i] = list(set(sublist))
            self.rules[key] = item
        
        if verbose:
            for item in self.rules:
                print((item, self.rules[item]))

        return self.rules

if __name__ == "__main__":
    tile_definitions = define_tiles()
    tile_rules = tile_definitions.define_rules(verbose = False)
    '''
    I then add some manual adjustments to the rulesets - specifically so one door won't face another
    I'd prefer a door lead into a corridor and then back into another door
    Algorithm works fine without these adjustments - but I want a certain aesthetic and flow to the level
    '''
    tile_rules = tile_rules.manual_adjustments(verbose = True)