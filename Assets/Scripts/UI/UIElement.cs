using UnityEngine;

public class UIElement : MonoBehaviour
{
    [SerializeField]
    private UIType m_UIType;

    public UIType UIType 
    {
        get => m_UIType; 
    }
}
