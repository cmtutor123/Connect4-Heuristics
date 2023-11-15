using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class LDAIExample : AIPlayer
{
    //the last array index we placed a coin at.
    Vector2 lastCoinPos;

    public override void OnGameStart(bool isRedTurn)
    {
        //basically, if I am red, and it is red's turn, then it's my turn. 
        //otherwise, I'm yellow and isMyTurn should be the opposite of 
        //isRedTurn.
        //this is called an inline conditional and '?' is the "ternary operator"
        this.isMyTurn = amIRed ? isRedTurn : !isRedTurn;
        //if it's my turn
        if (isMyTurn)
        {
            //if best spot is open still, let's place a coin there.
            if (grid.slots[3][0].isEmpty())
            {
                //places a coin in the column passed,
                //we also need to pass a reference
                //to this AI script so that the GameGrid
                //can verify it is our turn.
                grid.placeCoin(3, this);
            }
        }
    }

    //override OnSwitchTurn and use it to make actions during your turn.
    public override void OnSwitchTurn(bool isRedTurn)
    {
        //basically, if I am red, and it is red's turn, then it's my turn. 
        //otherwise, I'm yellow and isMyTurn should be the opposite of 
        //isRedTurn.
        //this is called an inline conditional and '?' is the "ternary operator"
        this.isMyTurn = amIRed ? isRedTurn : !isRedTurn;
        if (isMyTurn)
        {
            //get the best evaluated position.
            int column = getBestColumn();
            //print the column we chose.
            print(getBestColumn().ToString().Color("yellow"));
            //places a coin in the column passed,
            //we also need to pass a reference
            //to this AI script so that the GameGrid
            //can verify it is our turn.
            grid.placeCoin(column, this);
            //set lastCoinPos to be the coin we just placed.
            lastCoinPos = new Vector2(column, grid.slots[column].FindLastIndex(s => !s.isEmpty()));
            //grid.slots[column].FindLastIndex(s => !s.isEmpty())
            //is an inline foreach loop that just finds the last index
            //in the List<Slot> where the coin isn't empty. 

        }

        //drawing the context on the context panel for this AI.
        if (isMyTurn)
        {
            panel.setContextText("It's My turn!".Color("white"));
        }
        else
        {
            panel.setContextText("Waiting...".Color("orange"));
        }
    }

    public override void OnWin(bool didRedWin)
    {
        //I'm not going to do anything special here.
        base.OnWin(didRedWin);
    }

    /// <summary>
    /// gets the column we should place our 
    /// coin into to make a good move.
    /// </summary>
    /// <returns>the column index</returns>
    public int getBestColumn()
    {


        #region Vertical Placement
        //loops through the list of list of slots, 2D List[i=x][j=y].
        for (int i = 0; i < grid.slots.Count; i++)
        {
            for (int j = 0; j < grid.slots[i].Count - 3; j++)
            {
                //check that none of the coins that are present
                //are of a different color, if they are all the
                //same color or are empty then we return the
                //first column index that allows us to make a
                //successful move. 
                if ((!grid.slots[i][j].isEmpty() && grid.slots[i][j].coin.CompareTag(this.tag) || grid.slots[i][j].isEmpty())
                    && (!grid.slots[i][j + 1].isEmpty() && grid.slots[i][j+1].coin.CompareTag(this.tag) || grid.slots[i][j + 1].isEmpty())
                    && (!grid.slots[i][j + 2].isEmpty() && grid.slots[i][j + 2].coin.CompareTag(this.tag) || grid.slots[i][j + 2].isEmpty())
                    && (!grid.slots[i][j + 3].isEmpty() && grid.slots[i][j + 3].coin.CompareTag(this.tag) || grid.slots[i][j + 3].isEmpty())
                    )
                {
                    //return column index.
                    return i;
                }
            }
        }
        #endregion

        #region Horizontal Placement
        //loops through the list of list of slots, 2D List[i=x][j=y].
        for (int i = 0; i < grid.slots.Count - 3; i++)
        {
            for (int j = 0; j < grid.slots[i].Count; j++)
            {
                //check that none of the 3 slots to the right
                //contain coins of a different color, if they are
                //all the same color or are empty then we return
                //the first column index that allows us to make a
                //successful move. 
                if ((!grid.slots[i][j].isEmpty() && grid.slots[i][j].coin.CompareTag(this.tag) || grid.slots[i][j].isEmpty())
                    && (!grid.slots[i + 1][j].isEmpty() && grid.slots[i + 1][j].coin.CompareTag(this.tag) || grid.slots[i + 1][j].isEmpty())
                    && (!grid.slots[i + 2][j].isEmpty() && grid.slots[i + 2][j].coin.CompareTag(this.tag) || grid.slots[i + 2][j].isEmpty())
                    && (!grid.slots[i + 3][j].isEmpty() && grid.slots[i + 3][j].coin.CompareTag(this.tag) || grid.slots[i + 3][j].isEmpty())
                    )
                {
                    return i;
                }
            }
        }
        #endregion

        //I didn't give you diagonal placement code.

        //default placement if we somehow didn't choose one of the two
        //options above.
        if (grid.slots[grid.slots.Count - (grid.slots.Count / 2)][0].isEmpty())
        {
            return grid.slots.Count - (grid.slots.Count / 2);
        }

        //return -1 if we somehow don't decide. This will throw an error.
        return -1;
    }
}
