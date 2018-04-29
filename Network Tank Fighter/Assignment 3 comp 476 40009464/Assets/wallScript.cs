using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class wallScript : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        CmdSpawnWalls();

	}
    
	
	// Update is called once per frame
	void Update () {
		
	}
    
    [Command]
    void CmdSpawnWalls()
    {
        GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");
        for(int i = 0; i < pillars.Length; i++)
        {
            NetworkServer.Spawn(pillars[i]);
            pillars[i].SetActive(true);
        }
        
        
    }
}
