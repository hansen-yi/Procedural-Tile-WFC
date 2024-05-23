//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UIElements;

//public class TileNodeOld : MonoBehaviour
//{
//    public GameObject leftNode;
//    public GameObject rightNode;
//    public GameObject topNode;
//    public GameObject bottomNode;
//    public GameObject aboveNode;
//    public GameObject belowNode;

//    public float prevId = -1.0f;
//    public float id = -1.0f;

//    public GameObject currentTile;

//    public GameObject clearTile;
//    public GameObject testTile;
//    public GameObject highlight;

//    public Graph Global;
//    public Dictionary<(float, float, float), GameObject> graph;

//    public int tileSide;

//    public bool edge;

//    public float entropy;


//    // Start is called before the first frame update
//    void Start()
//    {
//        //currentTile = Instantiate(clearTile, this.transform);
//        Quaternion rot = Quaternion.identity;
//        currentTile = Instantiate(highlight, this.transform);
//        //Instantiate(highlight, this.transform.position, rot);

//        //Instantiate(highlight, this.transform.position + new Vector3(0, 1f, 0), rot, this.transform);
//        //rot.eulerAngles = new Vector3(0, 0, 90);
//        //Instantiate(highlight, this.transform.position + new Vector3(-0.5f, 0.5f, 0), rot, this.transform);
//        //Instantiate(highlight, this.transform.position + new Vector3(0.5f, 0.5f, 0), rot);
//        //rot.eulerAngles = new Vector3(90, 0, 0);
//        //Instantiate(highlight, this.transform.position + new Vector3(0, 0.5f, -0.5f), rot, this.transform);
//        //Instantiate(highlight, this.transform.position + new Vector3(0, 0.5f, 0.5f), rot, this.transform);

//        //currentTile = Instantiate(clearTile, this.transform.position + new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);
//        Global = GameObject.Find("Graph").GetComponent<Graph>();
//        graph = Global.nodes;
//        tileSide = Global.tileDim;
//        edge = false;
//        entropy = float.MaxValue;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        UpdateNeighbors();
//        if (prevId != id && id != -1.0f)
//        {
//            prevId = id;
//            Destroy(currentTile);
//            Debug.Log("new id:" + id);
//            if (id == 1.0)
//            {
//                //currentTile = Instantiate(testTile, this.transform);
//                createGrass(tileSide, tileSide, tileSide);
//            }
//            else if (id == 2.0)
//            {
//                createDirt(tileSide, tileSide, tileSide);
//            }
//            else if (id == 3.0)
//            {
//                createStone(tileSide, tileSide, tileSide);
//            }
//            else if (id == 4.0)
//            {
//                createWater(tileSide, tileSide, tileSide);
//            }
//            List<GameObject> cycle = new List<GameObject>();
//            bool foundCycle = cycleCheck(new List<GameObject>(), new Dictionary<GameObject, GameObject>(), cycle);
//            Debug.Log("cycle found: " + foundCycle);
//            string cycleString = "";
//            foreach (GameObject go in cycle)
//            {
//                cycleString += go.transform.position + "; ";
//            }
//            Debug.Log(cycleString);
//            if (foundCycle)
//            {
//                findInside(cycle);
//            }
//            updateCubes();
//        }

//    }

//    private void OnMouseOver()
//    {
//        if (id == -1.0f)
//        {
//            //currentTile.SetActive(true);
//        }
//        else
//        {
//            if (Input.GetMouseButtonDown(1))
//            {
//                id = -1.0f;
//                Debug.Log("right clicked");
//            }
//        }
//    }

//    private void OnMouseExit()
//    {
//        //currentTile.SetActive(false);
//    }

//    public void OnMouseDown()
//    {
//        Debug.Log("please");
//        if (id == -1.0f)
//        {
//            //UpdateNeighbors();
//            SpawnNewSpots();
//            Collapse();
//            //fill(new List<GameObject>(), 1.0f);
//        }
//        /*Destroy(currentTile);*/
//        /*Custom Global = GameObject.Find("Custom").GetComponent<Custom>();
//        Dictionary<(float, float, float), Cell> modifiedCells = Global.modifiedCells;
//        foreach (Cell modCell in modifiedCells.Values)
//        {
//            modCell.collapsed = false;
//        }
//        this.starting = true;
//        Collapse();*/
//        /*currentTile = Instantiate(testTile, this.transform.position - new Vector3(0, 1.0f / 3.0f, 0), testTile.transform.rotation);*/
//    }

