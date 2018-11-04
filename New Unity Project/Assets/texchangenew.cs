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
       StartCoroutine(change()); 
	}
	
	// Update is called once per frame
	void Update () {
	}
    IEnumerator change(){
        var counter=0;
        while(counter>=0){
            counter++;
            if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Joy"){ 
              yield return new WaitForSeconds(0.1f);
        m_Renderer.material.mainTexture = texture1;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.material.mainTexture = texture2;  
            }
	            else if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Anger"){ 
                    counter++;
              yield return new WaitForSeconds(0.1f);
        m_Renderer.material.mainTexture = texture3;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.material.mainTexture = texture4;  
            }
            else if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Surprise"){ 
              yield return new WaitForSeconds(0.1f);
        m_Renderer.material.mainTexture = texture5;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.material.mainTexture = texture6;
        counter++;  
            }
                        else if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Disgust"){ 
              yield return new WaitForSeconds(0.1f);
        m_Renderer.material.mainTexture = texture7;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.material.mainTexture = texture8;
        counter++;  
            }
                                    else if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Sadness"){ 
              yield return new WaitForSeconds(0.1f);
        m_Renderer.material.mainTexture = texture9;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.material.mainTexture = texture10;
        counter++;  
            }
            else if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Fear"){ 
              yield return new WaitForSeconds(0.1f);
        m_Renderer.material.mainTexture = texture11;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.material.mainTexture = texture12;
        counter++;  
            }
            else{}
        }
    }
}

