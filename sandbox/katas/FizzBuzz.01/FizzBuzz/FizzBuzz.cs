
public class FizzBuzz
{
    /*definice metody, která počítá od 1 do 100 a
    -namísto každého čísla dělitelného 3 vypíše "Fizz" a
    -namísto čísla dělitelného 5 vypíše "Buzz" a
    -namísto čísla dělitelného 3 a 5 vypíše "FizzBuzz"*/
    public void OnCount(int lastNumber)
    {
        for (int actualNumber = 1; actualNumber <= lastNumber; actualNumber++)
        {
            if (actualNumber % 3 == 0 && actualNumber % 5 == 0)
            {
                Console.WriteLine("FizzBuzz");
            }

            else if (actualNumber % 3 == 0)
            {
                Console.WriteLine("Fizz");
            }

            else if (actualNumber % 5 == 0)
            {
                Console.WriteLine("Buzz");
            }

            else
            {
                Console.WriteLine(actualNumber);
            }
        }
    }
}