//    public void SpawnNewSpots()
//    {
//        (float, float, float) coordinate = (this.transform.position.x, this.transform.position.y, this.transform.position.z);
//        (float, float, float) leftCoord = (coordinate.Item1 - 1.0f, coordinate.Item2, coordinate.Item3);
//        (float, float, float) rightCoord = (coordinate.Item1 + 1.0f, coordinate.Item2, coordinate.Item3);
//        (float, float, float) bottomCoord = (coordinate.Item1, coordinate.Item2, coordinate.Item3 - 1.0f);
//        (float, float, float) topCoord = (coordinate.Item1, coordinate.Item2, coordinate.Item3 + 1.0f);
//        (float, float, float) belowCoord = (coordinate.Item1, coordinate.Item2 - 1.0f, coordinate.Item3);
//        (float, float, float) aboveCoord = (coordinate.Item1, coordinate.Item2 + 1.0f, coordinate.Item3);

//        if (!graph.ContainsKey(leftCoord))
//        {
//            leftNode = Global.createNode(new Vector3(leftCoord.Item1, leftCoord.Item2, leftCoord.Item3));
//            leftNode.GetComponent<TileNode>().rightNode = gameObject;
//            //leftNode.GetComponent<TileNode>().UpdateNeighbors();
//        }
//        //else if (graph.ContainsKey(leftCoord) && left)
//        //{
//        //    leftNode = Global.createNode(new Vector3(leftCoord.Item1, leftCoord.Item2, leftCoord.Item3));
//        //    leftNode.GetComponent<TileNode>().rightNode = gameObject;
//        //}
//        if (!graph.ContainsKey(rightCoord))
//        {
//            rightNode = Global.createNode(new Vector3(rightCoord.Item1, rightCoord.Item2, rightCoord.Item3));
//            rightNode.GetComponent<TileNode>().leftNode = gameObject;
//            //rightNode.GetComponent<TileNode>().UpdateNeighbors();
//        }
//        if (!graph.ContainsKey(bottomCoord))
//        {
//            bottomNode = Global.createNode(new Vector3(bottomCoord.Item1, bottomCoord.Item2, bottomCoord.Item3));
//            bottomNode.GetComponent<TileNode>().topNode = gameObject;
//            //bottomNode.GetComponent<TileNode>().UpdateNeighbors();
//        }
//        if (!graph.ContainsKey(topCoord))
//        {
//            topNode = Global.createNode(new Vector3(topCoord.Item1, topCoord.Item2, topCoord.Item3));
//            topNode.GetComponent<TileNode>().bottomNode = gameObject;
//            //topNode.GetComponent<TileNode>().UpdateNeighbors();
//        }
//        if (!graph.ContainsKey(belowCoord) && coordinate.Item2 != 0)
//        {
//            belowNode = Global.createNode(new Vector3(belowCoord.Item1, belowCoord.Item2, belowCoord.Item3));
//            belowNode.GetComponent<TileNode>().aboveNode = gameObject;
//            //belowNode.GetComponent<TileNode>().UpdateNeighbors();
//        }
//        if (!graph.ContainsKey(aboveCoord))
//        {
//            aboveNode = Global.createNode(new Vector3(aboveCoord.Item1, aboveCoord.Item2, aboveCoord.Item3));
//            aboveNode.GetComponent<TileNode>().belowNode = gameObject;
//            //aboveNode.GetComponent<TileNode>().UpdateNeighbors();
//        }
//    }

//    public void UpdateNeighbors()
//    {
//        (float, float, float) coordinate = (this.transform.position.x, this.transform.position.y, this.transform.position.z);

//        (float, float, float) leftCoord = (coordinate.Item1 - 1.0f, coordinate.Item2, coordinate.Item3);
//        (float, float, float) rightCoord = (coordinate.Item1 + 1.0f, coordinate.Item2, coordinate.Item3);
//        (float, float, float) bottomCoord = (coordinate.Item1, coordinate.Item2, coordinate.Item3 - 1.0f);
//        (float, float, float) topCoord = (coordinate.Item1, coordinate.Item2, coordinate.Item3 + 1.0f);
//        (float, float, float) belowCoord = (coordinate.Item1, coordinate.Item2 - 1.0f, coordinate.Item3);
//        (float, float, float) aboveCoord = (coordinate.Item1, coordinate.Item2 + 1.0f, coordinate.Item3);

