using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankScript : NetworkBehaviour {
    public GameObject bullet;
    public GameObject bulletSpawn;
    float reloadTime;
    bool powerUp;
    float powerUpTimer;
    int health;
    
	// Use this for initialization
	void Start () {
        reloadTime = 0.0f;
        powerUp = false;
        powerUpTimer = 0.0f;
        health = 3;
	}

    private void OnCollisionExit(Collision collision)
    {
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        print("EXIT");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            if(collision.gameObject.GetComponent<BulletScript>().shootingTank != this.gameObject)
            {
                health--;
                if(health == 0)
                {
                    CmdDestroy();
                }
            }
            
        }
        if(collision.gameObject.CompareTag("PowerUp"))
        {
            //Destroy(collision.gameObject);
            collision.gameObject.GetComponent<PowerUpScript>().timer = 0;
            collision.gameObject.GetComponent<PowerUpScript>().exists = false;
            //collision.gameObject.SetActive(false);
            //collision.get
            //NetworkServer.UnSpawn(collision.gameObject);
            powerUp = true;
            powerUpTimer = 5.0f;
            print("POWERUP");
        }
        print("ENTER");
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    // Update is called once per frame
    void Update ()
    {
        reloadTime += Time.deltaTime;
        powerUpTimer -= Time.deltaTime;

        if(powerUpTimer <= 0.0f)
        {
            powerUp = false;
        }

        if (hasAuthority == false)
        {
            return;
        }

        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.right * Time.deltaTime * 8);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.left * Time.deltaTime * 8);
        }
        if (Input.GetKey("a"))
        {
            transform.Rotate(Vector3.down * Time.deltaTime * 90);
        }
        if (Input.GetKey("d"))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 90);
        }
        if(Input.GetKeyDown("space"))
        {
            if(reloadTime >= 2.0f || powerUp == true)
            {
                CmdSpawnBullet();
                reloadTime = 0;
            }
            
            
        }

        if(reloadTime >= 2.0f || powerUp == true)
        {
            Renderer rend = this.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>();
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.red);
        }
        if(reloadTime <= 2.0f && powerUp == false)
        {
            Renderer rend = this.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>();
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.white);
        }
    }

    [Command]
    void CmdDestroy()
    {
        NetworkServer.Destroy(this.gameObject);
    }

    [Command]
    void CmdSpawnBullet()
    {
        AudioSource aud = GetComponent<AudioSource>();
        aud.time = 34.7f;
        aud.Play();
        GameObject bul = Instantiate(bullet, bulletSpawn.transform.position, transform.rotation);
        bul.GetComponent<BulletScript>().shootingTank = this.gameObject;
        bul.GetComponent<Rigidbody>().AddForce(bul.transform.right * 2000);
        NetworkServer.Spawn(bul);
    }
}

