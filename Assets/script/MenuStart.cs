using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour
{
    public SceneAsset scene;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.A))
        {
            SceneManager.LoadScene(scene.name);
            Debug.Log("teste");
        }
            
    }
}
