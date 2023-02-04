using UnityEngine;

public class RocketLauncher : PowerUp
{
    [SerializeField]
    private GameObject m_Rocket;

    public override void Fire()
    {
        Instantiate(m_Rocket, SpawnPoint.position, transform.rotation);

        Ammunition--;
    }
}
