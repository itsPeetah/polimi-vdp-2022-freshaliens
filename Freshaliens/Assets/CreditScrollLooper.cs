using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditScrollLooper : MonoBehaviour
{
    [SerializeField] private float minY = 0, maxY = 1000;

    private RectTransform ownTransform;

    private void Start()
    {
        ownTransform = transform as RectTransform;
    }

    private void Update()
    {
        if (ownTransform.anchoredPosition.y > maxY) ownTransform.anchoredPosition = new Vector2(ownTransform.anchoredPosition.x, minY);
        else if (ownTransform.anchoredPosition.y < minY) ownTransform.anchoredPosition = new Vector2(ownTransform.anchoredPosition.x, maxY);
    }
}
