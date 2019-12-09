using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public GameObject northWall;
    public GameObject southWall;
    public GameObject eastWall;
    public GameObject westWall;

	public Room northRoom;
	public Room southRoom;
	public Room eastRoom;
	public Room westRoom;

	public bool isVisited;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Room RandomUnvisitedNeighbor(){
	
		List<Room> unvisitedNeighbors = new List<Room> ();
		if (northRoom != null && !northRoom.isVisited) {
			unvisitedNeighbors.Add (northRoom);
		}
		if (southRoom != null && !southRoom.isVisited) {
			unvisitedNeighbors.Add (southRoom);
		}
		if (eastRoom != null && !eastRoom.isVisited) {
			unvisitedNeighbors.Add (eastRoom);
		}
		if (westRoom != null && !westRoom.isVisited) {
			unvisitedNeighbors.Add (westRoom);
		}

		if (unvisitedNeighbors.Count == 0) {
			//returns null if visited all neighborhs
			return null;
		} else {
			return unvisitedNeighbors [UnityEngine.Random.Range (0, unvisitedNeighbors.Count)];
		}


	}


}
