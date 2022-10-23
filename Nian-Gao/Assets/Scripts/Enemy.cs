using System;
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
    private bool shooting = true;
    public GameObject bullet;//The bullet object reference
    public GameObject player; //player object reference (must be the one in the scene)
    public float maxSpeed;//fastest enemy can go (thinking 1 or 2 slower than player)

    [SerializeField]
    private int bulletsAmount = 10;
    [SerializeField]
    private float startAngle = 0f, endAngle = 360f;

    private Vector2 bulletMoveDirection;
    

    /*shotArray
     * Creates the pattern of different shot types
     * 1 = SingleShot()
     * 2 = CircleForwardShot()
     * 3 = CircleShot()
    */ 
    private int[] shotArray = new int[] { 1, 2, 1, 3 };
    private int index = 0; //index of array

    //gets the scene switcher
    public GameObject switchScene;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        setHealth(200);
    }

    // Update is called once per frame
    void Update()
    {
        //ideally this won't need to be called here
        //transform.position = new Vector2(transform.position.x - .1f, transform.position.y); 
    }

    //An update that occures at fixed intervals to bypass any frame inconsistancy
   void FixedUpdate()
    {
        if(shooting)
        {
            switch(shotArray[index])
            {
                case 1:
                    SingleShot();
                    index++;
                    break;
                case 2:
                    CircleForwardShot();
                    index++;
                    break;
                case 3:
                    CircleShot();
                    index++;
                    break;
                default:
                    index = 0;
                    break;
            }
            if(index >= shotArray.Length)
            {
                index = 0;
            }
            shooting = false;//The player can no longer shoot
            StartCoroutine(ToggleShoot());//Calls a coroutine to wait and let the player shoot after a small delay
        }
        Move();
    }

    //override from character -> define enemy movement here (if it works better in Character feel free to move/change things)
    protected override void Move()
    {
        //temp vector2 to hold the new velocity
        Vector2 moveVelocity = Vector2.zero;

        //gets a reference to all the player bullets
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        //if there are bullets to avoid, attempts to avoid the close ones
        if(bullets != null)
        {
            //gets absolute value of current enemy x and y
            float x = Math.Abs(rb.GetComponent<Rigidbody2D>().position.x);
            float y = Math.Abs(rb.GetComponent<Rigidbody2D>().position.y);

            //loops through the bullets
            foreach (GameObject b in bullets)
            {
                //gets absolute value of bullet x and y
                float bx = Math.Abs(b.GetComponent<Rigidbody2D>().position.x);
                float by = Math.Abs(b.GetComponent<Rigidbody2D>().position.y);

                //checks if bullet is close enough to avoid
                if (x - bx < 6.0f || y - by < 4.5f)//these values seemed to work right, but feel free to change them
                {
                    moveVelocity += Flee(b.GetComponent<Rigidbody2D>().position);
                }
                
            }
        }

        //if there are no bullets close enough
        if(moveVelocity == Vector2.zero)
        {
            //calls seek to follow the player
            moveVelocity += Seek(player.GetComponent<Rigidbody2D>().position);
        }

        rb.velocity = moveVelocity.normalized * maxSpeed;
    }

    //helper function that steers the enemy towards the player
    protected Vector2 Seek(Vector2 targetPos)
    {
        //calculate our desired velocity
        //a vector towards target position
        Vector2 desiredVelocity = targetPos - rb.position;

        //scales to max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        //calculate the seek steering force
        Vector2 seekingForce = desiredVelocity - rb.velocity;

        return seekingForce;
    }

    //helper function that steers the enemy away from bullets
    protected Vector2 Flee(Vector2 targetPos)
    {
        //calculate our desired velocity
        //a vector away from target position
        Vector2 desiredVelocity = targetPos - rb.position;

        //scales to max speed
        desiredVelocity = -1.0f * (desiredVelocity.normalized * maxSpeed);

        //calculate the seek steering force
        Vector2 fleeingForce = desiredVelocity - rb.velocity;

        return fleeingForce;
    }

    //override from character -> runs when health is 0
    protected override void Die()
    {
        //Set the GameObject's Color to grey
        spriteSkin.color = Color.grey;

        //removes gameObject after 1 second
        Destroy(gameObject, 1);

        //Goes to win screen
        switchScene.GetComponent<SceneSwitcher>().Win();

    }

    //checks for collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Bullet")//Detects collision with a bullet
        {
            rb.velocity = new Vector2(0f, 0f);//Sets velocity to zero so this object is not pushed by the bullet
            Destroy(collision.gameObject);//Destroys the colliding bullet
            TakeDamage(10);
        }
    }

    //This coroutine will be used to toggle shooting on and off so that more than one particle can be used at one time
    private IEnumerator ToggleShoot()
    {
        yield return new WaitForSeconds(1.25f);//Waits for a short amount of time
        shooting = true;//lets the enemy shoot again
    }

    // Shoots a ring of bullets that expand as they travel
    private void CircleForwardShot()
    {
        float angleStep = (endAngle - startAngle) / bulletsAmount;
        float angle = startAngle;

        for (int i = 0; i < bulletsAmount + 1; i++)
        {
            //The angle detrinators
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            //builds position, I don't think we really need this but incase we ever switch to a more normal bullet system I am
            //leaving it as it doesn't hurt anything
            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            //Creates the bullets and sets their velocity directions and speed.
            GameObject b = Instantiate(bullet, bulMoveVector, Quaternion.identity);//instantiates a bullet
            b.GetComponent<Bullet>().SetXDirection(-bulDirX);
            b.GetComponent<Bullet>().SetYDirection(bulDirY); //can easily change to a decreasing ring size by making negative (looks cool)
            b.GetComponent<Bullet>().SetSpeed(1);
            angle += angleStep;
        }
        
    }

    //Normal straight ahead 1 bullet shot
    private void SingleShot()
    {
        GameObject b = Instantiate(bullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);//instantiates a bullet
        b.GetComponent<Bullet>().SetXDirection(-1f);
        b.GetComponent<Bullet>().SetYDirection(0f);
        b.GetComponent<Bullet>().SetSpeed(5);
    }

    //Shoots 8 bullets out in all directions
    private void CircleShot()
    {
        //left bullet
        GameObject left = Instantiate(bullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);//instantiates a bullet
        left.GetComponent<Bullet>().SetXDirection(-1f);
        left.GetComponent<Bullet>().SetYDirection(0f);
        left.GetComponent<Bullet>().SetSpeed(3);
        //right bullet
        GameObject right = Instantiate(bullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);//instantiates a bullet
        right.GetComponent<Bullet>().SetXDirection(1f);
        right.GetComponent<Bullet>().SetYDirection(0f);
        right.GetComponent<Bullet>().SetSpeed(5);
        //up bullet
        GameObject up = Instantiate(bullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);//instantiates a bullet
        up.GetComponent<Bullet>().SetXDirection(0f);
        up.GetComponent<Bullet>().SetYDirection(1f);
        up.GetComponent<Bullet>().SetSpeed(3);
        //down bullet
        GameObject down = Instantiate(bullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);//instantiates a bullet
        down.GetComponent<Bullet>().SetXDirection(0f);
        down.GetComponent<Bullet>().SetYDirection(-1f);
        down.GetComponent<Bullet>().SetSpeed(3);
        //left down bullet
        GameObject leftDown = Instantiate(bullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);//instantiates a bullet
        leftDown.GetComponent<Bullet>().SetXDirection(-1f);
        leftDown.GetComponent<Bullet>().SetYDirection(-1f);
        leftDown.GetComponent<Bullet>().SetSpeed(3);
        //right up bullet
        GameObject rightUp = Instantiate(bullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);//instantiates a bullet
        rightUp.GetComponent<Bullet>().SetXDirection(1f);
        rightUp.GetComponent<Bullet>().SetYDirection(1f);
        rightUp.GetComponent<Bullet>().SetSpeed(3);
        //left up bullet
        GameObject leftUp = Instantiate(bullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);//instantiates a bullet
        leftUp.GetComponent<Bullet>().SetXDirection(-1f);
        leftUp.GetComponent<Bullet>().SetYDirection(1f);
        leftUp.GetComponent<Bullet>().SetSpeed(3);
        //right down bullet
        GameObject rightDown = Instantiate(bullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);//instantiates a bullet
        rightDown.GetComponent<Bullet>().SetXDirection(1f);
        rightDown.GetComponent<Bullet>().SetYDirection(-1f);
        rightDown.GetComponent<Bullet>().SetSpeed(3);
    }

}
