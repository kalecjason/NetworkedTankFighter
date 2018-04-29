using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PowerUpScript : NetworkBehaviour {
    float counter;
    public float timer;
    public bool exists;

    public Material color1;
    public Material color2;
	// Use this for initialization
	void Start () {
        exists = true;
        counter = 0.0f;
        
        CmdSpawnPowerUp();
        timer = 15.0f;
	}
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;

        if((counter % 0.5f) >= 0.25f)
        {
            Renderer rend = this.gameObject.GetComponent<MeshRenderer>();
            rend.material = color1;
        }
        else if ((counter % 0.5f) <= 0.25f)
        {
            Renderer rend = this.gameObject.GetComponent<MeshRenderer>();
            rend.material = color2;
        }

        timer += Time.deltaTime;

        if(exists == false)
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.GetComponent<SphereCollider>().enabled = false;
            if(timer >= 5.0f)
            {
                // this.gameObject.SetActive(true);
                this.gameObject.GetComponent<MeshRenderer>().enabled = true;
                this.gameObject.GetComponent<SphereCollider>().enabled = true;
                CmdSpawnPowerUp();
                exists = true;
            }
        }
	}

    [Command]
    void CmdSpawnPowerUp()
    {
        NetworkServer.Spawn(this.gameObject);
    }




}
