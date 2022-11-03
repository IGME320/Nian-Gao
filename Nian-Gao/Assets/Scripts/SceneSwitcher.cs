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
            SceneManager.LoadScene("Level2", LoadSceneMode.Single);//loads level 2
        }
    }

    //Loads the restart scene when the player dies
    public void Restart()
    {
        SceneManager.LoadScene("Restart", LoadSceneMode.Single);//loads restart scene when called
    }

    //Loads next scene when player wins a level
    public void Win()
    {
        if(currentScene.name == "Level2")
        {
            SceneManager.LoadScene("Start_Menu", LoadSceneMode.Single);//loads start when player Win is called in the second level
        }
        else
        {
            SceneManager.LoadScene("Win_Restart", LoadSceneMode.Single);//loads Win scene when called
        }
    }
}