//        if (graph.ContainsKey(leftCoord) && leftNode == null)
//        {
//            leftNode = graph[leftCoord];
//        }
//        if (graph.ContainsKey(rightCoord) && rightNode == null)
//        {
//            rightNode = graph[rightCoord];
//        }
//        if (graph.ContainsKey(bottomCoord) && bottomNode == null)
//        {
//            bottomNode = graph[bottomCoord];
//        }
//        if (graph.ContainsKey(topCoord) && topNode == null)
//        {
//            topNode = graph[topCoord];
//        }
//        if (graph.ContainsKey(belowCoord) && belowNode == null)
//        {
//            belowNode = graph[belowCoord];
//        }
//        if (graph.ContainsKey(aboveCoord) && aboveNode == null)
//        {
//            aboveNode = graph[aboveCoord];
//        }
//    }

//    //public List<(float, float)> filterPossibilites(float leftID, float rightID, float bottomID, float topID, float belowID, float aboveID)
//    //{
//    //    bool nonClear = false;
//    //    if (leftID != -1.0f || rightID != -1.0f || bottomID != -1.0f || topID != -1.0f || belowID != -1.0f || aboveID != -1.0f)
//    //    {
//    //        nonClear = true;
//    //    }
//    //    List<(float, float)> leftPos = Global.possibleSideTiles[leftID];
//    //    List<(float, float)> rightPos = Global.possibleSideTiles[rightID];
//    //    List<(float, float)> topPos = Global.possibleSideTiles[topID];
//    //    List<(float, float)> bottomPos = Global.possibleSideTiles[bottomID];
//    //    //List<(float, float)> belowPos = Global.possibleSideTiles[belowID];
//    //    //List<(float, float)> abovePos = Global.possibleSideTiles[aboveID];

//    //    List<(float, float)> allPossibilies = combinePossibilities(leftPos, rightPos, bottomPos, topPos/*, belowPos, abovePos*/);
//    //    string log = "";
//    //    foreach (var pair in allPossibilies)
//    //    {
//    //        log += pair.Item1 + "=" + pair.Item2 + "; ";
//    //    }
//    //    Debug.Log(log);
//    //}

//    public List<(float, float)> combinePossibilities(params List<(float, float)>[] possibilities)
//    {
//        /*List<(float, float)> all = new List<(float, float)> ();
//        foreach (var list in possibilities)
//        {
//            foreach (var item in list)
//            {

//            }
//        }*/
//        int length = possibilities.Count(list => list.Count != 0);
//        Debug.Log("num not clear: " + length);
//        var all = possibilities
//            .SelectMany(list => list)
//            .GroupBy(tuple => tuple.Item1)
//            .Where(group => !group.Any(tuple => tuple.Item2 == 0))
//            .Select(group => (group.Key, group.Sum(tuple => tuple.Item2 / length)))
//            .ToList();
//        return all;
//    }

//    public float PickRandomID(List<(float, float)> possibilities)
//    {
//        float sum = possibilities.Sum(item => item.Item2);
//        float randomVal = Random.Range(0, sum);
//        float sumSoFar = 0.0f;

//        foreach (var item in possibilities)
//        {
//            sumSoFar += item.Item2;
//            if (randomVal < sumSoFar)
//            {
//                return item.Item1;
//            }
//        }

//        return possibilities[0].Item1; //this shouldn't happen
//    }

//    public void UpdateEntropy()
//    {
//        float leftID = (leftNode != null) ? leftNode.GetComponent<TileNode>().id : -1.0f;
//        float rightID = (rightNode != null) ? rightNode.GetComponent<TileNode>().id : -1.0f;
//        float bottomID = (bottomNode != null) ? bottomNode.GetComponent<TileNode>().id : -1.0f;
//        float topID = (topNode != null) ? topNode.GetComponent<TileNode>().id : -1.0f;
//        float belowID = (belowNode != null) ? belowNode.GetComponent<TileNode>().id : -1.0f;
//        float aboveID = (aboveNode != null) ? aboveNode.GetComponent<TileNode>().id : -1.0f;

