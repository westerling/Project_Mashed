using UnityEngine;

public class Downforce : MonoBehaviour
{
    [SerializeField]
    private Car m_Car;

    void Update()
    {
        var lift = m_Car.Config.Downforce * m_Car.Rigidbody.velocity.sqrMagnitude;
        m_Car.Rigidbody.AddForceAtPosition(lift * -transform.up, transform.position);
    }
}
