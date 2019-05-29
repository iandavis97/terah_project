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
                GameObject.GetComponent<DeedReporter>().ReportDeedByActor(GameObject2.GetComponent<OrkFactionMember>(), "Joy", GameObject.GetComponent<OrkFactionMember>()
);
                break;
            case "Anger":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, 200, 200);
                GameObject.GetComponent<DeedReporter>().ReportDeedByActor(GameObject2.GetComponent<OrkFactionMember>(), "Anger", GameObject.GetComponent<OrkFactionMember>()
);
                break;
            case "Surprise":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, 200, 200, -200);
                GameObject.GetComponent<DeedReporter>().ReportDeedByActor(GameObject2.GetComponent<OrkFactionMember>(), "Surprise", GameObject.GetComponent<OrkFactionMember>()
);
                break;
            case "Disgust":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, 200, -200);
                GameObject.GetComponent<DeedReporter>().ReportDeedByActor(GameObject2.GetComponent<OrkFactionMember>(), "Disgust", GameObject.GetComponent<OrkFactionMember>()
);
                break;
            case "Sadness":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, -200, -200);
                GameObject.GetComponent<DeedReporter>().ReportDeedByActor(GameObject2.GetComponent<OrkFactionMember>(), "Sadness", GameObject.GetComponent<OrkFactionMember>()
);
                break;
            case "Fear":
                GameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, -200, 200);
                GameObject.GetComponent<DeedReporter>().ReportDeedByActor(GameObject2.GetComponent<OrkFactionMember>(), "Fear", GameObject.GetComponent<OrkFactionMember>()
);
                break;
            case "Flatter":
GameObject.GetComponent<DeedReporter>().ReportDeedByActor(GameObject2.GetComponent<OrkFactionMember>(), "Flatter", GameObject.GetComponent<OrkFactionMember>()
); break;
            case "Insult":
                GameObject.GetComponent<DeedReporter>().ReportDeedByActor(GameObject2.GetComponent<OrkFactionMember>(), "Insult", GameObject.GetComponent<OrkFactionMember>()
                );
                break;
            case "Gift":
                GameObject.GetComponent<DeedReporter>().ReportDeedByActor(GameObject2.GetComponent<OrkFactionMember>(), "Gift", GameObject.GetComponent<OrkFactionMember>()
               );
                break;
            case "Annoy":
                GameObject.GetComponent<DeedReporter>().ReportDeedByActor(GameObject2.GetComponent<OrkFactionMember>(), "Annoy", GameObject.GetComponent<OrkFactionMember>()
               );
                break;
            case "Ignore":
                GameObject.GetComponent<DeedReporter>().ReportDeedByActor(GameObject2.GetComponent<OrkFactionMember>(), "Ignore", GameObject.GetComponent<OrkFactionMember>()
                );
                break;
            case "Intimidate":
                GameObject.GetComponent<DeedReporter>().ReportDeedByActor(GameObject2.GetComponent<OrkFactionMember>(), "Intimidate", GameObject.GetComponent<OrkFactionMember>()
               );
                break;



        }



    }
}
