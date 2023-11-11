using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class LDAIExample : AIPlayer
{
    //override OnSwitchTurn and use it to make actions during your turn.
    public override void OnSwitchTurn(bool isRedTurn)
    {
        this.isMyTurn = amIRed ? isRedTurn : !isRedTurn;
    }

    public override void OnWin(bool didRedWin)
    {
        base.OnWin(didRedWin);
    }

    //gets the column we should place our coin into to make a good move.
    public int getBestColumn()
    {
        //Vertical Check
        for (int i = 0; i < grid.slots.Count; i++)
        {
            for (int j = 0; j < grid.slots[i].Count - 3; j++)
            {
                if (!grid.slots[i][j].isEmpty() && !grid.slots[i][j + 1].isEmpty() && !grid.slots[i][j + 2].isEmpty() && !grid.slots[i][j + 3].isEmpty())
                {
                    if (grid.slots[i][j].coin.CompareTag(grid.slots[i][j + 1].coin.tag) && grid.slots[i][j + 1].coin.CompareTag(grid.slots[i][j + 2].coin.tag) && grid.slots[i][j + 2].coin.CompareTag(grid.slots[i][j + 3].coin.tag))
                    {
                        grid.slots[i][j].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        grid.slots[i][j + 1].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        grid.slots[i][j + 2].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        grid.slots[i][j + 3].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        return i;
                    }
                }
            }
        }

        //return -1 if we somehow don't decide. (just choose a different option)
        return -1;
    }
}
