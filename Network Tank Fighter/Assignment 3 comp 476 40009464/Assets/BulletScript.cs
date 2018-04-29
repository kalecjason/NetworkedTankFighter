using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletScript : NetworkBehaviour {
    public GameObject shootingTank;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnCollisionEnter(Collision collision)
    {

        /*this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        Destroy(this.gameObject.GetComponent<Rigidbody>());
        Destroy(this.gameObject.GetComponent<SphereCollider>());
        Destroy(this.gameObject, 3.0f);*/
        

        //Destroy(this.gameObject);

        if ((collision.gameObject.CompareTag("Pillar")))
        {
            AudioSource aud = GetComponent<AudioSource>();
            aud.time = 1.0f;
            aud.Play();
            CmdDestroyPillar(collision.gameObject);
            collision.gameObject.SetActive(false);
            //Destroy(collision.gameObject);
            
        }
        CmdDestroyBullet();

        /* if(collision.gameObject.CompareTag("Player"))
         {

         }*/


    }

    [Command]
    void CmdDestroyBullet()
    {
        NetworkServer.Destroy(this.gameObject);
    }

    [Command]
    void CmdDestroyPillar(GameObject pil)
    {
        NetworkServer.UnSpawn(pil);
        
        
    }
}
