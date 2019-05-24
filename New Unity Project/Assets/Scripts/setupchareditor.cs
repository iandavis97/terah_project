using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SetupChar))]
public class setupchareditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SetupChar myScript = (SetupChar)target;
        if (GUILayout.Button("Setup Character"))
        {
            myScript.SetupCharacter();
        }

    }
}


