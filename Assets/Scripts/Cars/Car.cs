using System;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_Rigidbody;

    [SerializeField]
    private Transform m_IconTransform;

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

    public Transform IconTransform 
    { 
        get => m_IconTransform; 
    }

    public event Action<bool> StatusUpdated;

    public void SetCarStatus(bool active)
    {
        StatusUpdated?.Invoke(active);
    }
}
