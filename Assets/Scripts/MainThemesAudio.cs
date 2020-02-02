using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainThemesAudio : MonoBehaviour
{
    private AudioManager audioManager;

    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("background");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
