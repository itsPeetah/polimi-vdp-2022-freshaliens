using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSprite : MonoBehaviour
{
    public Sprite soundOnImage;
    public Sprite soundOffImage;
    public Button button;
    
    private bool isOn = true;

 /*   [SerializeField] private enum buttonTypes
    {
        Master,
        Music,
        Sfx,
    }*/
    // Start is called before the first frame update

    private void Start()
    {
        soundOnImage = button.image.sprite;
    }

    private void Update()
    {
        if (AudioManager1.instance.sfxSource.mute && AudioManager1.instance.musicSource.mute == true)
            button.image.sprite = soundOffImage;
        if(AudioManager1.instance.sfxSource.mute == false && AudioManager1.instance.musicSource.mute == false)
            button.image.sprite = soundOnImage;
    }

    public void ButtonClicked()
    {
        if (isOn)
        {
            button.image.sprite = soundOffImage;
            isOn = false;
            
        }
        else
        {
            button.image.sprite = soundOnImage;
            isOn = true;
        }
    }
}
