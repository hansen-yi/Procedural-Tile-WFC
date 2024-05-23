using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Tile[] possibleUp;
    public Tile[] possibleDown;
    public Tile[] possibleLeft;
    public Tile[] possibleRight;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*void OnMouseDown()
    {
        Debug.Log("tiletest");
        *//*Destroy(currentTile);
        currentTile = Instantiate(testTile, this.transform.position, testTile.transform.rotation);*//*
    }*/
}