//        bool nonClear = false;
//        if (leftID != -1.0f || rightID != -1.0f || bottomID != -1.0f || topID != -1.0f /*|| belowID != -1.0f || aboveID != -1.0f*/)
//        {
//            nonClear = true;
//        }
//        Debug.Log(nonClear);
//        List<(float, float)> leftPos = (!nonClear || (nonClear && leftID != -1.0f)) ? Global.possibleSideTiles[leftID] : new List<(float, float)>();
//        List<(float, float)> rightPos = (!nonClear || (nonClear && rightID != -1.0f)) ? Global.possibleSideTiles[rightID] : new List<(float, float)>();
//        List<(float, float)> topPos = (!nonClear || (nonClear && topID != -1.0f)) ? Global.possibleSideTiles[topID] : new List<(float, float)>();
//        List<(float, float)> bottomPos = (!nonClear || (nonClear && bottomID != -1.0f)) ? Global.possibleSideTiles[bottomID] : new List<(float, float)>();
//        List<(float, float)> belowPos = (!nonClear || (nonClear && belowID != -1.0f)) ? Global.possibleSideTiles[belowID] : new List<(float, float)>();
//        List<(float, float)> abovePos = (!nonClear || (nonClear && aboveID != -1.0f)) ? Global.possibleSideTiles[aboveID] : new List<(float, float)>();

//        List<(float, float)> allPossibilities = combinePossibilities(leftPos, rightPos, bottomPos, topPos/*, belowPos, abovePos*/);

//        float totalWeights = 0.0f;
//        float totalWeightedWeights = 0.0f;
//        foreach (var pair in allPossibilities)
//        {
//            totalWeights += pair.Item2;
//            totalWeightedWeights += (pair.Item2 * Mathf.Log(pair.Item2));
//        }

//        entropy = Mathf.Log(totalWeights) - (totalWeightedWeights / totalWeights);
//        Debug.Log("new entropy: " + entropy);
//    }

//    public void Collapse()
//    {
//        float leftID = (leftNode != null) ? leftNode.GetComponent<TileNode>().id : -1.0f;
//        float rightID = (rightNode != null) ? rightNode.GetComponent<TileNode>().id : -1.0f;
//        float bottomID = (bottomNode != null) ? bottomNode.GetComponent<TileNode>().id : -1.0f;
//        float topID = (topNode != null) ? topNode.GetComponent<TileNode>().id : -1.0f;
//        float belowID = (belowNode != null) ? belowNode.GetComponent<TileNode>().id : -1.0f;
//        float aboveID = (aboveNode != null) ? aboveNode.GetComponent<TileNode>().id : -1.0f;

//        bool nonClear = false;
//        if (leftID != -1.0f || rightID != -1.0f || bottomID != -1.0f || topID != -1.0f /*|| belowID != -1.0f || aboveID != -1.0f*/)
//        {
//            nonClear = true;
//        }
//        Debug.Log(nonClear);
//        List<(float, float)> leftPos = (!nonClear || (nonClear && leftID != -1.0f)) ? Global.possibleSideTiles[leftID] : new List<(float, float)>();
//        List<(float, float)> rightPos = (!nonClear || (nonClear && rightID != -1.0f)) ? Global.possibleSideTiles[rightID] : new List<(float, float)>();
//        List<(float, float)> topPos = (!nonClear || (nonClear && topID != -1.0f)) ? Global.possibleSideTiles[topID] : new List<(float, float)>();
//        List<(float, float)> bottomPos = (!nonClear || (nonClear && bottomID != -1.0f)) ? Global.possibleSideTiles[bottomID] : new List<(float, float)>();
//        List<(float, float)> belowPos = (!nonClear || (nonClear && belowID != -1.0f)) ? Global.possibleSideTiles[belowID] : new List<(float, float)>();
//        List<(float, float)> abovePos = (!nonClear || (nonClear && aboveID != -1.0f)) ? Global.possibleSideTiles[aboveID] : new List<(float, float)>();

