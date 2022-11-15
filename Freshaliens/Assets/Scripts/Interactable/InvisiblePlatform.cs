using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InvisiblePlatform : Interactable
{
    [Range(0.5f,1f)]
    [SerializeField] private float _maxAlpha;
    [Range(0,0.5f)]
    [SerializeField] private float _minAlpha;

    
    private GameObject _gameObject;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    
    
    void Start()
    {
        _gameObject = gameObject;
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeAlpha(_minAlpha);
    }

    private void ChangeAlpha(float newAlpha)
    {
        Color currentColor = _spriteRenderer.color;
        currentColor.a = newAlpha;
        _spriteRenderer.color = currentColor;
        Debug.Log("changed");
    }
    
    protected void ChangeLayer(string newLayerName)
    {
        int newLayer = LayerMask.NameToLayer(newLayerName);
        _gameObject.layer = newLayer;
    }

    
    public override void OnFairyEnter()
    {
        ChangeAlpha(_maxAlpha);
        ChangeLayer("Ground");
    }
    
    public override void OnFairyExit()
    {
        ChangeAlpha(_minAlpha);
        ChangeLayer("InvisiblePlatform");
    }
    
}
