using System.Collections;
using UnityEngine;

public class Mine : Ammunition
{
    private float m_Radius = 10f;
    private float m_Power = 100000f;
    private float m_SafeTimer = 2f;
    private bool m_Active = false;

    public void ActivateMine()
    {
        StartCoroutine(SetMineSafety());
    }

    private IEnumerator SetMineSafety()
    {
        m_Active = false;
        yield return new WaitForSeconds(m_SafeTimer);
        m_Active = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_Active)
        {
            return;
        }

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
                rigidbody.AddExplosionForce(m_Power, explosionPos, m_Radius, 10000f);
            }
        }
        

        gameObject.SetActive(false);
    }
}
