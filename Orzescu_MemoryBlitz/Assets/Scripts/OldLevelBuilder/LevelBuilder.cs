using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelBuilder : MonoBehaviour {

	public int numRows;
	public int numCols;

	public float tileWidth = 1f;
	public float tileHeight = 1f;

	public GameObject[] roomTiles;
	public Room[,] grid;

	public bool isMapOfDay;
	public int seed;

	// Use this for initialization
	void Start () {
		BuildLevel ();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BuildLevel(){
	
		//Set our seed value for our Random
		int seed;

		//set our seed based off of our date
		DateTime currentTime;
		currentTime = DateTime.Now; // fills date time object with the time right now
		//seed = currentTime.DayOfYear * (int)UnityEngine.Random.Range(1,100);
		currentTime = currentTime.Date;
		seed = (int)currentTime.Ticks;

		UnityEngine.Random.InitState(seed);

		//Prepare the Grid array
		grid = new Room[numCols , numRows];


		// for each row in our grid
		for (int currentRow = 0; currentRow < numRows; currentRow++) {
			// for each column in that row
			for (int currentCol = 0; currentCol < numCols; currentCol++) {
				// instantiate a random tile
				GameObject tempTile = Instantiate(RandomTile()) as GameObject;
				// set the data for that tile
				//Set its name to include it's (x,y) position
				tempTile.name = "Tile(" + currentCol + "," + currentRow + ")";
				//Set its parent
				Transform tempTileTransform = tempTile.GetComponent<Transform>();
				tempTileTransform.parent = this.gameObject.GetComponent<Transform>();
				//set its position (and rotation?) in the game world
				Vector3 newPosition; 

				newPosition.x = currentCol * tileWidth;
				newPosition.y = 0.0f;
				newPosition.z = currentRow * tileHeight;

				//since it's already a child under a parent gameobject
				tempTileTransform.localPosition = newPosition;

				// add the tile to our grid (data)
				grid[currentCol, currentRow] = tempTile.GetComponent<Room>();
			
			}
		}
			
		// for each item in our grid
//		// open the appropriate doors (walls)
//
//		for (int x = 0; x < numCols; x++) {
//			for (int y = 0; y < numRows; y++) {
//			
//				if (x != 0) {
//					grid [x, y].westWall.SetActive (false);
//				}
//				if (x != (numCols - 1)) {
//					grid [x, y].eastWall.SetActive (false);
//				}
//				if (y != 0) {
//					grid [x, y].southWall.SetActive (false);
//				}
//				if (y != (numRows - 1)) {
//					grid [x, y].northWall.SetActive (false);
//				}
//
//
//			}
//		}

		for (int x = 0; x < numCols; x++) {
			for (int y = 0; y < numRows; y++) {

				if (x == 0) { 
					grid [x, y].westRoom = null;
				} else {
					grid [x, y].westRoom = grid [x - 1, y];
				}

				if (x == numCols - 1) {
					grid [x, y].eastRoom = null;
				} else {
					grid [x, y].eastRoom = grid [x + 1, y];
				}

				if (y == 0) {
					grid [x, y].southRoom = null;
				} else {
					grid [x, y].southRoom = grid [x, y - 1];
				}

				if (y == numRows - 1) {
					grid [x, y].northRoom = null;
				} else {
					grid [x, y].northRoom = grid [x, y + 1];
				}
			}
		}


		//create maze 
		OpenMaze();
	}

	public void OpenMaze(){
		// create a stack of visited rooms (se we can backtrack through it later)
		Stack<Room> visitedRooms = new Stack<Room>();

		// start with a random room from the grid, add it to the stack and mark it as visited
		visitedRooms.Push(grid[UnityEngine.Random.Range(0, numCols), UnityEngine.Random.Range(0,numRows)]);

		//start with the only room on the stack. 
		Room currentRoom = visitedRooms.Peek();
		currentRoom.isVisited = true; // make sure the start is visited when we start

		while (currentRoom != null) {
			// using the "top" item of the stack

			// if no unvisited neighbors
			Room nextRoom = currentRoom.RandomUnvisitedNeighbor();

			if (nextRoom == null) {
				//move back in the stack
				visitedRooms.Pop ();
			} else {

				OpenDoors (currentRoom, nextRoom);

				visitedRooms.Push (nextRoom);
				nextRoom.isVisited = true;
			}

			//(if we can't go back in the stack, then we are done,)

			// If there are unvisisted neighbors,

			// open the doors and put it on the top of the stack
			//mark as visited



			// Repeat...
			currentRoom = visitedRooms.Peek();
		}

	}

	public void OpenDoors (Room currentRoom, Room nextRoom){

		if (currentRoom.northRoom == nextRoom) {
			currentRoom.northWall.SetActive (false);
			nextRoom.southWall.SetActive (false);
		}
		if (currentRoom.southRoom == nextRoom) {
			currentRoom.southWall.SetActive (false);
			nextRoom.northWall.SetActive (false);
		}
		if (currentRoom.eastRoom == nextRoom) {
			currentRoom.eastWall.SetActive (false);
			nextRoom.westWall.SetActive (false);
		}
		if (currentRoom.westRoom == nextRoom) {
			currentRoom.westWall.SetActive (false);
			nextRoom.eastWall.SetActive (false);
		}


	}


	public GameObject RandomTile() {
		// TODO: return a random tile

		return roomTiles [UnityEngine.Random.Range (0, roomTiles.Length)];
	}
}
