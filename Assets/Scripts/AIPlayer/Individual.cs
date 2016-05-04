using System;
using System.Collections;
using System.Globalization;

public class Individual
{
    string code;
    float score;
    Random random = new Random();

    public void setCode(string code) {
        this.code = code;
    }

    public string getCode()
    {
        return code;
    }

    public void setScore(float score)
    {
        this.score = score;
    }

    public float getScore() {
        return score;
    }

    public void mutate()
    {
        string[] weights = code.Split('_');
        int numberToChange = (int)GetRandomNumber(0, weights.Length);
        for (int i = 0; i < numberToChange; i++)
        {
            int index = (int)GetRandomNumber(0, weights.Length);
            string weightString = weights[index];
            try
            {
                float weight = float.Parse(weightString, CultureInfo.InvariantCulture.NumberFormat);
                float random = GetRandomNumber(-1, 1);
                if (random < 0)
                {
                    float sign = GetRandomNumber(-1, 1);
                    if (sign < 0)
                    {
                        weight *= -GetRandomNumber(0f, 2f);
                    }
                    else
                    {
                        weight *= GetRandomNumber(0f, 2f);
                    }
                }
                else {
                    weight += GetRandomNumber(-1f, 1f);
                }
                weights[index] = weight.ToString();
            }
            catch (Exception e) {
                weights[index] = 0.01f.ToString();
            }
        }
        code = "";
        for (int i = 0; i < weights.Length; i++)
        {
            code += weights[i] + "_";
        }
    }

    public float GetRandomNumber(float minimum, float maximum)
    {
        return (float)random.NextDouble() * (maximum - minimum) + minimum;
    }
}
