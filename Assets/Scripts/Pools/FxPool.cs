using System.Linq;
using UnityEngine;

public class FxPool : ObjectPool
{
    public static FxPool Current;

    public override void CreateInstance()
    {
        Current = this;
    }

    public GameObject GetPooledObjectOfType(ParticleType particleType)
    {
        return PooledObjects.First(x => !(x.activeInHierarchy) && x.GetComponent<Particle>().ParticleType == particleType);
    }
}
