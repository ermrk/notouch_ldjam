using System;
using System.Collections;

public static class RandomGen
{

    private static readonly Random getrandom = new Random();
    private static readonly object syncLock = new object();

    public static float GetRandomNumber(float minimum, float maximum)
    {
        lock (syncLock)
        { // synchronize
            return (float)getrandom.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
