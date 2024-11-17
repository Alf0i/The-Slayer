using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    GameObject player;
    GameObject monster;
    Vector3 lastPlayerPosition;
    bool positionDefined;
    bool forceAdded;
   
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        monster = GameObject.FindWithTag("enemy");
        rb = GetComponent<Rigidbody>();
        positionDefined = false;
        forceAdded = false;
        lastPlayerPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        PositionDefinition();
        if (positionDefined)
        {
            
            if (!forceAdded)
            {
                Vector3 vector3 = Vector3.Slerp(lastPlayerPosition, monster.transform.position, 0);
                rb.AddForce(vector3 , ForceMode.Impulse);
                forceAdded = true;
            }
            
            if (Vector3.Distance(transform.position, monster.transform.position) > 30)
            {
                positionDefined = false;
                forceAdded = false;
                Destroy(gameObject);
            }
        }

    }

    void PositionDefinition()
    {
        if (!positionDefined)
        {
            transform.position = monster.transform.position;

            lastPlayerPosition = player.transform.position;
            positionDefined = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == player.GetComponent<CapsuleCollider>())
        {
            positionDefined = false;
            forceAdded = false;
            Destroy(gameObject);
            player.GetComponent<Player>().life--;
        }
        else if (collision.gameObject.tag == "Ground")
        {
            positionDefined = false;
            forceAdded = false;
            Destroy(gameObject);
        }
    }
}

