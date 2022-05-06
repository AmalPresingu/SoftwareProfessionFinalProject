//BOARD CODE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    int[,] solvedGrid = new int[9, 9];
    string s;

    int[,] riddleGrid = new int[9, 9];
    int piecesToErase = 35;

    public Transform A1, A2, A3, B1, B2, B3, C1, C2, C3;
    public GameObject buttonPrefab;

    public AudioSource winSound;
    public AudioSource wrongSound;
    public AudioSource nohintSound;

    List<NumberField> fieldList = new List<NumberField>();

    public enum Difficulties
    {
        DEBUG,
        EASY,
        MEDIUM,
        HARD
    }

    public GameObject winPanel;

    public Difficulties difficulty;

    int maxHints;

    void Start()
    {
        winPanel.SetActive(false);

        difficulty = (Board.Difficulties)Settings.difficulty;

        InitGrid(ref solvedGrid);

        ShuffleGrid(ref solvedGrid, 5);
        CreateRiddleGrid();

        CreateButtons();
    }

    void InitGrid(ref int[,] grid)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                grid[i, j] = (i * 3 + i / 3 + j) % 9 + 1;
            }
        }
    }

    void DebugGrid(ref int[,] grid)
    {
        s = "";
        int sep = 0;
        for (int i = 0; i < 9; i++)
        {
            s += "|";
            for (int j = 0; j < 9; j++)
            {
                s += grid[i, j].ToString();

                sep = j % 3;
                if (sep == 2)
                {
                    s += "|";
                }
            }
            s += "\n";
        }
    }

    void ShuffleGrid(ref int[,] grid, int shuffleAmount)
    {
        for (int i = 0; i < 9; i++)
        {
            int value1 = Random.Range(1, 10);
            int value2 = Random.Range(1, 10);
            MixTwoGridCells(ref grid, value1, value2);
        }
    }

    void MixTwoGridCells(ref int[,] grid, int value1, int value2)
    {
        int x1 = 0;
        int x2 = 0;
        int y1 = 0;
        int y2 = 0;

        for (int i = 0; i < 9; i += 3)
        {
            for (int k = 0; k < 9; k += 3)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        if (grid[i + j, k + l] == value1)
                        {
                            x1 = i + j;
                            y1 = k + l;
                        }

                        if (grid[i + j, k + l] == value2)
                        {
                            x2 = i + j;
                            y2 = k + l;
                        }
                    }
                }
                grid[x1, y1] = value2;
                grid[x2, y2] = value1;
            }
        }
    }

    void CreateRiddleGrid()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                riddleGrid[i, j] = solvedGrid[i, j];
            }
        }

        SetDifficulty();


        for (int i = 0; i < piecesToErase; i++)
        {
            int x1 = Random.Range(0, 9);
            int y1 = Random.Range(0, 9);

            while(riddleGrid[x1, y1] == 0)
            {
                x1 = Random.Range(0, 9);
                y1 = Random.Range(0, 9);
            }

            riddleGrid[x1, y1] = 0;
        }

    }

    void CreateButtons()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                GameObject newButton = Instantiate(buttonPrefab);

                NumberField numField = newButton.GetComponent<NumberField>();
                numField.SetValues(i, j, riddleGrid[i, j], i + "," + j, this);
                newButton.name = i + "," + j;

                if(riddleGrid[i, j] == 0)
                {
                    fieldList.Add(numField);
                }

                //A1
                if (i < 3 && j < 3)
                {
                    newButton.transform.SetParent(A1, false);
                }
                //A2
                if (i < 3 && j > 2 && j < 6)
                {
                    newButton.transform.SetParent(A2, false);
                }
                //A3
                if (i < 3 && j > 5)
                {
                    newButton.transform.SetParent(A3, false);
                }
                //B1
                if (i > 2 && i < 6 && j < 3)
                {
                    newButton.transform.SetParent(B1, false);
                }
                //B2
                if (i > 2 && i < 6 && j > 2 && j < 6)
                {
                    newButton.transform.SetParent(B2, false);
                }
                //B3
                if (i > 2 && i < 6 && j > 5)
                {
                    newButton.transform.SetParent(B3, false);
                }
                //C1
                if (i > 5 && j < 3)
                {
                    newButton.transform.SetParent(C1, false);
                }
                //C2
                if (i > 5 && j > 2 && j < 6)
                {
                    newButton.transform.SetParent(C2, false);
                }
                //C3
                if (i > 5 && j > 5)
                {
                    newButton.transform.SetParent(C3, false);
                }

            }
        }
    }

    public void SetInputInRiddleGrid(int x, int y, int value)
    {
        riddleGrid[x, y] = value;
    }

    void SetDifficulty()
    {
        switch(difficulty)
        {
            case Difficulties.DEBUG:
                piecesToErase = 5;
                maxHints = 3;
                break;
            case Difficulties.EASY:
                piecesToErase = 30;
                maxHints = 5;
                break;
            case Difficulties.MEDIUM:
                piecesToErase = 40;
                maxHints = 3;
                break;
            case Difficulties.HARD:
                piecesToErase = 60;
                maxHints = 2;
                break;
        }
    }

    public void CheckComplete()
    {
        if (winCondition())
        {
            winSound.Play();
            winPanel.SetActive(true);
        }
        else
        {
            wrongSound.Play();
        }
    }

    bool winCondition()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (riddleGrid[i, j] != solvedGrid[i, j])
                {
                    return false;
                }
            }
        }
        return true; 
    }

    public void ShowHint()
    {
        if (fieldList.Count > 0 && maxHints > 0)
        {
            int randIndex = Random.Range(0, fieldList.Count);

            maxHints--;
            riddleGrid[fieldList[randIndex].GetX(), fieldList[randIndex].GetY()] = solvedGrid[fieldList[randIndex].GetX(), fieldList[randIndex].GetY()];


            fieldList[randIndex].SetHint(riddleGrid[fieldList[randIndex].GetX(), fieldList[randIndex].GetY()]);
            fieldList.RemoveAt(randIndex);
        }
        else
        {
            nohintSound.Play();
        }
    }
}
