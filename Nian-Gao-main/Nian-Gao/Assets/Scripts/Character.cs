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
    protected float health;
    protected float maxHealth;
    public Vector2 position;
    public int shotDamage;//damage of a character's bullets
    public string shotType;//type of shot they are shooting, thought that having a field like this would make powerups easier
    protected string[] shotTypeArray = { "normal", "spread-shot", "speed-shot" };//array of different shot types for ^^

    [SerializeField]
    private float iFrames;
    [SerializeField]
    private int flashes;
    [SerializeField]
    protected SpriteRenderer sr;

    //reference to healthbar
    public HealthBar healthbar;

    // Start is called before the first frame update
    void Start()
    {
        //sets position
        position = transform.position;
        //sets max health
        healthbar.SetMaxHealth(health);

    }

    // Update is called once per frame
    void Update()
    {

    }

    //runs when a character gets hit will a bullet or runs into something
    protected void TakeDamage(int damage)
    {
        //subtracts the input damage from health
        health-=damage;
        //updates the healthbar
        healthbar.SetHealth(health);
        StartCoroutine(iFrameMoment());
        //checks for death
        if(health <= 0)
        {
            Die();
        }
    }

    //player and enemy will have slightly different methods of moving (being controlled or not) so this will be defined elsewhere
    abstract protected void Move();
    //again death will mean different things for both
    abstract protected void Die();

    public void setHealth(float _health)
    {
        health = _health;
        maxHealth = _health;
    }

    public float getHealth()
    {
        return health;
    }

    public bool healthCheck()
    {
        if(health <= maxHealth/2)
        {
            return true;
        }
        return false;
    }



    private IEnumerator iFrameMoment(){
        Physics2D.IgnoreLayerCollision(8, 7, true);
        Physics2D.IgnoreLayerCollision(8, 9, true);
        
        for(int i = 0; i<flashes; i++){
            sr.color = new Color(1, 0, 0, 0.5f);
            Debug.Log("hit");
            yield return new WaitForSeconds(0.15f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.15f);
        }
        Physics2D.IgnoreLayerCollision(8, 7, false);
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }
}
