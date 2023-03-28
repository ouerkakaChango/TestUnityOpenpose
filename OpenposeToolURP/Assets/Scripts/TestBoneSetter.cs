using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static XUtility.XTransformUtility;

public enum HumanoidDefaltDir
{
    up,
    nega_up,
    fwd,
    nega_fwd,
    right,
    nega_right,
}

public class TestBoneSetter : MonoBehaviour
{
    public SkinnedMeshRenderer skin;
    public Avatar avatar;
    public int testOpenposeBodyID;
    public static Dictionary<int, string> openposeBodyMapping = new Dictionary<int, string>();
    public static Dictionary<int, HumanoidDefaltDir> openposeHumanoidDirMapping = new Dictionary<int, HumanoidDefaltDir>();
    public static Dictionary<int, Dictionary<int, HumanoidDefaltDir>> openposeHumanoidLegionDirMapping = new Dictionary<int, Dictionary<int, HumanoidDefaltDir>>();
    public OpenposeJsonLoader openposeJsonLoader;

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
    public static void InitOpenposeMapping()
    {
        openposeBodyMapping = new Dictionary<int, string>();
        openposeHumanoidDirMapping = new Dictionary<int, HumanoidDefaltDir>();
        openposeHumanoidLegionDirMapping = new Dictionary<int, Dictionary<int, HumanoidDefaltDir>>();

        openposeHumanoidLegionDirMapping.Add(1, new Dictionary<int, HumanoidDefaltDir>());

        openposeBodyMapping.Add(0,"");//nose
        openposeBodyMapping.Add(1, "Neck"); openposeHumanoidLegionDirMapping[1].Add(2, HumanoidDefaltDir.right); openposeHumanoidLegionDirMapping[1].Add(5, HumanoidDefaltDir.nega_right);

        openposeBodyMapping.Add(2, "RightUpperArm");
        openposeBodyMapping.Add(3, "RightLowerArm");
        openposeBodyMapping.Add(4, "RightHand");

        openposeBodyMapping.Add(5, "LeftUpperArm");
        openposeBodyMapping.Add(6, "LeftLowerArm");
        openposeBodyMapping.Add(7, "LeftHand");

        openposeBodyMapping.Add(8, "Hips"); openposeHumanoidDirMapping.Add(8, HumanoidDefaltDir.up);

        openposeBodyMapping.Add(9, "RightUpperLeg"); openposeHumanoidDirMapping.Add(9, HumanoidDefaltDir.nega_up);
        openposeBodyMapping.Add(10, "RightLowerLeg"); openposeHumanoidDirMapping.Add(10, HumanoidDefaltDir.nega_up);
        openposeBodyMapping.Add(11, "RightFoot"); openposeHumanoidDirMapping.Add(11, HumanoidDefaltDir.fwd);

        openposeBodyMapping.Add(12, "LeftUpperLeg"); openposeHumanoidDirMapping.Add(12, HumanoidDefaltDir.up);
        openposeBodyMapping.Add(13, "LeftLowerLeg"); openposeHumanoidDirMapping.Add(13, HumanoidDefaltDir.up);
        openposeBodyMapping.Add(14, "LeftFoot"); openposeHumanoidDirMapping.Add(14, HumanoidDefaltDir.nega_fwd);

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
        int skinBoneInx = -1;
        for(int i=0;i<skin.bones.Length;i++)
        {
            if(skin.bones[i].name == boneName)
            {
                Debug.Log("bone found in skin, showing position! ");
                testBonePos = skin.bones[i].position;
                bShowTestBonePos = true;
                skinBoneInx = i;
                break;
            }
        }

        if(skinBoneInx==-1)
        {
            Debug.LogError("bone not found in skin,normally it won't happen");
            return;
        }

        //Test Set Bone Position 
        skin.bones[skinBoneInx].position = Vector3.zero;
    }

    public void RevertDebug()
    {
        var oriMatrix = transform.localToWorldMatrix;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        for(int i=0;i<skin.bones.Length;i++)
        {
            SetTransformByMatrix(skin.bones[i], skin.sharedMesh.bindposes[i].inverse);
        }

        SetTransformByMatrix(transform, oriMatrix);
    }

