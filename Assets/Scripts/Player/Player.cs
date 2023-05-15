using UnityEngine;

public class Player : MonoBehaviour
{
    private string m_Name;

    public string Name 
    {
        get => m_Name; 
        set => m_Name = value; 
    }

    public void AddListeners()
    {
        var car = transform.GetComponentInChildren<Car>();

        if (car == null)
        {
            return;
        }

        car.StatusUpdated += OnCarStatusUpdated;
    }

    public void RemoveListeners()
    {
        var car = transform.GetComponentInChildren<Car>();

        if (car == null)
        {
            return;
        }

        car.StatusUpdated -= OnCarStatusUpdated;
    }

    private void OnCarStatusUpdated(bool status)
    {
    }
}
