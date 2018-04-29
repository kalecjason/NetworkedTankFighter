using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionScript : NetworkBehaviour {
    public GameObject playerUnitPrefab;
    GameObject myPlayerUnit;

    // Use this for initialization
    void Start () {
        if (isLocalPlayer == false)
        { 
            return;
        }


        CmdspawnPlayer();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    [Command]
    void CmdspawnPlayer()
    {
        GameObject go = Instantiate(playerUnitPrefab);



        GameObject[] spawns = GameObject.FindGameObjectsWithTag("spawn");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        GameObject bestSpawn = spawns[0];
        float shortestDist = Vector3.Distance(spawns[0].transform.position, players[0].transform.position);

        //GameObject bestSpawn;
        //float shortestDist;

        for (int i = 0; i < spawns.Length; i++)
        {
            for(int j = 0; j < players.Length; j++)
            {
                float dist = Vector3.Distance(spawns[i].transform.position, players[j].transform.position);
                if(dist > shortestDist)
                {
                    bestSpawn = spawns[i];
                    shortestDist = dist;
                }
            }
        }
       
        go.transform.position = bestSpawn.transform.position;
        go.transform.rotation = bestSpawn.transform.rotation;


        myPlayerUnit = go;

        //go.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        go.transform.position = bestSpawn.transform.position;
    }
}
