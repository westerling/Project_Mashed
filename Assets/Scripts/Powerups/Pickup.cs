using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private WeaponType m_WeaponType;

    [SerializeField]
    private GameObject m_PickupGraphics;

    private PickupSpawn m_Spawner;

    public PickupSpawn Spawner 
    {
        get => m_Spawner; 
        set => m_Spawner = value; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PowerUpManager manager))
        {
            if (manager.AddPowerUp(m_WeaponType))
            {
                gameObject.SetActive(false);
                Spawner.SpawnPickupDelayed(5);
                return;
            }
        }
    }
}
