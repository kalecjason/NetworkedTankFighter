using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour {

    public struct nodeConnections
    {
        public GameObject node;
        public float cost;
    }

    public Material blue;
    public Material red;

    public List<nodeConnections> connections;
    

   // public float costSoFar;
   // public GameObject[] connection;


    public void changeColorRed()
    {
        this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }

    public void changeColorBlue()
    {
        this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
    }

    public void changeColorYellow()
    {
        this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
    }

    public void changeColorGreen()
    {
        this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
    }

    // Use this for initialization
    void Awake ()
    {
        connections = new List<nodeConnections>();

        //Gizmos.color = Color.blue;
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");

        for (int j = 0; j < nodes.Length; j++)
        {
            if (nodes[j].transform.position != this.transform.position)
            {
                Vector3 dir = nodes[j].transform.position - this.transform.position;
                RaycastHit hit;

                if (Physics.SphereCast(this.transform.position, 0.5f, dir, out hit))
                {
                    if (hit.transform.gameObject.CompareTag("Node"))
                    {
                        //Gizmos.DrawLine(transform.position, hit.transform.position);
                        bool alreadyInList = false;
                        for (int i = 0; i < connections.Count; i++)
                        {
                            if (hit.transform == connections[i].node.transform)
                            {
                                alreadyInList = true;
                            }
                        }
                        if (!alreadyInList)
                        {
                            nodeConnections newNode;
                            newNode.node = hit.transform.gameObject;
                            newNode.cost = Vector3.Distance(transform.position, newNode.node.transform.position);

                            connections.Add(newNode);
                        }

                    }
                }
            }
        }

    }
	
    public int connectionsCount()
    {
        return connections.Count;
    }

	// Update is called once per frame
	void Update () {
		
	}

    /*public void setConnection(GameObject node1, GameObject node2)
    {
        connection[0] = node1;
        connection[1] = node2;
    }*/

    /*public void setCost(float cost)
    {
        costSoFar = cost;
    }*/

    private void OnDrawGizmos()
    {
        connections = new List<nodeConnections>();

        Gizmos.color = Color.blue;
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");

        for(int j = 0; j < nodes.Length; j++)
        {
            if (nodes[j].transform.position != this.transform.position)
            {
                Vector3 dir = nodes[j].transform.position - this.transform.position;
                RaycastHit hit; 

                if(Physics.SphereCast(this.transform.position, 0.5f , dir, out hit))
                {
                    if (hit.transform.gameObject.CompareTag("Node"))
                    {
                        Gizmos.DrawLine(transform.position, hit.transform.position);
                        bool alreadyInList = false;
                        for(int i = 0; i < connections.Count; i++)
                        {
                            if (hit.transform == connections[i].node.transform)
                            {
                                alreadyInList = true;
                            }
                        }
                        if(!alreadyInList)
                        {
                            nodeConnections newNode;
                            newNode.node = hit.transform.gameObject;
                            newNode.cost = Vector3.Distance(transform.position, newNode.node.transform.position);

                            connections.Add(newNode);
                        }
                        
                    }
                }
            }
        }

        //TESTING
       /* for(int i = 0; i < connections.Count; i++)
        {
            print("node: " + connections[i].node.name + "   cost: " + connections[i].cost);
            print("count   " + connections.Count);
        }*/


        
    }
}
