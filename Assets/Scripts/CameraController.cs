using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
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
    private Dictionary<Vector2Int, PhotoObjectDetail> _photoAnalysis = new Dictionary<Vector2Int, PhotoObjectDetail>(); //(temp) dictionary for the photo analysis AND render creation
    private int _photoCount = 0;
    private int _photoDisplayNumber = -999; // set default as a number that will never be used
    private Dictionary<int, Dictionary<Vector2Int, PhotoObjectDetail>> _photoAnalysisDict = new Dictionary<int, Dictionary<Vector2Int, PhotoObjectDetail>>(); //dictionary for the photo analysis of all photos
    private Texture2D _photoRender; 
    private Dictionary<int, Texture2D> _photoRenderDict = new Dictionary<int, Texture2D>(); //dictionary for the render of all photos
    private Dictionary<int, int> _photoScoreDict = new Dictionary<int, int>(); //dictionary for the score of all photos

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
            if(_displayObject.TryGetComponent<Renderer>(out Renderer rendererComponent) != false){
                rendererComponent.material.mainTexture = _photoRenderDict[_photoDisplayNumber];
            }
            if(_displayObject.TryGetComponent<RawImage>(out RawImage imageComponent) != false){
                imageComponent.texture = _photoRenderDict[_photoDisplayNumber];
            }
            UnityEngine.Debug.Log("Photo #" + _photoDisplayNumber + " score: " + _photoScoreDict[_photoDisplayNumber]);
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
                float rayOriginX = 0f;
                float rayOriginY = 0f;
                // Renderer component needs flipped Texture2D due to plane rotation, RawImage does not
                if(_displayObject.TryGetComponent<Renderer>(out Renderer rendererComponent) != false){
                    rayOriginX = _resolutionCamera.x - (x / _resolutionPhoto.x) * _resolutionCamera.x;
                    rayOriginY = _resolutionCamera.y - (y / _resolutionPhoto.y) * _resolutionCamera.y;
                } else if(_displayObject.TryGetComponent<RawImage>(out RawImage imageComponent) != false){
                    rayOriginX = (x / _resolutionPhoto.x) * _resolutionCamera.x;
                    rayOriginY = (y / _resolutionPhoto.y) * _resolutionCamera.y;
                }
                    
                
                Vector3 rayOrigin = new Vector3(rayOriginX, rayOriginY, 0);
                PhotoObjectDetail photoDetails = null;
                if(Physics.Raycast(_camPhoto.ScreenPointToRay(rayOrigin), out RaycastHit hit, Mathf.Infinity)){
                    if((_raycastLayer & (1 << hit.collider.gameObject.layer)) != 0){ //if the layer of the object hit is in the layermask
                        PhotoObjectDetailController photoObjectDetailCtrl = hit.collider.gameObject.GetComponent<PhotoObjectDetailController>();
                        if(photoObjectDetailCtrl != null){
                            photoDetails = photoObjectDetailCtrl.GetPhotoObjectDetail();
                        } else {
                            // Check if the parent has a PhotoObjectDetailController script and use that one instead
                            Transform parentTransform = hit.collider.gameObject.transform.parent;
                            while(parentTransform != null && photoDetails == null){
                                photoObjectDetailCtrl = parentTransform.GetComponent<PhotoObjectDetailController>();
                                if(photoObjectDetailCtrl != null){
                                    photoDetails = photoObjectDetailCtrl.GetPhotoObjectDetail();
                                }
                                parentTransform = parentTransform.parent;
                            }
                        }
                    }
                }
                
                _photoAnalysis.Add(new Vector2Int(x, y), photoDetails);
            }
        }
        _photoAnalysisDict.Add(_photoCount, _photoAnalysis);

        ProcessAnalysis(_photoAnalysis);
    }

    private void ProcessAnalysis(Dictionary<Vector2Int, PhotoObjectDetail> photoAnalysisResults){
        Texture2D newRender = new Texture2D((int) _resolutionPhoto.x, (int) _resolutionPhoto.y);
        Dictionary<string, int> scoreAnalysisDict = new Dictionary<string, int>();
        for(int x = 0; x < _resolutionPhoto.x; x++){
            for(int y = 0; y < _resolutionPhoto.y; y++){
                // Render photo onto Texture2D
                PhotoObjectDetail photoDetail = photoAnalysisResults[new Vector2Int(x,y)];
                if(photoDetail == null || photoDetail.objectId == null){
                    continue;
                } else {
                    newRender.SetPixel(x, y, photoDetail.rendercolor);

                    // Score, only counting unique objects
                    if(!scoreAnalysisDict.ContainsKey(photoDetail.objectId)){
                        scoreAnalysisDict.Add(photoDetail.objectId, photoDetail.score);
                    }
                }
            }
        }
        newRender.Apply();
        _photoRender = newRender;
        _photoRenderDict.Add(_photoCount, _photoRender);

        int score = 0;
        foreach(KeyValuePair<string, int> scoreAnalysis in scoreAnalysisDict){
            score += scoreAnalysis.Value;
        }
        _photoScoreDict.Add(_photoCount, score);
    }

}
