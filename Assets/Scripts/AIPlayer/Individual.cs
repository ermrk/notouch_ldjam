using System;
using System.Collections;
using System.Globalization;

public class Individual
{
    string code;
    float score;

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
        int numberToChange = (int)RandomGen.GetRandomNumber(0, weights.Length);
        for (int i = 0; i < numberToChange; i++)
        {
            int index = (int)RandomGen.GetRandomNumber(0, weights.Length);
            string weightString = weights[index];
            try
            {
                float weight = float.Parse(weightString, CultureInfo.InvariantCulture.NumberFormat);
                float random = RandomGen.GetRandomNumber(-1, 1);
                if (random < 0)
                {
                    float sign = RandomGen.GetRandomNumber(-1, 1);
                    if (sign < 0)
                    {
                        weight *= -RandomGen.GetRandomNumber(0.75f, 1.25f);
                    }
                    else
                    {
                        weight *= RandomGen.GetRandomNumber(0.75f, 1.25f);
                    }
                }
                else {
                    weight += RandomGen.GetRandomNumber(-0.1f, 0.1f);
                }
                weights[index] = weight.ToString();
            }
            catch (Exception e) {
                weights[index] = 1.ToString();
            }
        }
        code = "";
        for (int i = 0; i < weights.Length; i++)
        {
            code += weights[i] + "_";
        }
    }
}
