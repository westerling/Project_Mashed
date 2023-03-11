using System.Collections;
using UnityEngine;

public class Mine : Ammunition
{
    private float m_Radius = 10f;
    private float m_Power = 100000f;
    private float m_SafeTimer = 2f;
    private bool m_SafeMode = true;

    public void ActivateMine()
    {
        StartCoroutine(SetMineSafety());
    }

    private IEnumerator SetMineSafety()
    {
        m_SafeMode = true;
        yield return new WaitForSeconds(m_SafeTimer);
        m_SafeMode = false;
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
                Debug.Log("Pang");
                rigidbody.AddExplosionForce(m_Power, explosionPos, m_Radius, 10000f);
            }
        }
        

        gameObject.SetActive(false);
    }
}
