using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureTile : MonoBehaviour {

    public Hex ownerHex;
	public Data dataOfLastOwner;



	void Start () {
        GameObject tempGamObj = FindParentWithTag(gameObject, "Hex");
        if (tempGamObj != null)
        {
            ownerHex = tempGamObj.GetComponent<Hex>();
        }
        dataOfLastOwner = null;

	}


	void Update () {
		
	}

	public void OnTriggerStay(Collider playerCollider){
        //If player walks into capture trigger
        if (playerCollider.gameObject.tag == "Player")
        {
            Data tempData = playerCollider.gameObject.GetComponent<Data>();
            if (dataOfLastOwner != tempData)
            {
                //capture the hex
                //set the ownership of the hex
                ownerHex.hexOwner = tempData;
                //add to the list of owned hexes by the capturing player
                tempData.ownedHexes.Add(ownerHex);
                //remove this hex from the previous owner
                if (dataOfLastOwner != null)
                {
                    dataOfLastOwner.ownedHexes.Remove(ownerHex);
                }
                //make the capturer the last owner
                dataOfLastOwner = tempData;
                //change the floor texture for the minimap
                ownerHex.ChangeFloorTexture(tempData.playerColor);
                //make it not capturable again to disable the capture point
                ownerHex.isCapturable = false;
                //respawn the minions
                ownerHex.SpawnAllMinions();
            }
        }
	}

	public static GameObject FindParentWithTag(GameObject childObject, string tag)
	{
		Transform t = childObject.transform;
		while (t.parent != null)
		{
			if (t.parent.tag == tag)
			{
				return t.parent.gameObject;
			}
			t = t.parent.transform;
		}
		Debug.Log ("could not find parent with tag : " + tag);
		return null; // Could not find a parent with given tag.
	}



}
