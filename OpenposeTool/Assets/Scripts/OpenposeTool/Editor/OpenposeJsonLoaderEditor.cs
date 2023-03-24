using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OpenposeJsonLoader))]
public class OpenposeJsonLoaderEditor : Editor
{
    OpenposeJsonLoader Target;

    void OnEnable()
    {
        Target = (OpenposeJsonLoader)target;
    }

    //@@@
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Parse"))
        {
            Target.Parse();
        }

        if (GUILayout.Button("EditorShowPoseKeyPoints"))
        {
            Target.EditorShowPoseKeypoints();
        }

        //serializedObject.ApplyModifiedProperties();
    }
}
