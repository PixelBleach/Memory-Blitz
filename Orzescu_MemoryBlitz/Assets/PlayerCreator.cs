using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCreator : MonoBehaviour {

    public GameObject playerOne;
    public GameObject playerTwo;

    public GameObject AIOne;
    public GameObject AITwo;
    public GameObject AIThree;
    public GameObject AIFour;

	// Use this for initialization
	void Start () {
		
        if (!GameManager.instance.isTwoPlayer)
        {
            //spawn player 1 amd AI's
            Debug.Log("ISSSSSSSSSSSSSSSSS 1 PLAYER");
            playerOne = GameManager.instance.SpawnPlayerOne();
            AIOne = GameManager.instance.SpawnAggressiveAIPlayer();
            AITwo = GameManager.instance.SpawnPassiveAIPlayer();
            AIThree = GameManager.instance.SpawnDefensiveAIPlayer();
            AIFour = GameManager.instance.SpawnDarkAIPlayer();
            AIController TempController = AIOne.GetComponent<AIController>();
            TempController.isActive = true;
            TempController = AITwo.GetComponent<AIController>();
            TempController.isActive = true;
            TempController = AIThree.GetComponent<AIController>();
            TempController.isActive = true;
            TempController = AIFour.GetComponent<AIController>();
            TempController.isActive = true;
        } 
        if (GameManager.instance.isTwoPlayer)
        {
            Debug.Log("ISSSSSSSSSSSSSSSSS 2 PLAYER");
            //spawn 2 players and ai's
            playerOne = GameManager.instance.SpawnPlayerOne();
            playerTwo = GameManager.instance.SpawnPlayerTwo();
            Player2Controller TempPlayController = playerTwo.GetComponent<Player2Controller>();
            TempPlayController.isPlayerTwo = true;
            AIOne = GameManager.instance.SpawnAggressiveAIPlayer();
            AITwo = GameManager.instance.SpawnPassiveAIPlayer();
            AIThree = GameManager.instance.SpawnDefensiveAIPlayer();
            AIFour = GameManager.instance.SpawnDarkAIPlayer();
            AIController TempController = AIOne.GetComponent<AIController>();
            TempController.isActive = true;
            TempController = AITwo.GetComponent<AIController>();
            TempController.isActive = true;
            TempController = AIThree.GetComponent<AIController>();
            TempController.isActive = true;
            TempController = AIFour.GetComponent<AIController>();
            TempController.isActive = true;
        }


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
