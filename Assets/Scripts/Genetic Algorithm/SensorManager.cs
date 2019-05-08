using System;
using System.Text;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
    [SerializeField]
    private Sensor[] m_Sensors;

    public float[] ToArray()
    {
        float[] array = new float[m_Sensors.Length];
        for (int i = 0; i < m_Sensors.Length; i++)
        {
            array[i] = m_Sensors[i].Distance;
        }

        return array;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        Array.ForEach(ToArray(), x => builder.Append(x));
        return builder.ToString();
    }
}
