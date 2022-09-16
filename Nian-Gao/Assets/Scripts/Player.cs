using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//idk what input system you want but make sure to add it here

//current namimg/coding conventions
//1) add comments to new code so everyone can quickly tell what it does
//2) name functions like this: ExampleFuntion()
//3) name variables like this: exampleVar
//4) explain your functions with comments, I don't care how simple they are, just do it
//this is all I have rn feel free to add more as they come up

public class Player : Character
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


    //override from character -> define player movement here (if it works better in Character feel free to move/change things)
    protected override void Move()
    {

    }

    //override from character -> runs when health is 0
    protected override void Die()
    {
        //should maybe run a death animation or change the color of the player sprite and prompt player to play again
        //this can wait till after sprint 2
    }

    //changes the player's shot
    private void ChangeShot()
    {
        //This can wait till after sprint 2
        //used to change player's shot type/damage after a powerup or level up
    }

    //when player presses KEY eating happens
    private void Eat()
    {
        //This can wait till after sprint 2
    }
}
