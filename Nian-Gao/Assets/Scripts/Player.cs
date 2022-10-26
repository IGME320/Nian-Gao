using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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
    //gets the sprite renderer
    public SpriteRenderer spriteSkin;
    //gets the scene switcher
    public GameObject switchScene;

    private bool shooting = true;//Used to toggle shooting on and off
    public GameObject bullet;//The bullet object reference
    public float shootDelay;//The delay between bullet spawns



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        setHealth(100);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //An update that occures at fixed intervals to bypass any frame inconsistancy
    void FixedUpdate()
    {
        Move();//Moves the player
        Vector3 targetDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;//gets the vector between the player and mouse
        transform.rotation = Quaternion.LookRotation(Vector3.back, targetDir);//Rotates the player so that the face the mouse
        transform.Rotate(new Vector3(0f, 0f, 90f));//A correction rotation so that the "front" of the player faces the mouse
        ShootPlayer(targetDir);//Shoots
    }

    /// <summary>
    /// Player Shoot method.
    /// Shoots n particles, default 1
    /// </summary>
    public void ShootPlayer(Vector3 target)
    {
        //Checks if the user is holding left click or the space bar AND if the player is able to shoot
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && shooting == true)
        {
            GameObject b = Instantiate(bullet, new Vector3(transform.position.x + 1f, transform.position.y +1f, transform.position.z), Quaternion.identity);//instantiates a bullet
            
            b.GetComponent<Bullet>().SetXDirection(target.x);//Makes it so that bullets are aimed using the mouse
            b.GetComponent<Bullet>().SetYDirection(target.y);//
            b.GetComponent<Bullet>().SetSpeed(3);//Makes the bullet slightly slower
            shooting = false;//The player can no longer shoot
            StartCoroutine(ToggleShoot());//Calls a coroutine to wait and let the player shoot after a small delay
        }
    }

    //This coroutine will be used to toggle shooting on and off so that more than one particle can be used at one time
    private IEnumerator ToggleShoot()
    {
        yield return new WaitForSeconds(shootDelay);//Waits for a short amount of time
        shooting = true;//lets the player shoot again
    }

    //override from character -> define player movement here (if it works better in Character feel free to move/change things)
    protected override void Move()
    {//sets the velocity variables
        float currentSpeedX = rb.velocity.x;
        float currentSpeedY = rb.velocity.y;
        //is basic speed
        currentSpeedX += (speed * Input.GetAxisRaw("Horizontal"));
        currentSpeedY += (speed * Input.GetAxisRaw("Vertical"));
        //adds damping
        currentSpeedX *= Mathf.Pow(1f - normalDamping, Time.deltaTime * 20f);
        currentSpeedY *= Mathf.Pow(1f - normalDamping, Time.deltaTime * 20f);

        //stops the object is the speed is really small value
        if (currentSpeedX < 1 && currentSpeedX > -1)
        {
            currentSpeedX = 0;
        }
        if (currentSpeedY < 1 && currentSpeedY > -1)
        {
            currentSpeedY = 0;
        }

        //adds speed
        rb.velocity = new Vector2(currentSpeedX, currentSpeedY);

    }

    //override from character -> runs when health is 0
    protected override void Die()
    {
        //Set the GameObject's Color to grey
        spriteSkin.color = Color.grey;

        //removes gameObject after 1 second
        Destroy(gameObject, 1);

        //switches the scene
        switchScene.GetComponent<SceneSwitcher>().Restart();
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

    //Checks for collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //checks for bullet collisions
        if(collision.transform.tag == "EnemyBullet")
        {
            TakeDamage(10);
            Destroy(collision.gameObject);
            rb.AddForce(-collision.rigidbody.velocity);
        }
        //checks for enemy collisions
        else if (collision.transform.tag == "Enemy")
        {
            TakeDamage(20);
            rb.AddForce(-collision.rigidbody.velocity);
        }
    }
}
