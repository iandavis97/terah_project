using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.LoveHate;
using ORKFramework;

public class colorswitcher : MonoBehaviour {
public Texture2D texture1;
public Texture2D texture2;
public Texture2D texture3;
public Texture2D texture4;
public Texture2D texture5;
public Texture2D texture6;
public Texture2D texture7;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if(ORK.Game.Variables.GetString("emotion")=="Joy"){ 
            gameObject.GetComponent<AmplifyColorEffect>().LutTexture= texture1;
	}
    else if(ORK.Game.Variables.GetString("emotion")=="Anger"){ 
            gameObject.GetComponent<AmplifyColorEffect>().LutTexture= texture2;}
	
    else {
                    gameObject.GetComponent<AmplifyColorEffect>().LutTexture= texture7;}

    }
}
