using System.Collections;
using UnityEngine;

public class Barrel : Ammunition
{
    [Header("Blast")]
    [SerializeField]
    private float m_Radius = 1.0f;

    [SerializeField]
    private float m_Power = 1000f;

    private bool m_Active = false;

    public bool Active 
    {
        get => m_Active; 
        set => m_Active = value; 
    }

    public void ActivateBarrel()
    {
        StartCoroutine(SetBarrelSafety());
        StartCoroutine(SetMaxTimer());
    }

    private IEnumerator SetMaxTimer()
    {
        yield return new WaitForSeconds(8f);
        Explode();
    }

    private IEnumerator SetBarrelSafety()
    {
        Active = false;
        yield return new WaitForSeconds(0.1f);
        Active = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Active)
        {
            return;
        }

        Explode();
    }

    private void Explode()
    {
        var explosionPos = transform.position;
        var colliders = Physics.OverlapSphere(explosionPos, m_Radius);

        var pooledObject = FxPool.Current.GetPooledObjectOfType(ParticleType.Explosion_m);
        
        if (pooledObject != null)
        {
            pooledObject.transform.position = explosionPos;
            pooledObject.SetActive(true);
        }

        foreach (var hit in colliders)
        {
            var rigidbody = hit.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(m_Power, explosionPos, m_Radius, 300f);
            }
        }

        Active = false;
        Deactivate();
    }
}
