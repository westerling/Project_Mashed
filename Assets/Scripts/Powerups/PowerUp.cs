using System;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField]
    private int m_Ammunition;

    [SerializeField]
    private Transform m_SpawnPoint;

    public int Ammunition 
    {
        get => m_Ammunition;
        set => m_Ammunition = value; 
    }
    
    public Transform SpawnPoint 
    {
        get => m_SpawnPoint; 
    }

    public abstract void Fire();
}
