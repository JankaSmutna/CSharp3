
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;

public class Greed
{
    public void SingleRollWithFiveDice()
    {
        //Vytvoření jednoho hodu 5 kostkami:
        int[] fiveDiceSingleRoll = new int[5];

        for (int i = 0; i < fiveDiceSingleRoll.Length; i++)
        {
            var number = new Random();
            fiveDiceSingleRoll[i] = number.Next(1, 7);
        }

        //Výpis padlých čísel a počtu jejich výskytů, finální skórování:
        var diceDots = fiveDiceSingleRoll.GroupBy(i => i);
        int finalScore = 0;

        foreach (var numberOfDots in diceDots)
        {
            int dots = numberOfDots.Key;
            int count = numberOfDots.Count();
            int score;
            Console.WriteLine("Number of dots: {0}, Occurence: {1}", dots, count);

            switch (dots)
            {
                case 1:
                    if (count > 0 & count < 3)
                    {
                        score = count * 100;
                    }
                    else if (count == 3)
                    {
                        score = 1000;
                    }
                    else if (count > 3)
                    {
                        score = 1000 + ((count - 3) * 100);
                    }
                    else
                    {
                        score = 0;
                    }
                    finalScore += score;
                    break;
                case 2:
                    if (count > 2)
                    {
                        score = 200;
                    }
                    else
                    {
                        score = 0;
                    }
                    finalScore += score;
                    break;
                case 3:
                    if (count > 2)
                    {
                        score = 300;
                    }
                    else
                    {
                        score = 0;
                    }
                    finalScore += score;
                    break;
                case 4:
                    if (count > 2)
                    {
                        score = 400;
                    }
                    else
                    {
                        score = 0;
                    }
                    finalScore += score;
                    break;
                case 5:
                    if (count > 0 & count < 3)
                    {
                        score = count * 50;
                    }
                    else if (count == 3)
                    {
                        score = 500;
                    }
                    else if (count > 3)
                    {
                        score = 500 + ((count - 3) * 50);
                    }
                    else
                    {
                        score = 0;
                    }
                    finalScore += score;
                    break;
                case 6:
                    if (count > 2)
                    {
                        score = 600;
                    }
                    else
                    {
                        score = 0;
                    }
                    finalScore += score;
                    break;
                default:
                    score = 0;
                    finalScore += score;
                    break;
            }
        }

        Console.WriteLine("The final score is: " + finalScore);
    }
}

