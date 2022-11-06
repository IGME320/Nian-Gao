using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//current namimg/coding conventions
//1) add comments to new code so everyone can quickly tell what it does
//2) name functions like this: ExampleFuntion()
//3) name variables like this: exampleVar
//4) explain your functions with comments, I don't care how simple they are, just do it
//this is all I have rn feel free to add more as they come up

public class SceneSwitcher : MonoBehaviour
{
    //keeps track of current scene
    Scene currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    //switches the scene based on what the current scene is
    public void ChangeScene()
    {

        if ((currentScene.name == "Start_Menu" || currentScene.name == "Restart"))//If any button is pressed and in either the start or restart menus
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);//Loads main scene
        }
        else if(currentScene.name == "Win_Restart") //takes the player to the next level
        {
            //switch that checks the last won level and sends the player to the next one
            //this is currently way more than needs to be here, but when we add more levels this will come in handy
            switch (SwitcherManager.thisManager.lastLevel)
            {
                case 1:
                    SceneManager.LoadScene("Level2", LoadSceneMode.Single);//loads level 2
                    break;
            }
            
        }
    }

    //Loads the restart scene when the player dies
    public void Restart()
    {
        SwitcherManager.thisManager.Reset();
        SceneManager.LoadScene("Restart", LoadSceneMode.Single);//loads restart scene when called
    }

    //Loads next scene when player wins a level
    public void Win()
    {
        SwitcherManager.thisManager.lastLevel++; //updates the last level won

        if (currentScene.name == "Level2")//this should always be the last level
        {
            SwitcherManager.thisManager.Reset();
            SceneManager.LoadScene("Start_Menu", LoadSceneMode.Single);//loads start when player Win is called in the second level
        }
        else//this runs for all other levels
        {
            SceneManager.LoadScene("Win_Restart", LoadSceneMode.Single);//loads Win scene when called
        }
    }
}

