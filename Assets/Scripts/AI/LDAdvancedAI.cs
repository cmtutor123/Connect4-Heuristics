using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDAdvancedAI : AIPlayer
{
    [Range(0, 3)]
    public int curDrawDirection = 0;
    public Vector2 slotNeighbourDrawPos = new Vector2(0, 0);
    public Vector2 slotNeighbourRect = new Vector2(1, 1);

    Vector2 lastCoinPos;

    //use this and reference it when we are going to check for all the various possible
    //placements, only check through this list and see if it's neighbours at it's coords
    //are open and then check for if there are any "threats" in the next 3 slots
    //and if there aren't then fill the next adjacent slot. 
    //start with coding horizontal and vertical then do diagonal later.
    private List<Slot> placedCoinSlots = new List<Slot>();

    public override void OnGameStart(bool isRedTurn)
    {
        //base.OnGameStart(isRedTurn);
        this.isMyTurn = amIRed ? isRedTurn : !isRedTurn;
        if (isMyTurn)
        {
            //lets make it so that at the beginning if we are going
            //first or the slot is open we should select the middle
            //slot.
            if (grid.slots[3][0].isEmpty())
            {
                grid.placeCoin(3, this);
                //add the coin we just put in that column.
                placedCoinSlots.Add(grid.slots[3].Find(s => !s.isEmpty()));
            }
        }
    }

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



        //check neighbours, check if there are any of the same color, and if there are any threats.
        //if there are only empty slots check for threats in all directions 4 out and first direction with
        //no threats will become our target.

        //maybe try to prioritize creating the T shape that creates an unwinnable situation
        //for the other player?

        int currentDesiredColumn;

        foreach (Slot s in placedCoinSlots)
        {
            

            List<List<Slot>> neighbours = getNeighbours((int)s.arrayPosition.x, (int)s.arrayPosition.y, 1, 1);
            //start movement direction for searching if we can create a continuous line of 4 by
            //first looking right, then go through all directions.
            

            //if this coin is surrounded by red then skip it.
            if (neighbours.TrueForAll(n => n.TrueForAll(nl => !nl.isEmpty())) && !neighbours.TrueForAll(n => n.TrueForAll(nl => nl.coin.CompareTag(this.tag))))
            {
                continue;
            }

            //horizontal desireability check
            //we make the radius 2 in each direction 
            //so we get an array of 5 slots to check
            //this means we can check from the beginning
            //coin in the list to whichever direction.
            List<Slot> horizontalSlots = getDirection(0, (int)s.arrayPosition.x, (int)s.arrayPosition.y, 2, 2);
            //if either all the coins in the row are the same color as us or they are empty we place a coin in the 
            //first horizontal open slot relative to ourselves.
            if (horizontalSlots.TrueForAll(hs => !hs.isEmpty() && hs.coin.CompareTag(this.tag) || hs.isEmpty()))
            {
                //set column number of the first empty slot.
                currentDesiredColumn = horizontalSlots.FindIndex(hs => hs.isEmpty());

                //for now we return immediately because I don't want to check for threats and do weights yet.
                return currentDesiredColumn;
            }

            List<Slot> verticalSlots = getDirection(1, (int)s.arrayPosition.x, (int)s.arrayPosition.y, 2, 2);

            if (verticalSlots.TrueForAll(vs => !vs.isEmpty() && vs.coin.CompareTag(this.tag) || vs.isEmpty()))
            {
                //set desired column to be the first empty slot.
                currentDesiredColumn = verticalSlots.FindIndex(vs => vs.isEmpty());

                //for now return immediately because I don't want to check for threats yet.
                return currentDesiredColumn;
            }

        }

        //return -1 if we somehow don't decide. This will throw an error.
        return -1;
    }

    public void drawNeighbours(int x, int y, int xRadius, int yRadius)
    {
        string s = "";
        int columnLimit = grid.slots.Count;
        for (int i = Mathf.Max(0, x-xRadius); i <= Mathf.Min(Mathf.Clamp(x+xRadius, 0, grid.slots.Count - 1), columnLimit); i++)
        {
            int rowLimit = grid.slots[i].Count;
            for (int j = Mathf.Max(0, y -yRadius); j <= Mathf.Min(Mathf.Clamp(y + yRadius, 0, grid.slots[i].Count - 1), rowLimit); j++)
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
        List<List<Slot>> neighbours = new List<List<Slot>>() { new List<Slot>(), new List<Slot>(), new List<Slot>(), new List<Slot>(), new List<Slot>(), new List<Slot>(), new List<Slot>() };

        int columnLimit = grid.slots.Count;
        for (int i = Mathf.Max(0, x - xRadius); i <= Mathf.Min(Mathf.Clamp(x + xRadius, 0, grid.slots.Count - 1), columnLimit); i++)
        {
            int rowLimit = grid.slots[i].Count;
            for (int j = Mathf.Max(0, y - yRadius); j <= Mathf.Min(Mathf.Clamp(y + yRadius, 0, grid.slots[i].Count - 1), rowLimit); j++)
            {
                if (grid.slots[i] != null && grid.slots[i][j] != null)
                neighbours[i].Add(grid.slots[i][j]);
            }
        }
        return neighbours;
    }

    //draws each slot in one of 8 directions relative to a given slot position
    public void drawDirection(int direction, int x, int y, int xRadius, int yRadius)
    {
        //do a switch statement depending on the direction we are given,
        //0: horizontal
        //1: vertical
        //2: diagonalR  --> /
        //3: diagonalL  --> \

        int columnLimit = grid.slots.Count;
        switch (direction)
        {
            case 0:
                for (int i = Mathf.Max(0, x - xRadius); i <= Mathf.Min(Mathf.Clamp(x + xRadius, 0, grid.slots.Count - 1), columnLimit); i++)
                {

                    int rowLimit = grid.slots[i].Count;
                    for (int j = Mathf.Max(0, y - yRadius); j <= Mathf.Min(Mathf.Clamp(y + yRadius, 0, grid.slots[i].Count - 1), rowLimit); j++)
                    {
                        //not horizontal, then skip this iteration.
                        if (!((x - i != 0 && y - j == 0) || i == x && j == y))
                        {
                            continue;
                        }
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
                }
                break;
            case 1:
                for (int i = Mathf.Max(0, x - xRadius); i <= Mathf.Min(Mathf.Clamp(x + xRadius, 0, grid.slots.Count - 1), columnLimit); i++)
                {

                    int rowLimit = grid.slots[i].Count;
                    for (int j = Mathf.Max(0, y - yRadius); j <= Mathf.Min(Mathf.Clamp(y + yRadius, 0, grid.slots[i].Count - 1), rowLimit); j++)
                    {
                        //not vertical, then skip this iteration.
                        if (!((x - i == 0 && y - j != 0) || i == x && j == y))
                        {
                            continue;
                        }
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
                }
                break;
            case 2:
                for (int i = Mathf.Max(0, x - xRadius); i <= Mathf.Min(Mathf.Clamp(x + xRadius, 0, grid.slots.Count - 1), columnLimit); i++)
                {

                    int rowLimit = grid.slots[i].Count;
                    for (int j = Mathf.Max(0, y - yRadius); j <= Mathf.Min(Mathf.Clamp(y + yRadius, 0, grid.slots[i].Count - 1), rowLimit); j++)
                    {
                        //not diagonal R, then skip this iteration.
                        if (!(y - j == x - i)) //--> /
                        {
                            continue;
                        }
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
                }
                break;
            case 3:
                for (int i = Mathf.Max(0, x - xRadius); i <= Mathf.Min(Mathf.Clamp(x + xRadius, 0, grid.slots.Count - 1), columnLimit); i++)
                {

                    int rowLimit = grid.slots[i].Count;
                    for (int j = Mathf.Max(0, y - yRadius); j <= Mathf.Min(Mathf.Clamp(y + yRadius, 0, grid.slots[i].Count - 1), rowLimit); j++)
                    {
                        //not diagonalL, then skip this iteration.
                        if (!(-1 * (y - j) == x - i))// --> \
                        {
                            continue;
                        }
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
                }
                break;
            default:
                break;
        }
    }

    public List<Slot> getDirection(int direction, int x, int y, int xRadius, int yRadius)
    {
        //do a switch statement depending on the direction we are given,
        //0: horizontal
        //1: vertical
        //2: diagonalR  --> /
        //3: diagonalL  --> \

        List<Slot> temp = new List<Slot>();

        int columnLimit = grid.slots.Count;
        switch (direction)
        {
            case 0:
                for (int i = Mathf.Max(0, x - xRadius); i <= Mathf.Min(Mathf.Clamp(x + xRadius, 0, grid.slots.Count - 1), columnLimit); i++)
                {

                    int rowLimit = grid.slots[i].Count;
                    for (int j = Mathf.Max(0, y - yRadius); j <= Mathf.Min(Mathf.Clamp(y + yRadius, 0, grid.slots[i].Count - 1), rowLimit); j++)
                    {
                        //not horizontal, then skip this iteration.
                        if (!((x - i != 0 && y - j == 0) || i == x && j == y))
                        {
                            continue;
                        }
                        temp.Add(grid.slots[i][j]);
                    }
                }
                break;
            case 1:
                for (int i = Mathf.Max(0, x - xRadius); i <= Mathf.Min(Mathf.Clamp(x + xRadius, 0, grid.slots.Count - 1), columnLimit); i++)
                {

                    int rowLimit = grid.slots[i].Count;
                    for (int j = Mathf.Max(0, y - yRadius); j <= Mathf.Min(Mathf.Clamp(y + yRadius, 0, grid.slots[i].Count - 1), rowLimit); j++)
                    {
                        //not vertical, then skip this iteration.
                        if (!((x - i == 0 && y - j != 0) || i == x && j == y))
                        {
                            continue;
                        }
                        temp.Add(grid.slots[i][j]);
                    }
                }
                break;
            case 2:
                for (int i = Mathf.Max(0, x - xRadius); i <= Mathf.Min(Mathf.Clamp(x + xRadius, 0, grid.slots.Count - 1), columnLimit); i++)
                {

                    int rowLimit = grid.slots[i].Count;
                    for (int j = Mathf.Max(0, y - yRadius); j <= Mathf.Min(Mathf.Clamp(y + yRadius, 0, grid.slots[i].Count - 1), rowLimit); j++)
                    {
                        //not diagonal R, then skip this iteration.
                        if (!(y - j == x - i)) //--> /
                        {
                            continue;
                        }
                        temp.Add(grid.slots[i][j]);
                    }
                }
                break;
            case 3:
                for (int i = Mathf.Max(0, x - xRadius); i <= Mathf.Min(Mathf.Clamp(x + xRadius, 0, grid.slots.Count - 1), columnLimit); i++)
                {

                    int rowLimit = grid.slots[i].Count;
                    for (int j = Mathf.Max(0, y - yRadius); j <= Mathf.Min(Mathf.Clamp(y + yRadius, 0, grid.slots[i].Count - 1), rowLimit); j++)
                    {
                        //not diagonalL, then skip this iteration.
                        if (!(-1 * (y - j) == x - i))// --> \
                        {
                            continue;
                        }
                        temp.Add(grid.slots[i][j]);
                    }
                }
                break;
            default:
                break;
        }

        return temp;
    }

    private void OnDrawGizmos()
    {
        if (grid != null)
        drawDirection(curDrawDirection, (int)slotNeighbourDrawPos.x, (int)slotNeighbourDrawPos.y, (int)slotNeighbourRect.x, (int)slotNeighbourRect.y);
    }
}
