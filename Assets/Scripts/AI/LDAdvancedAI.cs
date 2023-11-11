using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDAdvancedAI : AIPlayer
{
    Vector2 lastCoinPos;

    //override OnSwitchTurn and use it to make actions during your turn.
    public override void OnSwitchTurn(bool isRedTurn)
    {
        this.isMyTurn = amIRed ? isRedTurn : !isRedTurn;
        if (isMyTurn)
        {
            //place coin in the best evaluated position.
            int column = getBestColumn();
            print(getBestColumn().ToString().Color("yellow"));
            grid.placeCoin(column, this);
            lastCoinPos = new Vector2(column, grid.slots[column].FindLastIndex(s => !s.isEmpty()));

        }
    }

    public override void OnWin(bool didRedWin)
    {
        base.OnWin(didRedWin);
    }

    //gets the column we should place our coin into to make a good move.
    public int getBestColumn()
    {

        /*        //vertical placement
                if (grid.slots[(int)lastCoinPos.x][(int)lastCoinPos.y + 1].isEmpty())
                {
                    return (int)lastCoinPos.x;
                }

                //horizontal placement
                if (lastCoinPos.x + 1 < grid.slots.Count && grid.slots[(int)lastCoinPos.x+1][(int)lastCoinPos.y].isEmpty())
                {
                    return (int)lastCoinPos.x+1;
                }
                else if (lastCoinPos.x - 1 > -1 && grid.slots[(int)lastCoinPos.x - 1][(int)lastCoinPos.y].isEmpty())
                {
                    return (int)lastCoinPos.x - 1;
                }
        */


        //Vertical Placement
        for (int i = 0; i < grid.slots.Count; i++)
        {
            for (int j = 0; j < grid.slots[i].Count - 3; j++)
            {
                if ((!grid.slots[i][j].isEmpty() && grid.slots[i][j].coin.CompareTag(this.tag) || grid.slots[i][j].isEmpty())
                    && (!grid.slots[i][j + 1].isEmpty() && grid.slots[i][j + 1].coin.CompareTag(this.tag) || grid.slots[i][j + 1].isEmpty())
                    && (!grid.slots[i][j + 2].isEmpty() && grid.slots[i][j + 2].coin.CompareTag(this.tag) || grid.slots[i][j + 2].isEmpty())
                    && (!grid.slots[i][j + 3].isEmpty() && grid.slots[i][j + 3].coin.CompareTag(this.tag) || grid.slots[i][j + 3].isEmpty())
                    )
                {
                    return i;
                }
            }
        }

        //Horizontal Placement
        for (int i = 0; i < grid.slots.Count - 3; i++)
        {
            for (int j = 0; j < grid.slots[i].Count; j++)
            {

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

        //default placement.
        if (grid.slots[grid.slots.Count - (grid.slots.Count / 2)][0].isEmpty())
        {
            return grid.slots.Count - (grid.slots.Count / 2);
        }

        //return -1 if we somehow don't decide. This will throw an error.
        return -1;
    }
}
