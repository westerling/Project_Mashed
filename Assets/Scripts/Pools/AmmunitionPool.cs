using System.Linq;
using UnityEngine;

public class AmmunitionPool : ObjectPool
{
    public static AmmunitionPool Current;

    public override void CreateInstance()
    {
        Current = this;
    }

    public GameObject GetPooledObjectOfType(WeaponType weaponType)
    {
        return PooledObjects.First(x => !(x.activeInHierarchy) && x.GetComponent<Ammunition>().WeaponType == weaponType);
    }

    public override GameObject GetPooledObject()
    {
        for (int i = 0; i < PooledObjects.Count; i++)
        {
            if (!PooledObjects[i].activeInHierarchy)
            {
                return PooledObjects[i];
            }
        }

        return null;
    }
}
