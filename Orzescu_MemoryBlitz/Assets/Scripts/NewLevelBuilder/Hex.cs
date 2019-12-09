using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {

    [Header("GameObject Data")]
	public GameObject floor;
    public GameObject capturePoint;
    public GameObject allWalls; // included just in case. no use yet
    public GameObject northEastWall;
    public GameObject eastWall; 
    public GameObject southEastWall;
    public GameObject northWestWall;
    public GameObject westWall;
    public GameObject southWestWall;

    [Header("Neighboring Hexes")]
    public Hex northEastHex;
    public Hex eastHex;
    public Hex southEastHex;
    public Hex northWestHex;
    public Hex westHex;
    public Hex southWestHex;

    [Header("Hex Data")]
    public Data hexOwner;
    public bool isVisited;
	public bool isCapturable;
    public Transform playerSpawnPoint;
    public Transform powerupSpawnPoint;
    public List<Transform> minionSpawnPoints;
    public List<GameObject> minions;
    private int deadMinions;

    public void Awake()
    {
        //add spawn points to game manager
        GameManager.instance.playerSpawnPoints.Add(playerSpawnPoint);
    }

    public void Start()
    {
        if (minions.Count != minionSpawnPoints.Count)
        {
            SpawnAllMinions();
            deadMinions = 0;
        }
    }

	public void Update(){
        //check if the minions on the tile have been killed off by a player or an AI

        CheckForDeadMinions();

        //If they have been, allow capture by other player/AI
        if (isCapturable == false)
        {
            capturePoint.SetActive(false);
        } else
        {
            capturePoint.SetActive(true);
        }
		

		//If no capture... respawn minions after an amount of time
	}

    public void CheckForDeadMinions()
    {
        if (minions.Count == 0)
        {
            Debug.Log("All Minions have been killed in : " + gameObject.name);
            deadMinions = 0;
            isCapturable = true;
        }
    }

    public void ChangeFloorTexture(Material PlayerColor){
        //change color of floor
        Renderer floorRenderer = floor.gameObject.GetComponent<Renderer>();
        floorRenderer.material = PlayerColor;
	}

    public Hex AI_GetNextHex(AIController AIController)
    {
        if (AIController.AIPersonality == AIController.AIPersonalities.Aggressive)
        {
            //if northeast wall is open
            if (northEastWall.activeSelf == false)
            {
                //if said hex is owned by a player other than the AI searching for a new tile
                if (northEastHex.hexOwner != null && !AIController.data.ownedHexes.Contains(northEastHex))
                {
                    //return the player owned hex to the aggressive AI
                    return northEastHex;
                }
            }
            //if east wall is open
            if (eastWall.activeSelf == false)
            {
                //if said hex is owned by a player other than the AI searching for a new tile
                if (eastHex.hexOwner != null && !AIController.data.ownedHexes.Contains(eastHex))
                {
                    //return the player owned hex to the aggressive AI
                    return eastHex;
                }
            }
            //if south east wall is open
            if (southEastWall.activeSelf == false)
            {
                //if said hex is owned by a player other than the AI searching for a new tile
                if (southEastHex.hexOwner != null && !AIController.data.ownedHexes.Contains(southEastHex))
                {
                    //return the player owned hex to the aggressive AI
                    return southEastHex;
                }
            }
            //if north west wall is open
            if (northWestWall.activeSelf == false)
            {
                //if said hex is owned by a player other than the AI searching for a new tile
                if (northWestHex.hexOwner != null && !AIController.data.ownedHexes.Contains(northWestHex))
                {
                    //return the player owned hex to the aggressive AI
                    return northWestHex;
                }
            }
            //if west wall is open
            if (westWall.activeSelf == false)
            {
                //if said hex is owned by a player other than the AI searching for a new tile
                if (westHex.hexOwner != null && !AIController.data.ownedHexes.Contains(westHex))
                {
                    //return the player owned hex to the aggressive AI
                    return westHex;
                }
            }
            //if south west wall is open
            if (southWestWall.activeSelf == false)
            {
                //if said hex is owned by a player other than the AI searching for a new tile
                if (southWestHex.hexOwner != null && !AIController.data.ownedHexes.Contains(southWestHex))
                {
                    //return the player owned hex to the aggressive AI
                    return southWestHex;
                }
            }

            //IF NO NEIGHBORING HEXES ARE OWNED BY OTHER PLAYERS Return the first available hex that's traversable to.

            //if northeast wall is open
            if (northEastWall.activeSelf == false)
            {
                return northEastHex;
            }
            //if east wall is open
            if (eastWall.activeSelf == false)
            {
                return eastHex;
            }
            //if south east wall is open
            if (southEastWall.activeSelf == false)
            {
                return southEastHex;
            }
            //if north west wall is open
            if (northWestWall.activeSelf == false)
            {
                return northWestHex;
            }
            //if west wall is open
            if (westWall.activeSelf == false)
            {
                return westHex;
            }
            //if south west wall is open
            if (southWestWall.activeSelf == false)
            {
                return southWestHex;
            }
        }
        if (AIController.AIPersonality == AIController.AIPersonalities.Dark)
        {
            //returns the very first territory that has an open door to it, and is not owned by the dark AI

            //if northeast wall is open
            if (northEastWall.activeSelf == false)
            {
                if (!AIController.data.ownedHexes.Contains(northEastHex))
                {
                    return northEastHex;
                }
            }
            //if east wall is open
            if (eastWall.activeSelf == false)
            {
                if (!AIController.data.ownedHexes.Contains(eastHex))
                {
                    return eastHex;
                }
            }
            //if southeast wall is open
            if (southEastWall.activeSelf == false)
            {
                if (!AIController.data.ownedHexes.Contains(southEastHex))
                {
                    return southEastHex;
                }
            }
            //if northwest wall is open
            if (northWestWall.activeSelf == false)
            {
                if (!AIController.data.ownedHexes.Contains(northWestHex))
                {
                    return northWestHex;
                }
            }
            //if west wall is open
            if (westWall.activeSelf == false)
            {
                if (!AIController.data.ownedHexes.Contains(westHex))
                {
                    return westHex;
                }
            }
            //if southwest wall is open
            if (southWestWall.activeSelf == false)
            {
                if (!AIController.data.ownedHexes.Contains(southWestHex))
                {
                    return southWestHex;
                }
            }
        }

        if (AIController.AIPersonality == AIController.AIPersonalities.Defensive)
        {
            //if the defensive AI has not yet claimed all it's territory
            if (AIController.captureLimit != AIController.data.ownedHexes.Count)
            {
                //return the first available territory for it to capture
                //if northeast wall is open
                if (northEastWall.activeSelf == false)
                {
                    if (!AIController.data.ownedHexes.Contains(northEastHex))
                    {
                        return northEastHex;
                    }
                }
                //if east wall is open
                if (eastWall.activeSelf == false)
                {
                    if (!AIController.data.ownedHexes.Contains(eastHex))
                    {
                        return eastHex;
                    }
                }
                //if southeast wall is open
                if (southEastWall.activeSelf == false)
                {
                    if (!AIController.data.ownedHexes.Contains(southEastHex))
                    {
                        return southEastHex;
                    }
                }
                //if northwest wall is open
                if (northWestWall.activeSelf == false)
                {
                    if (!AIController.data.ownedHexes.Contains(northWestHex))
                    {
                        return northWestHex;
                    }
                }
                //if west wall is open
                if (westWall.activeSelf == false)
                {
                    if (!AIController.data.ownedHexes.Contains(westHex))
                    {
                        return westHex;
                    }
                }
                //if southwest wall is open
                if (southWestWall.activeSelf == false)
                {
                    if (!AIController.data.ownedHexes.Contains(southWestHex))
                    {
                        return southWestHex;
                    }
                }

            } else {
                return null;
            }
        }
        if (AIController.AIPersonality == AIController.AIPersonalities.Passive)
        {
            //if northeast wall is open
            if (northEastWall.activeSelf == false)
            {
                if (northEastHex.hexOwner == null)
                {
                    return northEastHex;
                }
            }
            //if east wall is open
            if (eastWall.activeSelf == false)
            {
                if (eastHex.hexOwner == null)
                {
                    return eastHex;
                }
            }
            //if sotuheast wall is open
            if (southEastWall.activeSelf == false)
            {
                if (southEastHex.hexOwner == null)
                {
                    return southEastHex;
                }
            }
            //if northwest wall is open
            if (northWestWall.activeSelf == false)
            {
                if (northWestHex.hexOwner == null)
                {
                    return northWestHex;
                }
            }
            //if west wall is open
            if (westWall.activeSelf == false)
            {
                if (westHex.hexOwner == null)
                {
                    return westHex;
                }
            }
            //if southwest wall is open
            if (southWestWall.activeSelf == false)
            {
                if (southWestHex.hexOwner == null)
                {
                    return southWestHex;
                }
            }

            return null;
        }
        return null;
    }


    public Hex RandomUnvisistedNeighborHex()
    {
        //Create a list of unvisited hexes
        List<Hex> unvisitedNeighborHexes = new List<Hex>();

        //Check through available neighbors && if they're visited or not, if not, add them to the list of unvisited hexes
        if (northEastHex != null && !northEastHex.isVisited)
        {
            unvisitedNeighborHexes.Add(northEastHex);
        }
        if (eastHex != null && !eastHex.isVisited)
        {
            unvisitedNeighborHexes.Add(eastHex);
        }
        if (southEastHex != null && !southEastHex.isVisited)
        {
            unvisitedNeighborHexes.Add(southEastHex);
        }
        if (northWestHex != null && !northWestHex.isVisited)
        {
            unvisitedNeighborHexes.Add(northWestHex);
        }
        if (westHex != null && !westHex.isVisited)
        {
            unvisitedNeighborHexes.Add(westHex);
        }
        if (southWestHex != null && !southWestHex.isVisited)
        {
            unvisitedNeighborHexes.Add(southWestHex);
        }
        //If no neighborhs return null
        if (unvisitedNeighborHexes.Count == 0)
        {
            return null;
        } else {
            //return a neighbor from the unvisited list
            return unvisitedNeighborHexes[UnityEngine.Random.Range(0, unvisitedNeighborHexes.Count)];
        }
    }

    public GameObject SpawnMinion(Transform spawnPosition)
    {
        GameObject newMinion = Instantiate(GameManager.instance.minionPrefab, spawnPosition.position, Quaternion.identity, this.transform) as GameObject;

        return newMinion;
    }

    public void SpawnAllMinions()
    {
        foreach (Transform tf in minionSpawnPoints)
        {
            GameObject tempMinion = SpawnMinion(tf);
            minions.Add(tempMinion);
        }

        for (int i = 0; i < minions.Count; i++)
        {
            Minion mn = minions[i].GetComponent<Minion>();
            mn.ownerHex = this;
            minions[i].name = this.name + " Minion : " + i;
        }
    }

}
