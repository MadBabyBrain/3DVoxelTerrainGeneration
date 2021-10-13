using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    public int width, depth, height;
    [Range(0.0001f, 1)]
    public float noisescale;
    [Range(0f, 1f)]
    public float threshold;

    public int[,,] ground;
    public CombineInstance[] combine;


    // Start is called before the first frame update
    void Start()
    {
        Draw();
    }

    public void Create()
    {
        Clear();
        Draw();
    }

    public void Clear()
    {
        GameObject[] children = new GameObject[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            children[i] = child.gameObject;
            i++;
        }
        foreach (GameObject child in children)
        {
            DestroyImmediate(child.gameObject);
        }
        gameObject.GetComponent<MeshFilter>().mesh = null;
    }

    private void Draw()
    {
        ground = new int[width + 1, height + 1, depth + 1];
        int x, y, z, i = 0;
        for (x = 0; x <= width; x++)
        {
            for (y = 0; y <= height; y++)
            {
                for (z = 0; z <= depth; z++)
                {
                    float xy = Mathf.PerlinNoise(x * noisescale, y * noisescale);
                    float yx = Mathf.PerlinNoise(y * noisescale, x * noisescale);

                    float xz = Mathf.PerlinNoise(x * noisescale, z * noisescale);
                    float zx = Mathf.PerlinNoise(z * noisescale, x * noisescale);

                    float yz = Mathf.PerlinNoise(y * noisescale, z * noisescale);
                    float zy = Mathf.PerlinNoise(z * noisescale, y * noisescale);

                    float a = Mathf.Lerp(0, 1, (xy + yx + xz + zx + yz + zy) / 6f);
                    // Debug.Log(a);

                    if (a >= threshold)
                    {
                        ground[x, y, z] = 1;
                        GameObject newobj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        newobj.transform.position = new Vector3(x, y, z);
                        newobj.transform.parent = gameObject.transform;
                        i++;
                    }
                }
            }
        }
        combine = new CombineInstance[i];
        i = 0;
        foreach (Transform child in transform)
        {
            combine[i].mesh = child.gameObject.GetComponent<MeshFilter>().sharedMesh;
            combine[i].transform = child.localToWorldMatrix;
            child.gameObject.SetActive(false);
            i++;
        }

        transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        transform.GetComponent<MeshFilter>().sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
        transform.gameObject.SetActive(true);

    }
}
