using UnityEngine;

public class StartCheckpoint : Checkpoint
{
    [SerializeField]
    private GameObject[] m_StartPositions;

    public GameObject[] StartPositions
    {
        get => m_StartPositions;
    }
}
