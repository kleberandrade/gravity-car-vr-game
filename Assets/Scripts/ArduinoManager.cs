using ArduinoBluetoothAPI;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ArduinoPacket
{
    public float leftBrake;
    public float rightBrake;
    public float steerAngle;
}

public class ArduinoManager : MonoBehaviour
{
    private BluetoothHelper m_Helper;

    public static ArduinoManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    [SerializeField]
    private string m_DeviceName = "HC-06";

    [SerializeField]
    private Text m_RawText;

    public ArduinoPacket Packet { get; set; } = new ArduinoPacket();

    private void Start()
    {
        ShowText("Connecting...");

        try
        {
            m_Helper = BluetoothHelper.GetInstance(m_DeviceName);
            m_Helper.OnConnected += OnBluetoothConnected;
            m_Helper.OnConnectionFailed += OnConnectionFailed;
            m_Helper.OnDataReceived += OnBluetoothDataReceived;

            m_Helper.setTerminatorBasedStream("\n");
            Connect();
        }
        catch (BluetoothHelper.BlueToothNotEnabledException ex) { ShowText(ex.Message); }
        catch (BluetoothHelper.BlueToothNotReadyException ex) { ShowText(ex.Message); }
        catch (BluetoothHelper.BlueToothNotSupportedException ex) { ShowText(ex.Message); }
        catch (BluetoothHelper.BlueToothPermissionNotGrantedException ex) { ShowText(ex.Message); }
    }

    public void Connect()
    {
        if (m_Helper != null)
        {
            m_Helper.Connect();
        }
    }

    public void Disconnect()
    {
        if (m_Helper != null)
        {
            m_Helper.Disconnect();
        }
    }

    private void OnBluetoothDataReceived()
    {
        string json = m_Helper.Read();
        ShowText($"{json}\nLeft: {Packet.leftBrake}\tSteer: {Packet.steerAngle}\tRight: {Packet.rightBrake}");

        Packet = JsonUtility.FromJson<ArduinoPacket>(json);
    }

    private void OnConnectionFailed()
    {
        ShowText("Failed connect...");
    }

    void OnBluetoothConnected()
    {
        try
        {
            m_Helper.StartListening();
        }
        catch (Exception ex)
        {
            ShowText(ex.Message);
        }
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    private void ShowText(string text)
    {
        if (m_RawText)
        {
            m_RawText.text = text;
        }
    }
}
