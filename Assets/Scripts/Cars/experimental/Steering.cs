using UnityEngine;

public class Steering : MonoBehaviour
{

    [SerializeField]
    private Rigidbody m_CarRigidBody;

    [SerializeField]
    private Transform[] m_TireTransforms;

    [SerializeField]
    private float m_TireMass = 30f;

    [SerializeField]
    private AnimationCurve m_TireGripFactor;

    private void FixedUpdate()
    {
        foreach (var wheel in m_TireTransforms)
        {
            if (Physics.Raycast(transform.position, -transform.up, out var hit, 3f/*,Globals.TerrainLayerMask*/ ))
            {
                var steeringDirection = wheel.right;

                var tireWorldDirection = m_CarRigidBody.GetPointVelocity(wheel.position);

                var steeringVelocity = Vector3.Dot(steeringDirection, tireWorldDirection);

                var gripFactor = m_TireGripFactor.Evaluate(steeringVelocity / tireWorldDirection.magnitude);

                var desiredVelocityChange = -steeringVelocity * gripFactor;

                var desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime;

                m_CarRigidBody.AddForceAtPosition(steeringDirection * m_TireMass * desiredAcceleration, wheel.position);
            }
        }
    }
}
