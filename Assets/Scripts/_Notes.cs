/*******************************
*********** NOTES **************
*******************************/


/* ACTIONS:
01. [x] Setup test scene
02. [x] When holding right click, use a secondary zoomed in (lower res) camera
03. [x] When zoomed in, click left click to snap the picture 
    [x] a.) How to take the photo?
        https://discussions.unity.com/t/how-to-save-a-picture-take-screenshot-from-a-camera-in-game/5792/3
        i.) how to take the photo so that I can actually analyze it? Or is analyzing needed early and then the result is compiled into an image?
        Solution: shoot rays from the camera and store the color of the object hit by the ray in a dictionary
    [x] b.) How to "store" the photo (for game use only, so do not store to disk)?
        i.) Create a Texture2D and apply the colors to it
        ii.) Apply the Texture2D to a plane
04. [x] Display the photo on a plane
05. [x] Store multiple photos (in a dictionary?) and make it able to display them. To start, just make it possible to switch between them on the same render object (plane).
06. [x] Create PhotoObjectDetail class and use this for the photo analysis, allowing multiple stats to be tracked per photo.
07. [x] Change the 'CreateRender' method into a 'ProcessAnalysis' method that does both rendering and score calculation.
08. [x] Create UI image display and modify the code to utilize this UI component instead of the plane 
09.  [x] Create TestScene to get a feel for a real game scene 
    a.) [x] add 2DSprite package
    b.) [x] Insert sprites into scene
    >   Problem: Sprites are 2D objects and require Physics2D colliders to be detected by raycasting.
                 What do I want to do? 3D models or 2D sprites?
        Answer: 3D models are easier to set up and provide better layering and raycasting options.
10. [x] Create test assets in 3D, that still look like 2D sprites or work on 'artstyle'
11. [x] Set up first test 3d object in the test scene and make it possible to take a photo of it.
12. Think about art style and how to make it look good. 
    a). Draw a character digitally
    b). Create the 3D outline for the character and use the character as the texture
    c). Import the character into the Unity scene and check if it works
    d). If it works, check if the workflow can be improved for future characters
13. Create a sample scene to reflect a 'real' static scene
14. Create a sample scene to reflect a 'real' dynamic scene 
    a). Check how to animate the scene
    b). How to tell a simple story with the scene
    c). Make things happen in different areas of the scene, so that the player has to move around to see everything and take photos of it
15. Create a post-scene end-screen with the photos taken and a score for each photo 



Far future steps:
[i] Post-scene photo analysis with name and score.
[ii] During photo analysis, show which part is being analysed (highlighted in some way)
[iii] Add a "photo album" to the game??
[iv] ...

*/
