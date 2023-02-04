using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : Singleton<FXController>
{
	[Header("Particles settings")]
	[SerializeField]
	private ParticleSystem m_AsphaltSmokeParticles;

	[Header("Trail settings")]
	[SerializeField]
	private TrailRenderer m_TrailRenderer;
	
	[SerializeField]
	private Transform m_TrailsTransform;

	private Queue<TrailRenderer> m_FreeTrails = new Queue<TrailRenderer>();

	protected override void AwakeSingleton()
	{
		m_TrailRenderer.gameObject.SetActive(false);
	}

	public ParticleSystem GetAspahaltParticles()
	{ 
		return m_AsphaltSmokeParticles; 
	}

	public TrailRenderer GetTrail(Vector3 startPos)
	{
		var trail = new TrailRenderer();

		if (m_FreeTrails.Count > 0)
		{
			trail = m_FreeTrails.Dequeue();
		}
		else
		{
			trail = Instantiate(m_TrailRenderer, m_TrailsTransform);
		}

		trail.transform.position = startPos;
		trail.gameObject.SetActive(true);

		return trail;
	}

	/// <summary>
	/// Set trail as free and wait life time.
	/// </summary>
	public void SetFreeTrail(TrailRenderer trail)
	{
		StartCoroutine(WaitVisibleTrail(trail));
	}

	/// <summary>
	/// The trail is considered busy until it disappeared.
	/// </summary>
	private IEnumerator WaitVisibleTrail(TrailRenderer trail)
	{
		trail.transform.SetParent(m_TrailsTransform);
		yield return new WaitForSeconds(trail.time);
		trail.Clear();
		trail.gameObject.SetActive(false);
		m_FreeTrails.Enqueue(trail);
	}
}