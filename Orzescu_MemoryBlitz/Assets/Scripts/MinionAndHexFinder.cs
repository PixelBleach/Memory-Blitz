using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAndHexFinder : MonoBehaviour {

    public AIController AIController;


    void Awake()
    {
        GameObject parentObject;
        parentObject = FindParentWithTag(gameObject, "Player");
        if (parentObject != null)
        {
            AIController = parentObject.GetComponent<AIController>();
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Minion tempMinion = collider.gameObject.GetComponent<Minion>();
            AIController.currentHex = tempMinion.ownerHex;
            AIController.minionsToKill = AIController.currentHex.minions;
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
        Debug.Log("could not find parent with tag : " + tag);
        return null; // Could not find a parent with given tag.
    }

}
