using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Current;

    [SerializeField]
    private Menu m_MainMenu;

    [SerializeField]
    private Menu m_PlayerMenu;

    [SerializeField]
    private Menu m_PauseMenu;

    private Menu m_ActiveMenu;

    private void Awake()
    {
        Current = this;

        GameManager.Current.PlayerJoined += OnPlayerJoined;
        GameManager.Current.PlayerLeft += OnPlayerLeft;
    }

    private void Start()
    {
        if (m_ActiveMenu == null)
        {    
            m_ActiveMenu = EventSystem.current.currentSelectedGameObject?.GetComponentInParent<Menu>();
        }
    }

    public void SetPauseMenuActiveMenu()
    {
        SetActiveMenu(m_PauseMenu);
    }

    public void SetActiveMenu(Menu activeMenu)
    {
        m_ActiveMenu = activeMenu;
    }

    private void OnPlayerJoined(Player playerJoining)
    {
        AddListeners(playerJoining);
    }

    private void OnPlayerLeft(Player playerLeaving)
    {
        RemoveListeners(playerLeaving);
    }

    private void AddListeners(Player playerJoining)
    {
        if (playerJoining.TryGetComponent(out InputManager inputManager))
        {
            inputManager.StartButton += PausePerformed;
            inputManager.Joystick += SteerPerformed;
            inputManager.AButton += APerformed;
        }
    }
    
    private void RemoveListeners(Player playerLeaving)
    {
        if (playerLeaving.TryGetComponent(out InputManager inputManager))
        {
            inputManager.StartButton -= PausePerformed;
            inputManager.Joystick -= SteerPerformed;
            inputManager.AButton -= APerformed;
        }
    }

    private void APerformed()
    {
        if (m_ActiveMenu == null)
        {
            return;
        }

        if (m_ActiveMenu.gameObject.activeInHierarchy)
        {
            m_ActiveMenu?.ShootPerformed();
        }
    }

    private void SteerPerformed(float obj)
    {
        if (m_ActiveMenu == null)
        {
            return;
        }

        if (m_ActiveMenu.gameObject.activeInHierarchy)
        {
            m_ActiveMenu?.SteerPerformed(obj);
        }
    }

    private void PausePerformed()
    {
        if (m_ActiveMenu == null)
        {
            return;
        }

        m_ActiveMenu?.PausePerformed();
    }

    private void OnDestroy()
    {
        GameManager.Current.PlayerJoined -= OnPlayerJoined;
        GameManager.Current.PlayerLeft -= OnPlayerLeft;

        foreach (var player in GameManager.Current.ActivePlayers)
        {
            RemoveListeners(player);
        }
    }
}
