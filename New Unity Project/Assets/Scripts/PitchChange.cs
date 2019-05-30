using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class PitchChange : MonoBehaviour
{
    public float pitchMin, pitchMax, volumeMin, volumeMax;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePitch()
    {
        var typewriter = GetComponent<AbstractTypewriterEffect>();
        typewriter.audioSource.pitch = Random.Range(pitchMin, pitchMax);
        typewriter.audioSource.volume = Random.Range(volumeMin, volumeMax);
    }
    public void ResetPitch()
    {
        var typewriter = GetComponent<AbstractTypewriterEffect>();
        typewriter.audioSource.pitch = 1;
        typewriter.audioSource.volume = 1;
    }
}
