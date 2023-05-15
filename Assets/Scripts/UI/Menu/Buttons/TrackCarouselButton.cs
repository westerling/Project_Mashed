public class TrackCarouselButton : LaunchSettingsCarouselButton
{
    public override void OnLeft()
    {
        m_LaunchSettingsMenu.PreviousTrack();
    }

    public override void OnRight()
    {
        m_LaunchSettingsMenu.NextTrack();
    }
}
