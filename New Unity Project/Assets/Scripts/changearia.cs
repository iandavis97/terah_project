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

        switch (Emotion){
            case "Joy":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, 200, 200, 200);
                GameObject2.GetComponent<DeedReporter>().ReportDeed("Joy", GameObject.GetComponent<OrkFactionMember>()
);
                break;
            case "Anger":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, 200, 200);
                GameObject2.GetComponent<DeedReporter>().ReportDeed("Anger", GameObject.GetComponent<OrkFactionMember>()
);
                break;
            case "Surprise":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, 200, 200, -200);
                GameObject2.GetComponent<DeedReporter>().ReportDeed("Surprise", GameObject.GetComponent<OrkFactionMember>()
);
                break;
            case "Disgust":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, 200, -200);
                GameObject2.GetComponent<DeedReporter>().ReportDeed("Disgust", GameObject.GetComponent<OrkFactionMember>()
);
                break;
            case "Sadness":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, -200, -200);
                GameObject2.GetComponent<DeedReporter>().ReportDeed("Sadness", GameObject.GetComponent<OrkFactionMember>()
);
                break;
            case "Fear":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, -200, 200);
                GameObject2.GetComponent<DeedReporter>().ReportDeed("Fear", GameObject.GetComponent<OrkFactionMember>()
);
                break;
            case "Flatter":
GameObject2.GetComponent<DeedReporter>().ReportDeed("Flatter", GameObject.GetComponent<OrkFactionMember>()
); break;
            case "Insult":
                GameObject2.GetComponent<DeedReporter>().ReportDeed("Insult", GameObject.GetComponent<OrkFactionMember>()
                );
                break;
            case "Gift":
                GameObject2.GetComponent<DeedReporter>().ReportDeed("Gift", GameObject.GetComponent<OrkFactionMember>()
               );
                break;
            case "Annoy":
                GameObject2.GetComponent<DeedReporter>().ReportDeed("Annoy", GameObject.GetComponent<OrkFactionMember>()
               );
                break;
            case "Ignore":
                GameObject2.GetComponent<DeedReporter>().ReportDeed("Ignore", GameObject.GetComponent<OrkFactionMember>()
                );
                break;
            case "Intimidate":
                GameObject2.GetComponent<DeedReporter>().ReportDeed("Intimidate", GameObject.GetComponent<OrkFactionMember>()
               );
                break;



        }



    }
}
