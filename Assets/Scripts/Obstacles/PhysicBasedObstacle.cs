using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicBasedObstacle : MonoBehaviour
{
    protected Vector3 AngularVelocity { get; set; }
    [SerializeField] protected Rigidbody _rigidBody;
    [Tooltip("1: Clockwise // -1 Counterclockwise ")]
    [SerializeField] protected int Direction = 1;
    [SerializeField] protected float AppliatableForce = 5f;

}
