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
public SpriteRenderer  m_Renderer;
public GameObject Calyx;
	// Use this for initialization
	void Start () {
        m_Renderer = gameObject.GetComponent<SpriteRenderer>();
        Calyx = gameObject;
       StartCoroutine(change()); 
	}
	
	// Update is called once per frame
	void Update () {
	}
    IEnumerator change(){
        var counter=0;
        while(counter>=0){
            if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Joy"){ 
              yield return new WaitForSeconds(0.1f);
        m_Renderer.sprite = texture1;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.sprite = texture2;
        counter++;  
            }
	            else if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Anger"){ 
              yield return new WaitForSeconds(0.1f);
        m_Renderer.sprite = texture3;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.sprite = texture4;
        counter++;  
            }
            else if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Surprise"){ 
              yield return new WaitForSeconds(0.1f);
        m_Renderer.sprite = texture5;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.sprite = texture6;
        counter++;  
            }
                        else if(Calyx.GetComponent<EmotionalState>().GetCurrentEmotionName()=="Disgust"){ 
              yield return new WaitForSeconds(0.1f);
        m_Renderer.sprite = texture7;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.sprite = texture8;
        counter++;  
            }
            else{}
        }
    }
}