//        List<(float, float)> allPossibilities = combinePossibilities(leftPos, rightPos, bottomPos, topPos/*, belowPos, abovePos*/);
//        string log = "All possibilities: ";
//        foreach (var pair in allPossibilities)
//        {
//            log += pair.Item1 + "=" + pair.Item2 + "; ";
//        }
//        Debug.Log(log);
//        id = PickRandomID(allPossibilities);

//        //update neighbor cell entropies
//        List<GameObject> sideNodes = new List<GameObject> { leftNode, rightNode, bottomNode, topNode };
//        foreach (GameObject node in sideNodes)
//        {
//            if (node != null && node.GetComponent<TileNode>().id == -1.0f)
//            {
//                node.GetComponent<TileNode>().UpdateEntropy();
//            }
//        }
//        Update();
//    }

//    public void createGrass(int numX, int numY, int numZ)
//    {
//        Vector3 pos = this.transform.position;
//        GameObject container = new GameObject("Container");
//        container.transform.position = pos;
//        container.transform.parent = this.transform;
//        for (int x = -numX / 2; x <= numX / 2; x++)
//        {
//            for (int y = 0; y < numY; y++)
//            {
//                for (int z = -numZ / 2; z <= numZ / 2; z++)
//                {
//                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//                    cube.GetComponent<Renderer>().material.color = Color.green;
//                    cube.transform.localScale = new Vector3(1.0f / numX, 1.0f / numY, 1.0f / numZ);
//                    cube.transform.position = new Vector3(x * 1.0f / numX /*- (1.0f / numX / 2.0f)*/ + pos.x,
//                                                          y * 1.0f / numY + (1.0f / numY / 2.0f) + pos.y,
//                                                          z * 1.0f / numZ /*+ (1.0f / numZ / 2.0f)*/ + pos.z);
//                    //cubes[x, y, z] = cube;
//                    cube.transform.SetParent(container.transform);
//                }
//            }
//        }
//        currentTile = container;
//    }

//    public void createDirt(int numX, int numY, int numZ)
//    {
//        Vector3 pos = this.transform.position;
//        GameObject container = new GameObject("Container");
//        container.transform.position = pos;
//        container.transform.parent = this.transform;
//        for (int x = -numX / 2; x <= numX / 2; x++)
//        {
//            for (int y = 0; y < numY; y++)
//            {
//                for (int z = -numZ / 2; z <= numZ / 2; z++)
//                {
//                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//                    cube.GetComponent<Renderer>().material.color = new Color(130f / 255f, 100f / 255f, 57f / 255f);
//                    cube.transform.localScale = new Vector3(1.0f / numX, 1.0f / numY, 1.0f / numZ);
//                    cube.transform.position = new Vector3(x * 1.0f / numX /*+ (1.0f / numX / 2.0f)*/ + pos.x,
//                                                          y * 1.0f / numY + (1.0f / numY / 2.0f) + pos.y,
//                                                          z * 1.0f / numZ /*+ (1.0f / numZ / 2.0f)*/ + pos.z);
//                    //cubes[x, y, z] = cube;
//                    cube.transform.SetParent(container.transform);
//                }
//            }
//        }
//        currentTile = container;
//    }

