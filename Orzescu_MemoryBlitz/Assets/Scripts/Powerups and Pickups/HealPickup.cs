using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPickup : Pickup {

    public HealPowerup powerup;
    public AudioSource healPowerupAudioSource;


    public void OnTriggerEnter(Collider otherCollider)
    {
        Debug.Log(gameObject.name + " Collided with : " + otherCollider.gameObject.name);

        

        PowerupController otherPUController;
        otherPUController = otherCollider.gameObject.GetComponent<PowerupController>();
        if (otherPUController != null)
        {
            otherPUController.AddPowerup(powerup);
            AudioSource.PlayClipAtPoint(SoundManager.instance.powerUpSound, gameObject.transform.position ,SoundManager.instance.sfxVolume);
            Destroy(gameObject);
        }
    }
}
