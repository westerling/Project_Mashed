public class VehicleCarouselButton : LaunchSettingsCarouselButton
{
    public override void OnLeft()
    {
        m_LaunchSettingsMenu.PreviousCar();
    }

    public override void OnRight()
    {
        m_LaunchSettingsMenu.NextCar();
    }
}
