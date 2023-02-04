using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Powerup;

    [SerializeField]
    private GameObject m_PickupGraphics;

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent(out PowerUpManager manager))
        {
            if (manager.AddPowerUp(m_Powerup))
            {
                StartCoroutine(Invisible());
                return;
            }
        }
    }

    public void ResetPickup()
    {
        StopAllCoroutines();

        ToggleVisibility(true);
    }

    private IEnumerator Invisible()
    {
        ToggleVisibility(false);
        yield return new WaitForSeconds(16);
        ToggleVisibility(true);
    }

    private void ToggleVisibility(bool enable)
    {
        m_PickupGraphics.SetActive(enable);
        GetComponent<Collider>().enabled = enable;
    }
}
