using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event Action<float> RightTrigger;
    public event Action<float> Joystick;
    public event Action<float> BButton;
    public event Action AButton;
    public event Action StartButton;

    [SerializeField]
    private PlayerInput m_Controls;

    private void Start()
    {
        m_Controls.actions["RightTrigger"].performed += RightTriggerPerformed;
        m_Controls.actions["RightTrigger"].canceled += RightTriggerPerformed;
        m_Controls.actions["Joystick"].performed += JoystickPerformed;
        m_Controls.actions["Joystick"].canceled += JoystickPerformed;
        m_Controls.actions["A"].performed += APerformed;
        m_Controls.actions["B"].performed += BPerformed;
        m_Controls.actions["B"].canceled += BPerformed;
        m_Controls.actions["Start"].performed += StartPerformed;
    }

    private void BPerformed(InputAction.CallbackContext obj)
    {
        BButton?.Invoke(obj.ReadValue<float>());
    }

    private void APerformed(InputAction.CallbackContext obj)
    {
        AButton?.Invoke();
    }

    private void JoystickPerformed(InputAction.CallbackContext obj)
    {
        Joystick?.Invoke(obj.ReadValue<float>());
    }

    private void RightTriggerPerformed(InputAction.CallbackContext obj)
    {
        RightTrigger?.Invoke(obj.ReadValue<float>());
    }

    private void StartPerformed(InputAction.CallbackContext obj)
    {
        StartButton?.Invoke();
    }

    private void OnDestroy()
    {
        m_Controls.actions["RightTrigger"].performed -= RightTriggerPerformed;
        m_Controls.actions["RightTrigger"].canceled -= RightTriggerPerformed;
        m_Controls.actions["Joystick"].performed -= JoystickPerformed;
        m_Controls.actions["Joystick"].canceled -= JoystickPerformed;
        m_Controls.actions["A"].performed -= APerformed;
        m_Controls.actions["B"].performed -= BPerformed;
        m_Controls.actions["B"].canceled -= BPerformed;
        m_Controls.actions["Start"].performed -= StartPerformed;
    }
}
