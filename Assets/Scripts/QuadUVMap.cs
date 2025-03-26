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
            // Get the mesh data
            Vector2[] originalUVs = quadMesh.uv;
            foreach (Vector2 uv in originalUVs)
            {
                Debug.Log(uv.x + " " + uv.y);
            }

            // Create a new UV array (optional, but good practice to avoid modifying the original)
            Vector2[] newUVs = new Vector2[originalUVs.Length];
            originalUVs.CopyTo(newUVs, 0);

            // Modify the UVs for the quad's vertices
            // Example: Map the texture to a specific area (e.g., 0.25 to 0.75 in both x and y)
            newUVs[0] = new Vector2(0, 0); // Bottom-left
            newUVs[1] = new Vector2(textureScaleU, 0); // Bottom-right
            newUVs[2] = new Vector2(0, textureScaleV); // Top-left
            newUVs[3] = new Vector2(textureScaleU, textureScaleV); // Top-right

            // Assign the new UVs to the mesh
            quadMesh.uv = newUVs;
        }
    }
}