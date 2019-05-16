using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.LoveHate;
public class texchangenew : MonoBehaviour {
public Texture2D texture1;
public Texture2D texture2;
public Texture2D texture3;
public Texture2D texture4;
public Texture2D texture5;
public Texture2D texture6;
public Texture2D texture7;
public Texture2D texture8;
public Texture2D texture9;
public Texture2D texture10;
public Texture2D texture11;
public Texture2D texture12;
public Renderer  m_Renderer;
public GameObject Calyx;
    
	// Use this for initialization
	void Start () {
        m_Renderer = gameObject.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
                   if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Joy"){
                            
                    StopCoroutine("changeanger");
                    StopCoroutine("changesurprise");
                    StopCoroutine("changedisgust");
                    StopCoroutine("changesadness");
                    StopCoroutine("changefear");

                       StartCoroutine("changejoy"); 
                
            }
	            else if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Anger"){
                            
                    StopCoroutine("changejoy");
                    StopCoroutine("changesurprise");
                    StopCoroutine("changedisgust");
                    StopCoroutine("changesadness");
                    StopCoroutine("changefear");

                   StartCoroutine("changeanger");
                    
            }
              else if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Surprise"){
                            
                      StopCoroutine("changejoy");
                    StopCoroutine("changeanger");
                    StopCoroutine("changedisgust");
                    StopCoroutine("changesadness");
                    StopCoroutine("changefear");

                    StartCoroutine("changesurprise");
                    
            }
              else if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Disgust"){
                            
        StopCoroutine("changejoy");
                    StopCoroutine("changeanger");
                    StopCoroutine("changesurprise");
                    StopCoroutine("changesadness");
                    StopCoroutine("changefear");
                     StartCoroutine("changedisgust");
                    
            }
              else if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Sadness"){
                            
                            StopCoroutine("changejoy");
                    StopCoroutine("changeanger");
                    StopCoroutine("changesurprise");
                    StopCoroutine("changedisgust");
                    StopCoroutine("changefear");
                    StartCoroutine("changesadness");
                    
            }
              else if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Fear"){
                            
        StopCoroutine("changejoy");
                    StopCoroutine("changeanger");
                    StopCoroutine("changesurprise");
                    StopCoroutine("changesadness");
                    StopCoroutine("changedisgust");
                    StartCoroutine("changefear");
                    
            }
               
            else{}
	}
        

        IEnumerator changejoy(){
         yield return new WaitForSeconds(0.1f);
        m_Renderer.material.mainTexture = texture1;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.material.mainTexture = texture2; 
        StopCoroutine("changejoy");}
    
    IEnumerator changeanger(){
    yield return new WaitForSeconds(0.1f);
        m_Renderer.material.mainTexture = texture3;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.material.mainTexture = texture4; 
        StopCoroutine("changeanger");}
    
        IEnumerator changesurprise(){
    yield return new WaitForSeconds(0.1f);
        m_Renderer.material.mainTexture = texture5;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.material.mainTexture = texture6; 
                    StopCoroutine("changesurprise");}

    
        IEnumerator changedisgust(){
    yield return new WaitForSeconds(0.1f);
        m_Renderer.material.mainTexture = texture7;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.material.mainTexture = texture8; 
                    StopCoroutine("changedisgust");}

    
        IEnumerator changesadness(){
    yield return new WaitForSeconds(0.1f);
        m_Renderer.material.mainTexture = texture9;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.material.mainTexture = texture10; 
                            StopCoroutine("changesadness");}
    
        IEnumerator changefear(){
    yield return new WaitForSeconds(0.1f);
        m_Renderer.material.mainTexture = texture11;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.material.mainTexture = texture12; 
                            StopCoroutine("changefear");}
}
