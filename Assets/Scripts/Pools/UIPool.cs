using System.Linq;
using UnityEngine;

public class UIPool : ObjectPool
{

    public static UIPool Current;
    public override void CreateInstance()
    {
        Current = this;
    }

    public GameObject GetPooledObjectOfType(UIType uIType)
    {
        return PooledObjects.First(x => !(x.activeInHierarchy) && x.GetComponent<UIElement>().UIType == uIType);
    }
}
