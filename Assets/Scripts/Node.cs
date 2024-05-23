using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[Serializable]
public class CubeArray
{
    public GameObject[,,] cubes;
}*/

public class Node : MonoBehaviour
{
    public int side = 4;
    int numX;
    int numY;
    int numZ;

    public GameObject[,,] cubes;

    public GameObject[,,] leftCubes;
    public GameObject[,,] rightCubes;
    public GameObject[,,] topCubes;
    public GameObject[,,] bottomCubes;

    public int left = 4;
    public int right = 1;
    public int top = 4;
    public int bottom = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        numX = side;
        numY = side;
        numZ = side;

        cubes = new GameObject[numX, numY, numZ];
        leftCubes = new GameObject[numX, numY, numZ];
        rightCubes = new GameObject[numX, numY, numZ];
        topCubes = new GameObject[numX, numY, numZ];
        bottomCubes = new GameObject[numX, numY, numZ];

        for (int x = 0; x < numX; x++)
        {
            for (int y = 0; y < numY; y++)
            {
                for (int z = 0; z < numZ; z++)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.GetComponent<Renderer>().material.color = Color.green;
                    cube.transform.localScale = new Vector3(1.0f / numX, 1.0f / numY, 1.0f / numZ);
                    cube.transform.position = new Vector3(x * 1.0f / numX + (1.0f / numX / 2.0f), 
                                                          y * 1.0f / numY + (1.0f / numY / 2.0f), 
                                                          z * 1.0f / numZ + (1.0f / numZ / 2.0f));
                    cubes[x, y, z] = cube;
                }
            }
        }

        for (int x = 0; x < numX; x++)
        {
            for (int y = 0; y < numY; y++)
            {
                for (int z = 0; z < numZ; z++)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.GetComponent<Renderer>().material.color = Color.blue;
                    cube.transform.localScale = new Vector3(1.0f / numX, 1.0f / numY, 1.0f / numZ);
                    cube.transform.position = new Vector3(x * 1.0f / numX + (1.0f / numX / 2.0f) - 1.0f,
                                                          y * 1.0f / numY + (1.0f / numY / 2.0f),
                                                          z * 1.0f / numZ + (1.0f / numZ / 2.0f));
                    leftCubes[x, y, z] = cube;
                }
            }
        }

        for (int x = 0; x < numX; x++)
        {
            for (int y = 0; y < numY; y++)
            {
                for (int z = 0; z < numZ; z++)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.GetComponent<Renderer>().material.color = Color.blue;
                    cube.transform.localScale = new Vector3(1.0f / numX, 1.0f / numY, 1.0f / numZ);
                    cube.transform.position = new Vector3(x * 1.0f / numX + (1.0f / numX / 2.0f) + 1.0f,
                                                          y * 1.0f / numY + (1.0f / numY / 2.0f),
                                                          z * 1.0f / numZ + (1.0f / numZ / 2.0f));
                    rightCubes[x, y, z] = cube;
                }
            }
        }

        for (int x = 0; x < numX; x++)
        {
            for (int y = 0; y < numY; y++)
            {
                for (int z = 0; z < numZ; z++)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.GetComponent<Renderer>().material.color = Color.blue;
                    cube.transform.localScale = new Vector3(1.0f / numX, 1.0f / numY, 1.0f / numZ);
                    cube.transform.position = new Vector3(x * 1.0f / numX + (1.0f / numX / 2.0f),
                                                          y * 1.0f / numY + (1.0f / numY / 2.0f),
                                                          z * 1.0f / numZ + (1.0f / numZ / 2.0f) + 1.0f);
                    topCubes[x, y, z] = cube;
                }
            }
        }

        for (int x = 0; x < numX; x++)
        {
            for (int y = 0; y < numY; y++)
            {
                for (int z = 0; z < numZ; z++)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.GetComponent<Renderer>().material.color = Color.blue;
                    cube.transform.localScale = new Vector3(1.0f / numX, 1.0f / numY, 1.0f / numZ);
                    cube.transform.position = new Vector3(x * 1.0f / numX + (1.0f / numX / 2.0f),
                                                          y * 1.0f / numY + (1.0f / numY / 2.0f),
                                                          z * 1.0f / numZ + (1.0f / numZ / 2.0f) - 1.0f);
                    bottomCubes[x, y, z] = cube;
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        /*for ( int x = 0; x < numX; x++)
        {
            for (int z = 0; z < numZ; z++)
            {

            }
        }*/

        for (int x = 0; x < numX; x++)
        {
            for (int z = 0; z < numZ; z++)
            {
                for (int y = 0; y < numY; y++)
                {
                    float currSpot = (float) x / numX;
                    float yValue = Mathf.Lerp(left, right, currSpot);
                    float currSpot2 = (float) z / numZ;
                    float yValue2 = Mathf.Lerp(bottom, top, currSpot2);
                    if (y < yValue && y < yValue2)
                    {
                        cubes[x, y, z].SetActive(true);
                    }
                    else
                    {
                        cubes[x, y, z].SetActive(false);
                    }
                }
            } 
        }

        for (int x = 0; x < numX; x++)
        {
            for (int z = 0; z < numZ; z++)
            {
                for (int y = 0; y < numY; y++)
                {
                    if (y < left)
                    {
                        leftCubes[x, y, z].SetActive(true);
                    }
                    else
                    {
                        leftCubes[x, y, z].SetActive(false);
                    }

                    if (y < right)
                    {
                        rightCubes[x, y, z].SetActive(true);
                    }
                    else
                    {
                        rightCubes[x, y, z].SetActive(false);
                    }

                    if (y < top)
                    {
                        topCubes[x, y, z].SetActive(true);
                    }
                    else
                    {
                        topCubes[x, y, z].SetActive(false);
                    }

                    if (y < bottom)
                    {
                        bottomCubes[x, y, z].SetActive(true);
                    }
                    else
                    {
                        bottomCubes[x, y, z].SetActive(false);
                    }
                }
            }
        }
    }
}
