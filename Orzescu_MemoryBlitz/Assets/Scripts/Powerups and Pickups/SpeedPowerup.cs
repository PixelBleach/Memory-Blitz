using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeedPowerup : Powerup {

    public float speedIncreaseMultiplier;
    private float oldForwardSpeed;
    private float oldBackwardSpeed;

    public override void OnActivate(Data target)
    {
        base.OnActivate(target);

        oldForwardSpeed = target.moveForwardSpeed;
        oldBackwardSpeed = target.moveBackwardSpeed;
        target.moveForwardSpeed = target.moveForwardSpeed * speedIncreaseMultiplier;
        target.moveBackwardSpeed = target.moveBackwardSpeed * speedIncreaseMultiplier;

    }

    public override void OnDeactivate(Data target)
    {
        base.OnDeactivate(target);

        target.moveForwardSpeed = oldForwardSpeed;
        target.moveBackwardSpeed = oldBackwardSpeed;
    }

    public override void OnUpdate(Data target)
    {
        base.OnUpdate(target);
    }


}
