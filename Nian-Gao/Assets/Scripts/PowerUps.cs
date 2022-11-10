using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUp//These are all the current power up types, they are subject to change
    //Left outside of the class so that all scripts can access them
{
    None,//No power up
    QuickShot,//Increases player fire rate
    StackSpread,//Gives the player a spread shot that is stacked vertically
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
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();//gets the sprite renderer
        

        //determines the type of this power up
        int rand = Random.Range(0, 2) + 1;//Will generate a number between 0 and 2, then will add 1 so that the PowerUp.None is not chosen
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
                duration = 3f;
                break;
            case PowerUp.StackSpread:
                sprite.color = Color.blue;
                duration = 3f;
                break;
        }
    }
}
