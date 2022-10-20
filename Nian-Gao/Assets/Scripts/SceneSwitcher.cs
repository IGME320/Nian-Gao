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

        if ((currentScene.name == "Start_Menu" || currentScene.name == "Restart" || currentScene.name == "Win_Restart"))//If any button is pressed and in either the start or restart menus
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);//Loads main scene
        }
    }

    //Loads the restart scene when the player dies
    public void Restart()
    {
        SceneManager.LoadScene("Restart", LoadSceneMode.Single);//loads restart scene when called
    }

    public void Win()
    {
        SceneManager.LoadScene("Win_Restart", LoadSceneMode.Single);//loads Win scene when called
    }
}

