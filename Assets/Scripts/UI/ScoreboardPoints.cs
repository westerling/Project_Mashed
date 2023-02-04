using UnityEngine;

public class ScoreboardPoints : MonoBehaviour
{
    [SerializeField]
    private GameObject m_PointImage;

    [SerializeField]
    private GameObject m_UnderscoreImage;

    public GameObject Point 
    { 
        get => m_PointImage;
    }

    public GameObject UnderscoreImage 
    { 
        get => m_UnderscoreImage;
    }
}
