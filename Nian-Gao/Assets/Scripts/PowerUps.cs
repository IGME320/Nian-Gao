using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUp//These are all the current power up types, they are subject to change
    //Left outside of the class so that all scripts can access them
{
    None,//No power up
    QuickShot,//Increases player fire rate
    StackSpread,//Gives the player a spread shot that is stacked vertically
    SpreadShot,//Gives the player a forking spreadshot
    Heal,//Heals the player
    Shield,//Gives the player a shield to block damage
}
public class PowerUps : MonoBehaviour
{
    private PowerUp pUp;
    public PowerUp PowerUpType//type get so that it can not be manually change in the scene or prefab
    {
        get { return pUp; }
    }

    private SpriteRenderer sprite;//the sprite renderer for this object

    private float duration = 1f;//The duration in seconds the power up lasts
    public float Duration
    {
        get { return duration; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();//gets the sprite renderer


        //determines the type of this power up
        int rand = Random.Range(1, 6);//Generates a random number between 1(inclusive) and 6(exclusive)
        pUp = (PowerUp)rand;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAttributes();
    }

    //Changes the color of the power up and duration based on what it does
    void ChangeAttributes()
    {
        switch(pUp)
        {
            case PowerUp.QuickShot:
                sprite.color = Color.red;
                duration = 5f;
                break;
            case PowerUp.StackSpread:
                sprite.color = Color.yellow;
                duration = 5f;
                break;
            case PowerUp.SpreadShot:
                sprite.color = Color.magenta;
                duration = 5f;
                break;
            case PowerUp.Shield:
                sprite.color = Color.white;
                duration = 10f;
                break;
            case PowerUp.Heal:
                sprite.color = Color.green;
                duration = 0f;
                break;
        }
    }
}
