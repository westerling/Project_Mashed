using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayersMenu : Menu
{
    [Header("Menus")]
    [SerializeField]
    private Menu m_LaunchSettingsMenu;

    [Header("UI")]
    [SerializeField]
    private List<GameObject> m_PlayerPanels = new List<GameObject>();

    [SerializeField]
    private TMP_Text m_StartText;

    private const string m_ReadyText = "Press A to start";
    private const string m_NotReadyText = "Waiting for more players...";

    private void OnEnable()
    {
        CheckPlayers();
    }

    protected override void OnPlayerJoined(Player playerJoining)
    {
        base.OnPlayerJoined(playerJoining);

        CheckPlayers();
    }

    protected override void OnPlayerLeft(Player playerLeaving)
    {
        base.OnPlayerLeft(playerLeaving);

        CheckPlayers();
    }
    
    public void OpenLaunchSettingsMenu()
    {
        if (GameManager.Current.ActivePlayers.Count > 0)
        {
            OpenMenu(m_LaunchSettingsMenu);
        }
    }

    private void CheckPlayers()
    {
        m_StartText.text = GameManager.Current.ActivePlayers.Count > 0 ? m_ReadyText : m_NotReadyText;

        for (int i = 0; i < m_PlayerPanels.Count; i++)
        {
            if (m_PlayerPanels[i].TryGetComponent(out PlayerMenuUI playerMenuUI))
            {
                if (GameManager.Current.ActivePlayers.Count > i)
                {
                    playerMenuUI.PresentPlayer();
                }
                else
                {
                    playerMenuUI.AbsentPlayer();
                }
            }
        }
    }
}
