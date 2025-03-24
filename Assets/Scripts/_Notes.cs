/*******************************
*********** NOTES **************
*******************************/


/* ACTIONS:
1. [x] Setup test scene
2. [x] When holding right click, use a secondary zoomed in (lower res) camera
3. [x] When zoomed in, click left click to snap the picture 
    [x] a.) How to take the photo?
        https://discussions.unity.com/t/how-to-save-a-picture-take-screenshot-from-a-camera-in-game/5792/3
        i.) how to take the photo so that I can actually analyze it? Or is analyzing needed early and then the result is compiled into an image?
        Solution: shoot rays from the camera and store the color of the object hit by the ray in a dictionary
    [x] b.) How to "store" the photo (for game use only, so do not store to disk)?
        i.) Create a Texture2D and apply the colors to it
        ii.) Apply the Texture2D to a plane
4. [x] Display the photo on a plane
5. [x] Store multiple photos (in a dictionary?) and make it able to display them. To start, just make it possible to switch between them on the same render object (plane).
6. [x] Create PhotoObjectDetail class and use this for the photo analysis, allowing multiple stats to be tracked per photo.
7. [x] Change the 'CreateRender' method into a 'ProcessAnalysis' method that does both rendering and score calculation.
8. Create TestScene to get a feel for a real game scene
9. 




Far future steps:
[i] Post-scene photo analysis with name and score.
[ii] During photo analysis, show which part is being analysed (highlighted in some way)
[iii] Add a "photo album" to the game??
[iv] ...

*/
