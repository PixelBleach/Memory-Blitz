using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Powerup {

    public string name;
    public bool isPermanent;
    public float duration;
    public float timeRemaining;

    public virtual void OnActivate (Data target)
    {
        //Set the timer 
        if (!isPermanent)
        {
            timeRemaining = duration;
        }
    }

    public virtual void OnDeactivate (Data target)
    {

    }

    public virtual void OnUpdate(Data target)
    {
        if (!isPermanent)
        {
            timeRemaining -= Time.deltaTime;
        }
    }

}
