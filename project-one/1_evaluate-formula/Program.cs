using System;

public class Program
{
    // Функція для перетворення boolean в 0 або 1 для виводу
    static int ToBit(bool value) => value ? 1 : 0;

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Формула: (НЕ a АБО b) ТОДІ І ТІЛЬКИ ТОДІ (a І НЕ c)
        string header = " a b c | НЕ(a) | НЕ(c) | (НЕ(a) ∨ b) | (a ∧ НЕ(c)) | (НЕ(a) ∨ b) ⇔ (a ∧ НЕ(c))";
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        bool allResultsTrue = true;
        bool atLeastOneTrue = false;

        // Перебір усіх 8 комбінацій для a, b, c (0, 1)
        for (int aVal = 0; aVal <= 1; aVal++)
        for (int bVal = 0; bVal <= 1; bVal++)
        for (int cVal = 0; cVal <= 1; cVal++)
        {
            // Перетворення цілих чисел на логічні змінні
            bool a = aVal == 1;
            bool b = bVal == 1;
            bool c = cVal == 1;

            // Проміжні заперечення
            bool not_a = !a;
            bool not_c = !c;
            
            // 1. Ліва частина: (НЕ a) ∨ b
            bool part_left = not_a || b;
            
            // 2. Права частина: a ∧ (НЕ c)
            bool part_right = a && not_c;
            
            // 3. Кінцевий результат: Ліва ⇔ Права
            bool finalResult = (part_left == part_right);
            
            // Вивід рядка таблиці істинності
            Console.WriteLine($" {ToBit(a)} {ToBit(b)} {ToBit(c)} |  {ToBit(not_a)}  |  {ToBit(not_c)}  |      {ToBit(part_left)}      |      {ToBit(part_right)}     |                {ToBit(finalResult)}");
            
            // Оновлення статусу формули
            if (finalResult)
            {
                atLeastOneTrue = true;
            }
            else
            {
                allResultsTrue = false;
            }
        }

        Console.WriteLine(new string('-', header.Length));
        Console.WriteLine("\nОцінка формули:");

        if (allResultsTrue)
        {
            Console.WriteLine("Формула є тавтологією та виконуваною (завжди істинна).");
        }
        else if (atLeastOneTrue)
        {
            Console.WriteLine("Формула є виконуваною (нейтральною) (містить 0 та 1).");
        }
        else
        {
            Console.WriteLine("Формула є суперечністю та НЕвиконуваною (завжди хибна).");
        }
    }
}