    public void SetBodyPoseByOpenposeJson(int loopnum=2)
    {
        var pv = GetComponent<PntsVisualizer>();

        //simple set
        //float scaleX = transform.localScale.x;
        //for(int i=0;i<25;i++)
        //{
        //    int boneInx = GetBoneInxByOpenID(i,avatar,skin);
        //    if(boneInx != -1)
        //    {
        //        Vector2 uvPos;
        //        float trust;
        //        openposeJsonLoader.GetBodyKey2D(i, out uvPos, out trust);
        //        if(trust<0.001)
        //        {
        //            Debug.Log("openID " + i + " not trust");
        //            return;
        //        }
        //        skin.bones[boneInx].position = transform.position + scaleX*( - transform.right * uvPos.x + transform.up * uvPos.y);
        //        pv.Add(skin.bones[boneInx].position);
        //        //skin.bones[inx] = openposeJsonLoader.;
        //    }
        //    else
        //    {
        //        //Debug.Log("openID " + i + " not found");
        //    }
        //}

        //change rot since hip 力发于胯
        //Note: cannot done automaticlly,for changing pos/scale would be unaccepetable
        //---$$$
        Vector3 dir =Vector3.zero;
        TryMapSetRotation(12, 13);
        TryMapSetRotation(13, 14);
        TryMapSetRotation(14, 19);//foot-toe

        TryMapSetRotation(9, 10);
        TryMapSetRotation(10, 11);
        TryMapSetRotation(11, 22);//foot-toe

        TryMapSetRotation(8, 1);

        //(1, 2);
        //有shoulder，所以特殊一点，量的是neck to arm,修改的是shoulder，和之前的逻辑还不太一样
        //否则应该和之前一样TryMapSetRotation(1,2); neck->upperArm
        //TryMapSetLegionPointRotation(1,2,GetBoneInxByName("rightShould"))

        //TryMapSetRotation(1, 5);

        //___

        if (loopnum == 1)
        {
            return;
        }
        //again to make sure all updated
        SetBodyPoseByOpenposeJson(1);
    }
    //#######################################################
    void TryMapSetRotation(int openID1,int openID2)
    {
        Vector3 dir = Vector3.zero;
        if (GetWorldDirBetween(openID1, openID2, out dir))
        {
            int boneInx1 = GetBoneInxByOpenID(openID1, avatar, skin);
            int boneInx2 = GetBoneInxByOpenID(openID2, avatar, skin);
            if (boneInx1 == -1 || boneInx2 == -1)
            {
            }
            else
            {
                SetHumanoidDir(openID1, boneInx1, dir);
            }
        }
    }

    void TryMapSetLegionPointRotation(int openID1, int openID2, int realBone)
    {
        Vector3 dir = Vector3.zero;
        if (GetWorldDirBetween(openID1, openID2, out dir))
        {
            int boneInx1 = GetBoneInxByOpenID(openID1, avatar, skin);
            int boneInx2 = GetBoneInxByOpenID(openID2, avatar, skin);
            if (boneInx1 == -1 || boneInx2 == -1)
            {
            }
            else
            {
                SetLegionHumanoidDir(openID1, realBone, dir, openID2);
            }
        }
    }

    void SetHumanoidDir(int openID1, int boneInx1, Vector3 dir)
    {
        Transform trans = skin.bones[boneInx1].transform;
        //Prepare
        if (openID1 == 14)
        {//foot to toe
            dir = Vector3.Lerp(dir, -trans.forward, 0.5f);
        }
        //Do
        if(openposeHumanoidDirMapping[openID1] == HumanoidDefaltDir.up)
        {
            var oriRight = trans.right;
            trans.up = dir;
            if (Vector3.Dot(oriRight, trans.right) < 0)
            {//hand fix rotation inverse change
                trans.rotation = trans.rotation * Quaternion.Euler(0, 180, 0);
            }
        }
        else if (openposeHumanoidDirMapping[openID1] == HumanoidDefaltDir.nega_up)
        {
            var oriRight = trans.right;
            trans.up = -dir;
            if (Vector3.Dot(oriRight, trans.right) < 0)
            {//hand fix rotation inverse change
                trans.rotation = trans.rotation * Quaternion.Euler(0, 180, 0);
            }
        }
        else if (openposeHumanoidDirMapping[openID1] == HumanoidDefaltDir.fwd)
        {
            var oriUp = trans.up;
            trans.forward = dir;
            if (Vector3.Dot(oriUp, trans.up) < 0)
            {//hand fix rotation inverse change
                trans.rotation = trans.rotation * Quaternion.Euler(0, 0, 180);
            }
        }
        else if(openposeHumanoidDirMapping[openID1] == HumanoidDefaltDir.nega_fwd)
        {
            var oriUp = trans.up;
            trans.forward = -dir;
            if(Vector3.Dot(oriUp, trans.up)<0)
            {//hand fix rotation inverse change
                trans.rotation = trans.rotation * Quaternion.Euler(0, 0, 180);
            }
        }
    }

