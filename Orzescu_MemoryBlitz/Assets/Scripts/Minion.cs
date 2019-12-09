using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour {

    //public Data data;
    public Mover mover;
    public Health health;

    public Transform availableVisuals;
    public List<GameObject> visuals;

    public Vector3 startPosition;
    public Vector3 currentLocation;
    public Vector3 moveDestination;

    public Hex ownerHex;

    public float minionHealth;
	public float currentMinionHealth;

    public float wanderArea;
    public float wanderSpeed;
    public float wanderRotation;
    public float wanderCooldown;
    public float maxWanderDistance;
	public float closeEnoughDistance = 0.2f;

    public bool tooFar;

    [SerializeField]
    private float cooldown;

	// Use this for initialization
	void Start () {

        //data = GetComponent<Data>();
        mover = GetComponent<Mover>();
        health = GetComponent<Health>();

        startPosition = this.transform.position;

        cooldown = wanderCooldown;

		moveDestination = new Vector3((Random.insideUnitSphere.x * wanderArea + startPosition.x), this.gameObject.transform.position.y, (Random.insideUnitSphere.z * wanderArea + startPosition.z));

        availableVisuals = gameObject.transform.FindChild("Visuals");

        foreach (Transform child in availableVisuals)
        {
            visuals.Add(child.gameObject);
        }

        SelectVisual();

    }
	
	// Update is called once per frame
	void Update () {

        Wander();
		//minionHealth = health.CurrentHealth;
	}

    public void Wander()
    {
        currentLocation = this.transform.position;
        if (tooFar)
        {
            //Debug.Log("Moving Back to Start Position...");
            mover.Move(currentLocation, startPosition, wanderSpeed, wanderRotation);
			Debug.DrawLine (transform.position, moveDestination,Color.red);
            moveDestination = startPosition;
			if ((currentLocation - startPosition).magnitude <= closeEnoughDistance)
            {
                tooFar = false;
            }
        } else {
            //chose location to wander to
            //moveDestination = new Vector3(Random.insideUnitSphere.x * wanderArea, this.gameObject.transform.position.y, Random.insideUnitSphere.z * wanderArea);

			if ((moveDestination - currentLocation).magnitude >= closeEnoughDistance)
            {
                //move to location

                //if Can't reach destination in time, return to start
                
                if (cooldown <= 0) {
                    //Debug.Log(this.name + " took too long to get to destination. Returning to start position...");
                    tooFar = true;
                    cooldown = wanderCooldown;
                } else {
                    mover.Move(currentLocation, moveDestination, wanderSpeed, wanderRotation);
					Debug.DrawLine (transform.position, moveDestination, Color.red);
                    //Debug.Log(this.name + " moving to new position.");
                    cooldown -= Time.deltaTime;
                }
            } else {
                cooldown = wanderCooldown;
				moveDestination = new Vector3((Random.insideUnitSphere.x * wanderArea + currentLocation.x), this.gameObject.transform.position.y, (Random.insideUnitSphere.x * wanderArea + currentLocation.z));
				Debug.DrawLine (transform.position, moveDestination,Color.red);
                
            }
            if ((currentLocation - startPosition).magnitude >= maxWanderDistance)
            {
                tooFar = true;
            }
        }
    }

    public void SelectVisual()
    {
        int numVisuals = visuals.Count;
        int pickRandom = Random.Range(0, numVisuals);
        //Debug.Log(pickRandom);
        for (int i = 0; i < numVisuals; i++)
        {
            if (i == pickRandom)
            {
                visuals[i].SetActive(true);
            } else
            {
                visuals[i].SetActive(false);
            }
        }

    }

	public void OnTriggerEnter(Collider collidedWithAttackBox){
		
		if (collidedWithAttackBox.gameObject.tag == "MeeleAttack") {
			Debug.Log (gameObject.name + "was hit by " + collidedWithAttackBox.gameObject.transform.parent.name);
			//if hit with meele attack, take dmg 
			Meele tempMeele = collidedWithAttackBox.gameObject.transform.parent.GetComponent<Meele>();
			float dmgToTake = tempMeele.dmgOnHit;
			health.TakeDamage (dmgToTake);
			Debug.Log (gameObject.name + " took " + dmgToTake + " damage from " + collidedWithAttackBox.gameObject.transform.parent.name + ".");
            if (health.CurrentHealth <= 0)
            {
                ownerHex.minions.Remove(this.gameObject);
            }

		}
	}
}
