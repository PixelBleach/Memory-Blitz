using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meele : MonoBehaviour {

	public Data data;
	public Health health;
	public Animator anim;

    public AudioSource meeleAudioSource;

	public BoxCollider hitbox;
	public BoxCollider attackbox;

	public float timeAttackBoxActive;
	public int dmgModifier;
	public int dmgOnHit;

	// Use this for initialization
	void Start () {
		data = gameObject.GetComponent<Data> ();
		health = gameObject.GetComponent<Health> ();
		anim = gameObject.GetComponent<Animator> ();
        meeleAudioSource = gameObject.GetComponent<AudioSource>();

		attackbox.enabled = false;
	}

	//ON HIT FROM ANOTHER ATTACK
	public void OnTriggerEnter(Collider colliderOnHitbox) {
		//if hit by 
		if (colliderOnHitbox.gameObject.tag == "MeeleAttack" && (colliderOnHitbox.gameObject.transform.parent.name != this.gameObject.name)) {
            meeleAudioSource.PlayOneShot(SoundManager.instance.damagedSound, SoundManager.instance.sfxVolume);
            Debug.Log ("You were hit by " + colliderOnHitbox.gameObject.name);
			//TAKE DMG FROM THE OTHER OBJECT
			//get dmg dealt from other object
			Meele tempMeele = colliderOnHitbox.gameObject.transform.parent.GetComponent<Meele>();
			float dmgToTake = tempMeele.dmgOnHit;
			//deal dmg to health of this object
			data.SendMessage ("TakeDamage", dmgToTake);
				
				
		}

	}

	public void PlayAttackAnimation()
	{
		anim.Play("Attack");
        meeleAudioSource.PlayOneShot(SoundManager.instance.attackSound, SoundManager.instance.sfxVolume);
	}

	//meele attack simply means enable the attackbox for a period of time so that other scripts w/ this component could potentially check for triggers. 
	public IEnumerator MeeleAttack() {

		PlayAttackAnimation ();
	
		attackbox.enabled = true;
		yield return new WaitForSeconds (timeAttackBoxActive);
		attackbox.enabled = false;

		Debug.Log ("Attack Coroutine Finished : attack box disabled");

	}

	// Update is called once per frame
	void Update () {
		
	}
}
