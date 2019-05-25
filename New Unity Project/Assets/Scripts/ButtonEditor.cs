using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(changeariabutton))]
public class ButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        changeariabutton myScript = (changeariabutton)target;
        if (GUILayout.Button("Joy"))
        {
            myScript.Joy();
        }

        if (GUILayout.Button("Anger"))
        {
            myScript.Anger();
        }

        if (GUILayout.Button("Surprise"))
        {
            myScript.Surprise();
        }

        if (GUILayout.Button("Disgust"))
        {
            myScript.Disgust();
        }

        if (GUILayout.Button("Sadness"))
        {
            myScript.Sadness();
        }
        if (GUILayout.Button("Fear"))
        {
            myScript.Fear();
        }
    }
}

#endif
