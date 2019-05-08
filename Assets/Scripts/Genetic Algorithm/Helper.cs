using System;

public class Helper
{
    private static Random m_Random;

    private Helper() { }

    private static void InitializeRandomNumber()
    {
        if (m_Random == null)
        {
            m_Random = new Random();
        }
    }

    public static void ResetRandomNumber(int seed)
    {
        m_Random = new Random(seed);
    }

    public static int NextInt(int min, int max)
    {
        InitializeRandomNumber();
        return m_Random.Next(min, max);
    }

    public static int NextInt(int max)
    {
        InitializeRandomNumber();
        return NextInt(0, max);
    }

    public static double NextDouble()
    {
        InitializeRandomNumber();
        return m_Random.NextDouble();
    }

    public static float NextFloat()
    {
        InitializeRandomNumber();
        return (float)m_Random.NextDouble();
    }

    public static float NextFloat(float min, float max)
    {
        InitializeRandomNumber();
        return (float)(min + m_Random.NextDouble() * (max - min));
    }
}