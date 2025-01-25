using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Monster : MonoBehaviour
{
    public Text lifeText;

    GameObject m_Monster;
    public Text Text;
    Color monsterColor;
    [HideInInspector] public int life;
    
    public GameObject Bullet;
    public bool BulletCreated;

    GameObject player;
    Vector3 mov;
    NavMeshAgent agent;
    float countChangeDir;
    float veloc;
    int velocMult;
    float xValue;
    float zValue;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        veloc = 20;
        life = 3;
        monsterColor = GetComponent<Renderer>().material.color;
        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = true;
        countChangeDir = 0;
        velocMult = 1;
        Random.Range(-veloc, veloc);
        mov = new Vector3(Random.value, 0, Random.value);

        BulletCreated = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerIsNear();
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
                    
                    }
                }
            }

        }


        

        if (Vector3.Distance(transform.position, player.transform.position) < 30)
        {
            lifeText.text = life.ToString();
            velocMult = 5;
            GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            
            Vector3 vec = (player.transform.position - transform.position).normalized;
                Debug.DrawLine( transform.position, player.transform.position, Color.cyan,1f);
                agent.Move(vec * velocMult * Time.deltaTime);
            Shooting();

            gameObject.transform.LookAt(player.transform);
        }
        else 
        {
            BulletCreated = false; 
            
            if (Vector3.Distance(transform.position, player.transform.position) < 60)
                velocMult = 15;
            else
            {
                
                velocMult = 0;
            }
            
            countChangeDir++;
            if (countChangeDir > 600)
            {
                xValue = Random.value;
                zValue = Random.value;
                countChangeDir = 0;
                mov = new Vector3(xValue, 0, zValue);

            }
            

            agent.Move(mov * velocMult * Time.deltaTime);


        }

        //enemy attack
        Die();
    }

    void Die()
    {
        if(life <= 0)
        {
            lifeText.text = "0";
            Text.text = "Hunted";
            Destroy(this.gameObject);
            //gameObject.SetActive(false);
        }
    }

    bool PlayerIsNear()
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= 5) return true;
        return false; 
    }

    
    
    public void Shooting()
    {

        
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 30f))
        {
            var obj = hit.collider.gameObject;
            if (obj.CompareTag("Player"))
            {
                if (!BulletCreated)
                {
                    Instantiate<GameObject>(Bullet, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.position.z), Quaternion.identity, this.gameObject.transform);
                    BulletCreated = true;
                }
            }
        }
        
    }
}
