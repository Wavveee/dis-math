using System;
using System.Text;

class Program
{
    // Глобальне полотно для малювання ASCII-схеми
    static char[,] canvas;
    static int rows = 16;
    static int cols = 80;


    /// Вставляє рядок на полотно у заданій позиції (r - рядок, c - стовпець).
    static void Put(int r, int c, string s)
    {
        if (r < 0 || r >= rows) return;
        for (int i = 0; i < s.Length; i++)
        {
            int cc = c + i;
            if (cc >= 0 && cc < cols) canvas[r, cc] = s[i];
        }
    }


    /// Малює блок паралельно з'єднаних контактів на полотні.

    /// <param name="y_mid">Рядок, що відповідає центральній лінії.</param>
    /// <param name="start_c">Початковий стовпець блоку.</param>
    /// <param name="label_top">Мітка для верхнього (нормально розімкненого) контакту.</param>
    /// <param name="label_bottom">Мітка для нижнього (нормально розімкненого) контакту.</param>
    static void DrawParallelBlock(int y_mid, int start_c, string label_top, string label_bottom)
    {
        int y_top = y_mid - 2;
        int y_bottom = y_mid + 2;
        int contact_c = start_c + 5;
        int end_c = start_c + 12;

        // Вертикальні лінії (шини)
        for (int r = y_top; r <= y_bottom; r++)
        {
            Put(r, start_c, "|");
            Put(r, end_c, "|");
        }

        // Верхній контакт (паралель)
        // [ ] позначає нормально розімкнений контакт
        Put(y_top, start_c + 1, "----[ ]----");
        // Мітка контакту (над ним)
        Put(y_top - 1, contact_c + 1, label_top);

        // Нижній контакт (паралель)
        Put(y_bottom, start_c + 1, "----[ ]----");
        // Мітка контакту (над ним)
        Put(y_bottom - 1, contact_c + 1, label_bottom);
    }


    static void Main()
    {
        //кодування для коректного відображення укр символів
        Console.OutputEncoding = Encoding.UTF8;

        // Ініціалізація полотна
        canvas = new char[rows, cols];
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                canvas[r, c] = ' ';

        // 1. Оновлення заголовка для нової формули
        Put(0, 0, "Контактна схема для (ā+b)(a+c)(b+c)");
        int midY = 8;

        // Координати початку блоків
        int block1_start = 10;
        int block2_start = 30;
        int block3_start = 50;
        
        // 2. Оновлення блоків згідно з новою формулою: Y = (ā+b) ∧ (a+c) ∧ (b+c)
        
        // Блок 1: (ā+b)
        // !a - це нормально замкнений контакт. У даній ASCII-схемі
        // використовуємо '!' для позначення інверсії, що означає нормально замкнений контакт.
        DrawParallelBlock(midY, block1_start, "!a", "b"); 

        // Блок 2: (a+c)
        DrawParallelBlock(midY, block2_start, "a", "c");  

        // Блок 3: (b+c)
        DrawParallelBlock(midY, block3_start, "b", "c");  

        // З'єднувальні лінії між блоками (послідовне з'єднання)
        Put(midY, 0, "Вхід -----");
        Put(midY, 22, "--------");
        Put(midY, 42, "--------");
        Put(midY, 62, "---- Вихід");

        // Виведення полотна на екран
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
                Console.Write(canvas[r, c]);
            Console.WriteLine();
        }

        Console.WriteLine("\n\nАналіз схеми та таблиця істинності");
        // Оновлення формули
        Console.WriteLine("Формула: Y = (¬a ∨ b) ∧ (a ∨ c) ∧ (b ∨ c)");
        
        // Оновлення заголовків стовпців
        Console.WriteLine("----------------------------------------------------------------");
        Console.WriteLine(" a | b | c | (ā+b) | (a+c) | (b+c) |     (ā+b)(a+c)(b+c)     |  Стан ланцюга");
        Console.WriteLine("----------------------------------------------------------------");

        for (int a_val = 0; a_val <= 1; a_val++)
        for (int b_val = 0; b_val <= 1; b_val++)
        for (int c_val = 0; c_val <= 1; c_val++)
        {
            bool a = a_val == 1;
            bool b = b_val == 1;
            bool c = c_val == 1;

            // 3. Оновлення логіки обчислень відповідно до нової формули
            bool P1 = !a || b;   // (ā+b)
            bool P2 = a || c;    // (a+c)
            bool P3 = b || c;    // (b+c)

            bool Y = P1 && P2 && P3; 
            
            // Перетворення булевих значень на 0 або 1
            int res_P1 = P1 ? 1 : 0;
            int res_P2 = P2 ? 1 : 0;
            int res_P3 = P3 ? 1 : 0;
            int result_Y = Y ? 1 : 0;
            
            Console.WriteLine($" {a_val} | {b_val} | {c_val} |   {res_P1}   |   {res_P2}   |   {res_P3}   |           {result_Y}           |  {(result_Y == 1 ? "замкнений" : "розімкнений")} " );
        }
        Console.WriteLine("----------------------------------------------------------------");
    }
}