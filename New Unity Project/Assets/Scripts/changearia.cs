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
    public GameObject GameObject;
    public GameObject GameObject2;
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

    private void OnDestroy()
    {
        Lua.UnregisterFunction("ChangeAria");

    }

    public void ChangeAria (string Emotion, string name, string actorname){
        
        //playing sound effect when ARIA changes
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sfx;
        audioSource.PlayOneShot(sfx,.5f);
        
        GameObject = GameObject.Find(name);
        GameObject2 = GameObject.Find(actorname);
        GameObject2.GetComponent<DeedReporter>().ReportDeed(Emotion, GameObject.GetComponent<OrkFactionMember>());


        switch (Emotion){
            case "Joy":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, 200, 200, 200);
                break;
            case "Anger":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, 200, 200);
                break;
            case "Surprise":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, 200, 200, -200);
                break;
            case "Disgust":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, 200, -200);
                break;
            case "Sadness":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, -200, -200);
                break;
            case "Fear":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, -200, 200);
                break;



        }



    }
}
