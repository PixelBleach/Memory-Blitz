using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletIdentity : MonoBehaviour {

    private string tagReference;

    public GameObject BulletImpactParticles;

	// Use this for initialization
	void Start () {

        tagReference = gameObject.name;

	}
	
    public void OnCollisionEnter(Collision collision)
    {
        GameObject explosionPF;
        if (tagReference != collision.gameObject.name)
        {
            //Seond some sort of score trigger to the player who hit another player
            Debug.Log(tagReference + " hit " + collision.gameObject.name);
            
        } else
        {

        }
        explosionPF = Instantiate(BulletImpactParticles, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
        Destroy(this.gameObject);
        Destroy(explosionPF, 5);
    }



}
