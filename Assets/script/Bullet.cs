using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    GameObject player;
    Monster monster;
    GameObject terrain;

    bool forceAdded;
   
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        monster = gameObject.GetComponentInParent<Monster>();
        terrain = GameObject.FindWithTag("Ground");
        rb = GetComponent<Rigidbody>();
        forceAdded = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!forceAdded)
        {
            Vector3 direc = (player.transform.position - monster.gameObject.transform.position).normalized;
            rb.AddForce(direc * 3f, ForceMode.Impulse);
            

            forceAdded = true;
        }
        else
        {
            if (Vector3.Distance(transform.position, monster.gameObject.transform.position) > 30)
            {
                monster.BulletCreated = false;
                forceAdded = false;
                Destroy(gameObject);
            }
        }
            
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("PlayerC"))
        {
            Debug.Log("Colidiu 2");
            monster.GetComponent<Monster>().BulletCreated = false;
            forceAdded = false;
            Destroy(gameObject);
            player.GetComponent<Player>().life--;
        }
        else if (collision.collider.gameObject == terrain)
        {
            Debug.Log("Colidiu 3");
            forceAdded = false;
            Destroy(gameObject);
        }
    }
    
}

