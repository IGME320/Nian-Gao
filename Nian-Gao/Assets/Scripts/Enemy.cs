using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//current namimg/coding conventions
//1) add comments to new code so everyone can quickly tell what it does
//2) name functions like this: ExampleFuntion()
//3) name variables like this: exampleVar
//4) explain your functions with comments, I don't care how simple they are, just do it
//this is all I have rn feel free to add more as they come up

public class Enemy : Character
{
    //gets the sprite renderer
    public SpriteRenderer spriteSkin;
    private Rigidbody2D rb;//The enemy's rigid body

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //ideally this won't need to be called here
        //transform.position = new Vector2(transform.position.x - .1f, transform.position.y); 
    }

    //override from character -> define enemy movement here (if it works better in Character feel free to move/change things)
    protected override void Move()
    {
        //this can wait till after sprint 2
    }

    //override from character -> runs when health is 0
    protected override void Die()
    {
        //Set the GameObject's Color to grey
        spriteSkin.color = Color.grey;

        //removes gameObject after 1 second
        Destroy(gameObject, 1);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Bullet")//Detects collision with a bullet
        {
            rb.velocity = new Vector2(0f, 0f);//Sets velocity to zero so this object is not pushed by the bullet
            Destroy(collision.gameObject);//Destroys the colliding bullet
        }
        TakeDamage();
    }
}
