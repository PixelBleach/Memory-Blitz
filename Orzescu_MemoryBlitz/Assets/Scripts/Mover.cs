using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public Transform tf;
    public CharacterController cc;
    //public Transform visuals;
    //public float bankingAngle;

	//public float gravity = 10.0f;


	// Use this for initialization
	void Awake () {

        tf = gameObject.GetComponent<Transform>();
        cc = this.gameObject.GetComponent<CharacterController>();

	}
	
    //move in a simple direction given by a vector 3
    public void Move(Vector3 moveSpeedAndDirection)
    {
        cc.SimpleMove(moveSpeedAndDirection); //already handles speed over time, so we don't need to do speed * time.deltatime
    }

    //move to a predetermined location from a start location
    public void Move(Vector3 startPosition, Vector3 endPosition, float speed, float rotationSpeed)
    {
		float step = speed * Time.deltaTime;
		float rotStep = rotationSpeed * Time.deltaTime;
		Vector3 targetDirection = endPosition - startPosition;
		//turn to destination
	
		Quaternion targetRotation = Quaternion.LookRotation (targetDirection, Vector3.up);
		if (tf.rotation != targetRotation) {
			transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, rotationSpeed);
		} else {
			//move towards destination
			if (tf.position != endPosition) {
				cc.Move (targetDirection * step);
				//tf.position = Vector3.MoveTowards(startPosition, endPosition, step);
			}
		}

    }

    public void Turn(float turnSpeedAndDirection)
    {
        tf.Rotate(new Vector3(0, turnSpeedAndDirection * Time.deltaTime, 0));

        //if (turnSpeedAndDirection > 0)
        //{
        //    visuals.localEulerAngles = new Vector3(0, 0, -bankingAngle);
        //} else
        //{
        //    visuals.localEulerAngles = new Vector3(0, 0, bankingAngle);
        //}

        //bankingAngle = bankingAngle * turnSpeedAndDirection;
        //Debug.Log(bankingAngle);
        //visuals.Rotate(new Vector3(0, 0, bankingAngle * (turnSpeedAndDirection / turnSpeedAndDirection)));
    }



}
