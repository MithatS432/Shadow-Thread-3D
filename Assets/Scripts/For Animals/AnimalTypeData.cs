using UnityEngine;

[CreateAssetMenu(menuName = "Animals/Animal Type")]
public class AnimalTypeData : ScriptableObject
{
    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;

    [Header("Detection")]
    public float detectionRange;
    public float loseInterestRange;

    [Header("Wander")]
    public float wanderRadius;
}
