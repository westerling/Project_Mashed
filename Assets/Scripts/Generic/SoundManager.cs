using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_NormalSound;

    [SerializeField]
    private AudioSource m_3dSound;

    [SerializeField]
    private AudioClip m_MenuClickForward;

    [SerializeField]
    private AudioClip m_MenuClickbackward;

    [SerializeField]
    private AudioClip m_StartSound;

    [SerializeField]
    private AudioClip m_MenuMusic;

    public static SoundManager Current;

    private void Awake()
    {
        Current = this;
    }

    public void PlayStartSound()
    {
        m_NormalSound.PlayOneShot(m_StartSound);
    }
}
