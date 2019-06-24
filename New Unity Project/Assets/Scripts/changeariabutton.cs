using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using PixelCrushers.LoveHate;
using PixelCrushers.LoveHate;
using PixelCrushers.LoveHate.ORKFrameworkSupport;
public class changeariabutton : MonoBehaviour
{
    // Start is called before the first frame update

    public void Joy (){

                gameObject.GetComponent<OrkFactionMember>().pad.Modify(0, 200, 200, 200);

    }
    public void Anger()
    {

        this.gameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, 200, 200);
    }
    public void Surprise()
    {

        this.gameObject.GetComponent<OrkFactionMember>().pad.Modify(0, 200, 200, -200);

    }
    public void Disgust()
    {

        this.gameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, 200, -200);

    }
    public void Sadness()
    {

        this.gameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, -200, -200);

    }


    public void Fear()
    {

        this.gameObject.GetComponent<OrkFactionMember>().pad.Modify(0, -200, -200, 200);

    }

    public void Unknown()
    {

        this.gameObject.GetComponent<OrkFactionMember>().pad.Modify(0, 0, 0, 0);

    }







}

