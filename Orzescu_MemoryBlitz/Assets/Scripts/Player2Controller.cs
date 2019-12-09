using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour {

    [Header("Components")]
    public Data data;
    public bool isPlayerTwo = true;

    [Header("Input Keys for Player 2")]
    public KeyCode forwardKeyP2;
    public KeyCode backwardKeyP2;
    public KeyCode turnLeftKeyP2;
    public KeyCode turnRightKeyP2;
    public KeyCode attackKeyP2;

    [Header("Attacking Settings")]
    [Tooltip("Time between attacks in seconds")]
    public float attackingCooldown;

    private bool canFire;
    private float countdown;
    public Camera playerCamera;

    public Animator anim;

	// Use this for initialization
	void Start () {
        canFire = true;
        countdown = attackingCooldown;
        anim = gameObject.GetComponent<Animator>();
        GameManager.instance.playersList.Add(this.gameObject);

        if (GameManager.instance.isTwoPlayer == true)
        {
            if (!isPlayerTwo)
            {
                playerCamera.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
            }
            if (isPlayerTwo)
            {
                playerCamera.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 forward;

        UpdateUIInformation();

        countdown -= Time.deltaTime;

        anim.SetFloat("DirZ", 0);

        // MOVEMENT BLOCK

        if (isPlayerTwo)
        {
            float moveSpeed = 0.0f;
            if (Input.GetKey(forwardKeyP2))
            {
                moveSpeed = data.moveForwardSpeed;
                anim.SetFloat("DirZ", 1);
            }
            if (Input.GetKey(backwardKeyP2))
            {
                //TODO: Make backwards movement maybe?? for now basic backwards movement
                moveSpeed = data.moveBackwardSpeed;
                anim.SetFloat("DirZ", -1);
            }
            //AFTER WE DETERMINE which direction the player wants to move, then send message to move
            forward = this.gameObject.transform.forward * moveSpeed;
            data.gameObject.SendMessage("Move", forward);

            // TURNING BLOCK
            if (Input.GetKey(turnRightKeyP2))
            {
                data.gameObject.SendMessage("Turn", data.turnSpeed);
            }
            if (Input.GetKey(turnLeftKeyP2))
            {
                data.gameObject.SendMessage("Turn", -1 * data.turnSpeed);
            }

            // ATTACKING BLOCK
            if (Input.GetKey(attackKeyP2))
            {
                if (countdown <= 0) //We can shoot! Cooldown expired
                {
                    data.meele.StartCoroutine("MeeleAttack");

                    countdown = attackingCooldown;
                }
                else {
                    //We can't shoot!
                    Debug.Log("Attacking Cooldown has not expired yet. You have " + countdown + " seconds until you can attack again");

                }

            }
        }
        

    }

    public void UpdateUIInformation()
    {
        data.livesText.text = data.currentPlayerLives.ToString();
        data.scoreText.text = data.ownedHexes.Count.ToString();
    }
}
