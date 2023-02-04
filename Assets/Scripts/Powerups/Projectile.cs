using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_RigidBody;

    [SerializeField]
    private float m_LaunchForce;

    [SerializeField]
    private float m_Radius = 10f;

    [SerializeField]
    private float m_Power = 100000f;

    [SerializeField]
    private ParticleSystem m_Particles;
    private void Start()
    {
        m_RigidBody.velocity = transform.forward * m_LaunchForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        var explosionPos = transform.position;
        var colliders = Physics.OverlapSphere(explosionPos, m_Radius);
        
        foreach (var hit in colliders)
        {
            var rigidbody = hit.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(m_Power, explosionPos, m_Radius, 300f);
            }
        }

        m_Particles.Play();

        Destroy(gameObject);
    }
}
