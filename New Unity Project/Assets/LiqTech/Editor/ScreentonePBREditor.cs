using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class ScreentonePBREditor : ShaderGUI
{
    override public void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        var isTwoTone = !(materialEditor.target as Material).IsKeywordEnabled("FULLCOLOR");

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

        GUILayout.Space(30);

        properties[5].floatValue = EditorGUILayout.Slider("Glossiness", properties[5].floatValue, 0, 1);
        properties[6].floatValue = EditorGUILayout.Slider("Metallic", properties[6].floatValue, 0, 1);

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
    }
}
