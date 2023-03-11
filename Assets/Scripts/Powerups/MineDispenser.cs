public class MineDispenser : Weapon
{
    public override void Fire()
    {
        var pooledObject = AmmunitionPool.Current.GetPooledObjectOfType(WeaponType.Mine);

        if (pooledObject != null)
        {
            pooledObject.transform.position = transform.position;
            pooledObject.transform.rotation = transform.rotation;
            pooledObject.SetActive(true);

            if (pooledObject.TryGetComponent(out Mine mine))
            {
                mine.ActivateMine();
            }
        }

        Ammunition--;
    }

    public override void SetInitialValues()
    {
        Ammunition = 2;
        AutomaticFiring = false;
        WeaponType = WeaponType.Mine;
    }
}
