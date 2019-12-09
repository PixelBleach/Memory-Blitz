using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {


    private Data data;


    public GameObject bulletPrefab;
    public Transform bulletOrigin;
    public float bulletAliveTime;

    public Animator anim;

	// Use this for initialization
	void Start () {

        data = gameObject.GetComponent<Data>();
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Attack(float attackDamage)
    {
        anim.Play("Attack");
    }

    public void Shoot(float firingForce)
    {
        GameObject bPrefab;
        bPrefab = Instantiate(bulletPrefab, bulletOrigin.position, bulletOrigin.rotation) as GameObject;
        bPrefab.transform.position = bulletOrigin.position;

        //bPrefab.name = data.starShipTag;

        Rigidbody bRigidBody = bPrefab.GetComponent<Rigidbody>();

        Vector3 firingVector = bulletOrigin.transform.forward;
        firingVector.y = 0;
        //firingVector = new Vector3(firingVector.x * firingForce, firingVector.y, firingVector.z);
        Debug.Log(firingVector);
        Vector3 DirectionVector = Vector3.Normalize(firingVector) * firingForce;
        Debug.Log(DirectionVector);
        bRigidBody.AddForce(DirectionVector, ForceMode.Force);
 
        Destroy(bPrefab, bulletAliveTime);

    }




}
