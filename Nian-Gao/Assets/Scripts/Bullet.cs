using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    //Variables and constants
    public float SPEED = 10;
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
    
        rb.velocity = new Vector2(SPEED*speedMultiplier*Xdirection, SPEED*speedMultiplier*Ydirection);//Moves the bullet at a fixed veloccity (x,y)
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Border")
        {
            Destroy(self);
        }
        if(collision.transform.tag == "Shield" && transform.tag != "Bullet")//if an enemy bullet strikes the shield
        {
            Destroy(self);
        }    
        if(collision.transform.tag == "Bullet" || collision.transform.tag == "EnemyBullet")
        {
            if(transform.tag == "Bullet")//Specifies that only player bullets count toward the collisions
            {
                GameObject.Find("Player").GetComponent<Player>().BCol++;//inrements bullet collision tracker
            }
            
            Destroy(collision.gameObject);
        }
    }
    
    //used for changing direction of velocity
    public void SetXDirection(float dir)
    {
        Xdirection = dir;
    }

    //used for changing direction of velocity
    public void SetYDirection(float dir)
    {
        Ydirection = dir;
    }

    //changes speed of bullet
    public void SetSpeed(float speed)
    {
        SPEED = speed;
    }
}
