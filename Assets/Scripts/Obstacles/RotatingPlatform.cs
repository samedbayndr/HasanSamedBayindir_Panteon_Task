using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : PhysicBasedObstacle
{

    private void Start()
    {
        AngularVelocity = new Vector3(0, 0, 35);
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion deltaR = Quaternion.Euler((AngularVelocity * Direction) * Time.fixedDeltaTime);
        _rigidBody.MoveRotation(_rigidBody.rotation * deltaR);
    }


}
