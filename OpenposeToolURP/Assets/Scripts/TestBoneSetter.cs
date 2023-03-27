using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoneSetter : MonoBehaviour
{
    public SkinnedMeshRenderer skin;
    public Avatar avatar;
    public int testOpenposeBodyID;
    public Dictionary<int, string> openposeBodyMapping = new Dictionary<int, string>();
    Vector3 testBonePos;
    bool bShowTestBonePos=false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if(bShowTestBonePos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(testBonePos, 0.4f*Vector3.one);
            //Gizmos.DrawSphere(testBonePos, 0.4f);
        }
    }

    //#########################################################

    public void PrintBones()
    {
        if(skin==null)
        {
            return;
        }
        Debug.Log(skin.bones.Length);
        string boneNames = "";
        for (int i=0;i<skin.bones.Length;i++)
        {
            boneNames += skin.bones[i].name+"\n";
        }
        Debug.Log(boneNames);
    }

    public void TestAvatar()
    {
        HumanBone[] mappingArr = avatar.humanDescription.human;
        Debug.Log("mapping " + mappingArr.Length);
        string humanNames = "";
        for(int i=0;i<mappingArr.Length;i++)
        {
            humanNames += mappingArr[i].humanName + "\n";
        }
        Debug.Log(humanNames);

        string boneNames = "";
        for(int i=0;i<mappingArr.Length;i++)
        {
            boneNames += mappingArr[i].boneName + "\n";
        }
        Debug.Log(boneNames);
    }

    //https://cmu-perceptual-computing-lab.github.io/openpose/web/html/doc/md_doc_02_output.html
    public void InitOpenposeMapping()
    {
        openposeBodyMapping = new Dictionary<int, string>();
        openposeBodyMapping.Add(0,"");//nose
        openposeBodyMapping.Add(1, "Neck");

        openposeBodyMapping.Add(2, "RightUpperArm");
        openposeBodyMapping.Add(3, "RightLowerArm");
        openposeBodyMapping.Add(4, "RightHand");

        openposeBodyMapping.Add(5, "LeftUpperArm");
        openposeBodyMapping.Add(6, "LeftLowerArm");
        openposeBodyMapping.Add(7, "LeftHand");

        openposeBodyMapping.Add(8, "Hips");

        openposeBodyMapping.Add(9, "RightUpperLeg");
        openposeBodyMapping.Add(10, "RightLowerLeg");
        openposeBodyMapping.Add(11, "RightFoot");

        openposeBodyMapping.Add(12, "LeftUpperLeg");
        openposeBodyMapping.Add(13, "LeftLowerLeg");
        openposeBodyMapping.Add(14, "LeftFoot");

        openposeBodyMapping.Add(15, "RightEye");
        openposeBodyMapping.Add(16, "LeftEye");

        openposeBodyMapping.Add(17, "");//left ear
        openposeBodyMapping.Add(18, "");//right ear

        openposeBodyMapping.Add(19, "LeftToes");
        openposeBodyMapping.Add(20, "");//"LSmallToe"
        openposeBodyMapping.Add(21, "");//"LHeel"

        openposeBodyMapping.Add(22, "RightToes");
        openposeBodyMapping.Add(23, "");//"RSmallToe"
        openposeBodyMapping.Add(24, "");//"RHeel"

    }

    public void DebugOpenposeMapping()
    {
        if (openposeBodyMapping.Count != 25)
        {
            InitOpenposeMapping();
        }
        string humanName = openposeBodyMapping[testOpenposeBodyID];
        Debug.Log(humanName);
        if(humanName=="")
        {
            Debug.Log("No mapping for this openpose ID in UnityHumanoid");
            return;
        }
        HumanDescription humanDesc = avatar.humanDescription;
        string boneName = GetBoneNameFromHumanName(humanName, humanDesc);
        if(boneName == "")
        {
            Debug.Log("No found target bone");
            return;
        }

        Debug.Log("bone found: " + boneName);
        for(int i=0;i<skin.bones.Length;i++)
        {
            if(skin.bones[i].name == boneName)
            {
                Debug.Log("bone found in skin, showing position! ");
                testBonePos = skin.bones[i].position;
                bShowTestBonePos = true;
                break;
            }
        }
    }

    string GetBoneNameFromHumanName(string humanName, HumanDescription desc)
    {
        for(int i=0;i<desc.human.Length;i++)
        {
            if(desc.human[i].humanName == humanName)
            {
                return desc.human[i].boneName;
            }
        }
        return "";
    }

    //Mapping from Openpose-BodyKeypoints ---> HumanName ---> BoneName
}