//    public void createStone(int numX, int numY, int numZ)
//    {
//        Vector3 pos = this.transform.position;
//        GameObject container = new GameObject("Container");
//        container.transform.position = pos;
//        container.transform.parent = this.transform;
//        for (int x = -numX / 2; x <= numX / 2; x++)
//        {
//            for (int y = 0; y < numY; y++)
//            {
//                for (int z = -numZ / 2; z <= numZ / 2; z++)
//                {
//                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//                    cube.GetComponent<Renderer>().material.color = Color.gray;
//                    cube.transform.localScale = new Vector3(1.0f / numX, 1.0f / numY, 1.0f / numZ);
//                    cube.transform.position = new Vector3(x * 1.0f / numX /*+ (1.0f / numX / 2.0f)*/ + pos.x,
//                                                          y * 1.0f / numY + (1.0f / numY / 2.0f) + pos.y,
//                                                          z * 1.0f / numZ /*+ (1.0f / numZ / 2.0f)*/ + pos.z);
//                    //cubes[x, y, z] = cube;
//                    cube.transform.SetParent(container.transform);
//                }
//            }
//        }
//        currentTile = container;
//    }
//    public void createWater(int numX, int numY, int numZ)
//    {
//        Vector3 pos = this.transform.position;
//        GameObject container = new GameObject("Container");
//        container.transform.position = pos;
//        container.transform.parent = this.transform;
//        for (int x = -numX / 2; x <= numX / 2; x++)
//        {
//            for (int y = 0; y < numY; y++)
//            {
//                for (int z = -numZ / 2; z <= numZ / 2; z++)
//                {
//                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//                    cube.GetComponent<Renderer>().material.color = Color.blue;
//                    cube.transform.localScale = new Vector3(1.0f / numX, 1.0f / numY, 1.0f / numZ);
//                    cube.transform.position = new Vector3(x * 1.0f / numX /*+ (1.0f / numX / 2.0f)*/ + pos.x,
//                                                          y * 1.0f / numY + (1.0f / numY / 2.0f) + pos.y,
//                                                          z * 1.0f / numZ /*+ (1.0f / numZ / 2.0f)*/ + pos.z);
//                    //cubes[x, y, z] = cube;
//                    cube.transform.SetParent(container.transform);
//                    //currentTile = cube;
//                }
//            }
//        }
//        currentTile = container;
//    }

//    public bool cycleCheck(List<GameObject> visited, Dictionary<GameObject, GameObject> parents, List<GameObject> cycle)
//    {
//        visited.Add(this.gameObject);
//        List<GameObject> sideNodes = new List<GameObject> { leftNode, rightNode, bottomNode, topNode };
//        foreach (GameObject node in sideNodes)
//        {
//            if (node != null && node.GetComponent<TileNode>().id != -1.0f)
//            {
//                //parents.Add(node, this.gameObject);
//                if (!visited.Contains(node))
//                {
//                    //if (node.GetComponent<TileNode>().cycleCheck(visited, this.gameObject))
//                    parents.Add(node, this.gameObject);
//                    if (node.GetComponent<TileNode>().cycleCheck(visited, parents, cycle))
//                    {
//                        //cycle.Add(this.gameObject);
//                        return true;
//                    }
//                }
//                //else if (visited.Contains(node) && node != parent)
//                else if (visited.Contains(node) && node != parents.GetValueOrDefault(this.gameObject) && node == visited[0])
//                //last condition checks that it starts and ends at the same point
//                {
//                    //cycle.Add(this.gameObject);
//                    GameObject curr = this.gameObject;
//                    while (curr != null)
//                    {
//                        //cycle.Add(curr);
//                        //if (cycle.Contains(curr))
//                        if (cycle.Count >= 2 && cycle[1] == curr)
//                        {
//                            cycle.RemoveAt(0);
//                            //curr = parents.GetValueOrDefault(curr);
//                            //continue;
//                        }
//                        else
//                        {
//                            cycle.Insert(0, curr);
//                        }

//                        curr = parents.GetValueOrDefault(curr);
//                        //if (curr == node)
//                        //{
//                        //    break;
//                        //}
//                    }
//                    curr = node;
//                    while (curr != null)
//                    {
//                        //if (cycle.Contains(curr))
//                        //{
//                        if (cycle.Count >= 2 && cycle.IndexOf(curr, cycle.Count - 2) != -1)
//                        {
//                            cycle.RemoveAt(cycle.Count - 1);
//                            //curr = parents.GetValueOrDefault(curr);
//                            //continue;
//                        }

//                        else
//                        {
//                            cycle.Add(curr);
//                        }

//                        curr = parents.GetValueOrDefault(curr);
//                    }
//                    return true;
//                }
//            }
//        }

//        return false;
//    }

//    public void fill(List<GameObject> visited, float id)
//    {
//        if (visited.Contains(this.gameObject))
//        {
//            return;
//        }
//        visited.Add(this.gameObject);

//        Destroy(this.currentTile);
//        //createStone(1, 1, 1);
//        //createWater(1, 1, 1);
//        this.id = id;
//        List<GameObject> sideNodes = new() { leftNode, rightNode, bottomNode, topNode };
//        foreach (GameObject node in sideNodes)
//        {
//            //TileNode tileData = node.GetComponent<TileNode>();
//            if (node != null && node.GetComponent<TileNode>().id == -1.0f)
//            {
//                /*Destroy(tileData.currentTile);*/
//                node.GetComponent<TileNode>().fill(visited, id);
//            }
//        }
//    }

