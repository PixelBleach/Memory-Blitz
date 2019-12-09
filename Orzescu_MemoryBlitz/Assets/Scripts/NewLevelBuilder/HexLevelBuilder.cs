using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HexLevelBuilder : MonoBehaviour {

    public int numRows;
    public int numCols;

    public float tileWidth = 1f;
    public float tileHeight = 1f;

    public GameObject[] hexTiles;
    public Hex[,] grid;

    public Vector3 startPosition = Vector3.zero;

    public bool isMapOfDay;
    public int seed;

	// Use this for initialization
	void Start () {
        isMapOfDay = GameManager.instance.isMapOfDay;
        BuildLevel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuildLevel()
    {
        //if it's the map of the day
        if (isMapOfDay)
        {
            //Set current seed based off of system clock date
            DateTime currentTime;
            currentTime = DateTime.Now;
            currentTime = currentTime.Date; // set it to just the date (so it's the map of the day)
            seed = (int)currentTime.Ticks;

            UnityEngine.Random.InitState(seed);

        }

            //Build Level - create a grid
            grid = new Hex[numCols, numRows];

            for (int currentCol = 0; currentCol < numCols; currentCol++)
            {
                for (int currentRow = 0; currentRow < numRows; currentRow++)
                {
                    //instantiate a random tile
                    GameObject tempHex = Instantiate(RandomTile()) as GameObject;
                    //Set the tile's name to include it's grid location
                    tempHex.name = "Hex(" + currentCol + "," + currentRow + ")";
                    //Set it's parent to the generation object
                    Transform tempHexTransform = tempHex.GetComponent<Transform>();
                    tempHexTransform.parent = this.gameObject.GetComponent<Transform>();
                    //Set  it's position in the game world
                    Vector3 newPosition;
                    newPosition = CalcWorldPos(currentCol, currentRow);
                    tempHexTransform.localPosition = newPosition;
                    //add the tile to the grid
                    grid[currentCol, currentRow] = tempHex.GetComponent<Hex>();

                }
            }

            //Assign the Hexes Neighbors

            #region NorthWestHexes
            //Assign all NorthWest Hexes
            for (int currentCol = 0; currentCol < numCols; currentCol++)
            {
                for (int currentRow = 0; currentRow < numRows; currentRow++)
                {
                    if (currentRow % 2 == 0) // if even row
                    {
                        if (currentRow == 0 || currentCol == 0) //if furthest left tiles on even column or top row
                        {
                            grid[currentCol, currentRow].northWestHex = null;
                        } else { // all other tiles
                            grid[currentCol, currentRow].northWestHex = grid[currentCol - 1, currentRow - 1];
                        }

                    }
                    if (currentRow % 2 != 0) //if odd row
                    {
                        grid[currentCol, currentRow].northWestHex = grid[currentCol, currentRow - 1];
                    }
                }
            }
            #endregion

            #region West Hexes
            //Assign all West Hexes
            for (int currentCol = 0; currentCol < numCols; currentCol++)
            {
                for (int currentRow = 0; currentRow < numRows; currentRow++)
                {
                    if (currentCol == 0)
                    {
                        grid[currentCol, currentRow].westHex = null;
                    }
                    else
                    {
                        grid[currentCol, currentRow].westHex = grid[currentCol - 1, currentRow];
                    }
                }
            }
            #endregion

            #region SouthWest Hexes
            //Assign all SouthWest Hexes
            for (int currentCol = 0; currentCol < numCols; currentCol++)
            {
                for (int currentRow = 0; currentRow < numRows; currentRow++)
                {
                    if (currentRow % 2 == 0) // if row is even
                    {
                        if (currentRow == numRows - 1 || currentCol == 0) //if furthest left tiles on even row or bottom row
                        {
                            grid[currentCol, currentRow].southWestHex = null;
                        }
                        else { // all other tiles
                            grid[currentCol, currentRow].southWestHex = grid[currentCol - 1, currentRow + 1];
                        }

                    }
                    if (currentRow % 2 != 0) //if row is odd
                    {
                        if (currentRow == numRows - 1) //if bottom row
                        {
                            grid[currentCol, currentRow].southWestHex = null;
                        } else {
                            grid[currentCol, currentRow].southWestHex = grid[currentCol, currentRow + 1];
                        }
                    }
                }
            }
            #endregion

            #region NorthEast Hexes
            //Assign all NorthEast Hexes
            for (int currentCol = 0; currentCol < numCols; currentCol++)
            {
                for (int currentRow = 0; currentRow < numRows; currentRow++)
                {
                    if (currentRow % 2 == 0) // if even row
                    {
                        if (currentRow == 0) // if furthest right collumn, or top row
                        {
                            grid[currentCol, currentRow].northEastHex = null;
                        } else { // all other even tiles
                            grid[currentCol, currentRow].northEastHex = grid[currentCol, currentRow - 1];
                        }
                    }
                    if (currentRow % 2 != 0) //if odd row
                    {
                        if (currentCol == numCols - 1)
                        {
                            grid[currentCol, currentRow].northEastHex = null;
                        } else {
                            grid[currentCol, currentRow].northEastHex = grid[currentCol + 1, currentRow - 1];
                        }
                    }
                }
            }
            #endregion

            #region East Hexes
            //Assign all East Hexes
            for (int currentCol = 0; currentCol < numCols; currentCol++)
            {
                for (int currentRow = 0; currentRow < numRows; currentRow++)
                {
                    if (currentCol == numCols - 1)
                    {
                        grid[currentCol, currentRow].eastHex = null;
                    } else {
                        grid[currentCol, currentRow].eastHex = grid[currentCol + 1, currentRow];
                    }
                }
            }
            #endregion Region

            #region SouthEast Hexes
            //Assign all SouthEast Hexes
            for (int currentCol = 0; currentCol < numCols; currentCol++)
            {
                for (int currentRow = 0; currentRow < numRows; currentRow++)
                {
                    if (currentRow % 2 == 0) // if even row
                    {
                        if (currentRow == numRows - 1) //if bottom row
                        {
                            grid[currentCol, currentRow].southEastHex = null;
                        } else { // all elements in even row
                            grid[currentCol, currentRow].southEastHex = grid[currentCol, currentRow + 1];
                        }
                    }
                    if (currentRow % 2 != 0) // if odd row
                    {
                        if (currentRow == numRows - 1 || currentCol == numCols -1) //if bottom row
                        {
                            grid[currentCol, currentRow].southEastHex = null;
                        } else {
                            grid[currentCol, currentRow].southEastHex = grid[currentCol + 1, currentRow + 1];
                        }
                    }
                }
            }
            #endregion

            OpenMaze();
    }

    public void CalcStartPos() //Makes sure that hexagonal offsets from the start will always work if the start is set to another world position
    {
        float offset = 0;
        if (numRows /2 % 2 != 0)
        {
            offset = tileWidth / 2;
        }

        float x = -tileWidth * (numCols / 2) - offset;
        float z = tileHeight * 0.75f * (numRows / 2);

        startPosition = new Vector3(x, 0, z);
    }

    public Vector3 CalcWorldPos(float gridXPos, float gridYPos)
    {
        CalcStartPos();

        float offset = 0;

        if (gridYPos % 2 != 0)
        {
            offset = tileWidth / 2;
        }

        float x = startPosition.x + gridXPos * tileWidth + offset;
        float z = startPosition.z - gridYPos * tileHeight * 0.75f;

        return new Vector3(x, 0, z);
    }

    public void OpenDoors(Hex currentHex, Hex nextHex)
    {
        if (currentHex.northEastHex == nextHex)
        {
            currentHex.northEastWall.SetActive(false);
            nextHex.southWestWall.SetActive(false);
        }
        if (currentHex.eastHex == nextHex)
        {
            currentHex.eastWall.SetActive(false);
            nextHex.westWall.SetActive(false);
        }
        if (currentHex.southEastHex == nextHex)
        {
            currentHex.southEastWall.SetActive(false);
            nextHex.northWestWall.SetActive(false);
        }
        if (currentHex.northWestHex == nextHex)
        {
            currentHex.northWestWall.SetActive(false);
            nextHex.southEastWall.SetActive(false);
        }
        if (currentHex.westHex == nextHex)
        {
            currentHex.westWall.SetActive(false);
            nextHex.eastWall.SetActive(false);
        }
        if (currentHex.southWestHex == nextHex)
        {
            currentHex.southWestWall.SetActive(false);
            nextHex.northEastWall.SetActive(false);
        }
    }

    public GameObject RandomTile()
    {
        return hexTiles[UnityEngine.Random.Range(0, hexTiles.Length)];
    }

    public void OpenMaze()
    {
        //create a stack of hexes, to backtrack through later
        Stack<Hex> visitedHexes = new Stack<Hex>();

        //Pick a random hex within the grid, add it to the stack, and mark it as visited
        visitedHexes.Push(grid[UnityEngine.Random.Range(0, numCols), UnityEngine.Random.Range(0, numRows)]);
        Hex currentHex = visitedHexes.Peek(); //starting hex
        currentHex.isVisited = true;

        while (currentHex != null) //Iterate through 
        {
            Hex nextHex = currentHex.RandomUnvisistedNeighborHex();

            if (nextHex == null) //if no unvisited neighbors move back in the stack
            {
                visitedHexes.Pop();
            } else {

                OpenDoors(currentHex, nextHex);
                visitedHexes.Push(nextHex);
                nextHex.isVisited = true;

            }

            currentHex = visitedHexes.Peek();


            
        }
    }

}
