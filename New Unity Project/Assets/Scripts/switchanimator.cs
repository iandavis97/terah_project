using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.LoveHate;
public class switchanimator : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
if(gameObject.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Anger"){ 
         gameObject.GetComponent<Animator>().Play("angry");  
            }
       else if(gameObject.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Joy"){ 
         gameObject.GetComponent<Animator>().Play("Idle_Stance_2");  
            }
}

}

