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

    [SerializeField]
    private int bulletsAmount = 10;
    [SerializeField]
    private float startAngle = 0f, endAngle = 360f;

    private Vector2 bulletMoveDirection;
    
    public int[] shotArray = new int[] { 1, 2 };
    private int count = 0;

    //gets the scene switcher
    public GameObject switchScene;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //InvokeRepeating("CircleShot", 0f, 2f);
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
            switch(shotArray[count])
            {
                case 1:
                    SingleShot();
                    count++;
                    break;
                case 2:
                    CircleShot();
                    count++;
                    break;
                default:
                    count = 0;
                    break;
            }
            if(count >= shotArray.Length)
            {
                count = 0;
            }
            shooting = false;//The player can no longer shoot
            StartCoroutine(ToggleShoot());//Calls a coroutine to wait and let the player shoot after a small delay
        }
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

        //Goes to win screen
        switchScene.GetComponent<SceneSwitcher>().Win();

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

    //This coroutine will be used to toggle shooting on and off so that more than one particle can be used at one time
    private IEnumerator ToggleShoot()
    {
        yield return new WaitForSeconds(1.25f);//Waits for a short amount of time
        shooting = true;//lets the enemy shoot again
    }

    private void CircleShot()
    {
        float angleStep = (endAngle - startAngle) / bulletsAmount;
        float angle = startAngle;

        for (int i = 0; i < bulletsAmount + 1; i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject b = Instantiate(bullet, bulMoveVector, Quaternion.identity);//instantiates a bullet
            b.GetComponent<Bullet>().SetXDirection(-bulDirX);
            b.GetComponent<Bullet>().SetYDirection(5*bulDirY);
            b.GetComponent<Bullet>().SetSpeed(1);
            angle += angleStep;
        }
        
    }
    private void SingleShot()
    {
        GameObject b = Instantiate(bullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);//instantiates a bullet
        b.GetComponent<Bullet>().SetXDirection(-1f);
        b.GetComponent<Bullet>().SetYDirection(0f);
        b.GetComponent<Bullet>().SetSpeed(5);
    }

}
