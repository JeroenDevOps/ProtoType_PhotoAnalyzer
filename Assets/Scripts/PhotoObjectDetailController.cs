using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoObjectDetailController : MonoBehaviour
{
    // [SerializeField] private String _name = "";
    [SerializeField] private Color _rendercolor = Color.white;

    public Color GetColor(){
        return _rendercolor;
    }
}
