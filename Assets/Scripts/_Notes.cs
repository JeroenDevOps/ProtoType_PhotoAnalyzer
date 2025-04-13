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
    a.) [x] Try-out new style with 3D models and see if it works
    b.) [x] Urgent: Fix subobjects on the test-model:
        i.) [x] if the object does not have a PhotoObjectDetailController script, check if the parent has one and use that one instead  
    c.) [x] Rework texturing of 3D object so all UV maps are on 1 image, modifying the image may become possible with ChatGPT
    d.) Test ChatGPT for creating a texture for the 3D object
    e.) Improve modelling/texturing workflow
        i.) [x] Create photoshop file for UV map texture by using cutout/masking layer
        ii.) ...
13. Make the photos be real photos with the score colors inserted into the photo
    a.) Check how to take a screenshop in Unity (https://gamedevbeginner.com/how-to-capture-the-screen-in-unity-3-methods/)
    b.) Open the screenshot as a Texture2D and apply the score colors to it. (Make the default colour not be black anymore, just skip)
14. Create a sample scene to reflect a 'real' static scene
15. Create a sample scene to reflect a 'real' dynamic scene 
    a). Check how to animate the scene
    b). How to tell a simple story with the scene
    c). Make things happen in different areas of the scene, so that the player has to move around to see everything and take photos of it
16. Create a post-scene end-screen with the photos taken and a score for each photo 



Far future steps:
[i] Post-scene photo analysis with name and score.
[ii] During photo analysis, show which part is being analysed (highlighted in some way)
[iii] Add a "photo album" to the game??
[iv] ...





Notes:
- When importing a 3D model into the scene, make sure the following is done:
    - Add MeshColliders to all objects of the model
    - Add PhotoObjectDetailController to the parent object of the model
- It's possible to create a material with the image of the modified base UV mapping texture and apply it to the model
    - This allows for randomization options of the characters by e.g. using 10 different textures per model and applying them randomly to the model


*/
