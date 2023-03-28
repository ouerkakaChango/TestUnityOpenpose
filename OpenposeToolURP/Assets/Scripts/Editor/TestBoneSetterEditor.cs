using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestBoneSetter))]
public class TestBoneSetterEditor : Editor
{
    TestBoneSetter Target;
    void OnEnable()
    {
        Target = (TestBoneSetter)target;
    }

    //@@@
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("InitOpenposeMapping"))
        {
            TestBoneSetter.InitOpenposeMapping();
        }

        if (GUILayout.Button("PrintBones"))
        {
            Target.PrintBones();
        }

        if (GUILayout.Button("TestAvatar"))
        {
            Target.TestAvatar();
        }

        if (GUILayout.Button("Debug Openpose mapping"))
        {
            Target.DebugOpenposeMapping();
        }

        if (GUILayout.Button("RevertDebug"))
        {
            Target.RevertDebug();
        }

        if (GUILayout.Button("SetBodyPoseByOpenposeJson"))
        {
            Target.SetBodyPoseByOpenposeJson();
        }
    }
}