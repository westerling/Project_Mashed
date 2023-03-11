using UnityEngine;

public class RocketLauncher : Weapon
{
    [SerializeField]
    private GameObject m_Rocket;

    [SerializeField]
    private Targeter m_Targeter;

    private void OnEnable()
    {
        m_Rocket.SetActive(false);
    }

    public override void SetInitialValues()
    {
        Ammunition = 1;
        AutomaticFiring = false;
        WeaponType = WeaponType.Missile;
    }

    public override void Fire()
    {
        Debug.Log("Smäller av:");
        m_Rocket.transform.position = SpawnPoint.position;
        m_Rocket.transform.rotation = SpawnPoint.rotation;
        m_Rocket.SetActive(true);

        if (m_Targeter.Target != null)
        {
            if (m_Rocket.TryGetComponent(out HomingMissile missile))
            {
                missile.SetTarget(m_Targeter.Target.transform);
            }
        }

        Ammunition--;
    }
}
