public class TimeCarouselButton : LaunchSettingsCarouselButton
{
    public override void OnLeft()
    {
        m_LaunchSettingsMenu.PreviousTime();
    }

    public override void OnRight()
    {
        m_LaunchSettingsMenu.NextTime();
    }
}
