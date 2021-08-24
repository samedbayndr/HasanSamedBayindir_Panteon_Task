using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(this.gameObject.name + " object collide to "+ collision.transform.gameObject.name);
        //All racers have the Racer component. Therefore, if the racer touches any object
        //inherited from the Obstacle class, the "ObstacleCrash" event is triggered.
        Racer racerId = collision.gameObject.GetComponent<Racer>();
        if (racerId)
        {
            if (!racerId.IsRacerFall) 
                racerId.ObstacleCrash.Invoke();
            
        }
            
    }


}
