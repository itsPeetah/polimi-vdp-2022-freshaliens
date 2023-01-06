using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSprite : MonoBehaviour
{
    public enum VolumeButtonType {
        Master,
        Music,
        SFX
    }

    [SerializeField] private VolumeButtonType type = VolumeButtonType.Master;
    [SerializeField] private Sprite soundOnImage;
    [SerializeField] private Sprite soundOffImage;
    private Button button;
    
    private bool isOn = true;

    private void Start()
    {
        button = GetComponent<Button>();
        soundOnImage = button.image.sprite;
    }

    private void Update()
    {
        bool isOn;
        switch (type)
        {
            case VolumeButtonType.Music: isOn = !PlayerData.Instance.MuteMusic; break;
            case VolumeButtonType.SFX: isOn = !PlayerData.Instance.MuteSFX; break;
            default: isOn = !PlayerData.Instance.MuteMaster; break;
        }
        button.image.sprite = isOn ? soundOnImage : soundOffImage;
    }
}
