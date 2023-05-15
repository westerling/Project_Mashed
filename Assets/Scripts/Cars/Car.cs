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

    [SerializeField]
    private Collider m_FrontCollider;

    [SerializeField]
    private Collider m_RearCollider;

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
    public Collider FrontCollider 
    {
        get => m_FrontCollider;
    }
    public Collider RearCollider 
    {
        get => m_RearCollider;
    }

    public event Action<bool> StatusUpdated;

    public void SetCarStatus(bool active)
    {
        StatusUpdated?.Invoke(active);
    }
}
