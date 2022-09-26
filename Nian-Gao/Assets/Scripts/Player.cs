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

    public float speed;
    public float normalDamping;

    private Rigidbody2D rb;
    private CapsuleCollider2D cc;
    private bool shooting = true;//Used to toggle shooting on and off

    // Start is called before the first frame update
    void Start()
    {
        //ideally this won't need to be called here
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //An update that occures at fixed intervals to bypass any frame inconsistancy
    void FixedUpdate()
    {
        ShootPlayer(1);
        
    }

    private void FixedUpdate(){
        Move();
    }

    /// <summary>
    /// Player Shoot method.
    /// Shoots n particles, default 1
    /// </summary>
    /// <param name="n">number of particles</param>
    public void ShootPlayer(int n = 1)
    {
        //Checks if the user is holding left click or the space bar AND if the player is able to shoot
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && shooting == true)
        {
            bullet.Emit(n);//Emits n number of particles
            shooting = false;//The player can no longer shoot
            StartCoroutine(ToggleShoot());//Calls a coroutine to wait and let the player shoot after a small delay
        }
    }

    //This coroutine will be used to toggle shooting on and off so that more than one particle can be used at one time
    private IEnumerator ToggleShoot()
    {
        yield return new WaitForSeconds(0.25f);//Waits for a short amount of time
        shooting = true;//lets the player shoot again
    }

    //override from character -> define player movement here (if it works better in Character feel free to move/change things)
    protected override void Move()
    {
        //sets the velocity variables
        float currentSpeedX = rb.velocity.x;
        float currentSpeedY = rb.velocity.y;
        //is basic speed
        currentSpeedX += (speed * Input.GetAxisRaw("Horizontal"));
        currentSpeedY += (speed * Input.GetAxisRaw("Vertical"));
        //adds damping
        currentSpeedX *= Mathf.Pow(1f - normalDamping, Time.deltaTime * 20f);
        currentSpeedY *= Mathf.Pow(1f - normalDamping, Time.deltaTime * 20f);  

        //stops the object is the speed is really small value
        if (currentSpeedX < 1 && currentSpeedX > -1 ){
            currentSpeedX = 0;
        }
        if (currentSpeedY < 1 && currentSpeedY > -1 ){
            currentSpeedY = 0;
        }
        
        //adds speed
        rb.velocity = new Vector2(currentSpeedX, currentSpeedY);
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
