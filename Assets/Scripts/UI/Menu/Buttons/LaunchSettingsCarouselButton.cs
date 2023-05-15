using UnityEngine;

public abstract class LaunchSettingsCarouselButton : CarouselButton
{
    protected LaunchSettingsMenu m_LaunchSettingsMenu;

    private void Start()
    {
        m_LaunchSettingsMenu = GetComponentInParent<LaunchSettingsMenu>();
    }
}
