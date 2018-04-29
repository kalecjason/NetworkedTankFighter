using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ZombieScript : NetworkBehaviour
{
    public GameObject bullet;
    public GameObject bulletSpawn;
    float reloadTime;
    bool powerUp;
    float powerUpTimer;
    public GameObject[] path;
    bool playerSeen;
    int ind;

    // Use this for initialization
    void Start()
    {
        CmdSpawn();
        reloadTime = 0.0f;
        powerUp = false;
        powerUpTimer = 0.0f;
        ind = 1;
        playerSeen = false;
        
    }

    private void OnCollisionExit(Collision collision)
    {
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        print("EXIT");
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.CompareTag("Bullet"))
        {
            if (collision.gameObject.GetComponent<BulletScript>().shootingTank != this.gameObject)
            {
                CmdDestroy();
            }

        }*/
        if (collision.gameObject.CompareTag("PowerUp"))
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
    void Update()
    {
       
        reloadTime += Time.deltaTime;
        powerUpTimer -= Time.deltaTime;

        if (powerUpTimer <= 0.0f)
        {
            powerUp = false;
        }






        Vector3 dir = this.transform.right;
        RaycastHit hit;

        if(Physics.Raycast(this.transform.position, dir, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                if (reloadTime >= 2.0f || powerUp == true)
                {
                    CmdSpawnBullet();
                    reloadTime = 0;
                }
            }
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //GameObject closestPlayer = players[0];
        for (int i = 0; i < players.Length; i++)
        {
            Vector3 dir2 = players[i].transform.position - this.transform.position;
            RaycastHit hit2;
            if (Physics.Raycast(this.transform.position, dir2, out hit2))
            {
                if(hit2.transform.gameObject.CompareTag("Player"))
                {
                    playerSeen = true;
                    Quaternion neededRotation2 = Quaternion.LookRotation(hit2.transform.position - gameObject.transform.position);
                    neededRotation2 *= Quaternion.Euler(0, -90, 0);
                    gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, neededRotation2, Time.deltaTime*100.0f);
                    
                }
                else
                {
                    playerSeen = false;
                }
            }
        }

        if(!playerSeen)
        {
            Quaternion neededRotation = Quaternion.LookRotation(path[ind].transform.position - gameObject.transform.position);
            neededRotation *= Quaternion.Euler(0, -90, 0);
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, neededRotation, Time.deltaTime*100.0f);







            if((Quaternion.Angle(transform.rotation, neededRotation)) <= 5.0f)
            {
            transform.position = Vector3.MoveTowards(transform.position, path[ind].transform.position, Time.deltaTime * 5);
            }


            if (path[ind].transform.position == transform.position)
            {
                if (ind == (path.Length - 1))
                {
                    System.Array.Reverse(path);
                    ind = 0;
                }
                else
                {
                    ind++;
                }
            }
        }
        
        



        /*if (hasAuthority == false)
        {
            return;
        }*/

        /* if (Input.GetKey("w"))
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
         if (Input.GetKeyDown("space"))
         {
             if (reloadTime >= 3.0f || powerUp == true)
             {
                 CmdSpawnBullet();
                 reloadTime = 0;
             }
         }*/

        if (reloadTime >= 2.0f || powerUp == true)
        {
            Renderer rend = this.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>();
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.red);
        }
        if (reloadTime <= 2.0f && powerUp == false)
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
    void CmdSpawn()
    {
        NetworkServer.Spawn(this.gameObject);
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

