using System.Collections;
using UnityEngine;

public class Homing : MonoBehaviour
{
    private GameObject m_Target;
    private float m_Speed = 2f;

    public GameObject Target { get => m_Target; set => m_Target = value; }

    public void SetTarget()
    {
        StartCoroutine(SendHoming());
    }

    private IEnumerator SendHoming()
    {
        while (Vector3.Distance(Target.transform.position, transform.position) > 0.5f)
        {
            transform.position += (Target.transform.position - transform.position).normalized * m_Speed * Time.deltaTime;
            transform.LookAt(Target.transform);
            yield return null;
        }
    }
}
