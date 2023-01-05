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
