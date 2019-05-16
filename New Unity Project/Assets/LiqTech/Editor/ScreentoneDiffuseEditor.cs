using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ScreentoneDiffuseEditor : ShaderGUI
{
    override public void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        var isTwoTone = !(materialEditor.target as Material).IsKeywordEnabled("FULLCOLOR");
        //var isDoubleSided = 

        EditorGUILayout.LabelField(new GUIContent("Base texture"), EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        var r1 = EditorGUILayout.GetControlRect(false, 128, GUILayout.Width(128));
        properties[0].textureValue = (Texture2D)EditorGUI.ObjectField(r1, properties[0].textureValue, typeof(Texture2D), false);
        GUILayout.BeginVertical();
        properties[1].floatValue = EditorGUILayout.FloatField("Size", properties[1].floatValue);
        if (GUILayout.Button("Set native size")) properties[1].floatValue = properties[0].textureValue == null ? 0 : properties[0].textureValue.width;
        properties[2].floatValue = Mathf.Deg2Rad * EditorGUILayout.Slider("Angle", properties[2].floatValue * Mathf.Rad2Deg, 0, 360);

        properties[3].colorValue = EditorGUILayout.ColorField("Primary color", properties[3].colorValue);
        if (isTwoTone) properties[4].colorValue = EditorGUILayout.ColorField("Secondary color", properties[4].colorValue);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        EditorGUILayout.LabelField(new GUIContent("Shaded texture"), EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        var r2 = EditorGUILayout.GetControlRect(false, 128, GUILayout.Width(128));
        properties[5].textureValue = (Texture2D)EditorGUI.ObjectField(r2, properties[5].textureValue, typeof(Texture2D), false);
        GUILayout.BeginVertical();
        properties[6].floatValue = EditorGUILayout.FloatField("Size", properties[6].floatValue);
        if (GUILayout.Button("Set native size")) properties[6].floatValue = properties[5].textureValue == null ? 0 : properties[5].textureValue.width;
        properties[7].floatValue = Mathf.Deg2Rad * EditorGUILayout.Slider("Angle", properties[7].floatValue * Mathf.Rad2Deg, 0, 360);

        properties[8].colorValue = EditorGUILayout.ColorField("Primary color", properties[8].colorValue);
        if (isTwoTone) properties[9].colorValue = EditorGUILayout.ColorField("Secondary color", properties[9].colorValue);
        GUILayout.Space(18);
        properties[10].floatValue = EditorGUILayout.Slider("Brightness threshold", properties[10].floatValue, 0.00001f, 1);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.Space(30);

        var wanttTwoTone = EditorGUILayout.Toggle("Use two tone textures", isTwoTone);

        if (wanttTwoTone != isTwoTone)
        {
            if (!wanttTwoTone)
            {
                (materialEditor.target as Material).EnableKeyword("FULLCOLOR");
            }
            else
            {
                (materialEditor.target as Material).DisableKeyword("FULLCOLOR");
            }
        }


        var isTwoSided = properties[12].floatValue == 0;
        var wantTwoSided = EditorGUILayout.Toggle("Double sided", isTwoSided);
        properties[12].floatValue = wantTwoSided ? 0 : 2;
    }
}