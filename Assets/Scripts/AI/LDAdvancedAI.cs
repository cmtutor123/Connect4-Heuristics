using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDAdvancedAI : AIPlayer
{
    public Vector2 slotNeighbourDrawPos = new Vector2(0, 0);
    public Vector2 slotNeighbourRect = new Vector2(1, 1);

    Vector2 lastCoinPos;

    //use this and reference it when we are going to check for all the various possible
    //placements, only check through this list and see if it's neighbours at it's coords
    //are open and then check for if there are any "threats" in the next 3 slots
    //and if there aren't then fill the next adjacent slot. 
    //start with coding horizontal and vertical then do diagonal later.
    private List<Slot> placedCoinSlots = new List<Slot>();

    //override OnSwitchTurn and use it to make actions during your turn.
    public override void OnSwitchTurn(bool isRedTurn)
    {
        this.isMyTurn = amIRed ? isRedTurn : !isRedTurn;
        if (isMyTurn)
        {
            //getNeighbours(0, 0);
            //place coin in the best evaluated position.
            int column = getBestColumn();
            print(getBestColumn().ToString().Color("yellow"));
            grid.placeCoin(column, this);
            lastCoinPos = new Vector2(column, grid.slots[column].FindLastIndex(s => !s.isEmpty()));
            placedCoinSlots.Add(grid.slots[column].FindLast(s => !s.isEmpty()));
        }
    }

    public override void OnWin(bool didRedWin)
    {
        base.OnWin(didRedWin);
    }

    //gets the column we should place our coin into to make a good move.
    public int getBestColumn()
    {
        //Plan for the advanced AI:
        //Check for "Threats" first then block them or prevent them from happening in the first place.
        //Check for areas where it would be easy to setup a win-win situation where the other player 
        //cannot win regardless of which move they make.

        //For the checking of winnable movements we'll do a weighting system,
        //where the weight of a placement increases based on if there are any adjacent coins
        //of the same color, and increases based on how many coins there are of the same color.
        //[Column][Weight]
        Dictionary<int, float> weights = new Dictionary<int, float>();
        weights.Add(0, 0);
        weights.Add(1, 0);
        weights.Add(2, 0);
        weights.Add(3, 0);
        weights.Add(4, 0);
        weights.Add(5, 0);
        weights.Add(6, 0);


        //check a coin, and set it's weight based off of the adjacent coins. 
        //if there is a coin in a specific direction, then we need to get that coin's adjacent coins
        //and if those coins are in the same direction then we increase the weight of the column if 
        //placing a coin there would count as a win.

        //lets start at the origin slot.
        //lets make it so that at the beginning if we are going
        //first or the slot is open we should select the middle
        //slot.

        if (grid.slots[3][0].isEmpty())
        {
            return 3;
        }

        //check neighbours, check if there are any of the same color, and if there are any threats.
        //if there are only empty slots check for threats in all directions 4 out and first direction with
        //no threats will become our target.

        //maybe try to prioritize creating the T shape that creates an unwinnable situation
        //for the other player?

        //WAIT because this is a 3x3 array (roughly) everytime Nvm
        foreach (Slot s in placedCoinSlots)
        {
            int currentDesiredColumn;

            List<List<Slot>> neighbours = getNeighbours((int)s.arrayPosition.x, (int)s.arrayPosition.y, 1, 1);
            //start movement direction for searching if we can create a continuous line of 4 by
            //first looking right, then go through all directions.
            Vector2 currentDir = new Vector2(1, 0);
            Vector2 vertical = new Vector2(0, 1);
            Vector2 diagonalR = new Vector2(1, 1);
            Vector2 diagonalL = new Vector2(-1, 1);

            //if this coin is surrounded by red then skip it.
            if (!neighbours.TrueForAll(s => s.TrueForAll(sl => sl.coin.CompareTag(this.tag))))
            {
                continue;
            }
            
            //horizontal desireability check
            for (int i = 0; i < neighbours[0].Count; i++)
            {
                if (neighbours[i][0].isEmpty())
                {
                    //if the next slot to the right is empty then let's continue looking right to make sure
                    //there are no threats.
                    continue;
                }
                else if (neighbours[i][0].coin.CompareTag(this.tag))
                {
                    //if the tag of the coin is the same as this objects tag (this is our coin)
                    //then let's move to the right and make sure either the slot after this slot
                    //is empty or there are no threats.
                    neighbours = getNeighbours(i, 0, 1, 1);
                    currentDesiredColumn = i;
                }
            }
            
        }

        for (int i = 0; i < grid.slots.Count; i++)
        {
            for (int j = 0; j < grid.slots[i].Count; j++)
            {
                if ((!grid.slots[i][j].isEmpty() && grid.slots[i][j].coin.CompareTag(this.tag) || grid.slots[i][j].isEmpty()))
                {
                    //weights[i] = ;
                }
            }
        }


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

        //return -1 if we somehow don't decide. This will throw an error.
        return -1;
    }

    public void drawNeighbours(int x, int y, int xRadius, int yRadius)
    {
        string s = "";
        int column_limit = grid.slots.Count;
        for (int i = Mathf.Max(0, x-xRadius); i <= Mathf.Min(Mathf.Clamp(x+xRadius, 0, grid.slots.Count - 1), column_limit); i++)
        {
            int row_limit = grid.slots[i].Count;
            for (int j = Mathf.Max(0, y -yRadius); j <= Mathf.Min(Mathf.Clamp(y + yRadius, 0, grid.slots[i].Count - 1), row_limit); j++)
            {
                s += grid.slots[i][j].position.ToString().Color("purple") + " ";
                if (!grid.slots[i][j].isEmpty())
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.magenta;
                }
                Gizmos.DrawSphere(grid.slots[i][j].position, 0.5f);
            }
            s += "\n";
        }
        print(s);
    }

    public List<List<Slot>> getNeighbours(int x, int y, int xRadius, int yRadius)
    {
        List<List<Slot>> neighbours = new List<List<Slot>>(7);

        int column_limit = grid.slots.Count;
        for (int i = Mathf.Max(0, x - xRadius); i <= Mathf.Min(Mathf.Clamp(x + xRadius, 0, grid.slots.Count - 1), column_limit); i++)
        {
            int row_limit = grid.slots[i].Count;
            for (int j = Mathf.Max(0, y - yRadius); j <= Mathf.Min(Mathf.Clamp(y + yRadius, 0, grid.slots[i].Count - 1), row_limit); j++)
            {
                neighbours[i].Add(grid.slots[i][j]);
            }
        }
        return neighbours;
    }

    //draws each slot in one of 8 directions relative to a given slot position
    public void drawDirections(int x, int y, int xRadius, int yRadius)
    {
        string s = "";
        int column_limit = grid.slots.Count;
        for (int i = Mathf.Max(0, x - xRadius); i <= Mathf.Min(Mathf.Clamp(x + xRadius, 0, grid.slots.Count - 1), column_limit); i++)
        {

            int row_limit = grid.slots[i].Count;
            for (int j = Mathf.Max(0, y - yRadius); j <= Mathf.Min(Mathf.Clamp(y + yRadius, 0, grid.slots[i].Count - 1), row_limit); j++)
            {
                //not one of the 8 directions, then skip this iteration.
                if (!((x - i != 0 && y - j == 0) || (x - i == 0 && y - j != 0) || Mathf.Abs(y - j) == Mathf.Abs(x - i)))
                {
                    s += "*".Color("white") + " ";
                    continue;
                }

                s += grid.slots[i][j].position.ToString().Color("purple") + " ";
                if (!grid.slots[i][j].isEmpty())
                {
                    if (!grid.slots[i][j].coin.CompareTag(this.tag))
                    {
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        Gizmos.color = Color.green;
                    }
                }
                else
                {
                    Gizmos.color = Color.magenta;
                }
                Gizmos.DrawSphere(grid.slots[i][j].position, 0.5f);
            }
            s += "\n";
        }
        print(s);
    }

    private void OnDrawGizmos()
    {
        if (grid != null)
        drawDirections((int)slotNeighbourDrawPos.x, (int)slotNeighbourDrawPos.y, (int)slotNeighbourRect.x, (int)slotNeighbourRect.y);
    }
}
