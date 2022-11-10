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

    [SerializeField] private Camera mainCamera;

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
    private PowerUp currentPU = PowerUp.None;
    

    public float dashSpeed;//How far character goes when dashing
    public float startDashTime;
    private float dashTime;

    public bool isflipped;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        //if this is the first level, sets health to 100, else sets to the health from last level
        if(SwitcherManager.thisManager.playerHealth ==0)
        {
            setHealth(100);
        }
        else
        {
            setHealth(SwitcherManager.thisManager.playerHealth);
            healthbar.SetHealth(getHealth());
            //set anything else you want to pass on here
        }

        dashTime = startDashTime;
        shooting = true;//Makes sure the player is able to shoot at the beggining of levels
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
        flip();
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
    /// Shoots for the player
    /// </summary>
    /// <param name="target">The normalized vector that points from the player to the mouse</param>
    public void ShootPlayer(Vector3 target)
    {
        float delayMod = 1;//The modifier that change delay times

        //Checks if the user is holding left click or the space bar AND if the player is able to shoot
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && shooting == true)
        {
            Vector3 emitPos = new Vector3(transform.position.x - 1f, transform.position.y + 1f, transform.position.z);
            Quaternion emitQuat = Quaternion.identity;
            GameObject b;
            if(isflipped == true){
                emitQuat = Quaternion.Euler(0, 0, 180);
            }                
            else if(isflipped == false){
                emitQuat=  Quaternion.identity;
            }
            else{
                b = null;
            }
            
            switch (currentPU)
            { 
                case PowerUp.None:
                    CreateBullet(emitPos, emitQuat, target);
                    delayMod = 1f;
                    break;
                case PowerUp.QuickShot:
                    CreateBullet(emitPos, emitQuat, target);
                    delayMod = .5f;
                    break;
                case PowerUp.StackSpread:
                    //emits 3 bullets vertically stacked
                    CreateBullet(emitPos, emitQuat, target);
                    CreateBullet(new Vector3(emitPos.x, emitPos.y + 1, emitPos.z), emitQuat, target);
                    CreateBullet(new Vector3(emitPos.x, emitPos.y - 1, emitPos.z), emitQuat, target);
                    delayMod = 1f;
                    break;
            }
            
            shooting = false;//The player can no longer shoot
            StartCoroutine(ToggleShoot(delayMod));//Calls a coroutine to wait and let the player shoot after a small delay
        }
    }

    /// <summary>
    /// Creates a bullet that will travel to the target.
    /// </summary>
    /// <param name="startPos">The starting position of the bullet</param>
    /// <param name="startQuat">The starting orientation of the bullet</param>
    /// <param name="target">The target direction that is being shot in</param>
    private void CreateBullet(Vector3 startPos, Quaternion startQuat, Vector3 target)
    {
        GameObject b = Instantiate(bullet, startPos, startQuat);
        b.GetComponent<Bullet>().SetXDirection(target.x);
        b.GetComponent<Bullet>().SetYDirection(target.y);
        b.GetComponent<Bullet>().SetSpeed(3);
    }

    /// <summary>
    /// Toggles the player's ability to shoot
    /// </summary>
    /// <param name="delayMod">A number between 1 and 0 to alter the time delay</param>
    /// <returns></returns>
    private IEnumerator ToggleShoot(float delayMod = 1)
    {
        yield return new WaitForSeconds(shootDelay*delayMod);//Waits for a short amount of time
        shooting = true;//lets the player shoot again
    }

    private IEnumerator TogglePowerUp(float powerUpLength)
    {
        yield return new WaitForSeconds(powerUpLength);
        currentPU = PowerUp.None;
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

    public void Dash(){
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButton(1)) && dashTime <= 0){
            dashTime = startDashTime;
            Debug.Log("dash");
        }
        if(dashTime > 0){
            dashTime -= Time.deltaTime;
            
            rb.velocity = new Vector2(rb.velocity.x + (Input.GetAxisRaw("Horizontal")*dashSpeed), rb.velocity.y + (Input.GetAxisRaw("Vertical")*dashSpeed));
            
            
            if(Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") != 0){
                rb.velocity = Vector3.forward * dashSpeed;
            }
        }
    }


    //override from character -> runs when health is 0
    protected override void Die()
    {
        //Set the GameObject's Color to grey
        spriteSkin.color = Color.grey;
        shooting = false;//removes the player's ability to shot after they ahve died

        //removes gameObject after 2 seconds
        Destroy(gameObject, 2);

        //switches the scene
        switchScene.GetComponent<SceneSwitcher>().Restart();
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
        if (collision.transform.tag == "Enemy")
        {
            TakeDamage(20);
            rb.AddForce(-collision.rigidbody.velocity);
        }

        //Checks for power up collision
        if(collision.transform.tag == "Power Up")
        {
            currentPU = collision.gameObject.GetComponent<PowerUps>().PowerUpType;//Sets the power up type
            StartCoroutine(TogglePowerUp(collision.gameObject.GetComponent<PowerUps>().Duration));//Starts the timer on the power up
            Destroy(collision.gameObject);
        }
    }


    private void flip(){
        //Debug.Log(transform.position);
        //Debug.Log(mainCamera.ScreenToWorldPoint(Input.mousePosition).x);
        if(transform.position.x > mainCamera.ScreenToWorldPoint(Input.mousePosition).x){
            transform.localScale = new Vector3((float)0.5, (float)-0.5, 1);
            isflipped = true;
        }
        else{
            transform.localScale = new Vector3((float)0.5, (float)0.5, 1);
            isflipped = false;
        }
    }
}
