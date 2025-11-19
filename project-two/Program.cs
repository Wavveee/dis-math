using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Program
{
    static HashSet<string> InputSet(string name)
    {
        Console.Write($"Введіть елементи множини {name} (через пробіл): ");
        string input = Console.ReadLine();
        return new HashSet<string>(input.Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    static void PrintSet(string name, HashSet<string> set)
    {
        Console.Write($"{name} = {{ ");
        Console.Write(string.Join(", ", set.OrderBy(x => x)));
        Console.WriteLine(" }");
    }

    static HashSet<string> InputAndValidateSet(string name, HashSet<string> universalSet)
    {
        HashSet<string> currentSet;
        List<string> invalidElements;

        do
        {
            currentSet = InputSet(name);

            invalidElements = currentSet.Except(universalSet).ToList();

            if (invalidElements.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ПОМИЛКА: Елементи {{ {string.Join(", ", invalidElements)} }} відсутні в універсальній множині U.");
                Console.WriteLine($"Будь ласка, введіть множину {name} ще раз.");
                Console.ResetColor();
            }

        } while (invalidElements.Any());

        return currentSet;
    }

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;


        HashSet<string> U = InputSet("U ");

        HashSet<string> A = InputAndValidateSet("A", U);
        HashSet<string> B = InputAndValidateSet("B", U);
        HashSet<string> C = InputAndValidateSet("C", U);

        // 1. Різниця A і B (A \ B)
        HashSet<string> differenceAB = new HashSet<string>(A.Except(B));
        // повертає елементи, які є в A, але відсутні в B.

        // 2. Різниця B і C (B \ C)
        HashSet<string> differenceBC = new HashSet<string>(B.Except(C));
        // елементи, які є в B, але відсутні в C.

        // 3. Об'єднання результатів (A \ B) ∪ (B \ C)
        HashSet<string> result = new HashSet<string>(differenceAB.Union(differenceBC));
        // отримані множини в одну.

        Console.WriteLine("\nРезультати:");
        PrintSet("U", U);
        PrintSet("A", A);
        PrintSet("B", B);
        PrintSet("C", C);
        Console.WriteLine("---");
        PrintSet("(A \\ B)", differenceAB);
        PrintSet("(B \\ C)", differenceBC);
        PrintSet("(A \\ B) ∪ (B \\ C)", result);
    }
}