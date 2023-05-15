using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public abstract class Menu : MonoBehaviour
{
    [SerializeField]
    private bool m_EnabledOnStart = false;

    [SerializeField]
    private GameObject m_SelectedOnStart;

    private Menu m_ParentMenu;

    protected Menu ParentMenu 
    {
        get => m_ParentMenu; 
        set => m_ParentMenu = value; 
    }

    private void Awake()
    {
        GameManager.Current.PlayerJoined += OnPlayerJoined;
        GameManager.Current.PlayerLeft += OnPlayerLeft;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void EnableMenu()
    {
        if (m_SelectedOnStart == null)
        {
            return;
        }

        SelectFirstItem();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameObject.SetActive(m_EnabledOnStart);
    }

    private void OnEnable()
    {
        if (m_EnabledOnStart)
        {
            EnableMenu();
        }
    }

    protected void OpenMenu(Menu newMenu)
    {
        newMenu.ParentMenu = this;
        newMenu.gameObject.SetActive(true);
        newMenu.EnableMenu();

        MenuManager.Current.SetActiveMenu(newMenu);

        gameObject.SetActive(false);
    }

    private void SelectFirstItem()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_SelectedOnStart);
    }

    protected virtual void OnPlayerJoined(Player playerJoining)
    {
        //AddListeners(playerJoining);
    }

    protected virtual void OnPlayerLeft(Player playerLeaving)
    {
        //RemoveListeners(playerLeaving);
    }

    private void AddListeners(Player playerJoining)
    {
        if (playerJoining.TryGetComponent(out InputManager inputManager))
        {
            inputManager.StartButton += PausePerformed;
            inputManager.Joystick += SteerPerformed;
            inputManager.AButton += ShootPerformed;
        }
    }

    public virtual void ShootPerformed()
    {
        return;
    }

    public virtual void SteerPerformed(float obj)
    {
        return;
    }

    private void RemoveListeners(Player playerLeaving)
    {
        if (playerLeaving.TryGetComponent(out InputManager inputManager))
        {
            inputManager.StartButton -= PausePerformed;
            inputManager.Joystick -= SteerPerformed;
            inputManager.AButton -= ShootPerformed;
        }
    }

    public virtual void PausePerformed()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (ParentMenu == null)
        {
            return;
        }

        gameObject.SetActive(false);
        ParentMenu.gameObject.SetActive(true);

        if (ParentMenu.TryGetComponent(out Menu menu))
        {
            menu.EnableMenu();
        }
    }

    private void OnDestroy()
    {
        GameManager.Current.PlayerJoined -= OnPlayerJoined;
        GameManager.Current.PlayerLeft -= OnPlayerLeft;
        SceneManager.sceneLoaded -= OnSceneLoaded;

        return;

        foreach (var player in GameManager.Current.ActivePlayers)
        {
            RemoveListeners(player);
        }
    }
}
