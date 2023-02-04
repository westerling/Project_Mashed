using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public event Action CheckpointPassed;

    [SerializeField]
    private GameObject[] m_StartPositions;

    private bool m_Active = false;

    public bool Active 
    {
        get => m_Active; 
        set => m_Active = value; 
    }

    public GameObject[] StartPositions 
    { 
        get => m_StartPositions;
        set => m_StartPositions = value; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Car car))
        {
            if (Active)
            {
                CheckpointPassed?.Invoke();
            }
        }
    }
}
