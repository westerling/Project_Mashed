using System;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_Rigidbody;

    [SerializeField]
    private GameObject m_RoofRack;

    [SerializeField]
    private GameObject m_WarningSprite;

    [SerializeField]
    private CarConfig m_Config;

    public Rigidbody Rigidbody 
    {
        get => m_Rigidbody;
        set => m_Rigidbody = value; 
    }

    public CarConfig Config 
    {
        get => m_Config; 
    }

    public GameObject RoofRack 
    { 
        get => m_RoofRack; 
    }
    
    public GameObject WarningSprite 
    { 
        get => m_WarningSprite; 
    }

    public event Action<bool> StatusUpdated;

    public void SetCarStatus(bool active)
    {
        StatusUpdated?.Invoke(active);
    }
}
