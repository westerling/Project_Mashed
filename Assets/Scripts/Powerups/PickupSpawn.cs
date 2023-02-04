using System.Collections;
using UnityEngine;

public class PickupSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_PossiblePowerups;

    [SerializeField]
    private GameObject m_Graphics;

    private GameObject m_Powerup;

    public GameObject[] PossiblePowerups
    {
        get => m_PossiblePowerups;
        set => m_PossiblePowerups = value;
    }

    private void Start()
    {
        SpawnPickup();
    }

    private void SpawnPickup()
    {
        var go = Instantiate(PossiblePowerups[Random.Range(0, m_PossiblePowerups.Length)]);
        if (go.TryGetComponent(out Pickup pickup))
        {

        }
    }

    private IEnumerator Invisible()
    {
        yield return new WaitForSeconds(16);
        SpawnPickup();
    }
}
