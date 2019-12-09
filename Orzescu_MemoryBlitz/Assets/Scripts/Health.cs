using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }

    public Slider healthBarSlider;

    private Data data;
	private Minion minionData;

    private AudioSource healthAudioSource;

	// Use this for initialization
	void Start () {

        data = gameObject.GetComponent<Data>();
        healthAudioSource = gameObject.GetComponent<AudioSource>();

        if (data == null)
        {
			minionData = gameObject.GetComponent<Minion>();
            MaxHealth = minionData.minionHealth;
			minionData.currentMinionHealth = CurrentHealth;

        } else {
            MaxHealth = data.totalHitPoints;
            data.currentHitPoints = CurrentHealth;
        }

        CurrentHealth = MaxHealth;


        healthBarSlider.value = CalculateHealthPercentage();
    }

    void Update()
    {
    }
	
    public void TakeDamage(float dmgToTake)
    {
        CurrentHealth -= dmgToTake;

		if (data == null) {
			minionData.currentMinionHealth = CurrentHealth;
		} else {
			data.currentHitPoints = CurrentHealth;
		}
			
        healthBarSlider.value = CalculateHealthPercentage();

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        CurrentHealth = 0;
		if (data == null) {
			minionData.currentMinionHealth = CurrentHealth;
		} else {
			data.currentHitPoints = CurrentHealth;
		}

        healthBarSlider.value = CalculateHealthPercentage();
        //FOR NOW: Destroy's object
        //in future will most likely take the visuals of the game object, disable them, and disable the controller on the object also!
        if (gameObject.tag == "Player")
        {
            healthAudioSource.PlayOneShot(SoundManager.instance.deathSound, SoundManager.instance.sfxVolume);

            PlayerController tempPlayerController = gameObject.GetComponent<PlayerController>();
            if (tempPlayerController != null)
            {
                //do the death thing
                tempPlayerController.data.EnableDeathCanvas();
                tempPlayerController.data.currentPlayerLives -= 1;
                if (tempPlayerController.data.currentPlayerLives >= 1)
                {
                    tempPlayerController.data.health.CurrentHealth = tempPlayerController.data.totalHitPoints;
                    if (tempPlayerController.data.ownedHexes.Count >= 1)
                    {
                        int randomSpawnLocation = Random.Range(0, tempPlayerController.data.ownedHexes.Count);
                        tempPlayerController.data.mover.tf.position = tempPlayerController.data.ownedHexes[randomSpawnLocation].playerSpawnPoint.position;
                    } else
                    {
                        tempPlayerController.data.mover.tf.position = GameManager.instance.playerSpawnPoints[Random.Range(0, GameManager.instance.playerSpawnPoints.Count)].position;
                    }
                } else
                {
                    //show game over screen
                    tempPlayerController.data.mover.enabled = false;
                    tempPlayerController.data.EnableGameOverCanvas();
                }
            }
            Player2Controller tempPlayer2Controller = gameObject.GetComponent<Player2Controller>();
            if (tempPlayer2Controller != null)
            {
                //do the death thing
                //do the death thing
                tempPlayer2Controller.data.EnableDeathCanvas();
                tempPlayer2Controller.data.currentPlayerLives -= 1;
                if (tempPlayer2Controller.data.currentPlayerLives >= 1)
                {
                    tempPlayer2Controller.data.health.CurrentHealth = tempPlayer2Controller.data.totalHitPoints;
                    if (tempPlayer2Controller.data.ownedHexes.Count >= 1)
                    {
                        int randomSpawnLocation = Random.Range(0, tempPlayer2Controller.data.ownedHexes.Count);
                        tempPlayer2Controller.data.mover.tf.position = tempPlayer2Controller.data.ownedHexes[randomSpawnLocation].playerSpawnPoint.position;
                    }
                    else
                    {
                        tempPlayer2Controller.data.mover.tf.position = GameManager.instance.playerSpawnPoints[Random.Range(0, GameManager.instance.playerSpawnPoints.Count)].position;
                    }
                }
                else
                {
                    //show game over screen
                    tempPlayer2Controller.data.EnableGameOverCanvas();
                }
            }
            AIController tempAIController = gameObject.GetComponent<AIController>();
            if (tempAIController != null)
            {
                //do the death thing
                tempAIController.data.health.CurrentHealth = tempAIController.data.totalHitPoints;
                if (tempAIController.data.ownedHexes.Count >= 1)
                {
                     int randomSpawnLocation = Random.Range(0, tempAIController.data.ownedHexes.Count);
                     tempAIController.data.mover.tf.position = tempAIController.data.ownedHexes[randomSpawnLocation].playerSpawnPoint.position;
                }
                else
                {
                    tempAIController.data.mover.tf.position = GameManager.instance.playerSpawnPoints[Random.Range(0, GameManager.instance.playerSpawnPoints.Count)].position;
                }
                tempAIController.ChangeState(AIController.AIStates.Start);

            }
        }
        if (gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }

        Debug.Log(gameObject.name + "Took too much damage and died!");
    }

    public float CalculateHealthPercentage()
    {
        return CurrentHealth / MaxHealth;
    }
		
}
