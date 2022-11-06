using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitcherManager : MonoBehaviour
{
    //reference to this manager
    public static SwitcherManager thisManager;
    //keeps track of the last level played
    public int lastLevel = 0;
    //keeps track of player health
    public int playerHealth = 0;

    //add references to anything else we want between levels here

    //sets the reference to this manager and gets rid of others in the scene
    private void Awake()
    {
        if (thisManager != null)
        {
            Destroy(gameObject);
            return;
        }

        thisManager = this;
        DontDestroyOnLoad(gameObject);//keeps object from being destroyed
    }

    //call this when the game is reset to reset the vales here
    public void Reset()
    {
        lastLevel = 0;
        playerHealth = 0;
        //reset anything else here
    }
}
