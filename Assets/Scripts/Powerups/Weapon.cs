using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform m_SpawnPoint;

    private int m_Ammunition;
    private bool m_AutomaticFiring;

    private WeaponType m_WeaponType;

    public int Ammunition
    {
        get => m_Ammunition;
        set => m_Ammunition = value;
    }

    protected Transform SpawnPoint
    {
        get => m_SpawnPoint;
    }

    public WeaponType WeaponType
    {
        get => m_WeaponType;
        protected set => m_WeaponType = value;
    }
    public bool AutomaticFiring
    {
        get => m_AutomaticFiring;
        protected set => m_AutomaticFiring = value;
    }

    public abstract void SetInitialValues();

    public abstract void Fire();

    public abstract void OnPickup();
}
