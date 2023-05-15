using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Current;
    public event Action<Player> PlayerJoined;
    public event Action<Player> PlayerLeft;

    [Header("Gameplay")]
    [SerializeField]
    private GameObject[] m_Cars;

    [SerializeField]
    private string[] m_Tracks;

    [SerializeField]
    private GameObject[] m_Pools;

    [Header("UI")]
    [SerializeField]
    private GameObject m_LoadingScreen;

    [SerializeField]
    private Slider m_ProgressBar;

    private int m_CurrentLoadedScene = 0;
    private float m_TotalSceneProgress;
    private float m_GameTime = 100f;
    private bool m_UseGameTimer;
    private bool m_UseAirStrike = false;

    private GameObject m_Car;
    private List<Player> m_ActivePlayers = new List<Player>();
    private List<AsyncOperation> m_ScenesLoading = new List<AsyncOperation>();
    
    private GameState m_GameState;

    public List<Player> ActivePlayers 
    {
        get => m_ActivePlayers; 
        set => m_ActivePlayers = value; 
    }

    public GameObject Car
    { 
        get => m_Car; 
        set => m_Car = value; 
    }

    public string[] Tracks 
    {
        get => m_Tracks; 
    }

    public GameObject[] Cars 
    {
        get => m_Cars; 
    }
    
    public GameObject[] Pools 
    {
        get => m_Pools; 
    }
    public GameState GameState 
    { 
        get => m_GameState; 
        set => m_GameState = value; 
    }

    private void Awake()
    {
        Current = this;

        SceneManager.LoadSceneAsync((int)SceneIndexes.Title_Screen, LoadSceneMode.Additive);

        GameState = GameState.Menu;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.TryGetComponent(out Player player))
        {
            if (!ActivePlayers.Contains(player))
            {
                ActivePlayers.Add(player);
                PlayerJoined?.Invoke(player);
            }
        }
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        if (playerInput.TryGetComponent(out Player player))
        {
            if (ActivePlayers.Contains(player))
            {
                ActivePlayers.Remove(player);
                PlayerLeft?.Invoke(player);
            }
        }
    }

    public void LoadGame(int levelIndex, GameObject car, int gameLength, bool airStrike)
    {
        Car = car;
        m_UseGameTimer = gameLength > 0;
        m_GameTime = Globals.GetGameLength(gameLength);
        m_UseAirStrike = airStrike;

        UIManager.Current.ToggleLoadingScreen(true);

        m_ScenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Title_Screen));
        m_ScenesLoading.Add(SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive));

        m_CurrentLoadedScene = levelIndex;

        StartCoroutine(GetSceneLoadProgress());
    }

    public void UnloadTrack()
    {
        m_ScenesLoading.Add(SceneManager.UnloadSceneAsync(m_CurrentLoadedScene));
        m_ScenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Title_Screen, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());

        GameState = GameState.Menu;
    }

    public IEnumerator GetSceneLoadProgress()
    {
        for (var i = 0; i < m_ScenesLoading.Count; i++)
        {
            while(!m_ScenesLoading[i].isDone)
            {
                m_TotalSceneProgress = 0;

                foreach (var operation in m_ScenesLoading)
                {
                    m_TotalSceneProgress += operation.progress;
                }

                m_TotalSceneProgress = (m_TotalSceneProgress / m_ScenesLoading.Count) * 100;

                m_ProgressBar.value = Mathf.RoundToInt(m_TotalSceneProgress);

                yield return null;
            }
        }

        UIManager.Current.ToggleLoadingScreen(false);
        UIManager.Current.ToggleRaceUI(true);
    }
}
