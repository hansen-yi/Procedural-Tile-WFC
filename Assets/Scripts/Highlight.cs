using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Highlight : MonoBehaviour
{
    private List<GameObject> adjacentHighlightedNodes;

    // Start is called before the first frame update
    void Start()
    {
        adjacentHighlightedNodes = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Graph Global = GameObject.Find("Graph").GetComponent<Graph>();
        if (Global.multiSelect)
        {
            Global.UnhighlightMultiple(adjacentHighlightedNodes);
            /*foreach (GameObject node in adjacentHighlightedNodes)
            {
                node.GetComponent<TileNode>().OnMouseDown();
            }*/
            //Global.CollapseMultiple(adjacentHighlightedNodes);
            //StartCoroutine(Global.CollapseMultiple(adjacentHighlightedNodes));
            Global.StartMultipleCollapseAnimated(adjacentHighlightedNodes);

            //Global.toCollapseMultiple = adjacentHighlightedNodes;
            //Debug.Log("list of adjacent before:" + Global.toCollapseMultiple.Count);
            //StartCoroutine(Global.CollapseMultipleStaggered());
            //Global.StartMultipleCollapseAnimated();
            //Debug.Log("list of adjacent:" + Global.toCollapseMultiple.Count);
            //adjacentHighlightedNodes.Clear();

        }
        else
        {
            this.GetComponentInParent<TileNode>().OnMouseDown();
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (GetComponentInParent<TileNode>().Global.multiSelect)
            {
                GetComponentInParent<TileNode>().Global.UnhighlightMultiple(adjacentHighlightedNodes);
                adjacentHighlightedNodes.Clear();
                //OnMouseEnter();
                this.GetComponent<MeshRenderer>().enabled = true;
            }
            GetComponentInParent<TileNode>().Global.multiSelect = !GetComponentInParent<TileNode>().Global.multiSelect;
        }
    }
    private void OnMouseEnter()
    {
        Graph Global = GameObject.Find("Graph").GetComponent<Graph>();
        if (Global.multiSelect)
        {
            List<GameObject> toHighlight = Global.BFS(this.transform.parent.gameObject, -1.0f, Global.maxRange);
            adjacentHighlightedNodes = toHighlight;
            Global.HighlightMultiple(toHighlight);

        }
        else
        {
            if (this.GetComponentInParent<TileNode>().id == -1.0f)
            {
                //gameObject.SetActive(true);
                this.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        //else
        //{
        //    if (Input.GetMouseButtonDown(1))
        //    {
        //        id = -1.0f;
        //        Debug.Log("right clicked");
        //    }
        //}
        //gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        Graph Global = GameObject.Find("Graph").GetComponent<Graph>();
        if (Global.multiSelect)
        {
            //List<GameObject> toHighlight = Global.BFS(this.transform.parent.gameObject, -1.0f, 50);
            //Global.UnhighlightMultiple(toHighlight);
            Global.UnhighlightMultiple(adjacentHighlightedNodes);
            adjacentHighlightedNodes.Clear();
        }
        else
        {
            //gameObject.SetActive(false);
            this.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
