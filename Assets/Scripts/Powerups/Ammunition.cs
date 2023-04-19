using UnityEngine;

public abstract class Ammunition : MonoBehaviour
{
    [SerializeField]
    private WeaponType m_WeaponType;

    public WeaponType WeaponType 
    {
        get => m_WeaponType; 
    }

    protected void Deactivate()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(AmmunitionPool.Current.transform);
    }
}
