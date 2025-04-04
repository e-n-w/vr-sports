using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadUVMap : MonoBehaviour
{
    public float textureScaleU;
    public float textureScaleV;

    void Start()
    {
        Mesh quadMesh = gameObject.GetComponent<MeshFilter>().mesh;
        if (quadMesh != null)
        {
            Vector2[] originalUVs = quadMesh.uv;

            Vector2[] newUVs = new Vector2[originalUVs.Length];
            originalUVs.CopyTo(newUVs, 0);

            newUVs[0] = new Vector2(0, 0);
            newUVs[1] = new Vector2(textureScaleU, 0);
            newUVs[2] = new Vector2(0, textureScaleV);
            newUVs[3] = new Vector2(textureScaleU, textureScaleV);

            quadMesh.uv = newUVs;
        }
    }
}