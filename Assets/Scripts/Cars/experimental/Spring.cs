using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField]
    private float m_Strength = 100f;

    [SerializeField]
    private float m_Damping = 15f;

    [SerializeField]
    private float m_SuspensionRestDistance = 1f;

    [SerializeField]
    private float m_TireSize = 0.33f;

    [SerializeField]
    private Rigidbody m_CarRigidBody;

    [SerializeField]
    private Transform[] m_TireTransforms;


    private void FixedUpdate()
    {
        foreach (var wheel in m_TireTransforms)
        {
            if (Physics.Raycast(transform.position, -transform.up, out var hit, m_TireSize, Globals.TerrainLayerMask))
            {
                var springDirection = wheel.up;

                var tireWorldDirection = m_CarRigidBody.GetPointVelocity(wheel.position);

                var springOffset = m_SuspensionRestDistance - hit.distance;

                var velovity = Vector3.Dot(springDirection, tireWorldDirection);

                var force = (springOffset * m_Strength) - (velovity * m_Damping);

                m_CarRigidBody.AddForceAtPosition(springDirection * force, wheel.up);
            }
        }    
    }
}
