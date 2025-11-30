using System;
using System.Text;

namespace DiscreteMathProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; // Щоб коректно відображалась кирилиця

            while (true)
            {
                Console.WriteLine("\n=============================================");
                Console.WriteLine("Оберіть завдання (1-4) або 0 для виходу:");
                Console.WriteLine("1. Розклад бінома (ax + by)^n");
                Console.WriteLine("2. Знайти коефіцієнт біля x^j * y^(n-j) у (ax + by)^n");
                Console.WriteLine("3. Розклад полінома (ax + by + cz)^n");
                Console.WriteLine("4. Знайти коефіцієнт біля x^i * y^j * z^k у (ax + by + cz)^n");
                Console.WriteLine("=============================================");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": Task1(); break;
                    case "2": Task2(); break;
                    case "3": Task3(); break;
                    case "4": Task4(); break;
                    case "0": return;
                    default: Console.WriteLine("Невірний вибір, спробуйте ще раз."); break;
                }
            }
        }

        // --- ЗАВДАННЯ 1: Розклад бінома ---
        static void Task1()
        {
            Console.WriteLine("\n--- Завдання 1: Розклад (ax + by)^n ---");
            int a = GetInput("Введіть a: ");
            int b = GetInput("Введіть b: ");
            int n = GetInput("Введіть n (степінь): ");

            StringBuilder polynomial = new StringBuilder();
            Console.WriteLine("\n[Покрокові обчислення]:");

            // Формула: Сума від k=0 до n: C(n, k) * (ax)^(n-k) * (by)^k
            for (int k = 0; k <= n; k++)
            {
                int powerX = n - k;
                int powerY = k;

                // 1. Біноміальний коефіцієнт C(n, k)
                long binomCoeff = BinomialCoefficient(n, k);
                
                // 2. Коефіцієнти a і b у відповідних степенях
                long aPow = (long)Math.Pow(a, powerX);
                long bPow = (long)Math.Pow(b, powerY);

                // 3. Загальний коефіцієнт члена
                long termCoeff = binomCoeff * aPow * bPow;

                Console.WriteLine($"k={k}: C({n},{k})={binomCoeff} * {a}^{powerX}={aPow} * {b}^{powerY}={bPow} -> Коеф: {termCoeff}");

                // Формування рядка виводу
                if (termCoeff >= 0 && k > 0) polynomial.Append(" + ");
                polynomial.Append($"{termCoeff}");
                if (powerX > 0) polynomial.Append($"*x^{powerX}");
                if (powerY > 0) polynomial.Append($"*y^{powerY}");
            }

            Console.WriteLine($"\nРЕЗУЛЬТАТ: {polynomial}");
        }

        // --- ЗАВДАННЯ 2: Знайти коефіцієнт біля x^j * y^(n-j) ---
        static void Task2()
        {
            Console.WriteLine("\n--- Завдання 2: Коефіцієнт біля x^j * y^(n-j) ---");
            int a = GetInput("Введіть a: ");
            int b = GetInput("Введіть b: ");
            int n = GetInput("Введіть n: ");
            int j = GetInput("Введіть j (степінь x): ");

            int powerY = n - j;

            if (powerY < 0)
            {
                Console.WriteLine("Помилка: j не може бути більше n.");
                return;
            }

            Console.WriteLine($"\nШукаємо член з x^{j} * y^{powerY}");
            Console.WriteLine("[Обчислення]:");

            // Нам потрібен k, де степінь x (n-k) дорівнює j. Отже n-k = j => k = n-j.
            // Або можна просто взяти C(n, j), оскільки C(n, j) == C(n, n-j).
            // Формально член це: C(n, n-j) * (ax)^j * (by)^(n-j)
            
            long C_n_j = BinomialCoefficient(n, j); 
            long aPow = (long)Math.Pow(a, j);
            long bPow = (long)Math.Pow(b, powerY);

            Console.WriteLine($"1. C({n}, {j}) = {C_n_j}");
            Console.WriteLine($"2. a^{j} = {a}^{j} = {aPow}");
            Console.WriteLine($"3. b^{powerY} = {b}^{powerY} = {bPow}");
            
            long result = C_n_j * aPow * bPow;
            Console.WriteLine($"4. Множення: {C_n_j} * {aPow} * {bPow} = {result}");

            Console.WriteLine($"\nРЕЗУЛЬТАТ (Коефіцієнт): {result}");
        }

        // --- ЗАВДАННЯ 3: Розклад полінома (тринома) ---
        static void Task3()
        {
            Console.WriteLine("\n--- Завдання 3: Розклад (ax + by + cz)^n ---");
            int a = GetInput("Введіть a: ");
            int b = GetInput("Введіть b: ");
            int c = GetInput("Введіть c: ");
            int n = GetInput("Введіть n: ");

            StringBuilder polynomial = new StringBuilder();
            Console.WriteLine("\n[Покрокові обчислення]:");
            
            bool first = true;

            // Перебір усіх i, j, k таких, що i + j + k = n
            // Формула члена: (n! / (i!j!k!)) * (ax)^i * (by)^j * (cz)^k
            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= n - i; j++)
                {
                    int k = n - i - j;

                    // Мультиноміальний коефіцієнт: n! / (i! j! k!)
                    long multinomCoeff = Factorial(n) / (Factorial(i) * Factorial(j) * Factorial(k));
                    
                    long aPow = (long)Math.Pow(a, i);
                    long bPow = (long)Math.Pow(b, j);
                    long cPow = (long)Math.Pow(c, k);

                    long termCoeff = multinomCoeff * aPow * bPow * cPow;

                    Console.WriteLine($"i={i}, j={j}, k={k} -> {n}!/({i}!{j}!{k}!)={multinomCoeff} * {a}^{i}*{b}^{j}*{c}^{k} = {termCoeff}");

                    if (!first && termCoeff >= 0) polynomial.Append(" + ");
                    polynomial.Append($"{termCoeff}");
                    if (i > 0) polynomial.Append($"*x^{i}");
                    if (j > 0) polynomial.Append($"*y^{j}");
                    if (k > 0) polynomial.Append($"*z^{k}");

                    first = false;
                }
            }
            Console.WriteLine($"\nРЕЗУЛЬТАТ: {polynomial}");
        }

        // --- ЗАВДАННЯ 4: Коефіцієнт біля x^i y^j z^k ---
        static void Task4()
        {
            Console.WriteLine("\n--- Завдання 4: Коефіцієнт біля x^i * y^j * z^k ---");
            int a = GetInput("Введіть a: ");
            int b = GetInput("Введіть b: ");
            int c = GetInput("Введіть c: ");
            int n = GetInput("Введіть n: ");
            
            Console.WriteLine("Введіть степені i, j, k (сума має дорівнювати n):");
            int i = GetInput("i (степінь x): ");
            int j = GetInput("j (степінь y): ");
            int k = GetInput("k (степінь z): ");

            if (i + j + k != n)
            {
                Console.WriteLine($"Помилка: i+j+k ({i}+{j}+{k}={i+j+k}) не дорівнює n ({n}). Такого члена не існує в розкладі.");
                return;
            }

            Console.WriteLine("\n[Обчислення]:");
            // Формула: (n! / (i!j!k!)) * a^i * b^j * c^k

            long nFact = Factorial(n);
            long iFact = Factorial(i);
            long jFact = Factorial(j);
            long kFact = Factorial(k);
            
            long multiCoeff = nFact / (iFact * jFact * kFact);
            Console.WriteLine($"1. Мультиноміальний коеф: {n}! / ({i}!*{j}!*{k}!) = {nFact} / ({iFact}*{jFact}*{kFact}) = {multiCoeff}");

            long aPow = (long)Math.Pow(a, i);
            long bPow = (long)Math.Pow(b, j);
            long cPow = (long)Math.Pow(c, k);
            Console.WriteLine($"2. Параметри: a^{i}={aPow}, b^{j}={bPow}, c^{k}={cPow}");

            long result = multiCoeff * aPow * bPow * cPow;
            Console.WriteLine($"3. Фінальне множення: {multiCoeff} * {aPow} * {bPow} * {cPow} = {result}");

            Console.WriteLine($"\nРЕЗУЛЬТАТ (Коефіцієнт): {result}");
        }

        // --- ДОПОМІЖНІ ФУНКЦІЇ ---

        static int GetInput(string text)
        {
            Console.Write(text);
            return int.Parse(Console.ReadLine());
        }

        // Обчислення факторіалу
        static long Factorial(int num)
        {
            if (num <= 1) return 1;
            long result = 1;
            for (int i = 2; i <= num; i++) result *= i;
            return result;
        }

        // Обчислення біноміального коефіцієнта C(n, k) = n! / (k! * (n-k)!)
        static long BinomialCoefficient(int n, int k)
        {
            if (k < 0 || k > n) return 0;
            return Factorial(n) / (Factorial(k) * Factorial(n - k));
        }
    }
}