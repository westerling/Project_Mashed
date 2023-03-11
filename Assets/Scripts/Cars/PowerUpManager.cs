using System.Linq;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField]
    private Car m_Car;

    [SerializeField]
    private GameObject[] m_Weapons;

    private InputManager m_InputManager;
    private GameObject m_PowerUp;

    private void Awake()
    {
        foreach (var weaponGameObject in m_Weapons)
        {
            weaponGameObject.SetActive(false);

            if (weaponGameObject.TryGetComponent(out Weapon weapon))
            {
                weapon.SetInitialValues();
            }
        }
    }

    private void Start()
    {
        m_InputManager = GetComponentInParent<InputManager>();

        m_InputManager.Shoot += ShootPerformed;
    }

    public bool AddPowerUp(WeaponType weaponType)
    {
        if (m_PowerUp == null)
        {
            var weaponToActivate = m_Weapons.Where(x => x.GetComponent<Weapon>().WeaponType == weaponType).FirstOrDefault();

            m_PowerUp = weaponToActivate;
            m_PowerUp.SetActive(true);

            return true;
        }

        return false;
    }

    private void ShootPerformed()
    {
        if (m_PowerUp == null || 
            !m_PowerUp.activeInHierarchy)
        {
            return;
        }

        if (m_PowerUp.TryGetComponent(out Weapon powerUp))
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

        if (m_PowerUp.TryGetComponent(out Weapon powerUp))
        {
            if (powerUp.Ammunition <= 0)
            {
                m_PowerUp.SetActive(false);
                m_PowerUp = null;
            }
        }
    }

    private void OnDestroy()
    {
        m_InputManager.Shoot -= ShootPerformed;
    }
}
