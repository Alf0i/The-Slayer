using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class Player : MonoBehaviour
{
    public Event Event;
    // Start is called before the first frame update
    
    
    public float mouseSens = 100f;
    public Transform orientation;
    public Transform playerObj;
    public Transform cam;
    public float RotSpeed;
    Rigidbody rb;
    public float veloc = 10;
    public float jump;
    public float verticalForce;
    public float gravMult;
    private float grav;
    Vector3 moviment;
    Vector3 viewDir;
    CharacterController controller;
    public bool canAttack;
    public bool Attacked;
    float AttackCount;
    Vector3 mov;
    void Start()
    {
        moviment = Vector3.zero;
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        grav = 10f;
        canAttack = false;
        Attacked = false;
        AttackCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
            canAttack = true;

        else if (Input.GetKeyUp(KeyCode.Mouse1))
            canAttack = false;

        float hMov = Input.GetAxis("Horizontal");
        float vMov = Input.GetAxis("Vertical");

        // JUMP
        if (controller.isGrounded)
        {
            if (verticalForce < 0.0f)
            {
                verticalForce = -1f;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                verticalForce += jump;


            }
        }
        else
        {
            verticalForce -= grav * gravMult * Time.deltaTime;
        }

        if (Attacked)
        {
            AttackCount++;
            if (AttackCount > 20)
            {
                Attacked = false;
                AttackCount = 0;
            }
        }
        

        // Camera Rotation and Character Rotation
        if (canAttack)
        {

            viewDir =  cam.forward;
            viewDir.y = 0;
            playerObj.forward = viewDir;
            Vector3 inputdir1 = cam.forward * vMov + cam.right * hMov;
            mov = inputdir1 * veloc;
        }
        else
        {
            viewDir = gameObject.transform.position - new Vector3(cam.transform.position.x, gameObject.transform.position.y, cam.transform.position.z);
            orientation.forward = viewDir.normalized;


            
            Vector3 inputdir2 = orientation.transform.forward * vMov + orientation.right * hMov;

            if (inputdir2 != Vector3.zero)
            {
                inputdir2.Normalize();
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputdir2, Time.deltaTime * RotSpeed);

            }

            mov = inputdir2 * veloc;
        }

        // Moviment
        mov.y = verticalForce;
        controller.Move(mov * Time.deltaTime);

        
    }
}

