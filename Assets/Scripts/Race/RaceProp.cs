using System;
using Unity.Mathematics;
using UnityEngine;

public class RaceProp : MonoBehaviour
{
    private Vector3 m_StartPos;
    private quaternion m_StartRotation;

    void Start()
    {
        SetStartPosition();
        AddListeners();
    }

    private void SetStartPosition()
    {
        m_StartPos = transform.position;
        m_StartRotation = transform.rotation;
    }

    private void OnRaceFinished()
    {
        transform.position = m_StartPos;
        transform.rotation = m_StartRotation;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void AddListeners()
    {
        RaceManager.Current.RaceFinished += OnRaceFinished;
    }

    private void OnDestroy()
    {
        RaceManager.Current.RaceFinished -= OnRaceFinished;
    }
}
