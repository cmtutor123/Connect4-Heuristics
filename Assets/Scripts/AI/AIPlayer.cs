using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    //reference to the game grid
    public GameGrid grid;
    //referencee to the UI panel we can set text for.
    public AIPanelController panel;

    //if this AI is red, then all of their booleans for isMyTurn should be 
    //set normally from the events, but if they aren't red then they are 
    //yellow and this means that their isMyTurn should be the opposite
    //of what the event returns because the events are based off of a 
    //boolean called isRedTurn.
    public bool amIRed;
    public bool isMyTurn;

    private void OnEnable()
    {
        //add callback to OnGameStart global event for our OnGameStart method.
        GameStartEvent.OnGameStart += OnGameStart;
        //add callback to OnSwitchTurn global event for our OnSwitchTurn method.
        SwitchTurnEvent.OnSwitchTurn += OnSwitchTurn;
        //add callback to OnWin global event for our OnWin method.
        WinEvent.OnWin += OnWin;
    }

    private void OnDisable()
    {
        //remove callbacks so when we switch scenes these don't get called anymore.
        GameStartEvent.OnGameStart -= OnGameStart;
        SwitchTurnEvent.OnSwitchTurn -= OnSwitchTurn;
        WinEvent.OnWin -= OnWin;
    }

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GameGrid>();
        this.tag = amIRed ? "Red" : "Yellow";

        AIPanelController[] panels = FindObjectsOfType<AIPanelController>();
        //find the panel with the same tag as this object (the same color).
        panel = panels.First(p => p.CompareTag(this.tag));
        //set the name of the UI
        panel.setNameText(this.name);
    }

    // Update is called once per frame
    void Update()
    {
        //ignore putting code here.
    }

    /// <summary>
    /// Called when the "Start" button is pushed
    /// on the main menu of the scene. Use to make
    /// your initial move if you go first.
    /// </summary>
    /// <param name="isRedTurn">True if it is red's turn, false otherwise</param>
    public virtual void OnGameStart(bool isRedTurn)
    {
        //basically, if I am red, and it is red's turn, then it's my turn. 
        //otherwise, I'm yellow and isMyTurn should be the opposite of 
        //isRedTurn.
        //this is called an inline conditional and '?' is the "ternary operator"
        isMyTurn = amIRed ? isRedTurn : !isRedTurn;
    }

    /// <summary>
    /// Called when one of the players places a coin
    /// and the turn switches to the other player.
    /// </summary>
    /// <param name="isRedTurn">True if it is red's turn, false otherwise</param>
    public virtual void OnSwitchTurn(bool isRedTurn)
    {
        //basically, if I am red, and it is red's turn, then it's my turn. 
        //otherwise, I'm yellow and isMyTurn should be the opposite of 
        //isRedTurn.
        //this is called an inline conditional and '?' is the "ternary operator"
        isMyTurn = amIRed ? isRedTurn : !isRedTurn;
        if (isRedTurn)
        {
            print(amIRed ? this.gameObject.name + ": It's My turn!".Color("red") : this.gameObject.name + ": It's Red's turn!".Color("red"));
            panel.setContextText(amIRed ? this.gameObject.name + ": It's My turn!".Color("red") : this.gameObject.name + ": It's Red's turn!".Color("red"));
        }
        else
        {
            print(amIRed ? this.gameObject.name + ": It's Yellow's turn!".Color("yellow") : this.gameObject.name + ": It's my turn!".Color("yellow"));
            panel.setContextText(amIRed ? this.gameObject.name + ": It's Yellow's turn!".Color("yellow") : this.gameObject.name + ": It's my turn!".Color("yellow"));
        }
    }

    /// <summary>
    /// Called when one of the two players
    /// wins the game.
    /// 
    /// You don't really need to use this.
    /// </summary>
    /// <param name="didRedWin">True if red won, false otherwise</param>
    public virtual void OnWin(bool didRedWin)
    {
        if (didRedWin)
        {
            print(amIRed ? "I".Color("red") + " Won!".Color("green") : "Red".Color("red") + " Won!".Color("green"));
        }
        else
        {
            print(amIRed ? "Yellow".Color("yellow") + " Won!".Color("green") : "I".Color("yellow") + " Won!".Color("green"));
        }
    }
}
