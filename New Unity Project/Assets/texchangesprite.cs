using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.LoveHate;
public class texchangesprite : MonoBehaviour {
public Sprite texture1;
public Sprite texture2;
public Sprite texture3;
public Sprite texture4;
public Sprite texture5;
public Sprite texture6;
public Sprite texture7;
public Sprite texture8;
public Sprite texture9;
public Sprite texture10;
public Sprite texture11;
public Sprite texture12;
public SpriteRenderer  m_Renderer;
public GameObject Calyx;
	// Use this for initialization
	void Start () {
        m_Renderer = gameObject.GetComponent<SpriteRenderer>();
        Calyx = gameObject;
	}
	
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
        m_Renderer.sprite = texture1;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.sprite = texture2; 
        StopCoroutine("changejoy");}
    
    IEnumerator changeanger(){
    yield return new WaitForSeconds(0.1f);
        m_Renderer.sprite = texture3;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.sprite = texture4; 
        StopCoroutine("changeanger");}
    
        IEnumerator changesurprise(){
    yield return new WaitForSeconds(0.1f);
        m_Renderer.sprite = texture5;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.sprite = texture6; 
                    StopCoroutine("changesurprise");}

    
        IEnumerator changedisgust(){
    yield return new WaitForSeconds(0.1f);
        m_Renderer.sprite = texture7;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.sprite = texture8; 
                    StopCoroutine("changedisgust");}

    
        IEnumerator changesadness(){
    yield return new WaitForSeconds(0.1f);
        m_Renderer.sprite = texture9;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.sprite = texture10; 
                            StopCoroutine("changesadness");}
    
        IEnumerator changefear(){
    yield return new WaitForSeconds(0.1f);
        m_Renderer.sprite = texture11;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.sprite = texture12; 
                            StopCoroutine("changefear");}
}

