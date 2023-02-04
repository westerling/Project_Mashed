using UnityEngine;

public class Player : MonoBehaviour
{
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
