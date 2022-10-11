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
    //switches the scene based on what the current scene is
    public void ChangeScene()
    {
        //gets current scene
        Scene currentScene = SceneManager.GetActiveScene();
        //loads restart if main is open
        if (currentScene.buildIndex == 0)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        //loads main if restart is open
        else
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    //runs change scene when the restart button is pressed
    public void OnButtonPress()
    {
        ChangeScene();
    }
}

