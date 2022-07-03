using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaFader : MonoBehaviour
{
    private float _targetAlpha = 1;
    private Material _material;
    public bool IsVisible { get; private set; }

    void Start()
    {
        _material = GetComponent<Renderer>().material;
        CheckAlpha();
    }

    void Update()
    {
        if (_material.color.a != _targetAlpha)
        {
            Color targetColor = new Color(_material.color.r, _material.color.g, _material.color.b, _targetAlpha);
            _material.color = Vector4.Lerp(_material.color, targetColor, Time.deltaTime * 6f);
        }
        CheckAlpha();
    }

    private void CheckAlpha()
    {
        if (_material.color.a == 1)
        {
            IsVisible = true;
        }
        else IsVisible = false;
    }

    public void FadeIn()
    {
        _targetAlpha = 1;
    }

    public void FadeOut()
    {
        _targetAlpha = 0;
    }
}
