using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AIController : MonoBehaviour {

    public Data data;

    //Enums
    public enum AIStates { Start, Idle, CaptureTile, KillMinions, ChasePlayer, FleeFromPlayer, PatrolOwnedTerritory, MoveToNextHex, MoveToCurrentHex, GetHexInfo, Dead, Respawn };
    public enum AIPersonalities { Aggressive, Passive, Defensive, Dark };
    public enum AvoidanceStates { None, TurnToAvoid, MoveToAvoid };
    public enum PatrolTypes { Stop, Loop, PingPong };

    public bool isActive;

    public Stopwatch stopwatch;

    [Header("AI Settings")]
    public AIPersonalities AIPersonality;
    public float attackingCooldown;
    public float closeEnoughDistance;
    public float fleeDistance;
    public float captureLimit;
    public float timeDead;

    [Header("Sense Variables")]
    public float hearingSensitivity;
    public float FieldOfView;
    public float viewDistance;
    public GameObject minionFinder;

    [Header("Avoidance Settings")]
    public float obsAvoidDistance;
    public float avoidMoveTime;

    [Header("State Machine Tracking")]
    public Hex currentHex;
    public Hex nextHex;
    public GameObject currentPlayerTarget;
    [SerializeField]
    private float boredTimer;
    public float boredLimit;
    public AIStates AIState;
    public float timeEnteredAIState;
    public AvoidanceStates avoidState;
    public float timeEnteredAvoidState;
    public LayerMask obstacleLayers;

    [Header("Stored Visuals")]
    public Animator anim;
    public GameObject[] personalityVisuals;

    [Header("Waypoints")]
    public int currentWaypoint;
    public List<Transform> waypoints;

    [HideInInspector]
    public Transform tf;

    public List<GameObject> minionsToKill;

    // Use this for initialization
    void Start () {
        minionFinder.SetActive(false);
        data = gameObject.GetComponent<Data>();
        tf = gameObject.GetComponent<Transform>();
        anim = gameObject.GetComponent<Animator>();
        SelectVisual();

        if (AIPersonality == AIPersonalities.Aggressive)
        {
            boredLimit = 99999999999;
        }
	}
	
	// Update is called once per frame
	void Update () {

		if (!isActive)
        {
            return;
        }

        anim.SetFloat("DirZ", 0);

        #region Aggressive State Machine

        //Aggressive State Machine
        if (AIPersonality == AIPersonalities.Aggressive)
        {
            //Aggressive State Machine
            switch (AIState)
            {
                case AIStates.Start:
                    ChangeState(AIStates.GetHexInfo);
                    break;
                case AIStates.GetHexInfo:
                    FindCurrentHexandMinions();
                    if (currentHex.minions == minionsToKill)
                    {
                        ChangeState(AIStates.KillMinions);
                    }
                    if (CanSee(GameManager.instance.playersList))
                    {
                        ChangeState(AIStates.ChasePlayer);
                    }
                    if (isHealthBelow(0))
                    {
                        ChangeState(AIStates.Dead);
                    }
                    break;
                case AIStates.KillMinions:
                    KillMinions();
                    if (minionsToKill.Count == 0)
                    {
                        ChangeState(AIStates.CaptureTile);
                    }
                    if (CanSee(GameManager.instance.playersList))
                    {
                        ChangeState(AIStates.ChasePlayer);
                    }
                    if (isHealthBelow(0))
                    {
                        ChangeState(AIStates.Dead);
                    }

                    break;
                case AIStates.CaptureTile:
                    WalkToCapturePoint();
                    if (data.ownedHexes.Contains(currentHex))
                    {
                        nextHex = currentHex.AI_GetNextHex(this);
                        ChangeState(AIStates.MoveToNextHex);
                    }
                    if (CanSee(GameManager.instance.playersList))
                    {
                        ChangeState(AIStates.ChasePlayer);
                    }
                    if (isHealthBelow(0))
                    {
                        ChangeState(AIStates.Dead);
                    }
                    break;
                case AIStates.ChasePlayer:
                    Chase(currentPlayerTarget.transform);
                    if (isHealthBelow(0))
                    {
                        ChangeState(AIStates.Dead);
                    }
                    if (isHealthBelow(0))
                    {
                        ChangeState(AIStates.Dead);
                    }
                    break;
                case AIStates.MoveToNextHex:
                    MoveToTargetWithObsAvoidance(nextHex.playerSpawnPoint);
                    if ((data.mover.tf.position - nextHex.playerSpawnPoint.position).magnitude < closeEnoughDistance)
                    {
                        ClearMinionsToKillList();
                        ClearWaypointList();
                        currentHex = null;
                        nextHex = null;
                        ChangeState(AIStates.GetHexInfo);
                    }
                    if (CanSee(GameManager.instance.playersList))
                    {
                        ChangeState(AIStates.ChasePlayer);
                    }
                    if (isHealthBelow(0))
                    {
                        ChangeState(AIStates.Dead);
                    }
                    break;
                case AIStates.Idle:
                    ChangeState(AIStates.MoveToCurrentHex);
                    if (isHealthBelow(0))
                    {
                        ChangeState(AIStates.Dead);
                    }
                    UnityEngine.Debug.Log("I'm currently Idling");
                    break;
                case AIStates.Dead:
                    data.mover.enabled = false;
                    data.currentHitPoints = 0;
                    UnityEngine.Debug.Log("Dead");
                    break;
            }
        }

        #endregion

        #region Passive State Machine
        //Passive State Machine
        if (AIPersonality == AIPersonalities.Passive)
        {
            //Passive State Machine
            switch (AIState)
            {
                case AIStates.Start:
                    ChangeState(AIStates.GetHexInfo);
                    break;

                case AIStates.GetHexInfo:
                    FindCurrentHexandMinions();
                    if (currentHex.minions == minionsToKill)
                    {
                        ChangeState(AIStates.KillMinions);
                    }
                    if (isHealthBelow(0))
                    {
                        ChangeState(AIStates.Dead);
                    }
                    break;

                case AIStates.KillMinions:
                    if (CanSee(GameManager.instance.playersList))
                    {
                        ChangeState(AIStates.FleeFromPlayer);
                    }
                    KillMinions();
                    if (minionsToKill.Count == 0)
                    {
                        ChangeState(AIStates.CaptureTile);
                    }

                    break;

                case AIStates.CaptureTile:
                    WalkToCapturePoint();
                    if (data.ownedHexes.Contains(currentHex))
                    {
                        nextHex = currentHex.AI_GetNextHex(this);
                        ChangeState(AIStates.MoveToNextHex);
                    }
                    if (CanSee(GameManager.instance.playersList))
                    {
                        ChangeState(AIStates.FleeFromPlayer);
                    }
                    if (isHealthBelow(0))
                    {
                        ChangeState(AIStates.Dead);
                    }
                    break;

                case AIStates.FleeFromPlayer:
                    Flee(currentPlayerTarget.transform);
                    if (isHealthBelow(0))
                    {
                        ChangeState(AIStates.Dead);
                    }
                    break;

                case AIStates.MoveToNextHex:
                    MoveToTargetWithObsAvoidance(nextHex.playerSpawnPoint);
                    if ((data.mover.tf.position - nextHex.playerSpawnPoint.position).magnitude < closeEnoughDistance)
                    {
                        ClearMinionsToKillList();
                        ClearWaypointList();
                        currentHex = null;
                        nextHex = null;
                        ChangeState(AIStates.GetHexInfo);
                    }
                    if (CanSee(GameManager.instance.playersList))
                    {
                        ChangeState(AIStates.FleeFromPlayer);
                    }
                    if (isHealthBelow(0))
                    {
                        ChangeState(AIStates.Dead);
                    }
                    break;

                case AIStates.MoveToCurrentHex:
                    MoveToTargetWithObsAvoidance(currentHex.playerSpawnPoint);
                    if ((data.mover.tf.position - currentHex.playerSpawnPoint.position).magnitude < closeEnoughDistance  && currentHex.minions.Count == 0)
                    {
                        ClearMinionsToKillList();
                        ClearWaypointList();
                        currentHex = null;
                        nextHex = null;
                        ChangeState(AIStates.GetHexInfo);
                    } else {
                        ChangeState(AIStates.KillMinions);
                    }
                    if (CanSee(GameManager.instance.playersList))
                    {
                        ChangeState(AIStates.FleeFromPlayer);
                    }
                    if (isHealthBelow(0))
                    {
                        ChangeState(AIStates.Dead);
                    }
                    break;

                case AIStates.Idle:
                    ChangeState(AIStates.MoveToCurrentHex);
                    if (isHealthBelow(0))
                    {
                        ChangeState(AIStates.Dead);
                    }
                    break;

                case AIStates.Dead:
                    data.mover.enabled = false;
                    data.currentHitPoints = 0;
                    break;
            }
        }

        #endregion

        #region Defensive State Machine
        //Defensive State Machine
        if (AIPersonality == AIPersonalities.Defensive)
        {
            //Defensive State Machine
            switch (AIState)
            {
                case AIStates.Start:
                    ChangeState(AIStates.GetHexInfo);
                    break;
                case AIStates.GetHexInfo:
                    FindCurrentHexandMinions();
                    if (currentHex.minions == minionsToKill)
                    {
                        ChangeState(AIStates.KillMinions);
                    }
                    break;
                case AIStates.KillMinions:
                    KillMinions();
                    if (minionsToKill.Count == 0)
                    {
                        ChangeState(AIStates.CaptureTile);
                    }
                    break;
                case AIStates.CaptureTile:
                    WalkToCapturePoint();
                    if (data.ownedHexes.Contains(currentHex))
                    {
                        nextHex = currentHex.AI_GetNextHex(this);
                        ChangeState(AIStates.MoveToNextHex);
                    }
                    if (CanSee(GameManager.instance.playersList))
                    {
                        ChangeState(AIStates.ChasePlayer);
                    }
                    break;
                case AIStates.ChasePlayer:
                    Chase(currentPlayerTarget.transform);

                    break;
                case AIStates.MoveToNextHex:
                    MoveToTargetWithObsAvoidance(nextHex.playerSpawnPoint);
                    if ((data.mover.tf.position - nextHex.playerSpawnPoint.position).magnitude < closeEnoughDistance)
                    {
                        ClearMinionsToKillList();
                        ClearWaypointList();
                        currentHex = null;
                        nextHex = null;
                        ChangeState(AIStates.GetHexInfo);
                    }
                    break;
                case AIStates.PatrolOwnedTerritory:

                    break;
                case AIStates.Idle:
                    UnityEngine.Debug.Log("I'm currently Idling");
                    break;
                case AIStates.Dead:
                    UnityEngine.Debug.Log("Dead");
                    break;
            }
        }

        #endregion

        #region Dark State Machine
        //Dark State Machine
        if (AIPersonality == AIPersonalities.Dark)
        {
            //Dark State Machine
            switch (AIState)
            {
                case AIStates.Start:
                    ChangeState(AIStates.GetHexInfo);
                    break;
                case AIStates.GetHexInfo:
                    FindCurrentHexandMinions();
                    if (currentHex.minions == minionsToKill)
                    {
                        ChangeState(AIStates.KillMinions);
                    }
                    break;
                case AIStates.KillMinions:
                    KillMinions();
                    if (minionsToKill.Count == 0)
                    {
                        ChangeState(AIStates.CaptureTile);
                    }
                    break;
                case AIStates.CaptureTile:
                    WalkToCapturePoint();
                    if (data.ownedHexes.Contains(currentHex))
                    {
                        nextHex = currentHex.AI_GetNextHex(this);
                        ChangeState(AIStates.MoveToNextHex);
                    }
                    if (CanSee(GameManager.instance.playersList))
                    {
                        ChangeState(AIStates.ChasePlayer);
                    }
                    break;
                case AIStates.ChasePlayer:
                    Chase(currentPlayerTarget.transform);

                    break;
                case AIStates.MoveToNextHex:
                    MoveToTargetWithObsAvoidance(nextHex.playerSpawnPoint);
                    if ((data.mover.tf.position - nextHex.playerSpawnPoint.position).magnitude < closeEnoughDistance)
                    {
                        ClearMinionsToKillList();
                        ClearWaypointList();
                        currentHex = null;
                        nextHex = null;
                        ChangeState(AIStates.GetHexInfo);
                    }
                    break;
                case AIStates.Idle:
                    UnityEngine.Debug.Log("I'm currently Idling");
                    break;
                case AIStates.Dead:
                    UnityEngine.Debug.Log("Dead");
                    break;
            }
        }
        #endregion

    }

    #region State Machine Functions

    public void ChangeState(AIStates newState)
    {
        AIState = newState;
        timeEnteredAIState = Time.time;
    }

    void ChangeState(AvoidanceStates newState)
    {
        UnityEngine.Debug.Log("Changing to State : " + newState.ToString());
        //change the state
        avoidState = newState;
        timeEnteredAvoidState = Time.time;
    }

    #region Hex Related
    public void FindCurrentHexandMinions()
    {
        //stopwatch.Start();
        //enable trigger component to pass up minions on the current tile
        minionFinder.SetActive(true);
        if (currentHex.minions == minionsToKill)
        {
            minionFinder.SetActive(false);
        }
        //stopwatch.Stop();
        //UnityEngine.Debug.Log("Stopwatch Elapsed Time :" + stopwatch.Elapsed);
    }

    public void WalkToCapturePoint()
    {
        if (currentHex.isCapturable)
        {
            Transform target = currentHex.playerSpawnPoint.transform;
            MoveToTargetWithObsAvoidance(target);
        }
    }

    public void PatrolOwnedTerritory()
    {
        for (int i = 0; i < data.ownedHexes.Count; i++)
        {
        }
    }
    #endregion

    #region Minion Related

    void PopulateMinionsListandWaypoints()
    {
        //if waypoints list is empty and there are minions to kill
        if (minionsToKill.Count != 0 && waypoints.Count == 0)
        {
            //Get the minions Transforms and add them to the waypoint list
            foreach (GameObject minion in minionsToKill)
            {
                if (waypoints.Count != minionsToKill.Count)
                {
                    Transform tempTF = minion.GetComponent<Transform>();
                    waypoints.Add(tempTF);
                }
            }
        }
    }

    Transform GetNextTargetMinion()
    {
        for (int minion = 0; minion < waypoints.Count; minion++)
        {
            if (waypoints[minion] == null)
            {
                //do nothing
            }
            else
            {
                Transform nextMinion = waypoints[minion];
                return nextMinion;
            }
        }
        return null;
    }

    public void KillMinions()
    {
        PopulateMinionsListandWaypoints();
        //walk to each waypoint with obstacle avoidance, and kill the minion
        //Debug.Log("Waypoint list to kill minions is populated");
        Transform nextTarget = GetNextTargetMinion();
        if (nextTarget != null)
        {
            MoveToMinionAndKillMinion(nextTarget);
        }
    }

    void MoveToMinionAndKillMinion(Transform target)
    {
        //Move to each waypoint with obstacle avoidance.
        switch (avoidState)
        {
            case AvoidanceStates.None:

                //Check for Transitions
                if (!CanMoveForward(obsAvoidDistance))
                {
                    ChangeState(AvoidanceStates.TurnToAvoid);
                }


                //if we're not at the waypoint
                if (Vector3.Distance(data.mover.tf.position, target.position) > closeEnoughDistance)
                {
                    //create vector from our pos to target position
                    Vector3 vectorToTarget = target.position - data.mover.tf.position;
                    //create quaternion
                    Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
                    //if NOT facing goal
                    if (data.mover.tf.rotation != targetRotation)
                    {
                        //turn to face target
                        data.mover.tf.rotation = Quaternion.RotateTowards(data.mover.tf.rotation, targetRotation, data.turnSpeed * Time.deltaTime);
                    }
                    else {
                        //we're facing the target - this is intentionally blank
                    }
                    //Move Towards the taget
                    data.mover.Move(data.mover.tf.forward * data.moveForwardSpeed);
                    anim.SetFloat("DirZ", 1);
                }
                else {
                    //We are at the waypoint...
                    //THIS IS WHERE WE'LL KILL THE MINIONS and change the current waypoint on a minion death
                    if (target != null)
                    {
                        UnityEngine.Debug.Log("Attacking????");
                        data.meele.StartCoroutine("MeeleAttack");
                    }
                    else {
                        //minion is dead
                        ChangeState(AIStates.KillMinions);
                    }

                }
                break;

            case AvoidanceStates.MoveToAvoid:
                //Check for transitions
                if (Time.time - timeEnteredAvoidState > avoidMoveTime)
                {
                    ChangeState(AvoidanceStates.None);
                }
                if (!CanMoveForward(obsAvoidDistance))
                {
                    ChangeState(AvoidanceStates.TurnToAvoid);
                }
                //Do work
                data.mover.Move(data.mover.transform.forward * data.moveForwardSpeed);
                anim.SetFloat("DirZ", 1);
                break;

            case AvoidanceStates.TurnToAvoid:
                data.mover.Turn(data.turnSpeed);
                //Check for transitions
                if (CanMoveForward(obsAvoidDistance))
                {
                    ChangeState(AvoidanceStates.MoveToAvoid);
                }
                break;
        }
    }

    #endregion

    #region AI Actions

    void MoveToTargetWithObsAvoidance(Transform target)
    {
        //Move to each waypoint with obstacle avoidance.
        switch (avoidState)
        {
            case AvoidanceStates.None:

                //Check for Transitions
                if (!CanMoveForward(obsAvoidDistance))
                {
                    ChangeState(AvoidanceStates.TurnToAvoid);
                }

                //if we're not at the waypoint
                if (Vector3.Distance(data.mover.tf.position, target.position) > closeEnoughDistance)
                {
                    //create vector from our pos to target position
                    Vector3 vectorToTarget = target.position - data.mover.tf.position;
                    //create quaternion
                    Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
                    //if NOT facing goal
                    if (data.mover.tf.rotation != targetRotation)
                    {
                        //turn to face target
                        data.mover.tf.rotation = Quaternion.RotateTowards(data.mover.tf.rotation, targetRotation, data.turnSpeed * Time.deltaTime);
                    }
                    else {
                        //we're facing the target - this is intentionally blank
                    }
                    //Move Towards the taget
                    data.mover.Move(data.mover.tf.forward * data.moveForwardSpeed);
                    anim.SetFloat("DirZ", 1);
                }
                else {
                    //We are at the target  
                }

                break;


            case AvoidanceStates.MoveToAvoid:
                //Do work
                data.mover.Move(data.mover.transform.forward * data.moveForwardSpeed);
                anim.SetFloat("DirZ", 1);
                //Check for transitions
                if (Time.time - timeEnteredAvoidState > avoidMoveTime)
                {
                    ChangeState(AvoidanceStates.None);
                }
                if (!CanMoveForward(obsAvoidDistance))
                {
                    ChangeState(AvoidanceStates.TurnToAvoid);
                }
                break;

            case AvoidanceStates.TurnToAvoid:
                data.mover.Turn(data.turnSpeed);
                //Check for transitions
                if (CanMoveForward(obsAvoidDistance))
                {
                    ChangeState(AvoidanceStates.MoveToAvoid);
                }
                break;
        }
    }

    public void Chase(Transform target)
    {
        //Move to target with obstacle avoidance.
        switch (avoidState)
        {
            case AvoidanceStates.None:
                Vector3 forward;
                //Check for Transitions
                if (!CanMoveForward(obsAvoidDistance))
                {
                    ChangeState(AvoidanceStates.TurnToAvoid);
                }


                //if we're not at the waypoint
                if (Vector3.Distance(data.mover.tf.position, target.position) > closeEnoughDistance)
                {
                    //create vector from our pos to target position
                    Vector3 vectorToTarget = target.position - data.mover.tf.position;
                    //create quaternion
                    Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
                    //if NOT facing goal
                    if (data.mover.tf.rotation != targetRotation)
                    {
                        //turn to face target
                        data.mover.tf.rotation = Quaternion.RotateTowards(data.mover.tf.rotation, targetRotation, data.turnSpeed * Time.deltaTime);
                    }
                    else {
                        //we're facing the target - this is intentionally blank
                    }
                    //Move Towards the taget
                    forward = this.gameObject.transform.forward * data.moveForwardSpeed;
                    data.mover.Move(forward);
                    anim.SetFloat("DirZ", 1);
                }
                else {
                    //We are close enough to the player...
                    //THIS IS WHERE WE'LL KILL player and transition to something else if the AI get's "bored" chasing the player
                    if (target != null)
                    {
                        data.meele.StartCoroutine("MeeleAttack");
                        boredTimer += Time.deltaTime;
                        if (boredTimer >= boredLimit)
                        {
                            boredTimer = 0;
                            currentPlayerTarget = null;
                            ChangeState(AIStates.Idle);
                        }
                    }
                    else {
                        //player is dead
                        currentPlayerTarget = null;
                        ChangeState(AIStates.Idle);
                    }

                }
                break;

            case AvoidanceStates.MoveToAvoid:
                //Check for transitions
                if (Time.time - timeEnteredAvoidState > avoidMoveTime)
                {
                    ChangeState(AvoidanceStates.None);
                }
                if (!CanMoveForward(obsAvoidDistance))
                {
                    ChangeState(AvoidanceStates.TurnToAvoid);
                }
                //Do work
                data.mover.Move(data.mover.transform.forward * data.moveForwardSpeed);
                anim.SetFloat("DirZ", 1);
                break;

            case AvoidanceStates.TurnToAvoid:
                data.mover.Turn(data.turnSpeed);
                //Check for transitions
                if (CanMoveForward(obsAvoidDistance))
                {
                    ChangeState(AvoidanceStates.MoveToAvoid);
                }
                break;
        }

    }

    public void Flee(Transform target)
    {
        boredTimer += Time.deltaTime;
        //Find Vector to target
        Vector3 vectorToTarget = target.position - data.mover.tf.position;
        //flip the vector
        vectorToTarget = -1 * vectorToTarget;
        //Resize to the flee distance
        vectorToTarget = vectorToTarget * fleeDistance;
        //find the point at the end of that vector from my position
        Vector3 fleeTarget = data.mover.tf.position + vectorToTarget;
        //create a vector from our position to the target
        vectorToTarget = fleeTarget - data.mover.tf.position;
        //create quaternion to rotate ourselves towards that direction
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
        data.mover.tf.rotation = Quaternion.RotateTowards(data.mover.tf.rotation, targetRotation, data.turnSpeed * Time.deltaTime);
        //move there
        Vector3 forward;
        forward = data.mover.tf.forward;
        data.mover.Move(forward * data.moveForwardSpeed);
        anim.SetFloat("DirZ", 1);

        if (boredTimer >= boredLimit)
        {
            boredTimer = 0;
            currentPlayerTarget = null;
            ChangeState(AIStates.Idle);
        }
    }

    #endregion

    #region SenseFunctions

    bool CanSee(List<GameObject> target)
    {
        for(int currentPlayer = 0; currentPlayer < GameManager.instance.playersList.Count; currentPlayer++)
        {
            Transform targetTF = target[currentPlayer].GetComponent<Transform>();
            Vector3 vectorToTarget = targetTF.position - tf.position;
            float angleToTarget = Vector3.Angle(vectorToTarget, tf.forward);
            if (angleToTarget > FieldOfView)
            {
                return false;
            }
            else {
                RaycastHit hitInfo;
                if (Physics.Raycast(tf.position, vectorToTarget, out hitInfo, Mathf.Infinity))
                {
                    UnityEngine.Debug.DrawRay(tf.position, vectorToTarget, Color.blue);
                    if (hitInfo.collider.gameObject.tag == "Player")
                    {
                        //then I saw a player object
                        UnityEngine.Debug.Log("I saw a player object.");
                        currentPlayerTarget = target[currentPlayer];
                        return true;
                    }
                    else {
                        //something is obstructing the view of the player
                        return false;
                    }
                }

            }
        }
        //if it somehow made it down here, return false
        return false;
    }

    bool CanHear(GameObject target)
    {
        float hearingDistance;
        Noisemaker source = target.GetComponent<Noisemaker>();

        if (source == null)
        {
            return false;
        }
        else {
            if (source.isMakingNoise == true)
            {
                hearingDistance = hearingSensitivity * source.noiseVolume;
                if (Vector3.Distance(target.GetComponent<Transform>().position, tf.position) < hearingDistance)
                {
                    return true;
                }
            }
        }
        //if somehow this is reached, you can't hear them
        return false;
    }

    bool CanMoveForward(float distance)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(data.mover.tf.position, data.mover.tf.forward, out hitInfo, distance, obstacleLayers))
        {
            return false;
        }
        else {
            return true;
        }
    }

    bool isHealthBelow(float value)
    {
        if (data.health.CurrentHealth < value)
        {
            return true;
        }
        else {
            return false;
        }
    }

    bool isHealthAbove(float value)
    {
        if (data.health.CurrentHealth > value)
        {
            return true;
        }
        else {
            return false;
        }
    }

    #endregion

    #region Bookkeeping Related

    void ClearMinionsToKillList()
    {
        minionsToKill.Clear();
    }
    void ClearWaypointList()
    {
        waypoints.Clear();
    }

    public void SelectVisual()
    {
        foreach (GameObject GO in personalityVisuals)
        {
            GO.SetActive(false);
        }
        if (AIPersonality == AIPersonalities.Dark)
        {
            personalityVisuals[0].SetActive(true);
        }
        if (AIPersonality == AIPersonalities.Passive)
        {
            personalityVisuals[1].SetActive(true);
        }
        if (AIPersonality == AIPersonalities.Aggressive)
        {
            personalityVisuals[2].SetActive(true);
        }
        if (AIPersonality == AIPersonalities.Defensive)
        {
            personalityVisuals[3].SetActive(true);
        }
    }

    #endregion

    #endregion



























}
