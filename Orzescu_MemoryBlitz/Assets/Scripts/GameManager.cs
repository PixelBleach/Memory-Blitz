using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [Header("Game Settings")]
    public bool isTwoPlayer;
    public bool isMapOfDay;

    [Header("GameObject Prefabs")]
    public GameObject playerPrefab;
    public GameObject player2Prefab;
    public GameObject playerAIPrefab;
    public GameObject[] powerUpPrefabs;

    [Header("Player Information")]
    public PlayerController player;
    public List<GameObject> playersList;
    public List<Transform> playerSpawnPoints;
    public List<Material> availablePlayerColors;

    [Header("AI Information")]
    public GameObject minionPrefab;
    public int numberOfAI;
    public List<AIController> AIPlayersList;

    void Awake()
    {
        //This'll ensure only 1 gamemanager object is in a scene at any time
        if (instance != null)
        {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject SpawnPlayerOne()
    {
        GameObject newPlayer;
        newPlayer = Instantiate(playerPrefab, playerSpawnPoints[Random.Range(0, playerSpawnPoints.Count)].position, Quaternion.identity) as GameObject;
        playersList.Add(newPlayer);
        player = newPlayer.GetComponent<PlayerController>();

        return newPlayer;
    }

    public GameObject SpawnPlayerTwo()
    {
        GameObject newPlayer;
        newPlayer = Instantiate(player2Prefab, playerSpawnPoints[Random.Range(0, playerSpawnPoints.Count)].position, Quaternion.identity) as GameObject;
        playersList.Add(newPlayer);
        player = newPlayer.GetComponent<PlayerController>();

        return newPlayer;
    }

    public GameObject SpawnAggressiveAIPlayer()
    {
        Debug.Log("Spawning AI...");
        GameObject newAIPlayer;
        newAIPlayer = Instantiate(playerAIPrefab, playerSpawnPoints[Random.Range(0,playerSpawnPoints.Count)].position, Quaternion.identity) as GameObject;
        AIController tempAIController = newAIPlayer.GetComponent<AIController>();
        tempAIController.AIPersonality = AIController.AIPersonalities.Aggressive;
        tempAIController.SelectVisual();
        newAIPlayer.name = "Aggressive AI Player : ";

        return newAIPlayer;
    }

    public GameObject SpawnPassiveAIPlayer()
    {
        Debug.Log("Spawning AI...");
        GameObject newAIPlayer;
        newAIPlayer = Instantiate(playerAIPrefab, playerSpawnPoints[Random.Range(0, playerSpawnPoints.Count)].position, Quaternion.identity) as GameObject;
        AIController tempAIController = newAIPlayer.GetComponent<AIController>();
        tempAIController.AIPersonality = AIController.AIPersonalities.Passive;
        tempAIController.SelectVisual();
        newAIPlayer.name = "Passive AI Player : ";

        return newAIPlayer;
    }

    public GameObject SpawnDefensiveAIPlayer()
    {
        Debug.Log("Spawning AI...");
        GameObject newAIPlayer;
        newAIPlayer = Instantiate(playerAIPrefab, playerSpawnPoints[Random.Range(0, playerSpawnPoints.Count)].position, Quaternion.identity) as GameObject;
        AIController tempAIController = newAIPlayer.GetComponent<AIController>();
        tempAIController.AIPersonality = AIController.AIPersonalities.Defensive;
        tempAIController.SelectVisual();
        newAIPlayer.name = "Defensive AI Player : ";

        return newAIPlayer;
    }

    public GameObject SpawnDarkAIPlayer()
    {
        Debug.Log("Spawning AI...");
        GameObject newAIPlayer;
        newAIPlayer = Instantiate(playerAIPrefab, playerSpawnPoints[Random.Range(0, playerSpawnPoints.Count)].position, Quaternion.identity) as GameObject;
        AIController tempAIController = newAIPlayer.GetComponent<AIController>();
        tempAIController.AIPersonality = AIController.AIPersonalities.Dark;
        tempAIController.SelectVisual();
        newAIPlayer.name = "Dark AI Player : ";

        return newAIPlayer;
    }

    public void CreateNewAIPlayers()
    {
        if (playerSpawnPoints != null)
        {
            Debug.Log("Creating New Aggressive AI Player");
            GameObject newAggroAI = SpawnAggressiveAIPlayer();
            Debug.Log(newAggroAI.name + " COMPLETED");
            AIPlayersList.Add(newAggroAI.GetComponent<AIController>());
            playersList.Add(newAggroAI);
            newAggroAI.GetComponent<AIController>().isActive = true;

            Debug.Log("Creating New Passive AI Player");
            GameObject newPassiveAI = SpawnPassiveAIPlayer();
            Debug.Log(newPassiveAI.name + " COMPLETED");
            AIPlayersList.Add(newPassiveAI.GetComponent<AIController>());
            playersList.Add(newPassiveAI);
            newPassiveAI.GetComponent<AIController>().isActive = true;

            Debug.Log("Creating New Defensive AI Player");
            GameObject newDefensiveAI = SpawnDefensiveAIPlayer();
            Debug.Log(newDefensiveAI.name + " COMPLETED");
            AIPlayersList.Add(newDefensiveAI.GetComponent<AIController>());
            playersList.Add(newDefensiveAI);
            newDefensiveAI.GetComponent<AIController>().isActive = true;

            Debug.Log("Creating New Dark AI Player");
            GameObject newDarkAI = SpawnDarkAIPlayer();
            Debug.Log(newDarkAI.name + " COMPLETED");
            AIPlayersList.Add(newDarkAI.GetComponent<AIController>());
            playersList.Add(newDarkAI);
            newDarkAI.GetComponent<AIController>().isActive = true;
        }
    }


}
