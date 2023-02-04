using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_CarRigidBody;

    [SerializeField]
    private Transform[] m_TireTransforms;

    [SerializeField]
    private float m_TopSpeed = 100f;

    [SerializeField]
    private AnimationCurve m_TireGripFactor;

    [SerializeField]
    private Transform m_CarTransform;

    private float m_Acceleration;

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, out var hit, 3f))
        {
            m_Acceleration = 1f;

            foreach (var wheel in m_TireTransforms)
            {
                var accelerationDirection = wheel.forward;

                if (m_Acceleration > 0f)
                {
                    var speed = Vector3.Dot(m_CarTransform.forward, m_CarRigidBody.velocity);

                    var normalizedSpeed = Mathf.Clamp01(Mathf.Abs(speed) / m_TopSpeed);

                    var torqueFactor = m_TireGripFactor.Evaluate(normalizedSpeed) * m_Acceleration;

                    var force = accelerationDirection * torqueFactor;

                    Debug.DrawLine(wheel.position, force, Color.blue);

                    m_CarRigidBody.AddForceAtPosition(force, wheel.position);
                }
            }
        }
    }
}
