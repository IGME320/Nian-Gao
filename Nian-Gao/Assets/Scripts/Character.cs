using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

//current namimg/coding conventions
//1) add comments to new code so everyone can quickly tell what it does
//2) name functions like this: ExampleFuntion()
//3) name variables like this: exampleVar
//4) explain your functions with comments, I don't care how simple they are, just do it
//this is all I have rn feel free to add more as they come up

//parent class for both player and enemy
public abstract class Character : MonoBehaviour
{
    //fields
    //Feel free to add more as things come up
    public int currentHealth;
    public int maxHealth;
    public HealthBar healthBar;

    public Vector3 position;
    public int shotDamage;//damage of a character's bullets
    public string shotType;//type of shot they are shooting, thought that having a field like this would make powerups easier
    protected string[] shotTypeArray = { "normal", "spread-shot", "speed-shot" };//array of different shot types for ^^

    //reference to the bullet prefab
    public ParticleSystem bullet;

    // Start is called before the first frame update
    void Start()
    {
        //sets position
        position = transform.position;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //When called will emit n particles, default of 1
    protected void Shoot(int n = 1)
    {
        bullet.Emit(n);
    }

    //runs when a character gets hit will a bullet
    //param: damage: the amount of damage to be taken
    void TakeDamage(int damage)
    {
        //takes however much health, provides some sort of visual, and checks for death?
        //can wait until after sprint 2

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        //checks if health is 0 and runs death if it is
        if (currentHealth == 0)
        {
            Die();
        }
    }

    //player and enemy will have slightly different methods of moving (being controlled or not) so this will be defined elsewhere
    abstract protected void Move();
    //again death will mean different things for both
    abstract protected void Die();

}
