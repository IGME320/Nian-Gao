using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    //Variables and constants
    public int SPEED = 10;
    public float speedMultiplier;
    private Rigidbody2D rb;
    public GameObject self;
    public float Xdirection;
    public float Ydirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    
        rb.velocity = new Vector2(SPEED*speedMultiplier*Xdirection, Ydirection);//Moves the bullet at a fixed veloccity
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Border")
        {
            Destroy(self);
        }
    }
    
    public void SetXDirection(float dir)
    {
        Xdirection = dir;
    }
    public void SetYDirection(float dir)
    {
        Ydirection = dir;
    }
    public void SetSpeed(int speed)
    {
        SPEED = speed;
    }
}
