using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Camera _camMain;
    [SerializeField] private Camera _camPhoto;
    [SerializeField] private float _moveSpeedMain;
    [SerializeField] private float _moveSpeedPhoto;
    [SerializeField] private float _resolutionScalePhoto; //the scale difference between camera and photo
    [SerializeField] private LayerMask _raycastLayer;
    [SerializeField] private GameObject _displayObject;
    private Vector2 _resolutionPhoto;
    private Vector2 _resolutionCamera;
    private Dictionary<Vector2Int, Color> _photoAnalysis = new Dictionary<Vector2Int, Color>(); //(temp) dictionary for the photo analysis AND render creation
    private int _photoCount = 0;
    private int _photoDisplayNumber = -999; // set default as a number that will never be used
    private Dictionary<int, Dictionary<Vector2Int, Color>> _photoAnalysisDict = new Dictionary<int, Dictionary<Vector2Int, Color>>(); //dictionary for the photo analysis of all photos
    private Texture2D _photoRender; 
    private Dictionary<int, Texture2D> _photoRenderDict = new Dictionary<int, Texture2D>(); //dictionary for the render of all photos

    private bool _isPhotoMode;

    // Start is called before the first frame update
    void Start()
    {
        _camMain.enabled = true;
        _camPhoto.enabled = false; 
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        _resolutionCamera = new Vector2(_camPhoto.pixelWidth, _camPhoto.pixelHeight);
        _resolutionPhoto = _resolutionScalePhoto * _resolutionCamera;
    }

    // Update is called once per frame
    void Update()
    {
        #region ### Testing controls
        if(Input.GetKeyDown(KeyCode.Backspace)){
            _camMain.transform.position = new Vector3(0, 0, -10);
            _camPhoto.transform.position = new Vector3(0, 0, -8);
        }
        if(Input.GetKeyDown(KeyCode.L)){
            if(UnityEngine.Cursor.lockState == CursorLockMode.Locked){
                UnityEngine.Cursor.lockState = CursorLockMode.None;
            } else {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            }
        }
        #endregion

        #region ### Photo Controls
        #region ### Photo Creation
        int currentPhotoDisplay = _photoDisplayNumber;
        if(Input.GetMouseButton(1)){
            _isPhotoMode = true;
            if(Input.GetMouseButtonDown(0)){
                TakePicture();
                _photoDisplayNumber = _photoCount;
                _photoCount++;
            }
        } else {
            _isPhotoMode = false;
        }
        #endregion
        #region ### Photo Display
        if(_photoDisplayNumber != -999){
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                _photoDisplayNumber--;
                if(_photoDisplayNumber < 0){
                    _photoDisplayNumber = _photoAnalysisDict.Count - 1;
                }
            } 
            if(Input.GetKeyDown(KeyCode.RightArrow)){
                _photoDisplayNumber++;
                if(_photoDisplayNumber >= _photoAnalysisDict.Count){
                    _photoDisplayNumber = 0;
                }
            }
        }
        if(_photoDisplayNumber != -999 && currentPhotoDisplay != _photoDisplayNumber){
            DisplayNewPhoto(_photoDisplayNumber);
        }
        #endregion 
        #endregion

        #region ### Camera Selection & Movement
        _camMain.enabled = !_isPhotoMode;
        _camPhoto.enabled = _isPhotoMode;
        Vector3 movementInput = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
        float moveSpeed = (_isPhotoMode) ? _moveSpeedPhoto : _moveSpeedMain;
        _camMain.transform.position += movementInput * moveSpeed * Time.deltaTime;
        _camPhoto.transform.position += movementInput * moveSpeed * Time.deltaTime;
        #endregion
    }


    private void TakePicture(){
        _photoAnalysis.Clear();
        for(int x = 0; x < _resolutionPhoto.x; x++){
            for(int y = 0; y < _resolutionPhoto.y; y++){
                // not entirely sure why, but the image was flipped when I applied it to the Texture2D
                // so substracting from the full resolution made the image come out correctly
                float rayOriginX = _resolutionCamera.x - (x / _resolutionPhoto.x) * _resolutionCamera.x;
                float rayOriginY = _resolutionCamera.y - (y / _resolutionPhoto.y) * _resolutionCamera.y;
                
                Vector3 rayOrigin = new Vector3(rayOriginX, rayOriginY, 0);
                Color pixelColor = Color.black;
                if(Physics.Raycast(_camPhoto.ScreenPointToRay(rayOrigin), out RaycastHit hit, Mathf.Infinity)){
                    
                    if((_raycastLayer & (1 << hit.collider.gameObject.layer)) != 0){ //if the layer of the object hit is in the layermask
                        pixelColor = hit.collider.gameObject.GetComponent<PhotoObjectDetailController>().GetColor();
                    }
                }
                
                _photoAnalysis.Add(new Vector2Int(x, y), pixelColor);
            }
        }
        _photoAnalysisDict.Add(_photoCount, _photoAnalysis);

        CreateRender();
    }

    private void CreateRender(){
        Texture2D newRender = new Texture2D((int) _resolutionPhoto.x, (int) _resolutionPhoto.y);
        for(int x = 0; x < _resolutionPhoto.x; x++){
            for(int y = 0; y < _resolutionPhoto.y; y++){
                Color pixelColor = _photoAnalysis[new Vector2Int(x,y)];
                newRender.SetPixel(x, y, pixelColor);
            }
        }
        newRender.Apply();

        _photoRender = newRender;
        _photoRenderDict.Add(_photoCount, _photoRender);
    }

    private void DisplayNewPhoto(int displayNumber){
        _displayObject.GetComponent<Renderer>().material.mainTexture = _photoRenderDict[displayNumber];
    }
}
