public class AirstrikeCarouselButton : LaunchSettingsCarouselButton
{
    public override void OnLeft()
    {
        m_LaunchSettingsMenu.ToggleAirStrike();
    }

    public override void OnRight()
    {
        m_LaunchSettingsMenu.ToggleAirStrike();
    }
}
