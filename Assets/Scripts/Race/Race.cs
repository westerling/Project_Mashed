using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Race : MonoBehaviour
{
    [SerializeField]
    private Checkpoint[] m_Checkpoints;

    [SerializeField]
    private Checkpoint m_LastCheckpoint;
    
    [SerializeField]
    private Checkpoint m_NextCheckpoint;
        
    private bool m_RaceIsActive = false;

    private GameObject m_Leader;
    private List<Player> m_LastPositions = new List<Player>();
    private List<GameObject> m_ActiveCars = new List<GameObject>();
    private List<GameObject> m_InactiveCars = new List<GameObject>();
    private Dictionary<Player, int> m_PlayerPoints = new Dictionary<Player, int>();

    public static Race Current;


    public GameObject Leader 
    {
        get => m_Leader; 
        set => m_Leader = value; 
    }

    public List<GameObject> ActiveCars 
    {
        get => m_ActiveCars; 
    }

    public Vector3 PassiveLookAtPoint
    {
        get
        {
            if (m_NextCheckpoint == null)
            {
                return Vector3.zero;
            }

            return m_NextCheckpoint.transform.position;
        }
    }

    public bool RaceIsActive 
    {
        get => m_RaceIsActive;
        set => m_RaceIsActive = value;
    }
    
    public Dictionary<Player, int> PlayerPoints 
    { 
        get => m_PlayerPoints; 
        set => m_PlayerPoints = value; 
    }

    public void DeactivateCar(Car car)
    {
        if (ActiveCars.Contains(car.gameObject))
        {
            var player = car.GetComponentInParent<Player>();

            ActiveCars.Remove(car.gameObject);
            m_InactiveCars.Add(car.gameObject);

            UIManager.Current.ShowScoreText(player, true);
            
            car.SetCarStatus(false);

            GivePoints(car);
        }
    }

    private void Awake()
    {
        Current = this;

        UIManager.Current.ToggleLoadingScreen(false);
        UIManager.Current.ToggleRaceUI(false);
    }

    private void OnEnable()
    {
        StartRace();
    }

    private void Update()
    {
        FindLeader();
        CheckDistance();
        CheckPlayersLeft();
    }

    private void StartRace()
    {
        SpawnCars();
        ResetPoints();
        ResetRace();
    }

    private void ResetRace()
    {
        SetRaceStatus(false);
        ResetCarList();
        ResetAllCheckpoints();
        ResetPlayerStatus();
        PlaceCars();
        m_LastPositions.Clear();
        ResetCarPointImages();
        StartCoroutine(Countdown());
    }

    private void SetRaceStatus(bool enabled)
    {
        RaceIsActive = enabled;
    }

    private IEnumerator Countdown()
    {
        UIManager.Current.DisplayTrafficLight(true);
        UIManager.Current.TurnOffStartLights();

        for (var i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.5f);
            UIManager.Current.ToggleStartLight(i);
        }

        SetRaceStatus(true);

        yield return new WaitForSeconds(1f);

        UIManager.Current.DisplayTrafficLight(false);
    }

    private void ResetPoints()
    {
        UIManager.Current.SetupGameGUI();

        for (int i = 0; i < m_ActiveCars.Count; i++)
        {
            if (m_ActiveCars[i].TryGetComponent(out Car car))
            {
                var player = car.GetComponentInParent<Player>();
                var startPoints = Globals.StartPoints(m_ActiveCars.Count);

                PlayerPoints.Add(player, startPoints);

                UIManager.Current.SetPoints(player, startPoints);
            }
        }
    }

    private void SpawnCars()
    {
        foreach (var player in GameManager.Current.ActivePlayers)
        {
            var car = Instantiate(GameManager.Current.Car, player.gameObject.transform);

            ActiveCars.Add(car);
        }
    }

    private void ResetPlayerStatus()
    {
        foreach (var car in ActiveCars)
        {
            car.GetComponent<Car>().SetCarStatus(true);
        }
    }

    private void CheckPlayersLeft()
    {
        if (ActiveCars.Count <= 0)
        {
            CheckForWinner();
            ResetRace();
        }
    }

    private void ResetCarList()
    {
        ClearActiveCars();
        ActiveCars.AddRange(m_InactiveCars);
        m_InactiveCars.Clear();
    }

    private void PlaceCars()
    {
        for (int i = 0; i < ActiveCars.Count; i++)
        {
            ActiveCars[i].gameObject.transform.position = m_LastCheckpoint.StartPositions[i].transform.position;
            ActiveCars[i].gameObject.transform.forward = m_LastCheckpoint.StartPositions[i].transform.forward;

            ActiveCars[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void ResetCarPointImages()
    {
        for (int i = 0; i < m_ActiveCars.Count; i++)
        {
            if (m_ActiveCars[i].TryGetComponent(out Car car))
            {
                var player = car.GetComponentInParent<Player>();
                UIManager.Current.ShowScoreText(player, false);
            }
        }
    }

    private void GivePoints(Car car)
    {
        var player = car.GetComponentInParent<Player>();
        var position = m_InactiveCars.IndexOf(car.gameObject);
        var currentPoints = PlayerPoints[player];
        var pointsToRecieve = Globals.CalculatePoints(position, currentPoints, PlayerPoints.Count);
        
        PlayerPoints[player] += pointsToRecieve;

        Debug.Log("Player: " + player + " | Position: " + position + " | Current Points: " + currentPoints + " | Points to recieve: " + pointsToRecieve);

        UIManager.Current.SetPoints(player, PlayerPoints[player], pointsToRecieve);
    }

    private void CheckForWinner()
    {
        foreach (var playerDictionary in PlayerPoints)
        {
            if (playerDictionary.Value >= Globals.MaxPoints(GameManager.Current.ActivePlayers.Count))
            {
                //Game over
            }
        }
    }

    private void ClearActiveCars()
    {
        for (var i = ActiveCars.Count - 1; i >= 0; i--)
        {
            m_InactiveCars.Add(ActiveCars[i]);

            ActiveCars.RemoveAt(i);
        }

        ActiveCars.Clear();
    }

    private void CheckDistance()
    {
        AddWarnings();
        RemoveDistantCars();
    }

    private void AddWarnings()
    {
        var warningCars = m_ActiveCars.Where(x => Vector3.Distance(x.transform.position, Leader.transform.position) > 15f).ToList();

        foreach (var activeCar in ActiveCars)
        {
            if (activeCar.TryGetComponent(out Car car))
            {
                car.WarningSprite.SetActive(warningCars.Contains(activeCar));
            }
        }
    }

    private void RemoveDistantCars()
    {
        var inactiveCars = m_ActiveCars.Where(x => Vector3.Distance(x.transform.position, Leader.transform.position) > 25f).ToList();

        for (int i = inactiveCars.Count() - 1; i >= 0; i--)
        {
            if (inactiveCars[i].TryGetComponent(out Car car))
            {
                DeactivateCar(car);
            }
        }
    }

    private void FindLeader()
    {
        Leader = ActiveCars.OrderBy(car => (car.transform.position - m_NextCheckpoint.transform.position).sqrMagnitude).FirstOrDefault();
    }

    private void ResetAllCheckpoints()
    {
        foreach (var checkpoint in m_Checkpoints)
        {
            checkpoint.Active = false;
        }
     
        m_LastCheckpoint = m_Checkpoints[0];
        RemoveCheckpointListener();
        FindNextCheckpoint();
        AddCheckpointListener();
    }

    private void FindNextCheckpoint()
    {
        var index = Array.IndexOf(m_Checkpoints, m_LastCheckpoint);

        if (index < m_Checkpoints.Length - 1)
        {
            index++;
            m_NextCheckpoint = m_Checkpoints[index];
        }
        else
        {
            m_NextCheckpoint = m_Checkpoints[0];
        }

        m_NextCheckpoint.Active = true;
    }

    private void OnCheckpointPassed()
    {
        RemoveCheckpointListener();
        m_LastCheckpoint = m_NextCheckpoint;
        FindNextCheckpoint();
        AddCheckpointListener();
    }

    private void AddCheckpointListener()
    {
        if (m_NextCheckpoint == null)
        {
            return;
        }

        m_NextCheckpoint.CheckpointPassed += OnCheckpointPassed;
    }

    private void RemoveCheckpointListener()
    {
        if (m_NextCheckpoint == null)
        {
            return;
        }

        m_NextCheckpoint.CheckpointPassed -= OnCheckpointPassed;
    }
}
