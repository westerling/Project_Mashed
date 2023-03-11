using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickupPool : ObjectPool
{
    public static PickupPool Current;

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
        var tempList = new List<GameObject>();

        for (int i = 0; i < PooledObjects.Count; i++)
        {
            if (!PooledObjects[i].activeInHierarchy)
            {
                tempList.Add(PooledObjects[i]);
            }
        }

        if (tempList.Count > 0)
        {
            var randomNumber = Random.Range(0, tempList.Count);
            return tempList[randomNumber];
        }

        return null;
    }
}
