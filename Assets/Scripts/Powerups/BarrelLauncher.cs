using System.Collections.Generic;
using UnityEngine;

public class BarrelLauncher : Weapon
{
    [SerializeField]
    private Transform[] m_BarrelTransforms;

    private List<GameObject> m_Barrels = new List<GameObject>();

    public override void SetInitialValues()
    {
        AutomaticFiring = false;
        WeaponType = WeaponType.DrumBomb;
    }

    public override void Fire()
    {
        for (var i = m_Barrels.Count - 1; i >= 0; i--)
        {
            if (m_Barrels[i].TryGetComponent(out Joint joint))
            {
                Destroy(joint);
            }

            if (m_Barrels[i].TryGetComponent(out Rigidbody rigidBody))
            {
                rigidBody.velocity = transform.up * 10f;
                m_Barrels[i].GetComponent<Barrel>().ActivateBarrel();
                m_Barrels[i].gameObject.transform.SetParent(null);
            }

            Ammunition--;
            m_Barrels.RemoveAt(i);
            break;
        }
    }

    private void SetupBarrels()
    {
        m_Barrels.Clear();

        for (int i = 0; i < m_BarrelTransforms.Length; i++)
        {
            var pooledObject = AmmunitionPool.Current.GetPooledObjectOfType(WeaponType.DrumBomb);

            if (pooledObject != null)
            {
                pooledObject.transform.SetParent(transform);

                if (pooledObject.TryGetComponent(out Barrel barrel))
                {
                    barrel.Active = false;
                }

                pooledObject.transform.position = m_BarrelTransforms[i].position;
                pooledObject.transform.rotation = m_BarrelTransforms[i].rotation;
                pooledObject.SetActive(true);

                var joint = pooledObject.AddComponent<FixedJoint>();

                joint.connectedBody = gameObject.GetComponent<Rigidbody>();

                m_Barrels.Add(pooledObject);
            }
        }
    }

    public override void OnPickup()
    {
        Ammunition = 2;

        SetupBarrels();
    }
}
