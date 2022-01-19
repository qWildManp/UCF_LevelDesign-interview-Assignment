using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightShining : MonoBehaviour
{
    private Renderer _renderer;
    private Material _material;
    private readonly string _keyword = "_EMISSION";
    private readonly string _colorName = "_EmissionColor";
    private GameObject parentSet;

    private void Start()
    {
        parentSet = transform.parent.parent.gameObject;
        _renderer = gameObject.GetComponent<Renderer>();
        _material = _renderer.material;
        _material.EnableKeyword(_keyword);
    }
    private void Update()
    {
        if(parentSet.GetComponent<puzzleSetRule>().CheckSetCorrect())
            _material.SetColor(_colorName, Color.green);
        else
            _material.SetColor(_colorName, Color.red);
    }
    public void SetLightColor(bool result)
    {
        if(result)
            _material.SetColor(_colorName, Color.green);
        else
            _material.SetColor(_colorName, Color.red);
    }
}
