using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event Action<float> Acceleration;
    public event Action<float> Steer;
    public event Action<float> HandBrake;
    public event Action Shoot;
    public event Action Pause;

    [SerializeField]
    private PlayerInput m_Controls;

    private void Start()
    {
        m_Controls.actions["Acceleration"].performed += AccelerationPerformed;
        m_Controls.actions["Acceleration"].canceled += AccelerationPerformed;
        m_Controls.actions["Steer"].performed += SteerPerformed;
        m_Controls.actions["Steer"].canceled += SteerPerformed;
        m_Controls.actions["Shoot"].performed += ShootPerformed;
        m_Controls.actions["HandBrake"].performed += HandBrakePerformed;
        m_Controls.actions["HandBrake"].canceled += HandBrakePerformed;
        m_Controls.actions["Pause"].performed += PausePerformed;
    }

    private void HandBrakePerformed(InputAction.CallbackContext obj)
    {
        HandBrake?.Invoke(obj.ReadValue<float>());
    }

    private void ShootPerformed(InputAction.CallbackContext obj)
    {
        Shoot?.Invoke();
    }

    private void SteerPerformed(InputAction.CallbackContext obj)
    {
        Steer?.Invoke(obj.ReadValue<float>());
    }

    private void AccelerationPerformed(InputAction.CallbackContext obj)
    {
        Acceleration?.Invoke(obj.ReadValue<float>());
    }

    private void PausePerformed(InputAction.CallbackContext obj)
    {
        Pause?.Invoke();
    }

    private void OnDestroy()
    {
        m_Controls.actions["Acceleration"].performed -= AccelerationPerformed;
        m_Controls.actions["Acceleration"].canceled -= AccelerationPerformed;
        m_Controls.actions["Steer"].performed -= SteerPerformed;
        m_Controls.actions["Steer"].canceled -= SteerPerformed;
        m_Controls.actions["Shoot"].performed -= ShootPerformed;
        m_Controls.actions["HandBrake"].performed -= HandBrakePerformed;
        m_Controls.actions["HandBrake"].canceled -= HandBrakePerformed;
        m_Controls.actions["Pause"].performed -= PausePerformed;
    }
}
