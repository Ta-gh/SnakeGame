using System;
using System.Collections.Generic; // Добавили для работы со списками List
using System.Threading;

class Program
{
    static void Main()
    {
        Console.Title = "Змейка";
        Console.CursorVisible = false;
        Console.SetWindowSize(80, 25);
        Console.SetBufferSize(80, 25);

        // === ИЗМЕНЕНИЯ ЗДЕСЬ ===
        // Теперь змейка - это список координат
        // В C# есть специальный тип для хранения двух чисел - кортеж (int, int)
        List<(int X, int Y)> snake = new List<(int, int)>();
        
        // Добавляем голову (первый элемент списка)
        // Индекс 0 - это голова
        snake.Add((40, 12));
        
        // Добавим немного хвоста для начала, чтобы было видно
        // Хвост будет из двух сегментов за головой
        snake.Add((39, 12)); // Слева от головы
        snake.Add((38, 12)); // Еще левее
        // ======================

        int directionX = 1;
        int directionY = 0;

        while (true)
        {
            // Управление (без изменений)
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow: directionX = 0; directionY = -1; break;
                    case ConsoleKey.DownArrow: directionX = 0; directionY = 1; break;
                    case ConsoleKey.LeftArrow: directionX = -1; directionY = 0; break;
                    case ConsoleKey.RightArrow: directionX = 1; directionY = 0; break;
                }
            }

            // === ИЗМЕНЕНИЯ В ЛОГИКЕ ===
            // Получаем текущую голову (первый элемент списка)
            var head = snake[0];
            
            // Вычисляем, где будет новая голова
            int newHeadX = head.X + directionX;
            int newHeadY = head.Y + directionY;
            
            // Вставляем новую голову в начало списка
            // Insert(0, ...) - вставить на позицию 0 (в самое начало)
            snake.Insert(0, (newHeadX, newHeadY));
            
            // Удаляем последний элемент хвоста
            // snake.Count - это длина списка
            // snake.Count - 1 - это индекс последнего элемента
            snake.RemoveAt(snake.Count - 1);
            // =========================

            Console.Clear();

            // Рисуем границы (код тот же)
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write('#');
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write('#');
            }
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write('#');
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write('#');
            }

            // === ИЗМЕНЕНИЯ В ОТРИСОВКЕ ===
            // Рисуем всю змейку в цикле
            // Переменная segment будет по очереди принимать значения всех элементов списка
            foreach (var segment in snake)
            {
                Console.SetCursorPosition(segment.X, segment.Y);
                Console.Write('O'); // Каждый сегмент рисуем как O
            }
            // ===========================

            Thread.Sleep(100);
        }
    }
}
