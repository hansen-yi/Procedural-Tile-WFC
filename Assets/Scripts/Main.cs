/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; //not the right tile

public class Cell : MonoBehaviour { 
    List<Tile> possibleTiles = new List<Tile>();
    Tile displayTile;

    public void collapse()
    {
        int chosenTile = Random.Range(0, possibleTiles.Count);
        displayTile = possibleTiles[chosenTile];
    }
}



public class Main : MonoBehaviour
{
    bool placeholder;
    int totalSpace = 0;
    Cell[] cells = new Cell[0];
    void WaveFunctionCollapse()
    {
        bool collapse = false;
        Cell chosenCell = null;

        while (!collapse)
        {
            if (placeholder)
            {
                collapse = true;
            }
            else
            {
                chosenCell.collapse();
                updateCells();
            }
        }

    }

    Stack<Cell> stack = new Stack<Cell>();
    void updateCells(Cell changedCell)
    {
        List<Cell> neighbors = new List<Cell>(); //use changedCell
        for (int i = 0; i < neighbors.Count; i++)
        {
            stack.Push(neighbors[i]);
        }

        while (stack.Count > 0)
        {
            Cell currCell = stack.Pop();
            //change the neighbors?
            currCell.collapse();
            if (placeholder)
            {
                updateCells(currCell);
            }
        }

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
*/