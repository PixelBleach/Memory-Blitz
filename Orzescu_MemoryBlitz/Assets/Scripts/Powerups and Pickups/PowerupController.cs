using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour {

    public Data data;
    public List<Powerup> powerupList;

	void Start () {
        //get the data component from the gameobject
        data = GetComponent<Data>();	
	}
	
	void Update () {

        //Testing Key for powerups
        if (Input.GetKeyDown(KeyCode.P))
        {
            Powerup temp = new Powerup();
            temp.duration = 10;
            temp.name = "Test Powerup!";
            AddPowerup(temp);
        }

        UpdateAllPowerups();
	}

    public void UpdateAllPowerups()
    {
        List<Powerup> powerUpsToRemove = new List<Powerup>();

        foreach (Powerup pu in powerupList)
        {
            //Update the powerups
            pu.OnUpdate(data);
            //if powerup duration is over, add to remove list
            if (pu.timeRemaining <= 0)
            {
                powerUpsToRemove.Add(pu);
            }
        }

        //remove appropriate powerups
        foreach (Powerup pu in powerUpsToRemove)
        {
            RemovePowerup(pu);
        }
    }

    public void AddPowerup(Powerup powerupToAdd)
    {
        //add to list of powerups
        powerupList.Add(powerupToAdd);
        //activate powerup
        powerupToAdd.OnActivate(data);
    }

    public void RemovePowerup(Powerup powerupToRemove)
    {
        powerupList.Remove(powerupToRemove);
        powerupToRemove.OnDeactivate(data);
    }
}
