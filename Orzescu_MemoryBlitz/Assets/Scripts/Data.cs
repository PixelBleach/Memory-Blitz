using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Data : MonoBehaviour {

    [Header("Components")]
    public Mover mover;
    //public Shooter shooter;
    public Health health;
	public Meele meele;
    public GameObject minimap;
    [SerializeField]
    private GameObject minimapIndicator;
    public Transform minimap2PlayerPosition;

    [Header("UI Info")]
    public Text scoreText;
    public Text livesText;
    public Canvas deathCanvas;
    public Canvas gameOverCanvas;

	[Header("Capture Information")]
	public List<Hex> ownedHexes;

	[Header("Player Data")]
    public Material playerColor;
    public int totalPlayerLives;
    public int currentPlayerLives;
    [Tooltip("Turning Speed in Degree's per Second")]
    public float turnSpeed;
    [Tooltip("Move Speed in Meter's per Second")]
    public float moveForwardSpeed;
    [Tooltip("Move Speed in Meter's per Second")]
    public float moveBackwardSpeed;
    [Tooltip("Total HP")]
    public float totalHitPoints;
    public float currentHitPoints;
    public float currentScore;
    public float scoreIncrementOnEnemyKill;

    //Unused Stuff
    //public Transform bulletSpawnOrigin;
    //[Tooltip("Firing Force in Meter's per Second")]
    //public float bulletFiringForce;
    //[Tooltip("Damage done in Hitpoints")]
    //public float bulletDamageValue;
    //public float damageTakenPerBullet;

    void Start()
    {
        mover = this.gameObject.GetComponent<Mover>();
        //shooter = this.gameObject.GetComponent<Shooter>();
        health = this.gameObject.GetComponent<Health>();
		meele = this.gameObject.GetComponent<Meele> ();
        playerColor = GameManager.instance.availablePlayerColors[Random.Range(0, GameManager.instance.availablePlayerColors.Count)];
        GameManager.instance.availablePlayerColors.Remove(playerColor);
        Renderer tempRend = minimapIndicator.gameObject.GetComponent<Renderer>();
        tempRend.material = playerColor;

        currentPlayerLives = totalPlayerLives;

    }

    public void EnableDeathCanvas()
    {
        deathCanvas.enabled = true;
    }

    public void DisableDeathCanvas()
    {
        deathCanvas.enabled = false;
    }

    public void EnableGameOverCanvas()
    {
        gameOverCanvas.enabled = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
		
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
