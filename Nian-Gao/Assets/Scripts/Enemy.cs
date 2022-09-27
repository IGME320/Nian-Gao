using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        //Fetch the SpriteRenderer from the GameObject
        //spriteSkin = GetComponent<SpriteRenderer>();

        //Set the GameObject's Color to grey
        spriteSkin.color = Color.grey;

        //for testing, doesn't run rn tho
        UnityEngine.Debug.Log("death");

        //removes gameObject after 5 seconds
        Destroy(gameObject, 5);
        
    }
}
