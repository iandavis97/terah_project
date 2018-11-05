using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.LoveHate;

public class volumeswitcher : MonoBehaviour {
public Texture2D texture1;
public Texture2D texture2;
public Texture2D texture3;
public Texture2D texture4;
public Texture2D texture5;
public Texture2D texture6;
public GameObject Calyx;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Joy"){ 
            gameObject.GetComponent<AmplifyColorVolume>().LutTexture= texture1;
	}
    else if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Anger"){ 
            gameObject.GetComponent<AmplifyColorVolume>().LutTexture= texture2;}
	}
}
