using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var car = other.GetComponentInParent<Car>();

        if (car == null)
        {
            return;
        }

        Race.Current.DeactivateCar(car);
    }
}
