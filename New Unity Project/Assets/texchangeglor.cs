using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.LoveHate;
public class texchangeglor : MonoBehaviour {
public Texture2D texture1;
public Texture2D texture2;
public Renderer  m_Renderer;
public GameObject Calyx;
    
	// Use this for initialization
	void Start () {
        m_Renderer = gameObject.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
                   StartCoroutine("changejoy"); 
            }

        IEnumerator changejoy(){
         yield return new WaitForSeconds(0.1f);
        m_Renderer.material.mainTexture = texture1;
        yield return new WaitForSeconds(4.5f);
         m_Renderer.material.mainTexture = texture2; 
        StopCoroutine("changejoy");}
}