//    public void findInside(List<GameObject> cycleNodes)
//    {
//        Vector3 randPos = cycleNodes[0].transform.position;
//        float minX = randPos.x;
//        float maxX = randPos.x;
//        float minZ = randPos.z;
//        float maxZ = randPos.z;

//        foreach (GameObject node in cycleNodes)
//        {
//            Vector3 pos = node.transform.position;
//            if (pos.x < minX) { minX = pos.x; }
//            if (pos.x > maxX) { maxX = pos.x; }
//            if (pos.z < minZ) { minZ = pos.z; }
//            if (pos.z > maxZ) { maxZ = pos.z; }
//        }

//        //(float, float, float) outside = (minX - 1.0f, randPos.y, minZ - 1.0f);
//        Debug.Log("minX" + minX + "; maxX" + maxX + "; minZ" + minZ + "; maxZ" + maxZ);

//        int numTested = 0;

//        for (float x = minX; x <= maxX; x++)
//        {
//            for (float z = minZ; z <= maxZ; z++)
//            {
//                (float, float, float) testPoint = (x, randPos.y, z);
//                GameObject tile = Global.nodes.GetValueOrDefault(testPoint);
//                if (tile == null || tile.GetComponent<TileNode>().id != -1.0f)
//                {
//                    continue;
//                }

//                numTested++;

//                int count = 0;
//                for (int i = 0; i < cycleNodes.Count - 1; i++)
//                {
//                    Vector3 v1 = cycleNodes[i].transform.position;
//                    Vector3 v2 = cycleNodes[(i + 1) % cycleNodes.Count].transform.position;

//                    if (((v1.z <= testPoint.Item3 && testPoint.Item3 < v2.z) || (v2.z <= testPoint.Item3 && testPoint.Item3 < v1.z)) &&
//                        (testPoint.Item1 < (v2.x - v1.x) * (testPoint.Item3 - v1.z) / (v2.z - v1.z) + v1.x))
//                    {
//                        count++;
//                    }
//                }

//                if (count % 2 == 1)
//                {
//                    //break;
//                    tile.GetComponent<TileNode>().fill(new List<GameObject>(), 4.0f);
//                    Debug.Log("supposed to fill");
//                    return;
//                }
//            }
//        }

//        Debug.Log("numClearTested " + numTested);
//    }

//    public void updateCubes()
//    {
//        bool leftEmpty = (leftNode == null || leftNode.GetComponent<TileNode>().id == -1.0f);
//        bool rightEmpty = (rightNode == null || rightNode.GetComponent<TileNode>().id == -1.0f);
//        bool bottomEmpty = (bottomNode == null || bottomNode.GetComponent<TileNode>().id == -1.0f);
//        bool topEmpty = (topNode == null || topNode.GetComponent<TileNode>().id == -1.0f);

//        if (currentTile.transform.childCount == 0)
//        {
//            Debug.Log("found no children: " + gameObject.name + currentTile.name + id);
//            return;
//        }

//        float minX = currentTile.transform.GetChild(0).position.x;
//        float maxX = currentTile.transform.GetChild(0).position.x;
//        float minY = currentTile.transform.GetChild(0).position.y;
//        float maxY = currentTile.transform.GetChild(0).position.y;
//        float minZ = currentTile.transform.GetChild(0).position.z;
//        float maxZ = currentTile.transform.GetChild(0).position.z;

//        string childPositions = " Child positions:";
//        for (int i = 0; i < currentTile.transform.childCount; i++)
//        {
//            childPositions += currentTile.transform.GetChild(i).position.ToString();
//            Vector3 currChildPos = currentTile.transform.GetChild(i).position;
//            minX = Mathf.Min(minX, currChildPos.x);
//            minY = Mathf.Min(minY, currChildPos.y);
//            minZ = Mathf.Min(minZ, currChildPos.z);
//            maxX = Mathf.Max(maxX, currChildPos.x);
//            maxY = Mathf.Max(maxY, currChildPos.y);
//            maxZ = Mathf.Max(maxZ, currChildPos.z);
//        }

