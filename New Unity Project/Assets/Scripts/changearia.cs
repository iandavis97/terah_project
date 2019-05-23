using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using PixelCrushers.LoveHate;
using PixelCrushers.LoveHate;
using PixelCrushers.LoveHate.ORKFrameworkSupport;
public class changearia : MonoBehaviour
{
    public GameObject gameObject;
    public AudioClip sfx;
    // Start is called before the first frame update
    void Start()
    {
        Lua.RegisterFunction("ChangeAria", this, typeof(changearia).GetMethod("ChangeAria"));


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeAria (string Emotion, string name){
        //playing sound effect when ARIA changes
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sfx;
        audioSource.PlayOneShot(sfx,.75f);

        gameObject = GameObject.Find(name);
        switch (Emotion){
            case "Joy":
                gameObject.GetComponent<OrkFactionMember>().pad.Modify(0, 200, 200, 200);
                break;
            case "Anger":
                gameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, 200, 200);
                break;
            case "Surprise":
                gameObject.GetComponent<OrkFactionMember>().pad.Modify(0, 200, 200, -200);
                break;
            case "Disgust":
                gameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, 200, -200);
                break;
            case "Sadness":
                gameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, -200, -200);
                break;
            case "Fear":
                gameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, -200, 200);
                break;
        }

    }
}
