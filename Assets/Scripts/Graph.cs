//using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using System.Linq;
using AYellowpaper.SerializedCollections;

public class Graph : MonoBehaviour
{
    public int dimensions;
    public GameObject node;
    public Dictionary<(float, float, float), GameObject> nodes;
    //public Dictionary<float, float[]> possibleTiles;
    //[SerializedDictionary("Tile ID", "ID to Probability")]
    //public Dictionary<float, List<(float, float)>> possibleSideTiles; //maps ids to lists of ids with probabilities
    public Dictionary<float, List<Vector2>> possibleSideTiles; //maps ids to lists of ids with probabilities
    //[SerializedDictionary("Tile ID", "ID to Probability")]
    public Dictionary<float, List<float>> possibleAboveTiles; //maps ids to lists of ids with probabilities
    public Dictionary<float, List<Vector2>> possibleBelowTiles; //maps ids to lists of ids with probabilities

    public bool multiSelect/* = true*/;
    public int maxRange = 50;
    public int tileDim = 3;

    public float speed;

    public List<Vector2> clearNeighborProbabilities;
    public List<Vector2> grassNeighborProbabilities;
    public List<Vector2> dirtNeighborProbabilities;
    public List<Vector2> stoneNeighborProbabilities;

    public List<Vector2> waterNeighborProbabilities;

    public GameObject testObject;
    public List<GameObject> grassAssets;
    public List<GameObject> dirtAssets;
    public List<GameObject> stoneAssets;

    public List<Color> terrainColors;

    // Start is called before the first frame update
    void Start()
    {
        nodes = new Dictionary<(float, float, float), GameObject>();
        //possibleTiles = new Dictionary<float, float[]>();
        possibleSideTiles = new Dictionary<float, List<Vector2>>();
        possibleAboveTiles = new Dictionary<float, List<float>>();
        possibleBelowTiles = new Dictionary<float, List<Vector2>>();
        InitializeGrid();
        FillPossibilities();

        multiSelect = false;

        speed = 0.005f;

        terrainColors = new List<Color>{ Color.clear,
                                         Color.green,
                                         new Color(130f / 255f, 100f / 255f, 57f / 255f),
                                         Color.gray,
                                         new Color(141f / 255f, 216f / 255f, 204f / 255f)};

        //clearNeighborProbabilities = new()
        //{
        //    new Vector2(1.0f, 0.4f), //40% of grass
        //    new Vector2(2.0f, 0.2f), //20% of dirt
        //    new Vector2(3.0f, 0.4f)  //40% of stone
        //};
        //possibleSideTiles.Add(-1.0f, clearNeighborProbabilities);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    if (multiSelect)
        //    {

        //    }
        //    multiSelect = !multiSelect;
        //}
    }

    void InitializeGrid()
    {
        for (int y = -dimensions / 2; y <= dimensions / 2; y++)
        {
            for (int x = -dimensions / 2; x < dimensions / 2; x++)
            {
                GameObject newNode = Instantiate(node, new Vector3(x, 0, y), Quaternion.identity);
                nodes.Add((x, 0, y), newNode);
            }
        }

        foreach (var nodePair in nodes)
        {
            (float, float, float) coordinate = nodePair.Key;
            TileNode node = nodePair.Value.GetComponent<TileNode>();
            (float, float, float) leftCoord = (coordinate.Item1 - 1.0f, coordinate.Item2, coordinate.Item3);
            (float, float, float) rightCoord = (coordinate.Item1 + 1.0f, coordinate.Item2, coordinate.Item3);
            (float, float, float) bottomCoord = (coordinate.Item1, coordinate.Item2, coordinate.Item3 - 1.0f);
            (float, float, float) topCoord = (coordinate.Item1, coordinate.Item2, coordinate.Item3 + 1.0f);

            if (nodes.ContainsKey(leftCoord))
            {
                node.leftNode = nodes[leftCoord];
            }
            if (nodes.ContainsKey(rightCoord))
            {
                node.rightNode = nodes[rightCoord];
            }
            if (nodes.ContainsKey(topCoord))
            {
                node.topNode = nodes[topCoord];
            }
            if (nodes.ContainsKey(bottomCoord))
            {
                node.bottomNode = nodes[bottomCoord];
            }
        }
    }

    public GameObject createNode(Vector3 position)
    {
        GameObject newNode = Instantiate(node, position, Quaternion.identity);
        nodes.Add((position.x, position.y, position.z), newNode);
        Debug.Log("position y: " + position.y);
        return newNode;
    }

