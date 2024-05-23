using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom : MonoBehaviour
{
    public int dimensions;
    public Cell cell;
    public GameObject clearTile;
    public List<Cell> currentGrid;
    public Dictionary<(float, float, float), Cell> currentCells;
    public Dictionary<(float, float, float), Cell> modifiedCells;

    private void Awake()
    {
        currentCells = new Dictionary<(float, float, float), Cell>();
        modifiedCells = new Dictionary<(float, float, float), Cell>();
        InitializeGrid();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Visit each cube and determine if it has been clicked.
                for (int i = 0; i < currentGrid.Count; i++)
                {
                    if (hit.collider.gameObject == currentGrid[i])
                    {
                        // This cube was clicked.
                        Debug.Log("Hit " + currentGrid[i].name, currentGrid[i]);
                    }
                }
            }
            Debug.Log(hit.collider.gameObject);
            Debug.Log("test");
        }*/
    }
    void InitializeGrid()
    {
        for (int y = -dimensions / 2; y <= dimensions / 2; y++)
        {
            for (int x = -dimensions / 2; x < dimensions / 2; x++)
            {
                Cell newCell = Instantiate(cell, new Vector3(x, 1.0f / 3.0f, y), Quaternion.identity);
                /*newCell.createCell(false, new Tile[] {clearTile});*/
                newCell.currentTile = Instantiate(clearTile, new Vector3(x, 0, y), clearTile.transform.rotation);
                currentGrid.Add(newCell);
                currentCells.Add((x, 1.0f / 3.0f, y), newCell);
                /*currentTiles.Add((x, 1.0f / 3.0f, y), newCell);*/
            }
        }

        /*StartCoroutine(CheckEntropy());*/
    }
}
