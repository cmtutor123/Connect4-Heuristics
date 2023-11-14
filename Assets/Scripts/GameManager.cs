using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class GameManager
{
    private static GameManager _instance;

    [HideInInspector]
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    [HideInInspector]
    public static Scene currentScene;

    public List<string> scenes = new List<string>()
    {
        "TitleScene",
        "2PlayerScene"
    };

    public int currentIndex = 1;

    public int finalRobotCount = 0;

    private GameManager()
    {

        currentScene = SceneManager.GetActiveScene(); //set current scene.
    }

    public void setScene(string scene)
    {

        //load scene
        if (scene == "TitleScene")
        {
            currentIndex = 1; //reset index;
        }
        SceneManager.LoadScene(scene);

    }

    public void nextScene()
    {
        if (currentIndex == scenes.Count)
        {
            //Debug.Log("LAST SCENE");
            /*if (robots.Count > 0)
            {//if you save at least 1 robot you win.
                setScene("WinScene");
            }*/
            return;
        }
        SceneManager.LoadScene(scenes[currentIndex]);
        Debug.Log(scenes[currentIndex]);
        currentIndex++;
    }


    //use this for slowing down time during things like impacts and right as you die before we switch scenes. Basically it'll do impact frames.
    public IEnumerator slowTime(float scale, float duration)
    {
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }



}





