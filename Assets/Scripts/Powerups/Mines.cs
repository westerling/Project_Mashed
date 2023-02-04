using UnityEngine;

public class Mines : PowerUp
{
    [SerializeField]
    private GameObject m_Mine;

    public override void Fire()
    {
        Instantiate(m_Mine, SpawnPoint.position, transform.rotation);

        Ammunition--;
    }
}
