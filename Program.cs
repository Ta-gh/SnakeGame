using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.Title = "Змейка";
        Console.CursorVisible = false;
        Console.SetWindowSize(80, 25);
        Console.SetBufferSize(80, 25);
        
        // Создаем генератор случайных чисел
        Random random = new Random();
        
        // Змейка
        List<(int X, int Y)> snake = new List<(int, int)>();
        snake.Add((40, 12));
        snake.Add((39, 12));
        snake.Add((38, 12));
        
        // ЕДА
        int foodX;
        int foodY;
        
        // Генерируем еду
        GenerateFood(random, snake, out foodX, out foodY);
        
        int directionX = 1;
        int directionY = 0;
        
        while (true)
        {
            // Управление
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
            
            // Логика движения
            var head = snake[0];
            int newHeadX = head.X + directionX;
            int newHeadY = head.Y + directionY;
            
            // === ПРОВЕРКА СТОЛКНОВЕНИЯ С ГРАНИЦАМИ ===
            if (newHeadX <= 0 || newHeadX >= Console.WindowWidth - 1 ||
                newHeadY <= 0 || newHeadY >= Console.WindowHeight - 1)
            {
                // Игра окончена
                Console.Clear();
                Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2);
                Console.WriteLine("GAME OVER! Нажмите любую клавишу...");
                Console.ReadKey();
                break;
            }
            
            // === ПРОВЕРКА СТОЛКНОВЕНИЯ С СОБОЙ ===
            foreach (var segment in snake)
            {
                if (segment.X == newHeadX && segment.Y == newHeadY)
                {
                    Console.Clear();
                    Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2);
                    Console.WriteLine("GAME OVER! Нажмите любую клавишу...");
                    Console.ReadKey();
                    return;
                }
            }
            
            snake.Insert(0, (newHeadX, newHeadY));
            
            // Проверяем, съели ли мы еду
            if (newHeadX == foodX && newHeadY == foodY)
            {
                // Съели еду - не удаляем хвост
                GenerateFood(random, snake, out foodX, out foodY);
            }
            else
            {
                // Не съели - удаляем хвост
                snake.RemoveAt(snake.Count - 1);
            }
            
            // Отрисовка
            Console.Clear();
            
            // Рисуем границы
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
            
            // Рисуем змейку
            foreach (var segment in snake)
            {
                // Проверяем, что координаты в пределах окна
                if (segment.X >= 0 && segment.X < Console.WindowWidth &&
                    segment.Y >= 0 && segment.Y < Console.WindowHeight)
                {
                    Console.SetCursorPosition(segment.X, segment.Y);
                    Console.Write('O');
                }
            }
            
            // Рисуем еду
            if (foodX > 0 && foodX < Console.WindowWidth - 1 &&
                foodY > 0 && foodY < Console.WindowHeight - 1)
            {
                Console.SetCursorPosition(foodX, foodY);
                Console.Write('@');
            }
            
            Thread.Sleep(100);
        }
    }
    
    // Метод генерации еды
    static void GenerateFood(Random random, List<(int X, int Y)> snake, out int foodX, out int foodY)
    {
        bool isOnSnake;
        do
        {
            foodX = random.Next(1, Console.WindowWidth - 1);
            foodY = random.Next(1, Console.WindowHeight - 1);
             
            isOnSnake = false;
            foreach (var segment in snake)
            {
                if (segment.X == foodX && segment.Y == foodY)
                {
                    isOnSnake = true;
                    break;
                }
            }
        } while (isOnSnake);
    }
}