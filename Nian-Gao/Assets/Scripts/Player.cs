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
    //gets the scene switcher
    public GameObject switchScene;

    //shooting
    private bool shooting = true;//Used to toggle shooting on and off
    public GameObject bullet;//The bullet object reference
    public float shootDelay;//The delay between bullet spawns

    //power up tracking
    private PowerUp currentBPU;//The current powerUp that changes player bullet pattern
    public GameObject shieldObj;//The shield power up that the player will use
    private bool shieldAct = false;//is the shield ative

    //power up spawning variables
    private int bCollisions = 0; //The number of bullet collisions
    public GameObject powerUp;//reference to the powerup objecct for spawning
    public int BCol//public get set for use in the bullet script
    {
        get { return bCollisions; }
        set { bCollisions = value; }
    }
    

    public float dashSpeed;//How far character goes when dashing
    public float startDashTime;
    private float dashTime;

    public bool isflipped;

    Vector2 lastCommand;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        setHealth(200);
        dashTime = 0;
        lastCommand = new Vector2(1,0);
        
        //Needed set up for the shield powerup
        shieldObj.layer = 13;	
        shieldObj.gameObject.SetActive(false);	
        currentBPU = PowerUp.None;
        
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

        //Checks for power up spawning
        SpawnPowerUp();
    }
    //Checks for collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //checks for bullet collisions
        if (collision.transform.tag == "EnemyBullet")
        {
            if(!shieldAct)//if the shield is not active
            {
                TakeDamage(10);//take damage
            }
            Destroy(collision.gameObject);
            rb.AddForce(-collision.rigidbody.velocity);
        }
        //checks for enemy collisions
        if (collision.transform.tag == "Enemy")
        {
            if (!shieldAct)//if the shield is not active
            {
                TakeDamage(20);//take damage
            }
            rb.AddForce(-collision.rigidbody.velocity);
        }

        //Checks for power up collision
        if (collision.transform.tag == "Power Up")
        {
            ApplyPowerUp(collision.gameObject.GetComponent<PowerUps>().PowerUpType, collision.gameObject.GetComponent<PowerUps>().Duration);
            Destroy(collision.gameObject);
        }
    }


    //Shooting related methods
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
            Vector3 emitPos = new Vector3(transform.position.x - 1f, transform.position.y + 1f, transform.position.z);//sets the position that the base bullet will be fired from
            Quaternion emitQuat = Quaternion.identity;//the angle at which the bullet is rotated, if the player is flipped
            if(isflipped == true){
                emitQuat = Quaternion.Euler(0, 0, 180);//flips the bullet to match player flipping
            }                
            else if(isflipped == false){
                emitQuat=  Quaternion.identity;
            }
            
            //Checks whaty the current shooting power up is, and spawnd bullets and changes delay acordingly
            switch (currentBPU)
            { 
                case PowerUp.None://Base shooting
                    CreateBullet(emitPos, emitQuat, target);
                    delayMod = 1f;
                    break;
                case PowerUp.QuickShot://Shoots 2x fire
                    CreateBullet(emitPos, emitQuat, target);
                    delayMod = .5f;
                    break;
                case PowerUp.SpreadShot://emits 3 bullets vertically stacked that diverge
                    CreateBullet(emitPos, emitQuat, target);
                    CreateBullet(new Vector3(emitPos.x, emitPos.y + 1, emitPos.z), emitQuat, Quaternion.Euler(emitQuat.x, emitQuat.y,emitQuat.z+12.5f)*target);
                    CreateBullet(new Vector3(emitPos.x, emitPos.y - 1, emitPos.z), emitQuat, Quaternion.Euler(emitQuat.x, emitQuat.y,emitQuat.z - 12.5f)*target);
                delayMod = 1f;
                break;
                case PowerUp.StackSpread://emits 3 bullets vertically stacked that travel in parallel
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
        b.GetComponent<Bullet>().SetSpeed(5);
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

    //powerup related methods
    /// <summary>
    /// Togggles the shiled power up off after its duration runs out
    /// </summary>
    /// <param name="powerUpLength">The duration of the power up</param>
    /// <returns></returns>
    private IEnumerator ToggleShield(float powerUpLength)
    {
        yield return new WaitForSeconds(powerUpLength);        
        
        //turns the shield off
        shieldObj.layer = 13;
        shieldObj.gameObject.SetActive(false);
        shieldAct = false;

        //makes sure that the player is on
        gameObject.SetActive(true);
        gameObject.layer = 8;
    }
    /// <summary>
    /// Toggles currentBPU so that there are not multiple types of power ups that change the shot type
    /// </summary>
    /// <param name="powerUpLength">Duration of the power up</param>
    /// <returns></returns>
    private IEnumerator ToggleShootPowerUp(float powerUpLength)
    {
        yield return new WaitForSeconds(powerUpLength);
        currentBPU = PowerUp.None;
    }
    /// <summary>
    /// This method applies the effects of all powerups, and starts timers for their duration
    /// </summary>
    private void ApplyPowerUp(PowerUp newPower, float duration)
    {
        switch(newPower)
        {
            case PowerUp.None:
                break;
            case PowerUp.QuickShot:
            case PowerUp.StackSpread:
            case PowerUp.SpreadShot:
                StopCoroutine("TogglleShootPowerUp");//ends the previous shot type
                StartCoroutine(ToggleShootPowerUp(duration));//starts the new shot type
                currentBPU = newPower;
                break; 
            case PowerUp.Heal:
                health += maxHealth * .5f;
                break;
            case PowerUp.Shield:
                StopCoroutine("ToggleShield");//Ends the previous shield powerup, esseantially this resets the duration fo the shield powerup
                shieldObj.layer = 8;
                shieldObj.gameObject.SetActive(true);
                shieldAct = true;
                StartCoroutine(ToggleShield(duration));
                break;
        }
    }
    /// <summary>
    /// Spawns a power up on the player's loation once the right amount of bullet collisions is counted.
    /// </summary>
    private void SpawnPowerUp()
    {
        if(bCollisions == 20)//Once 20 collisions are counted
        {
            bCollisions = 0;//resets the collision counter

            Instantiate(powerUp, transform.position, Quaternion.identity);//Creates a random power up on the player
        }
    }

    public void TurnOffPowerUPs()
    {
        //stops all coroutines and resets powerup list
        StopAllCoroutines();

        currentBPU = PowerUp.None;//Resets the current bullet pattern powerup

        //turns the shield off
        shieldObj.layer = 13;
        shieldObj.gameObject.SetActive(false);
        shieldAct = false;

        //makes sure that the player is on
        gameObject.SetActive(true);
        gameObject.layer = 8;
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
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButton(1)) && dashTime <= 0){
            dashTime = startDashTime;
            //Debug.Log("dash");
        }
        if(Input.GetAxisRaw("Horizontal") != 0) 
            lastCommand = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        else if(Input.GetAxisRaw("Vertical") != 0)
            lastCommand = new Vector2(0, Input.GetAxisRaw("Vertical"));

        if(dashTime > 0){
            dashTime -= Time.deltaTime;
            
            rb.velocity = new Vector2(rb.velocity.x + (Input.GetAxisRaw("Horizontal")*dashSpeed), rb.velocity.y + (Input.GetAxisRaw("Vertical")*dashSpeed));

            if(Input.GetAxisRaw("Horizontal") != 0 && (Input.GetAxisRaw("Vertical") != 0))
                rb.velocity = rb.velocity + lastCommand * (dashSpeed);
            else
                rb.velocity = rb.velocity + lastCommand * (dashSpeed+50);

            //Debug.Log(lastCommand);

            
        }
    }


    //override from character -> runs when health is 0
    protected override void Die()
    {
        //Set the GameObject's Color to grey
        sr.color = Color.grey;

        TurnOffPowerUPs();//turns off power ups

        //removes gameObject after 2 seconds
        Destroy(gameObject, 2);

        //switches the scene
        switchScene.GetComponent<SceneSwitcher>().Restart();
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