//        //float sideLength = maxX - minX / tileSide;
//        float cubeSide = (maxX - minX) / (tileSide - 1);
//        float sideLength = cubeSide * tileSide;
//        //for (int i = 0; i < tileSide; i++)
//        //{
//        //    for (float x  = minX; x <= maxX; x += cubeSide)
//        //    {

//        //    }
//        //}
//        Debug.Log(sideLength);

//        float left = leftEmpty ? 0 : maxY;
//        float right = rightEmpty ? 0 : maxY;
//        float top = topEmpty ? 0 : maxY;
//        float bottom = bottomEmpty ? 0 : maxY;
//        Debug.Log("maxY: " + maxY + " left: " + left + " right: " + right + " top: " + top + " bottom: " + bottom);
//        bool changed = false;

//        edge = false;

//        for (int i = 0; i < currentTile.transform.childCount; i++)
//        {
//            Transform currChild = currentTile.transform.GetChild(i);
//            bool currActiveStatus = currChild.gameObject.activeSelf;
//            Vector3 currChildPos = currChild.position;
//            float xInterp = (float)(currChildPos.x - minX) / sideLength;
//            if (leftEmpty && rightEmpty)
//            {
//                xInterp = Mathf.Abs(0.5f - xInterp) * 2f;
//                left = maxY;
//            }
//            float yValue = Mathf.Lerp(left, right, xInterp) + cubeSide;
//            float zInterp = (float)(currChildPos.z - minZ) / sideLength;
//            if (topEmpty && bottomEmpty)
//            {
//                zInterp = Mathf.Abs(0.5f - zInterp) * 2f;
//                bottom = maxY;
//            }
//            float yValue2 = Mathf.Lerp(bottom, top, zInterp) + cubeSide;
//            if (currChildPos.y < yValue && currChildPos.y < yValue2)
//            {
//                currChild.gameObject.SetActive(true);
//                changed = changed || (currActiveStatus != true);
//            }
//            else
//            {
//                currChild.gameObject.SetActive(false);
//                changed = changed || (currActiveStatus != false);
//                edge = true;
//            }

//        }

//        Debug.Log(currentTile.transform.childCount + childPositions);
//        //Debug.Log(childPositions);

//        if (changed)
//        {
//            PropogateCubeChanges();
//        }

//        //bool leftEdge = (leftNode != null && leftNode.GetComponent<TileNode>().id != -1.0f && leftNode.GetComponent<TileNode>().edge);
//        //bool rightEdge = (rightNode != null && rightNode.GetComponent<TileNode>().id != -1.0f && rightNode.GetComponent<TileNode>().edge);
//        //bool bottomEdge = (bottomNode != null && bottomNode.GetComponent<TileNode>().id != -1.0f && bottomNode.GetComponent<TileNode>().edge);
//        //bool topEdge = (topNode != null && topNode.GetComponent<TileNode>().id != -1.0f && topNode.GetComponent<TileNode>().edge);

//        //if (leftEdge) { leftNode.GetComponent<TileNode>().updateCubes(); }
//        //if (rightEdge) { rightNode.GetComponent<TileNode>().updateCubes(); }
//        //if (bottomEdge) { bottomNode.GetComponent<TileNode>().updateCubes(); }
//        //if (topEdge) { topNode.GetComponent<TileNode>().updateCubes(); }
//    }

//    public void PropogateCubeChanges()
//    {
//        bool leftEmpty = (leftNode == null || leftNode.GetComponent<TileNode>().id == -1.0f);
//        bool rightEmpty = (rightNode == null || rightNode.GetComponent<TileNode>().id == -1.0f);
//        bool bottomEmpty = (bottomNode == null || bottomNode.GetComponent<TileNode>().id == -1.0f);
//        bool topEmpty = (topNode == null || topNode.GetComponent<TileNode>().id == -1.0f);

//        if (!leftEmpty) { leftNode.GetComponent<TileNode>().updateCubes(); }
//        if (!rightEmpty) { rightNode.GetComponent<TileNode>().updateCubes(); }
//        if (!bottomEmpty) { bottomNode.GetComponent<TileNode>().updateCubes(); }
//        if (!topEmpty) { topNode.GetComponent<TileNode>().updateCubes(); }
//    }
//}
