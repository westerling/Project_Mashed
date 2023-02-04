using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_RigidBody;

    private float m_Radius = 5f;
    private float m_Power = 10f;
    private float m_SafeTimer = 2f;
    private bool m_SafeMode = true;

    private void Update()
    {
        if (!m_SafeMode)
        {
            return;
        }

        m_SafeTimer -= Time.deltaTime;

        if (m_SafeTimer <= 0.0f)
        {
            m_SafeMode = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_SafeMode)
        {
            return;
        }

        var explosionPos = transform.position;
        var colliders = Physics.OverlapSphere(explosionPos, m_Radius);

        foreach (var hit in colliders)
        {
            var rigidbody = hit.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(m_Power, explosionPos, m_Radius, 3f);
            }
        }
        Debug.Log("Destroy");

        Destroy(gameObject);
    }
}
