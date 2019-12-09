using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealPowerup : Powerup {

    public float healValue;

    public override void OnActivate(Data target)
    {
        if (target.health.CurrentHealth != target.health.MaxHealth)
        {
            Debug.Log("Permanent Health Pickup aquired by player: Heal");
            //heal target if under max health. If overhealed, reduce to max health
            if (target.health.CurrentHealth < target.health.MaxHealth)
            {
                target.health.CurrentHealth += healValue;
                target.health.healthBarSlider.value = target.health.CalculateHealthPercentage();

                if (target.health.CurrentHealth > target.health.MaxHealth)
                {
                    target.health.CurrentHealth = target.health.MaxHealth;
                    target.health.healthBarSlider.value = target.health.CalculateHealthPercentage();
                }
            }
            //update the data object to reflect the same values as the health component
            target.currentHitPoints = target.health.CurrentHealth;
            target.health.healthBarSlider.value = target.health.CalculateHealthPercentage();
        }
    }

    public override void OnUpdate(Data target)
    {
        base.OnUpdate(target);
    }


}
