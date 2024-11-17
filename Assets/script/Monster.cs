using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Monster : MonoBehaviour
{
    public Text Text;
    Color monsterColor;
    [HideInInspector] public int life;
    
    public GameObject Bullet;
    bool BulletCreated;
    GameObject blt;

    public GameObject player;
    Vector3 mov;
    NavMeshAgent agent;
    float countChangeDir;
    float veloc = 20;
    int velocMult;
    float xValue;
    float zValue;
    bool Changed;


    // Start is called before the first frame update
    void Start()
    {
        life = 3;
        monsterColor = GetComponent<Renderer>().material.color;
        agent = GetComponent<NavMeshAgent>();
        mov = new Vector3(Random.value, 0, Random.value);
        agent.autoTraverseOffMeshLink = true;
        countChangeDir = 0;
        velocMult = 1;
        Random.Range(-veloc, veloc);
        Changed = false;

        BulletCreated = false;
    }

    // Update is called once per frame
    void Update()
    {
        Text.text =  Vector3.Distance(gameObject.transform.position, player.transform.position).ToString();

        if (!PlayerIsNear())
        {
            GetComponent<Renderer>().material.SetColor("_Color", monsterColor);
        }

        if (player.GetComponent<Player>().canAttack)
        {
            if (!player.GetComponent<Player>().Attacked)
            {
                if (PlayerIsNear())
                {

                    
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        Rigidbody rb = GetComponent<Rigidbody>();
                        rb.AddForce(- gameObject.transform.forward * 30,ForceMode.Impulse);
                        life--;
                        player.GetComponent<Player>().Attacked = true;
                        Debug.Log("ATTACK");
                    }
                }

            }
        }

        Die();

        if (Vector3.Distance(transform.position, player.transform.position) < 30)
        {
            BulletCreation();
            velocMult = 5;
            GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            if (!Changed)
            {
                Changed = true;
            }
            else //agent.SetDestination(player.transform.position);
            agent.Move((player.transform.position - transform.position) * Time.deltaTime);
        }
        else 
        {
            Changed = false;
            if (Vector3.Distance(transform.position, player.transform.position) < 60)
                velocMult = 15;
            else
            {
                Changed = false;
                velocMult = 0;
            }
            if (!Changed)
            {
                countChangeDir++;
                if (countChangeDir > 600)
                {
                    xValue = Random.value;
                    zValue = Random.value;
                    countChangeDir = 0;
                    mov = new Vector3(xValue, 0, zValue);

                }
            }

            agent.Move(mov * velocMult * Time.deltaTime);


        }

    }

    void Die()
    {
        if(life <= 0)
        {
            Text.text = "Slaied";
            gameObject.SetActive(false);
        }
    }

    bool PlayerIsNear()
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= 5) return true;
        return false; 
    }

    void BulletCreation()
    {
        if (!BulletCreated)
        {
            blt = Instantiate(Bullet);
            BulletCreated = true;
        }
        else
        {
            if (blt.IsDestroyed())
            {
                BulletCreated = false;
            }  
        }
    }
}
