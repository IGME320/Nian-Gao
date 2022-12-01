using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public float moveSpeed;
    public GameObject background;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frames
    void Update()
    { 
        transform.position -= new Vector3(moveSpeed,0,0);

        if(transform.position.x <= -38){
            transform.position = new Vector3(38, 0, 0);
        }
    }
}