    void SetLegionHumanoidDir(int openID1, int boneInx1, Vector3 dir, int openID2)
    {
        Transform trans = skin.bones[boneInx1].transform;
        //Prepare
        if (openID1 == 14)
        {//foot to toe
            dir = Vector3.Lerp(dir, -trans.forward, 0.5f);
        }
        //Do
        if(openposeHumanoidLegionDirMapping[openID1][openID2] == HumanoidDefaltDir.right)
        {
            //...
        }
        else if(openposeHumanoidLegionDirMapping[openID1][openID2] == HumanoidDefaltDir.nega_right)
        {
            //...
        }
        //if (openposeHumanoidDirMapping[openID1] == HumanoidDefaltDir.up)
        //{
        //    var oriRight = trans.right;
        //    trans.up = dir;
        //    if (Vector3.Dot(oriRight, trans.right) < 0)
        //    {//hand fix rotation inverse change
        //        trans.rotation = trans.rotation * Quaternion.Euler(0, 180, 0);
        //    }
        //}
        //else if (openposeHumanoidDirMapping[openID1] == HumanoidDefaltDir.nega_up)
        //{
        //    var oriRight = trans.right;
        //    trans.up = -dir;
        //    if (Vector3.Dot(oriRight, trans.right) < 0)
        //    {//hand fix rotation inverse change
        //        trans.rotation = trans.rotation * Quaternion.Euler(0, 180, 0);
        //    }
        //}
        //else if (openposeHumanoidDirMapping[openID1] == HumanoidDefaltDir.fwd)
        //{
        //    var oriUp = trans.up;
        //    trans.forward = dir;
        //    if (Vector3.Dot(oriUp, trans.up) < 0)
        //    {//hand fix rotation inverse change
        //        trans.rotation = trans.rotation * Quaternion.Euler(0, 0, 180);
        //    }
        //}
        //else if (openposeHumanoidDirMapping[openID1] == HumanoidDefaltDir.nega_fwd)
        //{
        //    var oriUp = trans.up;
        //    trans.forward = -dir;
        //    if (Vector3.Dot(oriUp, trans.up) < 0)
        //    {//hand fix rotation inverse change
        //        trans.rotation = trans.rotation * Quaternion.Euler(0, 0, 180);
        //    }
        //}
    }

    Vector3 worldPosFromUVPos(Vector2 uvPos)
    {
        //!!! approximately pos,may far from original pos;only keep relative relations and world dir rightness
        return transform.position + transform.localScale.x * (-transform.right * uvPos.x + transform.up * uvPos.y);
    }

    bool GetWorldDirBetween(int openID1,int openID2, out Vector3 dir)
    {
        dir = Vector3.zero;
        Vector2 uvPos1, uvPos2;
        float trust1,trust2;
        openposeJsonLoader.GetBodyKey2D(openID1, out uvPos1, out trust1);
        if (trust1 < 0.001)
        {
            return false;
        }
        openposeJsonLoader.GetBodyKey2D(openID2, out uvPos2, out trust2);
        if (trust2 < 0.001)
        {
            return false;
        }
        Vector3 p1 = worldPosFromUVPos(uvPos1);
        Vector3 p2 = worldPosFromUVPos(uvPos2);
        dir = (p2 - p1).normalized;
        return true;
    }

    public static string GetBoneNameFromHumanName(string humanName, HumanDescription desc)
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

    public static int GetBoneInxByOpenID(int openID, Avatar avatar, SkinnedMeshRenderer skin)
    {
        if (openposeBodyMapping.Count != 25)
        {
            InitOpenposeMapping();
        }
        string humanName = openposeBodyMapping[openID];
        if (humanName == "")
        {
            //Debug.Log("No mapping for this openpose ID in UnityHumanoid");
            return -1;
        }
        HumanDescription humanDesc = avatar.humanDescription;
        string boneName = GetBoneNameFromHumanName(humanName, humanDesc);
        if (boneName == "")
        {
            //Debug.Log("No found target bone");
            return -1;
        }

        //Debug.Log("bone found: " + boneName);

        for (int i = 0; i < skin.bones.Length; i++)
        {
            if (skin.bones[i].name == boneName)
            {
                //Debug.Log("bone found in skin, showing position! ");
                return i;
            }
        }

        return -1;
        //if (skinBoneInx == -1)
        //{
        //    Debug.LogError("bone not found in skin,normally it won't happen");
        //    return;
        //}
    }

    //Mapping from Openpose-BodyKeypoints ---> HumanName ---> BoneName
}
