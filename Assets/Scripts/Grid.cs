using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;

public class Grid : MonoBehaviour
{
    //coin prefabs
    public GameObject redCoin;
    public GameObject yellowCoin;

    //slots[x][y]
    List<List<Slot>> slots = new List<List<Slot>>(7) { new List<Slot>(5), new List<Slot>(5), new List<Slot>(5), new List<Slot>(5), new List<Slot>(5), new List<Slot>(5), new List<Slot>(5) };

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
                lastY = y;
            }
            lastX = x;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //TODO: 
            //Instantiate a coin in this column and lerp it to the bottom of the column.
        }
    }

    private void OnDrawGizmos()
    {
        foreach (List<Slot> ls in slots)
        {
            foreach (Slot s in ls)
            {
                s.drawGizmos();
            }
        }
    }

    public void placeCoin(int column)
    {
        if (!isColumnFull(column))
        {
            GameObject t = GameObject.Instantiate(redCoin, slots[column - 1].Find(s => s.isEmpty()).position, Quaternion.identity);
            slots[column - 1].Find(s => s.isEmpty()).coin = t.GetComponent<Coin>();
        }
    }

    /// <summary>
    /// Returns true if the column is empty, false otherwise.
    /// </summary>
    /// <param name="column">Column (1-7)</param>
    /// <returns></returns>
    public bool isColumnEmpty(int column)
    {
        return slots[column - 1].TrueForAll(s => s.isEmpty());
    }

    /// <summary>
    /// Returns true if the column is full, false otherwise.
    /// </summary>
    /// <param name="column">Column (1-7)</param>
    /// <returns></returns>
    public bool isColumnFull(int column)
    {
        return slots[column - 1].TrueForAll(s => !s.isEmpty());
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
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(position, 0.5f);
    }
}