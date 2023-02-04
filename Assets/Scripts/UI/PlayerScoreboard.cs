using TMPro;
using UnityEngine;

public class PlayerScoreboard : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_PointText;

    [SerializeField]
    private GameObject m_SmallScoreboard;

    [SerializeField]
    private GameObject m_LargeScoreboard;

    [SerializeField]
    private GameObject m_WinSymbol;

    [SerializeField]
    private GameObject m_ScoreTextImage;

    [SerializeField]
    private GameObject m_CarLogo;

    public GameObject SmallScoreboard 
    { 
        get => m_SmallScoreboard; 
    }

    public GameObject LargeScoreboard 
    { 
        get => m_LargeScoreboard;
    }

    public void ShowScoreText(bool enableScoreText)
    {
        m_ScoreTextImage.SetActive(enableScoreText);
        m_CarLogo.SetActive(!enableScoreText);
    }

    public void UpdateScore(int points, bool winChance, int pointsToRecieve)
    {
        m_PointText.text = pointsToRecieve.ToString();

        UpdateScore(points, winChance);
    }

    public void UpdateScore(int points, bool winChance)
    {
        if (SmallScoreboard.activeSelf)
        {
            m_WinSymbol.SetActive(points >= 10);
            SetPoints(SmallScoreboard, points, 8);
        }
        
        if (LargeScoreboard.activeSelf)
        {
            m_WinSymbol.SetActive(points >= 7);
            SetPoints(LargeScoreboard, points, 12);
        }
    }

    private void SetPoints(GameObject scoreboard, int points, int maxPoints)
    {
        if (scoreboard.TryGetComponent(out ScoreboardPoints scoreboardPoints))
        {
            foreach (Transform child in scoreboard.transform)
            {
                Destroy(child.gameObject);
            }

            for (var i = 0; i < maxPoints; i++)
            {
                var pointImage = i < points ? scoreboardPoints.Point : scoreboardPoints.UnderscoreImage;
                Instantiate(pointImage, scoreboard.transform);
            }
        }
    }
}
