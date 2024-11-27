using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpicAIPlayer : AIPlayer
{
    public const int COL_COUNT = 7;
    public const int ROW_COUNT = 5;
    public Dictionary<ulong, int> lookup;
    public const int RED_WIN = -2;
    public const int YELLOW_WIN = -3;
    public const int EMPTY = 0;
    public const int RED = 1;
    public const int YELLOW = 2;

    public void SetupLookup()
    {
        lookup = new Dictionary<ulong, int>();
        int[,] possibleGrid = new int[COL_COUNT, ROW_COUNT];
        GeneratePossibilities(possibleGrid);
    }

    public void GeneratePossibilities(int[,] possibleGrid)
    {
        lookup.Add(GetKey(possibleGrid), -1);
        if (!GameOver(possibleGrid))
        {
            for (int i = 0; i < )
        }
    }

    public ulong GetKey(int[,] possibleGrid)
    {
        ulong key = 0;
        for (int i = 0; i < COL_COUNT; i++)
        {
            key = AppendBits(key, (ulong)GetColCount(possibleGrid, i), 3);
            for (int j = 0; j < ROW_COUNT; j++)
            {
                ulong bit = 0;
                if (possibleGrid[i, j] == YELLOW)
                {
                    bit = 1;
                }
                key = AppendBits(key, bit, 1);
            }
        }
        return key;
    }

    public ulong AppendBits(ulong oldBits, ulong newBits, int size)
    {
        ulong combined = oldBits << size;
        combined |= newBits;
        return combined;
    }

    public int GetFirstPossible(int[,] possibleGrid, int col)
    {
        for (int i = 0; i < ROW_COUNT; i++)
        {
            if (possibleGrid[col, i] == EMPTY) return i;
        }
        return -1;
    }

    public int GetColCount(int[,] possibleGrid, int col)
    {
        int first = GetFirstPossible(possibleGrid, col);
        if (first == -1) return ROW_COUNT;
        else return first;
    }

    public bool RedTurn(int[,] possibleGrid)
    {
        int count = 0;
        foreach (int cell in possibleGrid)
        {
            if (cell != EMPTY) count++;
        }
        return count % 2 == 0;
    }

    public

    public override void OnGameStart(bool isRedTurn)
    {
        SetupLookup();
        isMyTurn = amIRed ? isRedTurn : !isRedTurn;
        if (isMyTurn)
        {
            if (grid.slots[3][0].isEmpty())
            {
                grid.placeCoin(3, this);
            }
        }
    }

    public override void OnSwitchTurn(bool isRedTurn)
    {
        isMyTurn = amIRed ? isRedTurn : !isRedTurn;
        if (isMyTurn)
        {
            int column = getBestMove();
            print(column.ToString().Color("yellow"));
            grid.placeCoin(column, this);
            //lastCoinPos = new Vector2(column, grid.slots[column].FindLastIndex(s => !s.isEmpty()));
        }
        if (isMyTurn)
        {
            panel.setContextText("It's My turn!".Color("white"));
        }
        else
        {
            panel.setContextText("Waiting...".Color("orange"));
        }
    }

    public int getBestMove()
    {
        // check if winning move
        int winningMove = GetWinningMove(amIRed);
        if (winningMove != -1) return winningMove;
        // check if opponent has winning move
        return -1;
    }

    public int GetWinningMove(bool checkRed)
    {
        // set color to check
        string checkColor = checkRed ? "Red" : "Yellow";
        string otherColor = checkRed ? "Yellow" : "Red";
        // check columns
        for (int i = 0; i < COL_COUNT; i++)
        {
            if (CanPlace(i))
            {
                int count = 0;
                for (int j = 0; j < ROW_COUNT; j++)
                {
                    string currentColor = "Empty";
                    if (grid.slots[i][j].coin != null) currentColor = grid.slots[i][j].coin.tag;
                    if (currentColor == checkColor)
                    {
                        count++;
                    }
                    else if (currentColor == otherColor)
                    {
                        count = 0;
                    }
                    else if (count == 3)
                    {
                        return i;
                    }
                }
            }
        }
        // check rows
        for (int j = 0; j < ROW_COUNT; j++)
        {
            bool leftOpen = false;
            bool rightOpen = false;
            int count = 0;
            for (int i = 0; i < COL_COUNT; i++)
            {
                string currentColor = "Empty";
                if (grid.slots[i][j].coin != null) currentColor = grid.slots[i][j].coin.tag;
                if (currentColor == checkColor)
                {
                    count++;
                    if (count == 3 && (leftOpen || rightOpen))
                    {
                        return i;
                    }
                }
                else if (currentColor == otherColor)
                {
                    count = 0;
                }
                else if (currentColor == "Empty")
                {

                }
            }
        }
        // check diagonals
        return -1;
    }

    public bool CanPlace(int col)
    {
        return grid.slots[col][4].isEmpty();
    }

    public int GetFirstEmpty(int col)
    {
        for (int i = 0; i < ROW_COUNT; i++)
        {
            if (grid.slots[col][i].isEmpty())
            {
                return i;
            }
        }
        return -1;
    }

    public class PossibleMoves
    {
        int[] moves = new int[COL_COUNT];

    }
}


