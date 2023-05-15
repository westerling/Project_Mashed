using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LaunchSettingsMenu : Menu
{
    [Header("UI")]
    [SerializeField]
    private TMP_Text m_TrackName;

    [SerializeField]
    private TMP_Text m_CarName;

    [SerializeField]
    private TMP_Text m_GameLengthText;

    [SerializeField]
    private TMP_Text m_AirStrikeText;

    private int m_CurrentCarIndex = 0;
    private int m_CurrentTrackIndex = 0;
    private int m_CurrentGameLengthIndex = 0;

    private bool m_AirStrike = true;

    private void Start()
    {
        m_CurrentCarIndex = 0;
        m_CurrentCarIndex = 0;
        m_CurrentGameLengthIndex = 0;
        m_AirStrike = true;

        SetCurrentTrack();
        SetCurrentCar();
        DisplayTimeString();
        DisplayCurrentAirStrikeOption();
    }

    public override void SteerPerformed(float obj)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (EventSystem.current.currentSelectedGameObject.TryGetComponent(out CarouselButton button))
        {
            if (obj < 0)
            {
                button.OnLeft();
            }
            else if (obj > 0)
            {
                button.OnRight();
            }
        }
    }

    public void NextTrack()
    {
        m_CurrentTrackIndex++;

        if (m_CurrentTrackIndex >= GameManager.Current.Tracks.Length)
        {
            m_CurrentTrackIndex = 0;
        }

        Debug.Log(m_CurrentTrackIndex);

        SetCurrentTrack();
    }

    public void PreviousTrack()
    {
        m_CurrentTrackIndex--;

        if (m_CurrentTrackIndex < 0)
        {
            m_CurrentTrackIndex = GameManager.Current.Tracks.Length - 1;
        }

        SetCurrentTrack();
    }

    public void NextCar()
    {
        m_CurrentCarIndex++;

        if (m_CurrentCarIndex >= GameManager.Current.Cars.Length)
        {
            m_CurrentCarIndex = 0;
        }

        SetCurrentCar();
    }

    public void PreviousCar()
    {
        m_CurrentCarIndex--;

        if (m_CurrentCarIndex < 0)
        {
            m_CurrentCarIndex = GameManager.Current.Cars.Length - 1;
        }

        SetCurrentCar();
    }

    public void NextTime()
    {
        m_CurrentGameLengthIndex++;

        if (m_CurrentGameLengthIndex > 4)
        {
            m_CurrentGameLengthIndex = 0;
        }

        DisplayTimeString();
    }

    public void PreviousTime()
    {
        m_CurrentGameLengthIndex--;

        if (m_CurrentGameLengthIndex < 0)
        {
            m_CurrentGameLengthIndex = 4;
        }

        DisplayTimeString();
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

    public void ToggleAirStrike()
    {
        m_AirStrike = !m_AirStrike;

        DisplayCurrentAirStrikeOption();
    }

    private void DisplayCurrentAirStrikeOption()
    {
        if (m_AirStrike)
        {
            m_AirStrikeText.text = "Yes";
        }
        else
        {
            m_AirStrikeText.text = "No";
        }
    }

    private void DisplayTimeString()
    {
        switch (m_CurrentGameLengthIndex)
        {
            case 0:
                m_GameLengthText.text = "No Limit";
                break;
            case 1:
                m_GameLengthText.text = "1:00";
                break;
            case 2:
                m_GameLengthText.text = "3:00";
                break;
            case 3:
                m_GameLengthText.text = "5:00";
                break;
            case 4:
                m_GameLengthText.text = "10:00";
                break;
            default:
                m_GameLengthText.text = "WTF";
                break;
        }
    }

    public void StartRace()
    {
        var car = GameManager.Current.Cars[m_CurrentCarIndex];
        var track = m_CurrentTrackIndex + 2;
        GameManager.Current.LoadGame(track, car, m_CurrentGameLengthIndex, m_AirStrike);
    }
}
