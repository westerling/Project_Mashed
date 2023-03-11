using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    private WeaponType m_WeaponType;

    [SerializeField]
    private int m_Ammunition;

    [SerializeField]
    private Transform m_SpawnPoint;

    [SerializeField]
    private bool m_AutomaticFiring;

    [SerializeField]
    private bool m_UseTargeter;

    [SerializeField]
    private List<GameObject> m_EnableOnFire;

    private Targeter m_Targeter;

    private void OnEnable()
    {
        if (TryGetComponent(out Targeter targeter))
        {
            m_Targeter = targeter;
        }
    }

    public int Ammunition
    {
        get => m_Ammunition;
        set => m_Ammunition = value;
    }

    public Transform SpawnPoint
    {
        get => m_SpawnPoint;
    }

    public bool UseTargeter
    {
        get => m_UseTargeter;
    }

    public WeaponType WeaponType
    {
        get => m_WeaponType;
    }

    public abstract void Fire();
}
