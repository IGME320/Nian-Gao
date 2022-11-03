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

    // Update is called once per frame
    void Update()
    { 
        transform.position -= new Vector3(moveSpeed,0,0);

        if(transform.position.x <= -38){
            Instantiate(background, new Vector3(76, 0, 1), Quaternion.Euler(0, 0, 90));
            Destroy(this.gameObject);
        }
    }
}
