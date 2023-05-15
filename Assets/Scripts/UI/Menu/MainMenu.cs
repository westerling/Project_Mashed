using UnityEngine;

public class MainMenu : Menu
{
    [Header("Menus")]
    [SerializeField]
    private Menu m_PlayMenu;

    [SerializeField]
    private Menu m_SettingsMenu;

    public void OpenPlayMenu()
    {
        OpenMenu(m_PlayMenu);
    }

    public void OpenSettings()
    {
        OpenMenu(m_SettingsMenu);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
