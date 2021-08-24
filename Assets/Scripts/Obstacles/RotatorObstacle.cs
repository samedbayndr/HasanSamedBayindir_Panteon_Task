using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorObstacle : PhysicBasedObstacle
{
    private void Start()
    {
        AngularVelocity = new Vector3(0, 45, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody racerRb = collision.gameObject.GetComponent<Rigidbody>();
        if (racerRb)
        {
            //The point where the racer touches the obstacle is taken and the force is applied in the opposite direction.
            racerRb.AddForce((-racerRb.velocity.normalized) * AppliatableForce, ForceMode.Force);


            //racerRb.AddForce(Vector3.up * AppliatableForce, ForceMode.Force);
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion deltaR = Quaternion.Euler((AngularVelocity * Direction) * Time.fixedDeltaTime);
        _rigidBody.MoveRotation(_rigidBody.rotation * deltaR);
    }


}
