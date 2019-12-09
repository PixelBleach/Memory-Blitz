using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {

    public GameObject powerupPrefabToSpawn;
    public Transform powerupSpawnLocation;
    public float cooldownBetweenSpawns;
    private GameObject spawnedPowerup;
    [SerializeField]
    private float countdownRemaining;


	void Start () {

        countdownRemaining = cooldownBetweenSpawns;
        powerupSpawnLocation = gameObject.transform.GetChild(0);

        //allow designers to assign powerups manually
        if (powerupPrefabToSpawn == null)
        {
            //if no powerup assigned, pick random one from list of registered powerups in the game manager. 
            powerupPrefabToSpawn = GameManager.instance.powerUpPrefabs[Random.Range(0, GameManager.instance.powerUpPrefabs.Length)];
        }


	}
	
	void Update () {

        //Debug.Log("running powerupspawner : " + gameObject.name);
        //only do if no powerup is currently spawned
        if (spawnedPowerup == null)
        {
            countdownRemaining -= Time.deltaTime;

            //if time to spawn, spawn, and reset timer
            if (countdownRemaining <= 0)
            {
                Debug.Log("Attempting Instantiation:");
                spawnedPowerup = Instantiate(powerupPrefabToSpawn, powerupSpawnLocation.position, Quaternion.identity, gameObject.transform) as GameObject;
                if (spawnedPowerup == null)
                {
                    Debug.Log("Instantiation Failed:");
                }
                countdownRemaining = cooldownBetweenSpawns;
            }
        }

	}
}
