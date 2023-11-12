using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;



public class GameGrid : MonoBehaviour
{

    //p1 and p2 are AI only, they cannot represent a human player,
    //we only use them to see if we should prevent the player from
    //making moves during their turn.
    public AIPlayer p1;
    public bool isP1Playing => p1 != null;

    public AIPlayer p2;
    public bool isP2Playing => p2 != null;

    private bool isRedTurn = true;

    public bool won => didWin(); //won lambda makes it so that
                                 //this variable is always set equal
                                 //to the didWin() call output.

    //last position a coin was placed at.
    public Vector2 lastCoinPos;

    //coin prefabs
    public GameObject redCoin;
    public GameObject yellowCoin;
    public GameObject tile;

    //slots[x][y]
    public List<List<Slot>> slots = new List<List<Slot>>(7) { new List<Slot>(5), new List<Slot>(5), new List<Slot>(5), new List<Slot>(5), new List<Slot>(5), new List<Slot>(5), new List<Slot>(5) };

    private void Awake()
    {
        //create a 7x5 2D array of slots.
        //I want the current position to start at 0 and so 
        //on the first iteration when it adds 1.25f
        //it'll start at zero.

        //offset by 0.25f
        float x;
        //we do -1.25 * column offset
        //to offset the columns to be relative to the center of the screen.
        float lastX = -1.25f * 4;
        for (int i = 0; i < 7; i++)
        {
            //offset by 1 with 0.25f of space between.
            x = lastX + 1.25f;
            //offset by 0.25f
            float y;
            float lastY = -1.25f * 3;
            for (int j = 0; j < 5; j++)
            {
                y = lastY + 1.25f;
                slots[i].Add(new Slot(i, j, x, y));
                //make the background tile
                GameObject.Instantiate(tile, new Vector2(x, y), Quaternion.identity);
                lastY = y;
            }
            lastX = x;
        }

        if (isP1Playing && isP2Playing)
        {
            p1.amIRed = true;
            p2.amIRed = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        foreach (List<Slot> ls in slots)
        {
            foreach (Slot s in ls)
            {
                //tell the grid to draw the gizmo of itself.
                s.drawGizmos();
            }
        }
    }

    //called when player clicks the start button in the menu.
    public void startGame()
    {
        //send global Game Start event
        GameStartEvent.GameStart(isRedTurn);
    }

    /// <summary>
    /// Called everytime a player makes a move.
    /// Used to reset data for bots by sending an event
    /// Used to change game data and check win conditions.
    /// </summary>
    public void switchTurn()
    {
        


        if (didWin())
        {
            //call global winEvent and tell them who won.
            WinEvent.Win(isRedTurn);
        }
        else
        {
            //change turns.
            isRedTurn = !isRedTurn;
            //call global switchTurnEvent.
            SwitchTurnEvent.SwitchTurn(isRedTurn);
        }
       

        
    }

    //this version of the method can only be called by the player.
    public void placeCoin(int column)
    {
        //verify it is the humans turn, human is always red when against AI.
        if (isP1Playing && p1.amIRed && isRedTurn || isP1Playing && !p1.amIRed && !isRedTurn)
        {
            //TODO: play some sort of error audio and then grey out the column
            //the player is trying to place a coin in.
            Debug.Log("It is not the players turn.".Color("Blue"));
            return;
        }

        if (!won && !isColumnFull(column))
        {
            //instatiate coin object at the location of the first empty slot.
            GameObject t = GameObject.Instantiate(isRedTurn ? redCoin : yellowCoin, slots[column].Find(s => s.isEmpty()).position, Quaternion.identity);
            //set the coin of the slot we just filled to be
            //the coin component of the coin object we just
            //created.
            slots[column].Find(s => s.isEmpty()).coin = t.GetComponent<Coin>();
            //set lastCoinPos to be the position of the newly created coin.
            lastCoinPos = new Vector2(column, slots[column].FindIndex(s => s.isEmpty()) - 1);
            //Switch turns
            switchTurn();
        }
        else
        {
            //TODO: play some audio and then also grey out the column for a moment.
        }
    }

    public void placeCoin(int column, AIPlayer cpu)
    {
        //if it's not the AI's turn, don't let them make a move.
        if (/*!(cpu == p1 && cpu != p2) || !(cpu == p2 && cpu != p1) && */cpu.amIRed && !isRedTurn || !cpu.amIRed && isRedTurn)
        {
            Debug.LogWarning("IT IS NOT " + cpu.gameObject.name + " TURN, VERIFY YOU ARE NOT TRYING TO PLAY DURING ANOTHER PLAYER'S TURN");
            //TODO: play audio and grey out the column they attempt to play on for a moment.
            return;
        }

        //Debug.Log("Who made move: " + cpu.gameObject.name.Color(isRedTurn ? "red" : "yellow"));

        if (!won && !isColumnFull(column))
        {
            //instatiate coin object at the location of the first empty slot.
            GameObject t = GameObject.Instantiate(isRedTurn ? redCoin : yellowCoin, slots[column].Find(s => s.isEmpty()).position, Quaternion.identity);
            //set the coin of the slot we just filled to be
            //the coin component of the coin object we just
            //created.
            slots[column].Find(s => s.isEmpty()).coin = t.GetComponent<Coin>();
            lastCoinPos = new Vector2(column, slots[column].FindIndex(s => s.isEmpty()) - 1);
            //Switch turns
            switchTurn();
        }
        else
        {
            //TODO: play some audio and then also grey out the column for a moment.
        }
    }

    /// <summary>
    /// Returns true if the column is empty, false otherwise.
    /// </summary>
    /// <param name="column">Column (0-6)</param>
    /// <returns></returns>
    public bool isColumnEmpty(int column)
    {
        return slots[column].TrueForAll(s => s.isEmpty());
    }

    /// <summary>
    /// Returns true if the column is full, false otherwise.
    /// </summary>
    /// <param name="column">Column (0-6)</param>
    /// <returns></returns>
    public bool isColumnFull(int column)
    {
        return slots[column].TrueForAll(s => !s.isEmpty());
    }

    public int getColumnCoinCount(int column)
    {
        if (isColumnEmpty(column))
        {
            return 0;
        }
        else if (isColumnFull(column))
        {
            return 7;
        }
        return slots[column].Where(s => !s.isEmpty()).Count();
    }

    /// <summary>
    /// get a list of all the slots with a coin in the given column.
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    public List<Slot> getCoinsInColumn(int column)
    {
        if (isColumnEmpty(column))
        {
            return null;
        }
        return slots[column].Where(s => !s.isEmpty())?.ToList();
    }

    public bool didWin()
    {

        //Vertical Check
        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = 0; j < slots[i].Count - 3; j++)
            {
                if (!slots[i][j].isEmpty() && !slots[i][j + 1].isEmpty() && !slots[i][j + 2].isEmpty() && !slots[i][j + 3].isEmpty())
                {
                    if (slots[i][j].coin.CompareTag(slots[i][j + 1].coin.tag) && slots[i][j + 1].coin.CompareTag(slots[i][j + 2].coin.tag) && slots[i][j + 2].coin.CompareTag(slots[i][j + 3].coin.tag))
                    {
                        slots[i][j].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        slots[i][j + 1].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        slots[i][j + 2].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        slots[i][j + 3].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        return true;
                    }
                }
            }
        }

