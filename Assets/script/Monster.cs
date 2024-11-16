using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;

public class Monster : MonoBehaviour
{
    public static Monster Instance;

    [HideInInspector] public int life;
    public GameObject player;
    Vector3 mov;
    NavMeshAgent agent;
    float countChangeDir;
    float veloc = 20;
    int velocMult;
    float xValue;
    float zValue;
    bool Changed;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        life = 3;

        agent = GetComponent<NavMeshAgent>();
        mov = new Vector3(Random.value, 0, Random.value);
        agent.autoTraverseOffMeshLink = true;
        countChangeDir = 0;
        velocMult = 1;
        Random.Range(-veloc, veloc);
        Changed = false;
    }

    // Update is called once per frame
    void Update()
    {
        Die();

        if (Vector3.Distance(transform.position, player.transform.position) < 20)
        {
            velocMult = 15;
            
            if (!Changed)
            {
                mov = mov - Vector3.Slerp(player.transform.forward, transform.forward, velocMult);
                Changed = true;
            }
            
        }
        else if (Vector3.Distance(transform.position, player.transform.position) > 20 && Vector3.Distance(transform.position, player.transform.position) < 60)
        {
            Changed = false;
            velocMult = 5;
        }
        else 
        {
            Changed = false;
            velocMult = 1;
        }
         countChangeDir++;
         if(countChangeDir > 1000)
         {
            xValue = Random.value;
            zValue = Random.value;
            countChangeDir = 0;
            mov = new Vector3(xValue, 0, zValue);
         }

        
        agent.Move(mov * velocMult * Time.deltaTime);
    }

    void Die()
    {
        if(life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
