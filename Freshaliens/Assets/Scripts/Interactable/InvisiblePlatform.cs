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

   private SpriteRenderer _spriteRenderer;
    
    
    void Start()
    {
        _gameObject = gameObject;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeAlpha(_minAlpha);
    }
    

    private void ChangeAlpha(float newAlpha)
    {
        Color currentColor = _spriteRenderer.color;
        currentColor.a = newAlpha;
        _spriteRenderer.color = currentColor;
    }

    public override void OnInteract()
    {
    }
    
    public override void OnFatinaEnter()
    {
        ChangeAlpha(_maxAlpha);
        ChangeLayer("TouchablePlatform");
    }
    
    public override void OnFatinaExit()
    {
        ChangeAlpha(_minAlpha);
        ChangeLayer("InvisiblePlatform");
    }
}
