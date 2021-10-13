using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(Generation)), CanEditMultipleObjects]
public class GenerationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Generation gen = (Generation)target;
        if (DrawDefaultInspector())
        {

        }
        if (GUILayout.Button("Generate"))
        {
            gen.Create();
        }
        if (GUILayout.Button("Clear"))
        {
            gen.Clear();
        }
    }
}
