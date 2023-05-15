using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Current;

    [SerializeField]
    private GameObject m_PauseMenu;

    [SerializeField]
    private GameObject m_LoadingScreen;

    [SerializeField]
    private GameObject m_RaceGUI;

    [SerializeField]
    private GameObject m_StartLight;

    [SerializeField]
    private Image[] m_StartLights;

    [SerializeField]
    private GameObject[] m_ScoreBoards;

    private Dictionary<Player, GameObject> m_PlayerScoreboardDictionary = new Dictionary<Player, GameObject>();

    private void Awake()
    {
        Current = this;
    }

    public void SetupGameGUI()
    {
        var activePlayers = GameManager.Current.ActivePlayers;

        ToggleScoreboardsVisibility(activePlayers.Count);

        for (int i = 0; i < activePlayers.Count; i++)
        {
            m_PlayerScoreboardDictionary.Add(activePlayers[i], m_ScoreBoards[i]);
        }

        foreach (var playerScoreboardDictionary in m_PlayerScoreboardDictionary)
        {
            if (playerScoreboardDictionary.Value.TryGetComponent(out PlayerScoreboard playerScoreboard))
            {
                switch (activePlayers.Count)
                {
                    case 2:
                    case 3:
                        playerScoreboard.SmallScoreboard.SetActive(true);
                        playerScoreboard.LargeScoreboard.SetActive(false);
                        break;
                    case 4:
                    default:
                        playerScoreboard.SmallScoreboard.SetActive(false);
                        playerScoreboard.LargeScoreboard.SetActive(true);
                        break;
                }
            }
        }
    }

    public void SetPoints(Player player, int totalPoints)
    {
        if (m_PlayerScoreboardDictionary[player].TryGetComponent(out PlayerScoreboard scoreboard))
        {
            scoreboard.UpdateScore(totalPoints, false);
        }
    }

    public void ShowScoreText(Player player, bool showScoreText)
    {
        if (m_PlayerScoreboardDictionary[player].TryGetComponent(out PlayerScoreboard scoreboard))
        {
            scoreboard.ShowScoreText(showScoreText);
        }
    }

    public void SetPoints(Player player, int totalPoints, int pointsRecieved)
    {
        if (m_PlayerScoreboardDictionary[player].TryGetComponent(out PlayerScoreboard scoreboard))
        {
            scoreboard.UpdateScore(totalPoints, false, pointsRecieved);
        }
    }

    private void ToggleScoreboardsVisibility(int players)
    {
        foreach (var scoreboard in m_ScoreBoards)
        {
            scoreboard.SetActive(false);
        }

        for (var i = 0; i < players; i++)
        {
            m_ScoreBoards[i].SetActive(true);
        }
    }

    public void TogglePauseMenu(bool active)
    {
        m_PauseMenu.SetActive(active);
    }

    public void ToggleLoadingScreen(bool active)
    {
        m_LoadingScreen.SetActive(active);
    }

    public void ToggleRaceUI(bool active)
    {
        m_RaceGUI.SetActive(active);
    }

    public void DisplayTrafficLight(bool visible)
    {
        m_StartLight.SetActive(visible);
    }

    public void TurnOffStartLights()
    {
        ToggleAllStartLights(Globals.TrafficBlack);
    }

    public void ToggleStartLight(int index)
    {
        if (index == 3)
        {
            ToggleAllStartLights(Globals.TrafficGreen);
            return;
        }

        m_StartLights[index].color = Globals.TrafficRed;
    }

    private void ToggleAllStartLights(Color color)
    {
        foreach (var light in m_StartLights)
        {
            light.color = color;
        }
    }
}
