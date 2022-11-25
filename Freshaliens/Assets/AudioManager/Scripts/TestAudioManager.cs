using System.Collections;
using System.Collections.Generic;
using GameManagement.Audio;
using UnityEngine;

public class TestAudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            AudioManager.Instance.PlayMenuMusicLoop();
        if (Input.GetKeyDown(KeyCode.Alpha1))
            AudioManager.Instance.PlayGameMusicLoop();
    }
}