    public void FillPossibilities()
    {
        //Clear
        clearNeighborProbabilities = new()
        {
            new Vector2(1.0f, 0.4f), //40% of grass
            new Vector2(2.0f, 0.2f), //20% of dirt
            new Vector2(3.0f, 0.4f)  //40% of stone
        };
        possibleSideTiles.Add(-1.0f, clearNeighborProbabilities);

        //Grass
        grassNeighborProbabilities = new()
        {
            new Vector2(1.0f, 0.75f), //75% of grass
            new Vector2(2.0f, 0.10f), //10% of dirt
            new Vector2(3.0f, 0.15f), //15% of stone
            new Vector2(4.0f, 0.0f)  //0% of water but it should be allowed
        };
        possibleSideTiles.Add(1.0f, grassNeighborProbabilities);

        //Dirt
        dirtNeighborProbabilities = new()
        {
            new Vector2(1.0f, 0.3f), //30% of grass
            new Vector2(2.0f, 0.7f), //70% of dirt
            new Vector2(3.0f, 0.0f), //0% of stone but it should be allowed
            new Vector2(4.0f, 0.0f)  //0% of water but it should be allowed
        };
        possibleSideTiles.Add(2.0f, dirtNeighborProbabilities);

        //Stone
        stoneNeighborProbabilities = new()
        {
            new Vector2(1.0f, 0.15f), //15% of grass
            new Vector2(2.0f, 0.0f), //10% of dirt //changed to 0
            new Vector2(3.0f, 0.85f), //75% of stone //changed to 85
            new Vector2(4.0f, 0.0f)  //0% of water but it should be allowed
        };
        possibleSideTiles.Add(3.0f, stoneNeighborProbabilities);

        waterNeighborProbabilities = new() { new Vector2(1.0f, 0.15f) };
        possibleSideTiles.Add(4.0f, waterNeighborProbabilities);

    }

    public List<GameObject> BFS(GameObject startNode, float id, int max)
    {
        int count = 0;
        List<GameObject> visited = new List<GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();

        visited.Add(startNode);
        queue.Enqueue(startNode);
        count++;

        while (queue.Count > 0 && count < max)
        {
            GameObject curr = queue.Dequeue();
            TileNode currNode = curr.GetComponent<TileNode>();
            List<GameObject> sideNodes = new List<GameObject> { currNode.leftNode, currNode.rightNode, currNode.bottomNode, currNode.topNode };
            foreach (GameObject node in sideNodes)
            {
                if (node != null && node.GetComponent<TileNode>().id == id && !visited.Contains(node))
                {
                    visited.Add(node);
                    queue.Enqueue(node);
                    count++;
                }
            }
        }

        return visited;
    }

    public void HighlightMultiple(List<GameObject> nodes)
    {
        foreach (GameObject node in nodes)
        {
            TileNode nodeData = node.GetComponent<TileNode>();
            nodeData.currentTile.GetComponent<MeshRenderer>().enabled = true;
            //nodeData.Collapse();
        }
    }

    public void UnhighlightMultiple(List<GameObject> nodes)
    {
        foreach (GameObject node in nodes)
        {
            TileNode nodeData = node.GetComponent<TileNode>();
            nodeData.currentTile.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    /*IEnumerator CheckEntropy()
    {
        List<Cell> tempGrid = new List<Cell>(gridComponents);
        tempGrid.RemoveAll(c => c.collapsed);
        tempGrid.Sort((a, b) => a.possibleTiles.Length - b.possibleTiles.Length);
        tempGrid.RemoveAll(a => a.possibleTiles.Length != tempGrid[0].possibleTiles.Length);

        yield return new WaitForSeconds(0.025f);
        iteration++;
        CollapseCell(tempGrid);
    }*/
    public void StartMultipleCollapseAnimated(List<GameObject> nodesToCollapse)
    {
        StartCoroutine(CollapseMultiple(nodesToCollapse));
    }
    public IEnumerator CollapseMultiple(List<GameObject> nodesToCollapse)
    //public void CollapseMultiple(List<GameObject> nodesToCollapse)
   
    {
        Debug.Log("ran");
        while (nodesToCollapse.Count > 0)
        {
            Debug.Log("Number of nodes to collapse: " + nodesToCollapse.Count);
            List<GameObject> temp = new(nodesToCollapse);
            temp.OrderBy(node => node.GetComponent<TileNode>().entropy);
            temp.RemoveAll(node => node.GetComponent <TileNode>().entropy != temp[0].GetComponent<TileNode>().entropy);
            Debug.Log("choosing from __ nodes: " + temp.Count);
            int index = Random.Range(0, temp.Count);
            GameObject selectedNode = temp[index];
            //yield return new WaitForSeconds(0.025f);
            selectedNode.GetComponent<TileNode>().Collapse();
            nodesToCollapse.Remove(selectedNode);
            Debug.Log("nodes left: " + nodesToCollapse.Count);
            yield return new WaitForSeconds(speed);
        }
        Debug.Log("finished" + nodesToCollapse.Count);
        //foreach (GameObject node in nodesToCollapse)
        //{
        //    node.GetComponent<TileNode>().Collapse();
        //}
    }

    //public List<GameObject> toCollapseMultiple = new List<GameObject>();

    //public IEnumerator CollapseMultipleStaggered()
    //{
    //    /*if (toCollapseMultiple.Count == 0)
    //    {
    //        yield break;
    //    }*/
    //    int iter = 0;
    //    while (toCollapseMultiple.Count > 0) { 
    //        List<GameObject> temp = new(toCollapseMultiple);
    //        temp.OrderBy(node => node.GetComponent<TileNode>().entropy);
    //        temp.RemoveAll(node => node.GetComponent<TileNode>().entropy != temp[0].GetComponent<TileNode>().entropy);
    //        //Debug.Log("choosing from __ nodes: " + temp.Count);
    //        int index = Random.Range(0, temp.Count);
    //        GameObject selectedNode = temp[index];
    //        Debug.Log("current node id" + selectedNode.GetComponent<TileNode>().id);
    //        selectedNode.GetComponent<TileNode>().Collapse();
            
    //        toCollapseMultiple.Remove(selectedNode);
    //        Debug.Log("iter" + iter);
             
    //        iter++;
    //        yield return null;
    //    }
    //}

}
