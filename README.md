# About:

This is the game development project repository for the game **Ferŷnier** made by students of MSc Computer Games, Queen Mary University of London.\*

# Installation:

1. [Install Unity version 2022.1.19f1](https://unity3d.com/get-unity/download/archive)

2. Install WebGL support.

3. Add project to the Unity Hub with the correct Unity version.

# Additional Links for Development Process:

-   [Design and Development Board](https://app.milanote.com/1OICvn1gp26tem?p=QvLbpGpKO8B)

# References:

-   [Reference for AI mapping technique](https://www.youtube.com/watch?v=iY1jnFvHgbE&t=513s)
-   [State Machine](https://www.youtube.com/watch?v=nnrOhb5UdRc)
-   [AI Sight](https://www.youtube.com/watch?v=j1-OyLo77ss)
-   [Field of View Effect](https://www.youtube.com/watch?v=CSeUMTaNFYk)
-   [Chase](https://www.youtube.com/watch?v=wp8m6xyIPtE)
-   [Player Movement](https://www.youtube.com/watch?v=HmXU4dZbaMw)
-   [Cloak Ability](https://www.youtube.com/watch?v=u8gssV_Ec-Y)

# Wave Function Collapse:

The wave function collapse algorithm, as well as the secondary clustering and clearning algorithm was implemented within Python to be used as an external tool to procedurally develop level plans that are represented as bitmaps and implemented with an in-engine script, developed during the prototyping phase. This tool was not developed to be a perfectly efficient reproduction of the WFC, but instead as a personal learning experiment to better understand and learn about the algorithms.

#### If you wish to test and experiment with the tool please adhere to the following instructions:

##### It is highly recommended that you create a fresh python virtual environment
###### Unix/MacOS
`python3 -m pip install --user virtualenv`<br />
`python3 -m venv env`
###### Windows
`py -m pip install --user virtualenv`<br />
`py -m venv env`
##### Activate venv and upon activation install python dependencies with the requirements file
###### Unix/MacOS
`python3 -m pip install [options] -r <requirements file>`
###### Windows
`py -m pip install [options] -r <requirements file>`

Execute ```main.py``` within the venv. The tool is CLI based and will request the dimensions as a single integer, for example a user input of 5 will result in an output of 5 x 5 pixels and therefore a maximum tile count of 25, where each pixel represents a tile. Please execute the file within the same directory where the program and Dungeon-Map-Generator-Swatch.jpg is saved.

**NOTE: It is recommended that a minimum dimension is no less than 4**

The second input that is required is the export directory string that contains both the file destination and the name of the file as a png. For example: `./example/dir/test.png`

For the purpose of level development the bitmaps were exported into Unity's assets directory where their compression was set to none and run through the DungeonBuilder.cs script to generate the games levels.

#### For referencing purposes the external packages that the tool directly calls upon are as follows:<br/>
**NOTE: All of these packages will be installed via the requirements.txt install file via pip and PyPi**<br/>
-   [NumPy](https://numpy.org/) Vectorisation of data and a requirement for Pillow
-   [Pillow](https://pillow.readthedocs.io/en/stable/) For Encoding the final output to PNG and process the RGB values from the swatch
-   [TQDM](https://github.com/tqdm/tqdm) For the simple loading bar
-   [SciPy](https://scipy.org/) Extremely useful set of functions that aided with the clustering algorithm after the WFC had terminated to make sure there are no inaccessible locations within the map
-   [Colorama](https://pypi.org/project/colorama/) To colour the verbose output

# 3D Art Assets:

A simple directory containing our blender files with the colour swatch required by the static meshes and drones and the texture map required by our character<br/>

The concept for the level buildings seen within the orthographic world map was inspired by some of the products produced by [MeshTint](https://www.meshtint.com/). All 3D assets are of our own design and artistic creation, I simply used the images from their products as conceptual inspiration.

# GUI heads up display files:
Five simple png bitmaps for our in game GUI, produced to provide a sci-fi feel and provide just an aesthetic function

# Sounds:

Almost all of the sounds that have been used in the game were created specifically for it, either using an online tool [jsfxr](https://sfxr.me/) or by recording them with a microphone and editing them afterward. Huge efforts were made to produce each and every sound for the game; however, due to a lack of resources, expertise, and equipment to create original audio files, some couldn't be produced from scratch.

Using an open-source website, [Freesound](https://freesound.org/), that offers sounds that can be used in projects, some sounds were used to get inspiration. Based on those sounds, new audio files were produced, edited, and passed through pitch and amplitude shifters, de-noisers, exponential fades, and other filters to make them suitable for the game:

- Drones' rotors: Eelke's sound was modified (available at https://freesound.org/people/Eelke/sounds/198235/) and used it in the game.
- Background ambience of a level: Edited the sound provided by Flamiffer (available at https://freesound.org/people/Flamiffer/sounds/530161) and used it in the game.
- The sound of the Decoy: Tweaked Breviceps' sound (available at https://freesound.org/people/Breviceps/sounds/493162) and used it in the game. 
