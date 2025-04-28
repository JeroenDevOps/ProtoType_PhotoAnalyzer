using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CameraController2 : MonoBehaviour
{
    [SerializeField] private Camera _camMain;
    [SerializeField] private Camera _camPhoto;
    [SerializeField] private float _moveSpeedMain;
    [SerializeField] private float _moveSpeedPhoto;
    private int _photoCount = 0;
    private Dictionary<int, Texture2D> _photos = new Dictionary<int, Texture2D>();
    private bool _isPhotoMode = false;
    [SerializeField] private GameObject _displayObject;
    private int _photoDisplayNumber = -999; // set default as a number that will never be used
    private bool _forceDisplay = false; // force the display to update when taking a photo
    
    
    // Start is called before the first frame update
    void Start()
    {
        _camMain.enabled = true;
        _camPhoto.enabled = false; 
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
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
        if(Input.GetKeyDown(KeyCode.Return)){
            Debug.Log("Photos taken: " + _photos.Count);
            Debug.Log("Photos: " + _photos);
        }
        #endregion

        if(Input.GetMouseButton(1)){
            _isPhotoMode = true;
            if(Input.GetMouseButtonDown(0)){
                StartCoroutine(TakePicture());
            }
        } else {
            _isPhotoMode = false;
        }

        #region ### Camera Selection & Movement
        _camMain.enabled = !_isPhotoMode;
        _camPhoto.enabled = _isPhotoMode;
        Vector3 movementInput = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
        float moveSpeed = (_isPhotoMode) ? _moveSpeedPhoto : _moveSpeedMain;
        _camMain.transform.position += movementInput * moveSpeed * Time.deltaTime;
        _camPhoto.transform.position += movementInput * moveSpeed * Time.deltaTime;
        #endregion
    
        

        #region ### Photo Display
        int currentPhotoDisplay = _photoDisplayNumber;
        if(_photoDisplayNumber != -999){
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                _photoDisplayNumber--;
                if(_photoDisplayNumber < 0){
                    _photoDisplayNumber = _photos.Count - 1;
                }
            } 
            if(Input.GetKeyDown(KeyCode.RightArrow)){
                _photoDisplayNumber++;
                if(_photoDisplayNumber >= _photos.Count){
                    _photoDisplayNumber = 0;
                }
            }
        }
        if(_photoDisplayNumber != -999 && (currentPhotoDisplay != _photoDisplayNumber || _forceDisplay)){
            _forceDisplay = false; // reset the force display flag
            if(_displayObject.TryGetComponent<Renderer>(out Renderer rendererComponent) != false){
                rendererComponent.material.mainTexture = _photos[_photoDisplayNumber];
                Debug.Log("PhotoPlane #" + _photoDisplayNumber);
            }
            if(_displayObject.TryGetComponent<RawImage>(out RawImage imageComponent) != false){
                imageComponent.texture = _photos[_photoDisplayNumber];
                Debug.Log("PhotoUI #" + _photoDisplayNumber);
            }
        }
        #endregion
    }

    IEnumerator TakePicture()
    {
        yield return new WaitForEndOfFrame();
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 16);
        _camPhoto.targetTexture = renderTexture;
        Texture2D photo = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        _camPhoto.Render();
        RenderTexture.active = renderTexture;
        photo.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        photo.Apply();
        _camPhoto.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
        
        _photos.Add(_photoCount, photo);
        _photoCount++;
        _photoDisplayNumber = _photoCount - 1;
        _forceDisplay = true;
    }
}