        //horizontal check
        for (int i = 0; i < slots.Count-3; i++)
        {
            for (int j = 0; j < slots[i].Count; j++)
            {
                if (!slots[i][j].isEmpty() && !slots[i + 1][j].isEmpty() && !slots[i+2][j].isEmpty() && !slots[i+3][j].isEmpty())
                {
                    if (slots[i][j].coin.CompareTag(slots[i+1][j].coin.tag) && slots[i+1][j].coin.CompareTag(slots[i+2][j].coin.tag) && slots[i+2][j].coin.CompareTag(slots[i+3][j].coin.tag))
                    {
                        slots[i][j].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        slots[i+1][j].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        slots[i+2][j].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        slots[i+3][j].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        return true;
                    }
                }
            }
        }

        //Right diagonal check
        for (int i = 3; i < slots.Count; i++)
        {
            for (int j = 0; j < slots[i].Count - 3; j++)
            {
                if (!slots[i][j].isEmpty() && !slots[i - 1][j + 1].isEmpty() && !slots[i - 2][j + 2].isEmpty() && !slots[i - 3][j + 3].isEmpty())
                {
                    if (slots[i][j].coin.CompareTag(slots[i - 1][j + 1].coin.tag) && slots[i - 1][j + 1].coin.CompareTag(slots[i - 2][j + 2].coin.tag) && slots[i - 2][j + 2].coin.CompareTag(slots[i - 3][j + 3].coin.tag))
                    {
                        slots[i][j].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        slots[i - 1][j + 1].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        slots[i - 2][j + 2].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        slots[i - 3][j + 3].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        return true;
                    }
                }
            }
        }

        //Left diagonal check
        for (int i = 3; i < slots.Count; i++)
        {
            for (int j = 3; j < slots[i].Count; j++)
            {
                if (!slots[i][j].isEmpty() && !slots[i - 1][j-1].isEmpty() && !slots[i - 2][j-2].isEmpty() && !slots[i - 3][j-3].isEmpty())
                {
                    if (slots[i][j].coin.CompareTag(slots[i - 1][j-1].coin.tag) && slots[i - 1][j-1].coin.CompareTag(slots[i - 2][j-2].coin.tag) && slots[i - 2][j-2].coin.CompareTag(slots[i - 3][j-3].coin.tag))
                    {
                        slots[i][j].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        slots[i - 1][j-1].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        slots[i - 2][j-2].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        slots[i - 3][j-3].coin.GetComponent<SpriteRenderer>().color = Color.green;
                        return true;
                    }
                }
            }
        }


        return false;
    }
}

//Component to represent a slot, 
//we will attatch it to an empty
//transform on creation.
public class Slot
{
    public Coin coin;

    //THIS IS NOT THE REAL IN GAME POSITION,
    //THIS IS THE POSITION WITHIN THE ARRAY 
    //OF SLOTS WITH X BEING THE INDEX OF THE
    //SECOND ARRAY WITHIN THE FIRST ARRAY, 
    //AND Y BEING THE POSITION WITHIN THAT
    //SECOND ARRAY
    //slots[x][y]
    public Vector2 arrayPosition;

    public Vector2 position;

    /// <summary>
    /// A slot within the 2D array containing a reference to the coin 
    /// (if there is one) and a reference to this slot's position.
    /// </summary>
    /// <param name="arrayPosition">position in the array where this slot is located</param>
    public Slot(Vector2 arrayPosition, Vector2 position)
    {
        this.arrayPosition = arrayPosition;
        this.position = position;
    }

    public Slot(int arrayX, int arrayY, Vector2 position)
    {

        this.arrayPosition = new Vector2(arrayX, arrayY);
        this.position = position;
    }

    public Slot(int arrayX, int arrayY, float x, float y)
    {

        this.arrayPosition = new Vector2(arrayX, arrayY);
        this.position = new Vector2(x, y);
    }

    public bool isEmpty()
    {
        if (coin == null)
        {
            return true;
        }
        return false;
    }

    public void drawGizmos()
    {
        Gizmos.color = this.isEmpty() ? Color.black : Color.green;
        Gizmos.DrawWireSphere(position, 0.5f);
    }
}