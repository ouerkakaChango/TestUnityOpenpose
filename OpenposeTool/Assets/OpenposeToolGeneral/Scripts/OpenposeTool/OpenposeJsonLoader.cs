using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MathHelper.Vec;
using static MathHelper.XMathFunc;

[System.Serializable]
public class OpenposeJsonPersonData
{
    public List<float> pose_keypoints_2d;

}

[System.Serializable]
public class OpenposeJsonRootData
{
    public string version;
    public List<OpenposeJsonPersonData> people;
}

public class OpenposeJsonLoader : MonoBehaviour
{
    public TextAsset json;
    public OpenposeJsonRootData rootData;
    public bool hasPased = false;
    public bool editorShowPoseKeypoints = false;
    public Vector2 picSize = new Vector2(512.0f, 512.0f);
    public Vector2 worldScale = new Vector2(10.0f, 10.0f);

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
        if (hasPased)
        {
            if (editorShowPoseKeypoints && rootData != null && rootData.people != null & rootData.people.Count == 1 &&
                rootData.people[0].pose_keypoints_2d != null &&
                rootData.people[0].pose_keypoints_2d.Count == 75)
            {
                //Debug.Log("drawing");
                //Gizmos.DrawSphere()
                var data = rootData.people[0].pose_keypoints_2d;
                for (int i = 0; i < 25; i++)
                {
                    int inx = i * 3;
                    Vector2 pixPos = new Vector2(data[inx], data[inx + 1]);
                    Vector2 pos2D = InverseV(Divide(pixPos, picSize)) * worldScale;
                    Vector3 pos = new Vector3(pos2D.x, pos2D.y, transform.position.z);
                    Gizmos.color = Color.Lerp(new Color(1,0,0,1.0f), new Color(0,1,0,1.0f), data[inx + 2]);
                    Gizmos.DrawSphere(pos, 0.2f);
                    UnityEditor.Handles.Label(pos+new Vector3(0.0f, -0.25f, 0.0f), i.ToString());
                    //if(i==24)
                    //{
                    //    Debug.Log(pixPos.x + " " + pixPos.y);
                    //}
                }
            }
        }

    }

    //########################################################
    public void Parse()
    {
        rootData = JsonUtility.FromJson<OpenposeJsonRootData>(json.text);
        Debug.Log(rootData.people[0].pose_keypoints_2d.Count);
        hasPased = true;
    }

    public void EditorShowPoseKeypoints()
    {
        editorShowPoseKeypoints = true;
    }
    //############################################################
    public Vector2 InverseV(Vector2 uv)
    {
        return new Vector2(uv.x, saturate(1 - uv.y));
    }
}
