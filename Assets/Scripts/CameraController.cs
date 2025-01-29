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
    private Vector2 _resolutionPhoto;
    private Vector2 _resolutionCamera;
    private Dictionary<Vector2Int, Color> _photoAnalysis = new Dictionary<Vector2Int, Color>(); //(temp) dictionary for the photo analysis AND render creation
    [SerializeField] private LayerMask _raycastLayer;
    private Texture2D _photoRender; 
    [SerializeField] private GameObject _displayObject;

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


        Vector3 movementInput = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
        if(Input.GetMouseButton(1)){
            _isPhotoMode = true;
            if(Input.GetMouseButtonDown(0)){
                TakePicture();
            }
            if(Input.GetMouseButtonUp(0)){
                _displayObject.GetComponent<Renderer>().material.mainTexture = _photoRender;
                Debug.Log("Display updated");
            }
        } else {
            _isPhotoMode = false;
        }

        _camMain.enabled = !_isPhotoMode;
        _camPhoto.enabled = _isPhotoMode;

        float moveSpeed = (_isPhotoMode) ? _moveSpeedPhoto : _moveSpeedMain;
        _camMain.transform.position += movementInput * moveSpeed * Time.deltaTime;
        _camPhoto.transform.position += movementInput * moveSpeed * Time.deltaTime;
    }


    private void TakePicture(){
        Debug.Log("Snap!");
        _photoAnalysis.Clear();
        for(int x = 0; x < _resolutionPhoto.x; x++){
            for(int y = 0; y < _resolutionPhoto.y; y++){
                // not entirely sure why, but the image was flipped when I applied it to the Texture2D
                // so substracting from the full resolution made the image come out right
                float rayOriginX = _resolutionCamera.x - (x / _resolutionPhoto.x) * _resolutionCamera.x;
                float rayOriginY = _resolutionCamera.y - (y / _resolutionPhoto.y) * _resolutionCamera.y;
                
                Vector3 rayOrigin = new Vector3(rayOriginX, rayOriginY, 0);
                Color pixelColor = Color.black;
                if(Physics.Raycast(_camPhoto.ScreenPointToRay(rayOrigin), out RaycastHit hit, Mathf.Infinity, _raycastLayer, QueryTriggerInteraction.UseGlobal)){
                    pixelColor = hit.collider.gameObject.GetComponent<PhotoObjectDetailController>().GetColor();
                }
                _photoAnalysis.Add(new Vector2Int(x, y), pixelColor);
            }
        }

        CreateRender();
        Debug.Log("Photo taken");
        
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
    }
}
