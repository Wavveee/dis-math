using System;
using System.Text;
using System.Numerics; // Для роботи з великими числами (BigInteger)

namespace DiscreteMathProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

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
                    default: Console.WriteLine("Невірний вибір. Спробуйте ще раз."); break;
                }
            }
        }

        // --- ЗАВДАННЯ 1 ---
        static void Task1()
        {
            Console.WriteLine("\n--- Завдання 1: Розклад (ax + by)^n ---");
            int a = GetInput("Введіть a: ");
            int b = GetInput("Введіть b: ");
            int n = GetInput("Введіть n: ");

            StringBuilder polynomial = new StringBuilder();
            Console.WriteLine("\n[Покрокові обчислення]:");

            for (int k = 0; k <= n; k++)
            {
                int powerX = n - k;
                int powerY = k;

                BigInteger binomCoeff = BinomialCoefficient(n, k);
                BigInteger aPow = BigInteger.Pow(a, powerX);
                BigInteger bPow = BigInteger.Pow(b, powerY);
                BigInteger termCoeff = binomCoeff * aPow * bPow;

                // Вивід кроку
                Console.WriteLine($"k={k}: x^{powerX} y^{powerY} | C({n},{k})={binomCoeff} * {a}^{powerX}(={aPow}) * {b}^{powerY}(={bPow}) = {termCoeff}");

                if (termCoeff >= 0 && k > 0) polynomial.Append(" + ");
                polynomial.Append($"{termCoeff}");
                if (powerX > 0) polynomial.Append($"*x^{powerX}");
                if (powerY > 0) polynomial.Append($"*y^{powerY}");
            }
            Console.WriteLine($"\nРЕЗУЛЬТАТ: {polynomial}");
        }

        // --- ЗАВДАННЯ 2 ---
        static void Task2()
        {
            Console.WriteLine("\n--- Завдання 2: Коефіцієнт біля x^j * y^(n-j) ---");
            int a = GetInput("Введіть a: ");
            int b = GetInput("Введіть b: ");
            int n = GetInput("Введіть n: ");
            int j = GetInput("Введіть j (степінь x): ");

            int powerY = n - j;
            if (powerY < 0) { Console.WriteLine("Помилка: j не може бути більше n"); return; }

            Console.WriteLine($"\nШукаємо член для степенів: x^{j} * y^{powerY}");
            
            BigInteger C_n_j = BinomialCoefficient(n, j);
            BigInteger aPow = BigInteger.Pow(a, j);
            BigInteger bPow = BigInteger.Pow(b, powerY);
            BigInteger result = C_n_j * aPow * bPow;

            Console.WriteLine("--- Деталі ---");
            Console.WriteLine($"1. Біноміальний коеф C({n},{j}): {C_n_j}");
            Console.WriteLine($"2. Частина a: {a}^{j} = {aPow}");
            Console.WriteLine($"3. Частина b: {b}^{powerY} = {bPow}");
            Console.WriteLine($"4. Добуток: {C_n_j} * {aPow} * {bPow} = {result}");
            
            Console.WriteLine($"\nВІДПОВІДЬ: {result}");
        }

        // --- ЗАВДАННЯ 3 (ОНОВЛЕНО З ДЕТАЛЯМИ) ---
        static void Task3()
        {
            Console.WriteLine("\n--- Завдання 3: Розклад (ax + by + cz)^n ---");
            int a = GetInput("Введіть a: ");
            int b = GetInput("Введіть b: ");
            int c = GetInput("Введіть c: ");
            int n = GetInput("Введіть n: ");

            StringBuilder polynomial = new StringBuilder();
            Console.WriteLine("\n[Починаємо перебір степенів i, j, k (сума = n)]");
            
            bool first = true;
            BigInteger nFact = Factorial(n); // Рахуємо n! один раз, щоб не повторювати

            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= n - i; j++)
                {
                    int k = n - i - j;
                    
                    Console.WriteLine($"\n--> Комбінація: x^{i}, y^{j}, z^{k}");

                    // 1. Факторіали
                    BigInteger iFact = Factorial(i);
                    BigInteger jFact = Factorial(j);
                    BigInteger kFact = Factorial(k);
                    
                    // 2. Мультиноміальний коефіцієнт P = n! / (i!j!k!)
                    BigInteger denom = iFact * jFact * kFact;
                    BigInteger multinomCoeff = nFact / denom;
                    
                    Console.Write($"   Мульти-коеф: {n}!/({i}!{j}!{k}!) = {nFact}/{denom} = {multinomCoeff}");

                    // 3. Степені чисел
                    BigInteger aPow = BigInteger.Pow(a, i);
                    BigInteger bPow = BigInteger.Pow(b, j);
                    BigInteger cPow = BigInteger.Pow(c, k);

                    // 4. Фінальний коефіцієнт
                    BigInteger termCoeff = multinomCoeff * aPow * bPow * cPow;
                    Console.WriteLine($" | З урахуванням a,b,c: {multinomCoeff} * {a}^{i} * {b}^{j} * {c}^{k} = {termCoeff}");

                    // Додавання до рядка
                    if (!first && termCoeff >= 0) polynomial.Append(" + ");
                    polynomial.Append($"{termCoeff}");
                    if (i > 0) polynomial.Append($"*x^{i}");
                    if (j > 0) polynomial.Append($"*y^{j}");
                    if (k > 0) polynomial.Append($"*z^{k}");
                    
                    first = false;
                }
            }
            Console.WriteLine($"\nРЕЗУЛЬТАТ (ПОЛІНОМ): {polynomial}");
        }

        // --- ЗАВДАННЯ 4 (ОНОВЛЕНО З ДЕТАЛЯМИ) ---
        static void Task4()
        {
            Console.WriteLine("\n--- Завдання 4: Коефіцієнт біля x^i * y^j * z^k ---");
            int a = GetInput("Введіть a: ");
            int b = GetInput("Введіть b: ");
            int c = GetInput("Введіть c: ");
            int n = GetInput("Введіть n: ");
            
            Console.WriteLine("Введіть степені для x, y, z:");
            int i = GetInput("i (степінь x): ");
            int j = GetInput("j (степінь y): ");
            int k = GetInput("k (степінь z): ");

            Console.WriteLine("\n--- Етап 1: Перевірка умови ---");
            if (i + j + k != n)
            {
                Console.WriteLine($"Помилка! Сума степенів {i}+{j}+{k} = {i + j + k}, а має дорівнювати n={n}.");
                Console.WriteLine("Такого члена в розкладі не існує (коефіцієнт 0).");
                return;
            }
            Console.WriteLine("Умова виконана. Починаємо розрахунок.");

            Console.WriteLine("\n--- Етап 2: Факторіали ---");
            BigInteger nFact = Factorial(n);
            BigInteger iFact = Factorial(i);
            BigInteger jFact = Factorial(j);
            BigInteger kFact = Factorial(k);

            Console.WriteLine($"n! ({n}!) = {nFact}");
            Console.WriteLine($"i! ({i}!) = {iFact}");
            Console.WriteLine($"j! ({j}!) = {jFact}");
            Console.WriteLine($"k! ({k}!) = {kFact}");

            Console.WriteLine("\n--- Етап 3: Мультиноміальний коефіцієнт ---");
            // Формула з файлу стор. 3: P = n! / (n1! n2! n3!)
            BigInteger denominator = iFact * jFact * kFact;
            BigInteger multiCoeff = nFact / denominator;
            Console.WriteLine($"P = {nFact} / ({iFact} * {jFact} * {kFact})");
            Console.WriteLine($"P = {nFact} / {denominator} = {multiCoeff}");

            Console.WriteLine("\n--- Етап 4: Врахування констант a, b, c ---");
            BigInteger aPow = BigInteger.Pow(a, i);
            BigInteger bPow = BigInteger.Pow(b, j);
            BigInteger cPow = BigInteger.Pow(c, k);
            Console.WriteLine($"a^{i} = {aPow}");
            Console.WriteLine($"b^{j} = {bPow}");
            Console.WriteLine($"c^{k} = {cPow}");

            Console.WriteLine("\n--- Етап 5: Фінальний результат ---");
            BigInteger result = multiCoeff * aPow * bPow * cPow;
            Console.WriteLine($"Коефіцієнт = {multiCoeff} * {aPow} * {bPow} * {cPow}");
            Console.WriteLine($"ВІДПОВІДЬ: {result}");
        }

        // --- Допоміжні функції ---
        static int GetInput(string text)
        {
            Console.Write(text);
            string s = Console.ReadLine();
            if (int.TryParse(s, out int val)) return val;
            return 0; // Якщо ввели не число
        }

        static BigInteger Factorial(int num)
        {
            if (num <= 1) return 1;
            BigInteger result = 1;
            for (int i = 2; i <= num; i++) result *= i;
            return result;
        }

        static BigInteger BinomialCoefficient(int n, int k)
        {
            if (k < 0 || k > n) return 0;
            return Factorial(n) / (Factorial(k) * Factorial(n - k));
        }
    }
}