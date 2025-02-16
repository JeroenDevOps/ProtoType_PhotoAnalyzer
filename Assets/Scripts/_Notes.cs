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
5. Store multiple photos (in a dictionary?) and make it able to display them. To start, just make it possible to switch between them on the same plane.


*/
