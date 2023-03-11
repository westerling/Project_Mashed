using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_ObjectsToPool;

    [SerializeField]
    private int m_AmountToPool;

    private List<GameObject> m_PooledObjects;

    public int AmountToPool
    {
        get => m_AmountToPool;
    }

    public List<GameObject> ObjectsToPool
    {
        get => m_ObjectsToPool;
    }

    public List<GameObject> PooledObjects
    {
        get => m_PooledObjects;
        set => m_PooledObjects = value;
    }

    private void Awake()
    {
        CreateInstance();
    }

    private void Start()
    {
        InstantiateObjects();
    }

    public abstract GameObject GetPooledObject();

    public abstract void CreateInstance();

    public void InstantiateObjects()
    {
        PooledObjects = new List<GameObject>();
        GameObject temp;

        foreach (var objectToPool in ObjectsToPool)
        {
            for (int i = 0; i < AmountToPool; i++)
            {
                temp = Instantiate(objectToPool, transform);
                temp.SetActive(false);
                PooledObjects.Add(temp);
            }
        }
    }
}
