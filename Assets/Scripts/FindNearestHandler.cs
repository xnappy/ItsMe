using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class FindNearestHandler : MonoBehaviour {

    public GameObject closest;

    public GameObject GetClosestObject(string tag, GameObject player)
    {
        GameObject[] haendler = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject h in haendler)
        {
            if (!closest)
            {
                closest = h;
            }
           
            if(Vector3.Distance(player.transform.position, h.transform.position) < Vector3.Distance(player.transform.position, closest.transform.position))
            {
                closest = h;
            }
        }
        //Debug.Log(Vector3.Distance(player.transform.position, closest.transform.position));
        return closest;
    }
}
