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
    // Start is called before the first frame update
    void Start()
    {
        //ideally this won't need to be called here
    }

    // Update is called once per frame
    void Update()
    {
        //ideally this won't need to be called here
    }

    //override from character -> define enemy movement here (if it works better in Character feel free to move/change things)
    protected override void Move()
    {
        //this can wait till after sprint 2
    }

    //override from character -> runs when health is 0
    protected override void Die()
    {
        //should maybe run a death animation or change the color of the sprite and delete it
        //this can wait till after sprint 2
    }

    private void OnCollisionEnter(Collision other)
    {
        print("running");
        if(other.transform.tag == "Bullet")
        {
            print("Hit");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        print("running");
        if (other.transform.tag == "Bullet")
        {
            print("Hit");
        }
    }
}
