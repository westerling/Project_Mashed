using UnityEngine;
using System;

public class OverheadCamera : MonoBehaviour
{
	[SerializeField] 
	private Vector3 m_Offset = -Vector3.forward;

	[Range(0, 10)]
	[SerializeField]
	private float m_LerpPositionMultiplier = 1f;
	
	[Range(0, 10)]
	[SerializeField]
	private float m_LerpRotationMultiplier = 1f;

	[SerializeField]
	private Rigidbody m_RigidBody;

	[SerializeField]
	private GameObject m_Leader;

	[Range(0, 10)]
	[SerializeField]
	private float m_testOne = 1f;

	[Range(0, 10)]
	[SerializeField]
	private float m_testTwo = 1f;

	[SerializeField]
	private Vector3 m_TargetPosition;

	private bool m_GameActive = false;

    private void Update()
    {
		GetLeader();
		GetCenterPosition();
		CheckWinner();
	}

    private void FixedUpdate()
	{
		if (m_GameActive)
        {
			ActiveGameCam();
			return;
        }

		WinnerGameCam();
	}

	private void ActiveGameCam()
    {
		if (m_Leader == null)
		{
			return;
		}

		m_RigidBody.velocity.Normalize();

		var currentRotation = transform.rotation;
		var leaderPosition = m_TargetPosition + m_Leader.transform.TransformDirection(m_Offset);

		transform.LookAt(m_Leader.transform);

		if (leaderPosition.y < m_TargetPosition.y)
		{
			leaderPosition.y = m_TargetPosition.y;
		}

		transform.position = Vector3.Lerp(transform.position, leaderPosition, Time.fixedDeltaTime * m_LerpPositionMultiplier);
		transform.rotation = Quaternion.Lerp(currentRotation, transform.rotation, Time.fixedDeltaTime * m_LerpRotationMultiplier);

		if (transform.position.y < 0.5f)
		{
			transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
		}
	}

	private void WinnerGameCam()
    {
		if (m_Leader == null)
		{
			return;
		}

		m_RigidBody.velocity.Normalize();

		var currentRotation = transform.rotation;
		var leaderPosition = m_TargetPosition + m_Leader.transform.TransformDirection(m_Offset);

		transform.LookAt(m_Leader.transform);

		if (leaderPosition.y < m_TargetPosition.y)
		{
			leaderPosition.y = m_TargetPosition.y;
		}

		transform.position = Vector3.Lerp(transform.position, leaderPosition, Time.fixedDeltaTime * m_testOne);
		transform.rotation = Quaternion.Lerp(currentRotation, transform.rotation, Time.fixedDeltaTime * m_testTwo);

		if (transform.position.y < 0.5f)
		{
			transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
		}
	}

    private void GetLeader()
	{
		m_Leader = Race.Current.Leader;
	}

	private void GetCenterPosition()
    {
		if (m_Leader == null || Race.Current.ActiveCars.Count <= 0)
		{
			return;
		}

		if (Race.Current.ActiveCars.Count <= 1)
        {
			m_TargetPosition =  m_Leader.transform.position;
        }

		var bounds = new Bounds(Race.Current.ActiveCars[0].transform.position, Vector3.zero);

        for (var i = 0; i < Race.Current.ActiveCars.Count; i++)
        {
			bounds.Encapsulate(Race.Current.ActiveCars[i].transform.position);
        }

		m_TargetPosition = bounds.center;
    }

	private void CheckWinner()
    {
		m_GameActive = Race.Current.ActiveCars.Count > 1;
	}
}
