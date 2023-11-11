using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    private bool showMenu = true;

    public Canvas menuCanvas;

    public Canvas gameCanvas;

    public bool shouldAutoSwitch = true;

    public TextMeshProUGUI tMesh;

    public List<GameObject> robotPrefabs = new List<GameObject>();

    public List<Transform> spawnPoints = new List<Transform>();

    public bool souldSpawnRobots = true;

    public TextMeshProUGUI winText;

    private void Start()
    {
        menuCanvas = GetComponent<Canvas>();
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            showMenu = !showMenu;
            menuCanvas.enabled = showMenu;
        }
    }

    public void setScene(string scene)
    {
        GameManager.instance.setScene(scene);
    }

    public void nextLevel()
    {
        GameManager.instance.nextScene();
        //showMenu = !showMenu;
        //menuCanvas.enabled = showMenu;
    }

    public void startLevel()
    {
        showMenu = !showMenu;
        menuCanvas.enabled = showMenu;
        gameCanvas.enabled = !showMenu;
    }

    public void exitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
