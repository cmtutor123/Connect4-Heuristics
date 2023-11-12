using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    //reference to the game grid
    public GameGrid grid;

    //if this AI is red, then all of their booleans for isMyTurn should be 
    //set normally from the events, but if they aren't red then they are 
    //yellow and this means that their isMyTurn should be the opposite
    //of what the event returns because the events are based off of a 
    //boolean called isRed.
    public bool amIRed;
    public bool isMyTurn;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GameGrid>();
        //add callback to OnGameStart global event for our OnGameStart method.
        GameStartEvent.OnGameStart += OnGameStart;
        //add callback to OnSwitchTurn global event for our OnSwitchTurn method.
        SwitchTurnEvent.OnSwitchTurn += OnSwitchTurn;
        //add callback to OnWin global event for our OnWin method.
        WinEvent.OnWin += OnWin;
        this.tag = amIRed ? "Red" : "Yellow";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnGameStart(bool isRedTurn)
    {
        isMyTurn = amIRed ? isRedTurn : !isRedTurn;
        //this is called on game start.
    }

    public virtual void OnSwitchTurn(bool isRedTurn)
    {
        isMyTurn = amIRed ? isRedTurn : !isRedTurn;
        if (isRedTurn)
        {
            print(amIRed ? this.gameObject.name + ": It's My turn!".Color("red") : this.gameObject.name + ": It's Red's turn!".Color("red"));
        }
        else
        {
            print(amIRed ? this.gameObject.name + ": It's Yellow's turn!".Color("yellow") : this.gameObject.name + ": It's my turn!".Color("yellow"));
        }
    }

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
