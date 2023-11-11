using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    //reference to the game grid
    GameGrid grid;

    //if this AI is red, then all of their booleans for isMyTurn should be 
    //set normally from the events, but if they aren't red then they are 
    //yellow and this means that their isMyTurn should be the opposite
    //of what the event returns because the events are based off of a 
    //boolean called isRed.
    private bool amIRed;
    private bool isMyTurn;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GameGrid>();
        //add callback to OnSwitchTurn global event for our OnSwitchTurn method.
        SwitchTurnEvent.OnSwitchTurn += OnSwitchTurn;
        WinEvent.OnWin += OnWin;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSwitchTurn(bool isRedTurn)
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

    private void OnWin(bool didRedWin)
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
