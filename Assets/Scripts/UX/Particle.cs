using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField]
    private ParticleType m_ParticleType;

    public ParticleType ParticleType 
    {
        get => m_ParticleType;
    }

    public void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(FxPool.Current.transform);
    }
}
