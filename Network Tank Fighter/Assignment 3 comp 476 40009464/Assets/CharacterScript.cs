using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour {
    //bool isMoving;
    Animator animator;

    public GameObject currentNode;
    public GameObject nextTravelNode;
    public GameObject targetNode;

    public List<Node> openList;
    public List<Node> closedList;
    public List<GameObject> shortestPath;

    int ind;

    bool rotationDoneA;

    public int choice;

    public struct Node
    {
        public GameObject nodeObject;
        public float costSoFar;
        public GameObject[] connection;

    }

    // Use this for initialization
    void Start () {
        //isMoving = false;
        animator = GetComponent<Animator>();

        
        rotationDoneA = false;

        openList = new List<Node>();
        closedList = new List<Node>();
        shortestPath = new List<GameObject>();


        ind = 1;


        if(choice == 1)
        {
            calculatePath();
        }
        else if (choice == 2)
        {
            calculatePath2();
        }
        
        targetNode.GetComponent<NodeScript>().changeColorRed();

        shortestPath.Reverse();

        nextTravelNode = shortestPath[ind];
        

       /* Node testNode;
        testNode.connection = new GameObject[2];*/
    }
	
    

	// Update is called once per frame
	void Update () {

        //followPath();

        transform.position = Vector3.MoveTowards(transform.position, nextTravelNode.transform.position, Time.deltaTime * 5);
        //=======================
      
            Quaternion neededRotation = Quaternion.LookRotation(nextTravelNode.transform.position - gameObject.transform.position);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, neededRotation, Time.deltaTime * 3.0f);
            print(Mathf.Abs(gameObject.transform.rotation.y - neededRotation.y));
       



        //======================
        if (transform.position == nextTravelNode.transform.position)
        {
            currentNode = nextTravelNode;
            nextPoint();
            
        }

        if(Input.GetKeyDown("1"))
        {
            choice = 1;
        }
        else if (Input.GetKeyDown("2"))
        {
            choice = 2;
        }

        if (Input.GetKeyDown("space"))
        {
            int rand = Random.Range(0, 57);
            GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
            GameObject nextNode = nodes[rand];
            targetNode.GetComponent<NodeScript>().changeColorBlue();
            for(int i = 0; i < nodes.Length; i++)
            {
                nodes[i].GetComponent<NodeScript>().changeColorBlue();
            }

            targetNode = nextNode;


            

            openList = new List<Node>();
            closedList = new List<Node>();
            shortestPath = new List<GameObject>();


            ind = 1;

            if (choice == 1)
            {
                calculatePath();
            }
            else if (choice == 2)
            {
                calculatePath2();
            }

            targetNode.GetComponent<NodeScript>().changeColorRed();

            shortestPath.Reverse();

            nextTravelNode = shortestPath[ind];

        }


        if(transform.position == targetNode.transform.position)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }
        
    }




    void nextPoint()
    {
        
        if(ind < shortestPath.Count)
        {
            nextTravelNode = shortestPath[ind];
            ind++;
        }
    }


    void calculatePath()
    {
        //initial open list
        for(int i = 0; i < currentNode.GetComponent<NodeScript>().connections.Count; i++)
        {
            Node newNode;
            newNode.nodeObject = currentNode.GetComponent<NodeScript>().connections[i].node;
            newNode.costSoFar = currentNode.GetComponent<NodeScript>().connections[i].cost;
            newNode.connection = new GameObject[2] { currentNode, currentNode.GetComponent<NodeScript>().connections[i].node};

            openList.Add(newNode);
        }

        bool goalNodeReached = false;
        while(!goalNodeReached)
        {
            Node nextNode = openList[0];
            //int nextNodeIndex = 0;
            for (int j = 0; j < openList.Count; j++)
            {
                if (openList[j].costSoFar < nextNode.costSoFar)
                {
                    nextNode = openList[j];
                    //nextNodeIndex = j;
                }
            }

            closedList.Add(nextNode);
            openList.Remove(nextNode);

            for (int k = 0; k < nextNode.nodeObject.GetComponent<NodeScript>().connectionsCount(); k++)
            {
                Node newNode;
                newNode.nodeObject = nextNode.nodeObject.GetComponent<NodeScript>().connections[k].node;
                newNode.costSoFar = nextNode.nodeObject.GetComponent<NodeScript>().connections[k].cost + nextNode.costSoFar;
                newNode.connection = new GameObject[2] { nextNode.nodeObject, nextNode.nodeObject.GetComponent<NodeScript>().connections[k].node };

                newNode.nodeObject.GetComponent<NodeScript>().changeColorYellow();
                openList.Add(newNode);
                // print(nextNode.nodeObject.GetComponent<NodeScript>().connections[k].node.name);
            }


            

            

            for (int l = 0; l < openList.Count; l++)
            {
                for (int m = 0; m < openList.Count; m++)
                {
                    if ((m != l) && (GameObject.ReferenceEquals(openList[l].nodeObject, openList[m].nodeObject)))
                    {
                        //print(openList[l].nodeObject.name + " === " + openList[m].nodeObject);
                        if (openList[l].costSoFar < openList[m].costSoFar)
                        {
                            openList.RemoveAt(m);
                        }
                        else
                        {
                            openList.RemoveAt(l);
                        }
                    }
                }
            }

            for (int i = 0; i < closedList.Count; i++)
            {
                for (int j = 0; j < openList.Count; j++)
                {
                    if (GameObject.ReferenceEquals(closedList[i].nodeObject, openList[j].nodeObject))
                    {
                        Debug.LogWarning("OPENLIST CONTAIS THING: " + openList[j].nodeObject.name );
                        openList.RemoveAt(j);
                    }
                }
            }
          


        //check if goal node is reached
        for (int p = 0; p < closedList.Count; p++)
            {
                if(GameObject.ReferenceEquals(closedList[p].nodeObject, targetNode))
                {
                    goalNodeReached = true;
                }
            }


               


                //TESTING
                for (int j = 0; j < openList.Count; j++)
         {
             print("(" + openList[j].nodeObject.name + ", " + openList[j].costSoFar + ", " + openList[j].connection[0].name + "->" + openList[j].connection[1].name + "), ");
         }
        print("==========================================");
        for (int j = 0; j < closedList.Count; j++)
        {
            print("(" + closedList[j].nodeObject.name + ", " + closedList[j].costSoFar + ", " + closedList[j].connection[0].name + "->" + closedList[j].connection[1].name + "), ");
        }
            print("****************************************************************************************");
            Debug.LogWarning("RIGTH HERE STUFF");

            //openList = currentNode.GetComponent<NodeScript>().connections;
        }
        //========================================================================================================================================================= end of test




      
        Debug.LogWarning("WARNING");

        Node nodeToAdd;
        nodeToAdd.nodeObject = null;
        nodeToAdd.costSoFar = 0;
        nodeToAdd.connection = new GameObject[2] { null, null };

        for (int i = 0; i < closedList.Count; i++)
        {
            if (GameObject.ReferenceEquals(closedList[i].nodeObject, targetNode))
            {
                nodeToAdd = closedList[i];
            }
        }
        

        int index = 0;
        if (GameObject.ReferenceEquals(nodeToAdd.nodeObject, targetNode))
        {

            shortestPath.Add(nodeToAdd.nodeObject);
        }

        //--------
        
        while (!shortestPath.Contains(currentNode))
        {
            
            for (int j = 0; j < closedList.Count; j++)
            {
                if (GameObject.ReferenceEquals(closedList[j].nodeObject, nodeToAdd.connection[0]))
                {
                    print("TEST2: " + closedList[j].nodeObject.name);
                    nodeToAdd = closedList[j];
                    shortestPath.Add(closedList[j].nodeObject);
                    closedList[j].nodeObject.GetComponent<NodeScript>().changeColorGreen();
                    //closedList.RemoveAt(j);
                    index = j;
                    break;
                }
            }
        }
            

      

        for (int i = 0; i < shortestPath.Count; i++)
        {
            print(shortestPath[i].name + "<-");
        }
    }





    void calculatePath2()
    {
        //initial open list
        for (int i = 0; i < currentNode.GetComponent<NodeScript>().connections.Count; i++)
        {
            Node newNode;
            newNode.nodeObject = currentNode.GetComponent<NodeScript>().connections[i].node;
            newNode.costSoFar = currentNode.GetComponent<NodeScript>().connections[i].cost;
            newNode.connection = new GameObject[2] { currentNode, currentNode.GetComponent<NodeScript>().connections[i].node };

            openList.Add(newNode);
        }

        bool goalNodeReached = false;
        while (!goalNodeReached)
        {
            Node nextNode = openList[0];
            //int nextNodeIndex = 0;
            for (int j = 0; j < openList.Count; j++)
            {
                float dist1 = Vector3.Distance(openList[j].nodeObject.transform.position, targetNode.transform.position);
                float dist2 = Vector3.Distance(nextNode.nodeObject.transform.position, targetNode.transform.position);
                if ((openList[j].costSoFar + dist1) < (nextNode.costSoFar + dist2))
                {
                    nextNode = openList[j];
                    //nextNodeIndex = j;
                }
            }

            closedList.Add(nextNode);
            openList.Remove(nextNode);

            for (int k = 0; k < nextNode.nodeObject.GetComponent<NodeScript>().connectionsCount(); k++)
            {
                Node newNode;
                newNode.nodeObject = nextNode.nodeObject.GetComponent<NodeScript>().connections[k].node;
                newNode.costSoFar = nextNode.nodeObject.GetComponent<NodeScript>().connections[k].cost + nextNode.costSoFar;
                newNode.connection = new GameObject[2] { nextNode.nodeObject, nextNode.nodeObject.GetComponent<NodeScript>().connections[k].node };

                newNode.nodeObject.GetComponent<NodeScript>().changeColorYellow();
                openList.Add(newNode);
                // print(nextNode.nodeObject.GetComponent<NodeScript>().connections[k].node.name);
            }






            for (int l = 0; l < openList.Count; l++)
            {
                for (int m = 0; m < openList.Count; m++)
                {
                    if ((m != l) && (GameObject.ReferenceEquals(openList[l].nodeObject, openList[m].nodeObject)))
                    {
                        //print(openList[l].nodeObject.name + " === " + openList[m].nodeObject);
                        if (openList[l].costSoFar < openList[m].costSoFar)
                        {
                            openList.RemoveAt(m);
                        }
                        else
                        {
                            openList.RemoveAt(l);
                        }
                    }
                }
            }

            for (int i = 0; i < closedList.Count; i++)
            {
                for (int j = 0; j < openList.Count; j++)
                {
                    if (GameObject.ReferenceEquals(closedList[i].nodeObject, openList[j].nodeObject))
                    {
                        Debug.LogWarning("OPENLIST CONTAIS THING: " + openList[j].nodeObject.name);
                        openList.RemoveAt(j);
                    }
                }
            }



            //check if goal node is reached
            for (int p = 0; p < closedList.Count; p++)
            {
                if (GameObject.ReferenceEquals(closedList[p].nodeObject, targetNode))
                {
                    goalNodeReached = true;
                }
            }





            //TESTING
            for (int j = 0; j < openList.Count; j++)
            {
                print("(" + openList[j].nodeObject.name + ", " + openList[j].costSoFar + ", " + openList[j].connection[0].name + "->" + openList[j].connection[1].name + "), ");
            }
            print("==========================================");
            for (int j = 0; j < closedList.Count; j++)
            {
                print("(" + closedList[j].nodeObject.name + ", " + closedList[j].costSoFar + ", " + closedList[j].connection[0].name + "->" + closedList[j].connection[1].name + "), ");
            }
            print("****************************************************************************************");
            Debug.LogWarning("RIGTH HERE STUFF");

            //openList = currentNode.GetComponent<NodeScript>().connections;
        }
        //========================================================================================================================================================= end of test





        Debug.LogWarning("WARNING");

        Node nodeToAdd;
        nodeToAdd.nodeObject = null;
        nodeToAdd.costSoFar = 0;
        nodeToAdd.connection = new GameObject[2] { null, null };

        for (int i = 0; i < closedList.Count; i++)
        {
            if (GameObject.ReferenceEquals(closedList[i].nodeObject, targetNode))
            {
                nodeToAdd = closedList[i];
            }
        }


        int index = 0;
        if (GameObject.ReferenceEquals(nodeToAdd.nodeObject, targetNode))
        {

            shortestPath.Add(nodeToAdd.nodeObject);
        }

        //--------

        while (!shortestPath.Contains(currentNode))
        {

            for (int j = 0; j < closedList.Count; j++)
            {
                if (GameObject.ReferenceEquals(closedList[j].nodeObject, nodeToAdd.connection[0]))
                {
                    print("TEST2: " + closedList[j].nodeObject.name);
                    nodeToAdd = closedList[j];
                    shortestPath.Add(closedList[j].nodeObject);
                    closedList[j].nodeObject.GetComponent<NodeScript>().changeColorGreen();
                    //closedList.RemoveAt(j);
                    index = j;
                    break;
                }
            }
        }




        for (int i = 0; i < shortestPath.Count; i++)
        {
            print(shortestPath[i].name + "<-");
        }
    }
}


