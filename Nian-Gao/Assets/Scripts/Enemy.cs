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
    public bool moveTrack;//if the enemy moves on a track or not
    private Vector2 seekPos = new Vector2(10.16f, -7.32f);//position the enemy seeks it is on a track

    [SerializeField]
    private int bulletsAmount = 10;
    [SerializeField]
    private float startAngle = 0f, endAngle = 360f;



    

    /*shotArray
     * Creates the pattern of different shot types
     * 1 = SingleShot()
     * 2 = CircleForwardShot()
     * 3 = CircleShot()
     * 4 = GridShot()
     * 5 = Rapid shots, shot time span between shots
    */ 
    private int[] shotArray = new int[] { 1, 2, 3, 4, 1, 1, 3, 4, 2, 4 };
    private int[] shotBelowHalf = new int[] { 5, 5, 5, 5, 5, 5, 5, 2, 2, 5, 5, 5, 3, 3, 2 };
    private int index = 0; //index of array

    //gets the scene switcher
    public GameObject switchScene;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        setHealth(1000);
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
            if(healthCheck())
            {
                switch (shotBelowHalf[index])
                {
                    case 1:
                        SingleShot();
                        index++;
                        shooting = false;//The player can no longer shoot
                        StartCoroutine(ToggleShoot(.75f));//Calls a coroutine to wait and let the player shoot after a small delay
                        break;
                    case 2:
                        CircleForwardShot();
                        index++;
                        shooting = false;//The player can no longer shoot
                        StartCoroutine(ToggleShoot(.1f));//Calls a coroutine to wait and let the player shoot after a small delay
                        break;
                    case 3:
                        CircleShot();
                        index++;
                        shooting = false;//The player can no longer shoot
                        StartCoroutine(ToggleShoot(.75f));//Calls a coroutine to wait and let the player shoot after a small delay
                        break;
                    case 4:
                        GridShot();
                        index++;
                        shooting = false;//The player can no longer shoot
                        StartCoroutine(ToggleShoot(.75f));//Calls a coroutine to wait and let the player shoot after a small delay
                        break;
                    case 5:
                        SingleShot();
                        shooting = false;//The player can no longer shoot
                        StartCoroutine(ToggleShoot(.1f));//Calls a coroutine to wait and let the player shoot after a small delay
                        index++;
                        break;
                    default:
                        index = 0;
                        break;
                }
                if (index >= shotBelowHalf.Length)
                {
                    index = 0;
                }
            }
            else
            {
                switch (shotArray[index])
                {
                    case 1:
                        SingleShot();
                        index++;
                        shooting = false;//The player can no longer shoot
                        StartCoroutine(ToggleShoot(1f));//Calls a coroutine to wait and let the player shoot after a small delay
                        break;
                    case 2:
                        CircleForwardShot();
                        index++;
                        shooting = false;//The player can no longer shoot
                        StartCoroutine(ToggleShoot(1f));//Calls a coroutine to wait and let the player shoot after a small delay
                        break;
                    case 3:
                        CircleShot();
                        index++;
                        shooting = false;//The player can no longer shoot
                        StartCoroutine(ToggleShoot(1f));//Calls a coroutine to wait and let the player shoot after a small delay
                        break;
                    case 4:
                        GridShot();
                        index++;
                        shooting = false;//The player can no longer shoot
                        StartCoroutine(ToggleShoot(1f));//Calls a coroutine to wait and let the player shoot after a small delay
                        break;
                    case 5:
                        SingleShot();
                        shooting = false;//The player can no longer shoot
                        StartCoroutine(ToggleShoot(.1f));//Calls a coroutine to wait and let the player shoot after a small delay
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
            }
            
            
            /*shooting = false;//The player can no longer shoot
            StartCoroutine(ToggleShoot(1.25f));//Calls a coroutine to wait and let the player shoot after a small delay*/
        }
        Move();
    }

    //override from character -> define enemy movement here (if it works better in Character feel free to move/change things)
    protected override void Move()
    {
        //temp vector2 to hold the new velocity
        Vector2 moveVelocity = Vector2.zero;

        //if the enemy moves on a track
        if (moveTrack)
        {
            moveVelocity += Track();
        }
        //follows player otherwise
        else
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

    //steers enemy on a square track
    protected Vector2 Track()
    {
        //checks if enemy is close to the seekPos and updates
        if (Math.Abs(rb.position.x - seekPos.x)<1 && Math.Abs(rb.position.y - seekPos.y) < 1)
        {
            if (seekPos == new Vector2(9.85f, 5.14f))//upper right pos
            {
                seekPos = new Vector2(10.16f, -7.32f);
            }
            else if (seekPos == new Vector2(10.16f, -7.32f))//lower right pos
            {
                seekPos = new Vector2(-9.99f, -7.32f);
            }
            else if (seekPos == new Vector2(-9.99f, -7.32f))//lower left pos
            {
                seekPos = new Vector2(-9.99f, 5.14f);
            }
            else if (seekPos == new Vector2(-9.99f, 5.14f))//upper left pos
            {
                seekPos = new Vector2(9.85f, 5.14f);
            }
        }
        //seeks the seekPos
        return Seek(seekPos);
    }

    //override from character -> runs when health is 0
    protected override void Die()
    {
        //Set the GameObject's Color to grey
        spriteSkin.color = Color.grey;

        //removes gameObject after 3 seconds
        Destroy(gameObject, 5);

        //gets player health for next level
        SwitcherManager.thisManager.playerHealth = player.GetComponent<Player>().getHealth();
        //get anything else you might want for the next level here

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
    private IEnumerator ToggleShoot(float time)
    {
        yield return new WaitForSeconds(time);//Waits for a short amount of time
        shooting = true;//lets the enemy shoot again
    }

    // Shoots a ring of bullets that expand as they travel
    private void CircleForwardShot()
    {
        float angleStep = (endAngle - startAngle) / bulletsAmount;
        float angle = startAngle;
        Vector3 direction = player.transform.position - transform.position;
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
            b.GetComponent<Bullet>().SetXDirection((-bulDirX+direction.x)/4);
            b.GetComponent<Bullet>().SetYDirection((bulDirY+direction.y)/4); //can easily change to a decreasing ring size by making negative (looks cool)
            b.GetComponent<Bullet>().SetSpeed(0.5f);
            angle += angleStep;
        }
        
    }

    //Normal straight ahead 1 bullet shot
    private void SingleShot()
    {
        Vector3 direction = player.transform.position - transform.position;  //used to aim at player
        GameObject b = Instantiate(bullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);//instantiates a bullet
        b.GetComponent<Bullet>().SetXDirection(direction.x/20);
        b.GetComponent<Bullet>().SetYDirection(direction.y/20);
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
        right.GetComponent<Bullet>().SetSpeed(3);
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

    //Creates a grid of bullets projected out from the enemy
    public void GridShot()
    {
        float[] angles = new float[] { .5f, .75f, 1f, -.5f, -.75f, -1f };
        foreach (float x in angles)
        {
            foreach (float y in angles)
            {
                GameObject left = Instantiate(bullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);//instantiates a bullet
                left.GetComponent<Bullet>().SetXDirection(x);
                left.GetComponent<Bullet>().SetYDirection(y);
                left.GetComponent<Bullet>().SetSpeed(3);
            }
        }
    }

}
