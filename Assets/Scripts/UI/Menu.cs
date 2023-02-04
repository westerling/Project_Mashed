using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField]
    private GameObject m_MainMenu;

    [SerializeField]
    private GameObject m_PlayerMenu;

    [SerializeField]
    private GameObject m_SettingsMenu;

    [SerializeField]
    private GameObject m_LevelMenu;

    [Header("Player Selection Menu")]
    [SerializeField]
    private List<GameObject> m_PlayerPanels = new List<GameObject>();
    
    [SerializeField]
    private Button m_PlayButton;

    [Header("Track Settings")]
    [SerializeField]
    private TMP_Text m_CarName;

    [SerializeField]
    private TMP_Text m_TrackName;

    private int m_CurrentCarIndex = 0;
    private int m_CurrentTrackIndex = 0;

    public void SelectPlayers()
    {
        m_MainMenu.SetActive(false);
        m_PlayerMenu.SetActive(true);
        CheckPlayers();
    }

    public void TrackSettings()
    {
        m_PlayerMenu.SetActive(false);
        m_LevelMenu.SetActive(true);
    }

    public void StartGame()
    {
        var car = GameManager.Current.Cars[m_CurrentCarIndex];
        var track = m_CurrentTrackIndex + 2;
        GameManager.Current.LoadGame(track, car);
    }

    private void Awake()
    {
        m_CurrentCarIndex = 0;
        m_CurrentTrackIndex = 0;

        SetCurrentCar();
        SetCurrentTrack();

        GameManager.Current.PlayerJoined += OnPlayerJoined;
        GameManager.Current.PlayerLeft += OnPlayerLeft;
    }

    private void Start()
    {
        CheckPlayers();
    }

    private void CheckPlayers()
    {
        m_PlayButton.interactable = GameManager.Current.ActivePlayers.Count > 0;

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

    private void SetCurrentCar()
    {
        if (GameManager.Current.Cars.Length == 0 || GameManager.Current.Cars == null)
        {
            m_CarName.text = "null";
            return;
        }

        if (GameManager.Current.Cars[m_CurrentCarIndex].TryGetComponent(out Car car))   
        {
            m_CarName.text = car.Config.CarName;
        }
    }

    private void SetCurrentTrack()
    {
        if (GameManager.Current.Tracks.Length == 0 || GameManager.Current.Tracks == null)
        {
            m_TrackName.text = "null";
            return;
        }

        m_TrackName.text = GameManager.Current.Tracks[m_CurrentTrackIndex];
    }

    public void NextCar()
    {
        if (m_CurrentCarIndex + 1 <= GameManager.Current.Cars.Length)
        {
            m_CurrentCarIndex++;
        }
        else
        {
            m_CurrentCarIndex = 0;
        }

        SetCurrentCar();
    }

    public void PreviousCar()
    {
        if (m_CurrentCarIndex <= 0)
        {
            m_CurrentCarIndex = GameManager.Current.Cars.Length - 1;
        }
        else
        {
            m_CurrentCarIndex--;
        }

        SetCurrentCar();
    }

    public void NextTrack()
    {
        if (m_CurrentTrackIndex + 1 <= GameManager.Current.Tracks.Length)
        {
            m_CurrentTrackIndex++;
        }
        else
        {
            m_CurrentTrackIndex = 0;
        }

        SetCurrentTrack();
    }

    public void PreviousTrack()
    {
        if (m_CurrentTrackIndex <= 0)
        {
            m_CurrentTrackIndex = GameManager.Current.Tracks.Length - 1;
        }
        else
        {
            m_CurrentTrackIndex--;
        }

        SetCurrentTrack();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnPlayerJoined()
    {
        CheckPlayers();
    }

    private void OnPlayerLeft()
    {
        CheckPlayers();
    }

    private void OnDestroy()
    {
        GameManager.Current.PlayerJoined -= OnPlayerJoined;
        GameManager.Current.PlayerLeft -= OnPlayerLeft;
    }
}
