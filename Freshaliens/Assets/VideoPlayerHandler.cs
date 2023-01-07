using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Freshaliens.Management;

// Classico script di merda fatto l'ultimo giorno
[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerHandler : MonoBehaviour
{
    private VideoPlayer vp;
    
    private void Start()
    {
        if (LevelManager.Exists)
            LevelManager.Instance.PlayingIntroVideo = true;
        vp = GetComponent<VideoPlayer>();
        vp.loopPointReached += (_) => OnVideoOver();
        vp.Prepare();
        float l = (float)vp.clip.length;
        vp.isLooping = true;
        vp.Play();
    }

    private void OnVideoOver() {
        Destroy(gameObject);
        if (LevelManager.Exists)
            LevelManager.Instance.PlayingIntroVideo = false;
    }
}

