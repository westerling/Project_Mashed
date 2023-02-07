using UnityEngine;

public class Launcher : PowerUp
{
    [SerializeField]
    private GameObject m_Rocket;

    public override void Fire()
    {
        Instantiate(m_Rocket, SpawnPoint.position, transform.rotation);

        Ammunition--;
    }
}
