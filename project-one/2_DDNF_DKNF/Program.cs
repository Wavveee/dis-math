using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        
        // 1. Задаємо ТРИМІСНУ БУЛЕВУ ФУНКЦІЮ таблично 
        // значення F для наборів 000, 001, 010, 011, 100, 101, 110, 111
        // F = {1, 0, 0, 1, 0, 1, 1, 0}
        // F = 1 для наборів: (000), (011), (101), (110)
        // F = 0 для наборів: (001), (010), (100), (111)
        int[] fixedFunctionValues = new int[] { 1, 0, 0, 1, 0, 1, 1, 0 }; 

        var dnfparts = new List<string>();
        var cnfparts = new List<string>();

        Console.WriteLine("--- Таблиця Істинності F(a, b, c) ---");
        Console.WriteLine(" a | b | c | F ");
        Console.WriteLine("---|---|---|---");

        for (int i = 0; i < 8; i++)
        {
            // Визначення значень змінних a, b, c
            bool a = (i & 4) != 0;
            bool b = (i & 2) != 0;
            bool c = (i & 1) != 0;

            // Отримання значення функції F із нашого фіксованого масиву
            bool value = (fixedFunctionValues[i] == 1);

            // Виведення рядка таблиці
           Console.WriteLine($" {(a ? 1 : 0)} | {(b ? 1 : 0)} | {(c ? 1 : 0)} | {fixedFunctionValues[i]} ");

            // Логіка для ДДНФ (F=1) та ДКНФ (F=0)
            if (value)
            {
                // Формування мінтерма для ДДНФ
                string term = "(" +
                              (a ? "a" : "¬a") + " ∧ " +
                              (b ? "b" : "¬b") + " ∧ " +
                              (c ? "c" : "¬c") + ")";
                dnfparts.Add(term);
            }
            else
            {
                // Формування макстерма для ДКНФ
                string clause = "(" +
                                (a ? "¬a" : "a") + " ∨ " +
                                (b ? "¬b" : "b") + " ∨ " +
                                (c ? "¬c" : "c") + ")";
                cnfparts.Add(clause);
            }
        }
        
        // Формування фінальних рядків ДДНФ
        string dnf = dnfparts.Count > 0 ? string.Join(" ∨ ", dnfparts) : "0";
        
        // Формування фінальних рядків ДКНФ
        string cnf = cnfparts.Count > 0 ? string.Join(" ∧ ", cnfparts) : "1";

        Console.WriteLine();
        Console.WriteLine("--- Результат Аналізу ---");
        Console.WriteLine("2. ДДНФ (Диз'юнктивна досконала нормальна форма):");
        Console.WriteLine("Y = " + dnf);
        Console.WriteLine();
        Console.WriteLine("3. ДКНФ (Кон'юнктивна досконала нормальна форма):");
        Console.WriteLine("Y = " + cnf);
    }
}