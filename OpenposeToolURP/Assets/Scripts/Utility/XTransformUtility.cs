using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XUtility
{
    public static class XTransformUtility
    {
        public static Vector3 PosFromMat4(in Matrix4x4 m)
        {
            float x = m.m03;
            float y = m.m13;
            float z = m.m23;
            return new Vector3(x, y, z);
        }

        public static Quaternion RotFromMat4(in Matrix4x4 matrix)
        {
            Vector3 forward;
            forward.x = matrix.m02;
            forward.y = matrix.m12;
            forward.z = matrix.m22;

            Vector3 upwards;
            upwards.x = matrix.m01;
            upwards.y = matrix.m11;
            upwards.z = matrix.m21;

            return Quaternion.LookRotation(forward, upwards).normalized;
        }

        public static Vector3 ScaleFromMat4(in Matrix4x4 matrix)
        {
            Vector3 scale;
            scale.x = new Vector3(matrix.m00, matrix.m01, matrix.m02).magnitude;
            scale.y = new Vector3(matrix.m10, matrix.m11, matrix.m12).magnitude;
            scale.z = new Vector3(matrix.m20, matrix.m21, matrix.m22).magnitude;
            return scale;
        }

        public static void SetTransformByMatrix(Transform trans, in Matrix4x4 matrix)
        {
            trans.position = PosFromMat4(matrix);
            trans.rotation = RotFromMat4(matrix);
            trans.localScale = ScaleFromMat4(matrix);
        }
    }

}