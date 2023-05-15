using UnityEngine;

public class PauseMenu : Menu
{
    [Header("Menus")]
    [SerializeField]
    private Menu m_PauseSettingsMenu;

    private void Awake()
    {
        Debug.Log("Hejsan");
    }

    public void ResumeGame()
    {
        UIManager.Current.TogglePauseMenu(false);
        Time.timeScale = 1f;
        RaceManager.Current.GameIsPaused = false;
    }

    public void PauseGame()
    {
        UIManager.Current.TogglePauseMenu(true);
        Time.timeScale= 0f;
        RaceManager.Current.GameIsPaused = true;
    }

    public void OpenPausSettingsMenu()
    {
        OpenMenu(m_PauseSettingsMenu);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        GameManager.Current.UnloadTrack();
    }

    public override void PausePerformed()
    {
        if (RaceManager.Current.RaceIsActive)
        {
            if (RaceManager.Current.GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
}
