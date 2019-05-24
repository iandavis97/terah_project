using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers.LoveHate;
using PixelCrushers.LoveHate.ORKFrameworkSupport;
using ORKFramework.Behaviours;
public class SetupChar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupCharacter()
    {
        this.gameObject.AddComponent<Rigidbody>();
        this.gameObject.AddComponent<changeariabutton>();
        this.gameObject.AddComponent<OrkFactionMember>();
        this.gameObject.AddComponent<EmotionalState>();
        this.gameObject.AddComponent<Usable>();
        this.gameObject.AddComponent<DialogueSystemTrigger>();
        this.gameObject.AddComponent<serializeload>();
        this.gameObject.AddComponent<deserializeload>();
        this.gameObject.AddComponent<EventInteraction>();
        this.gameObject.AddComponent<EventInteraction>();
    }
}
