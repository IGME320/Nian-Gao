using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    //Variables and constants
    const int SPEED = 5;
    public float speedMultiplier;
    private Rigidbody2D rb;
    public GameObject self;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(SPEED*speedMultiplier, 0f);//Moves the bullet at a fixed veloccity
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Border")
        {
            Destroy(self);
        }
    }
}
