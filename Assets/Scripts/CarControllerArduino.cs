using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarKinematics))]
public class CarControllerArduino : MonoBehaviour
{
    private CarKinematics m_CarKinematics;

    private ArduinoManager m_Arduino;

    private void Awake()
    {
        m_CarKinematics = GetComponent<CarKinematics>();
    }

    private void Start()
    {
        m_Arduino = ArduinoManager.Instance; 
    }

    private void Update()
    {
        m_CarKinematics.Horizontal = 1.0f - (m_Arduino.Packet.steerAngle / 1023.0f) * 2.0f;
        m_CarKinematics.Vertical = 0.2f;
        m_CarKinematics.LeftBrake = m_Arduino.Packet.leftBrake / 1023.0f;
        m_CarKinematics.RightBrake = m_Arduino.Packet.rightBrake / 1023.0f;
    }
}
