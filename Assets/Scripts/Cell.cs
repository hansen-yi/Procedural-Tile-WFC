using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class Cell : MonoBehaviour
{
    public bool collapsed;
    public Tile[] possibleTiles;

    public GameObject currentTile;
    public GameObject testTile;

    public GameObject[] tilesAsGameObjs;

    bool starting;

    public void createCell(bool collapseStatus, Tile[] tiles)
    {
        collapsed = collapseStatus;
        possibleTiles = tiles;
    }

    public void recreateCell(Tile[] tiles)
    {
        possibleTiles = tiles;
    }

    void OnMouseDown()
    {
        /*Debug.Log("please");*/
        /*Destroy(currentTile);*/
        Custom Global = GameObject.Find("Custom").GetComponent<Custom>();
        Dictionary<(float, float, float), Cell> modifiedCells = Global.modifiedCells;
        foreach (Cell modCell in modifiedCells.Values) 
        {
            modCell.collapsed = false;    
        }
        this.starting = true;
        Collapse();
        /*currentTile = Instantiate(testTile, this.transform.position - new Vector3(0, 1.0f / 3.0f, 0), testTile.transform.rotation);*/
    }
    
    void Collapse()
    {
        if (collapsed) { return; }
        collapsed = true;
        Vector3 pos = this.transform.position;
        Debug.Log(pos);
        Custom Global = GameObject.Find("Custom").GetComponent<Custom>();
        Dictionary<(float, float, float), Cell> currentCells = Global.currentCells;
        Dictionary<(float, float, float), Cell> modifiedCells = Global.modifiedCells;
        Tile up, down, left, right;
        if (currentCells.ContainsKey((pos.x, pos.y, pos.z - 1)) && (this.starting || currentCells[(pos.x, pos.y, pos.z - 1)].collapsed)) { up = currentCells[(pos.x, pos.y, pos.z - 1)].currentTile.GetComponent<Tile>(); }
        else { up = null; }
        if (currentCells.ContainsKey((pos.x, pos.y, pos.z + 1)) && (this.starting || currentCells[(pos.x, pos.y, pos.z + 1)].collapsed)) { down = currentCells[(pos.x, pos.y, pos.z + 1)].currentTile.GetComponent<Tile>(); }
        else { down = null; }
        if (currentCells.ContainsKey((pos.x + 1, pos.y, pos.z)) && (this.starting || currentCells[(pos.x + 1, pos.y, pos.z)].collapsed)) { left = currentCells[(pos.x + 1, pos.y, pos.z)].currentTile.GetComponent<Tile>(); }
        else { left = null; }
        if (currentCells.ContainsKey((pos.x - 1, pos.y, pos.z)) && (this.starting || currentCells[(pos.x - 1, pos.y, pos.z)].collapsed)) { right = currentCells[(pos.x - 1, pos.y, pos.z)].currentTile.GetComponent<Tile>(); }
        else { right = null; }
        Tile[] possible = null;
        Tile[] allPossibilities = new Tile[0];
        String checkPossibles = "";
        if (up != null)
        {
            /*Debug.Log("possible down from up: " + up.possibleDown);*/
            checkPossibles += "possible down from up: ";
            possible = up.possibleDown;
            allPossibilities = up.possibleDown;
            foreach (Tile tile in possible)
            {
                /*Debug.Log(tile.name);*/
                checkPossibles += tile.name + ", ";
            }
        }
        /*Debug.Log(possible.Length);*/
        if (down != null) 
        {
            /*Debug.Log("possible up from down: " + down.possibleUp);*/
            checkPossibles += "possible up from down: ";
            foreach (Tile tile in down.possibleUp)
            {
                checkPossibles += tile.name + ", ";
            }
            if (possible == null)
            {
                possible = down.possibleUp;
                allPossibilities = down.possibleUp;
            } 
            else
            {
                /*possible = up.possibleDown.Intersect(down.possibleUp).ToArray();*/
                possible = tileIntersection(possible, down.possibleUp);
                allPossibilities = allPossibilities.Concat(down.possibleUp).ToArray();
            }
        }
        if (left != null)
        {
            /*Debug.Log("possible right from left: " + left.possibleRight);*/
            checkPossibles += "possible right from left: ";
            foreach (Tile tile in left.possibleRight)
            {
                checkPossibles += tile.name + ", ";
            }
            if (possible == null) { possible = left.possibleRight; allPossibilities = left.possibleRight; }
            /*else { possible = possible.Intersect(left.possibleRight).ToArray(); }*/
            else { possible = tileIntersection(possible, left.possibleRight); allPossibilities = allPossibilities.Concat(left.possibleRight).ToArray(); }
        }
        if (right != null)
        {
            /*Debug.Log("possible left from right: " + right.possibleLeft);*/
            checkPossibles += "possible left from right: ";
            foreach (Tile tile in right.possibleLeft)
            {
                checkPossibles += tile.name + ", ";
            }
            if (possible == null) { possible = right.possibleLeft; allPossibilities = right.possibleLeft; }
            /*else { possible = possible.Intersect(right.possibleLeft).ToArray(); }*/
            else { possible = tileIntersection(possible, right.possibleLeft); allPossibilities = allPossibilities.Concat(right.possibleLeft).ToArray(); }
        }
        Debug.Log(checkPossibles);
        Debug.Log(possible.Length);
        int index = UnityEngine.Random.Range(0, possible.Length);
        /*Debug.Log(index);*/
        GameObject GOTile;
        if (possible.Length == 0)
        {
            Debug.Log("most possible" + sortPossibilites(allPossibilities)[0]);
            GOTile = findTile(sortPossibilites(allPossibilities)[0]);
            propogate();
        }
        else
        {
            GOTile = findTile(possible[index]);
        }

        

        Destroy(currentTile);
        if (!modifiedCells.ContainsKey((pos.x, pos.y, pos.z)))
        {
            modifiedCells.Add((pos.x, pos.y, pos.z), this);
        }
        if (stripClone(currentTile.name) != stripClone(GOTile.name))
        {
            /*propogate();*/
        }
        currentTile = Instantiate(GOTile, this.transform.position - new Vector3(0, 1.0f / 3.0f, 0), GOTile.transform.rotation);
        this.starting = false;

    }

    Tile[] tileIntersection(Tile[] arr1, Tile[] arr2 )
    {
        String[] arr1names = new String[arr1.Length];
        String[] arr2names = new String[arr2.Length];
        for (int i = 0; i < arr1.Length; i++)
        {
            arr1names[i] = stripClone(arr1[i].name);
        }
        for (int i = 0; i < arr2.Length; i++)
        {
            arr2names[i] = stripClone(arr2[i].name);
        }
        String[] intersectNames = arr1names.Intersect(arr2names).ToArray();
        List<Tile> intersection = new List<Tile>();
        foreach (Tile tile in arr1) 
        {
            if (intersectNames.Contains(stripClone(tile.name)))
            {
                intersection.Add(tile);
            }
        }
        return intersection.ToArray();
    }

    Tile[] sortPossibilites(Tile[] possibilites)
    {
        return possibilites.GroupBy(t => stripClone(t.name))
                    .OrderBy(g => g.Count())
                    .ThenBy(g => g.Key)
                    .SelectMany(g => g)
                    .ToArray();
    }

    String stripClone(String inputString)
    {
        if (inputString.Contains("(Clone)"))
        {
            return inputString.Replace("(Clone)", "");
        }
        else
        {
            return inputString;
        }
    }

    GameObject findTile(Tile tile)
    {
        foreach(GameObject tileGO in tilesAsGameObjs)
        {
            Debug.Log("gameobject names? " + tileGO.name + "; tile names: " + tile.name);
            if (tileGO.name == stripClone(tile.name)) 
            { 
                return tileGO;
            }
        }
        return null;
    }
    
    void propogate()
    {
        Vector3 pos = this.transform.position;
        Custom Global = GameObject.Find("Custom").GetComponent<Custom>();
        Dictionary<(float, float, float), Cell> currentCells = Global.currentCells;
        Tile up, down, left, right;
        if (currentCells.ContainsKey((pos.x, pos.y, pos.z - 1))) { up = currentCells[(pos.x, pos.y, pos.z - 1)].currentTile.GetComponent<Tile>(); }
        else { up = null; }
        if (currentCells.ContainsKey((pos.x, pos.y, pos.z + 1))) { down = currentCells[(pos.x, pos.y, pos.z + 1)].currentTile.GetComponent<Tile>(); }
        else { down = null; }
        if (currentCells.ContainsKey((pos.x - 1, pos.y, pos.z))) { left = currentCells[(pos.x - 1, pos.y, pos.z)].currentTile.GetComponent<Tile>(); }
        else { left = null; }
        if (currentCells.ContainsKey((pos.x + 1, pos.y, pos.z))) { right = currentCells[(pos.x + 1, pos.y, pos.z)].currentTile.GetComponent<Tile>(); }
        else { right = null; }
        /*Tile[] possible = null;*/
        
        if (up != null && stripClone(up.name) != "Clear-NoConstraints") { currentCells[(pos.x, pos.y, pos.z - 1)].Collapse(); }
        if (down != null && stripClone(down.name) != "Clear-NoConstraints") { currentCells[(pos.x, pos.y, pos.z + 1)].Collapse(); }
        if (left != null && stripClone(left.name) != "Clear-NoConstraints") { currentCells[(pos.x - 1, pos.y, pos.z)].Collapse(); }
        if (right != null && stripClone(right.name) != "Clear-NoConstraints") { currentCells[(pos.x + 1, pos.y, pos.z)].Collapse(); }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
