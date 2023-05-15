using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private Car m_Car;

    private float m_FrontHealth;
    private float m_RearHealth;

    private void Start()
    {
        m_FrontHealth = m_Car.Config.FrontDurability;
        m_RearHealth = m_Car.Config.RearDurability;
    }

    public void Damage()
    {

    }
}
