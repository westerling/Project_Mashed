using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField]
    private Targeter m_Targeter;

    [SerializeField]
    private Car m_Car;

    private InputManager m_InputManager;
    private GameObject m_PowerUp;

    public bool AddPowerUp(GameObject powerUp)
    {
        if (m_PowerUp == null)
        {
            m_PowerUp = Instantiate(powerUp, m_Car.RoofRack.transform.position, transform.rotation, transform);

            return true;
        }

        return false;
    }

    private void Start()
    {
        m_InputManager = GetComponentInParent<InputManager>();
        m_Targeter = gameObject.GetComponent<Targeter>();

        m_InputManager.Shoot += ShootPerformed;

        SetRackVisibility(false);
    }

    private void ShootPerformed()
    {
        if (m_PowerUp == null)
        {
            return;
        }

        if (m_PowerUp.TryGetComponent(out PowerUp powerUp))
        {
            powerUp.Fire();
            CheckAmmunition();
        }
    }

    private void CheckAmmunition()
    {
        if (m_PowerUp == null)
        {
            return;
        }

        if (m_PowerUp.TryGetComponent(out PowerUp powerUp))
        {
            if (powerUp.Ammunition <= 0)
            {
                Destroy(m_PowerUp);
            }
        }
    }

    private void SetRackVisibility(bool visible)
    {
        m_Car.RoofRack.SetActive(visible);
    }

    private void OnDestroy()
    {
        m_InputManager.Shoot -= ShootPerformed;
    }
}
