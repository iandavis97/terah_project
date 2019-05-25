using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
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
#endif




