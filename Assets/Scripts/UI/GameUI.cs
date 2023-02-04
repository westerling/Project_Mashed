using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_PointUI;

    public static GameUI Current;

    private void Awake()
    {
        Current = this;
    }
}
