using TMPro;
using UnityEngine;

public class PlayerMenuUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_Text;

    public void AbsentPlayer()
    {
        m_Text.text = "Press any button to join...";
    }

    public void PresentPlayer()
    {
        m_Text.text = "Ready!";
    }
}
