using UnityEngine;

public class RotateBar : MonoBehaviour
{
    private float m_MinAngle;

    private float m_MaxAngle;

    [SerializeField]
    private Vector3 m_Axis = Vector3.up;

    public enum ArduinoControlType { LeftBrake, RightBrake, SteerAngle }
    public ArduinoControlType m_Type;

    private void Update()
    {
        switch (m_Type)
        {
            case ArduinoControlType.LeftBrake:
                transform.rotation = Quaternion.Euler(m_Axis * ArduinoManager.Instance.Packet.leftBrake);
                break;
            case ArduinoControlType.RightBrake:
                transform.rotation = Quaternion.Euler(m_Axis * ArduinoManager.Instance.Packet.rightBrake);
                break;
            case ArduinoControlType.SteerAngle:
                transform.rotation = Quaternion.Euler(m_Axis * ArduinoManager.Instance.Packet.steerAngle);
                break;
            default:
                break;
        }
    }
}
