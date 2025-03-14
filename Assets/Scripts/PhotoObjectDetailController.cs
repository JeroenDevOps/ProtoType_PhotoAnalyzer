using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoObjectDetailController : MonoBehaviour
{
    [SerializeField] private String _name = "";
    [SerializeField] private Color _rendercolor = Color.white;
    [SerializeField] private int _score = 0;
    private PhotoObjectDetail _pod = new PhotoObjectDetail();

    void Start()
    {
        _pod.name = _name;
        _pod.rendercolor = _rendercolor;
        _pod.score = _score;
    }

    public PhotoObjectDetail GetPhotoObjectDetail(){
        return _pod;
    }
    

}
