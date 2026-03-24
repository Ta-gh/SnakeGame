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

        Random random = new Random();
        
        // Начальные параметры игры
        int score = 0; // Счет
        
        // Змейка (начинается с 3 сегментов)
        List<(int X, int Y)> snake = new List<(int, int)>();
        snake.Add((40, 12));
        snake.Add((39, 12));
        snake.Add((38, 12));

        // Генерируем первую еду
        int foodX, foodY;
        GenerateFood(random, snake, out foodX, out foodY);

        int directionX = 1;
        int directionY = 0;

        // Игровой цикл
        bool gameOver = false; // Флаг окончания игры
        
        while (!gameOver) // Играем, пока gameOver == false
        {
            // === УПРАВЛЕНИЕ ===
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        // Нельзя разворачиваться в обратную сторону
                        if (directionY != 1) // Если не движемся вниз
                        {
                            directionX = 0;
                            directionY = -1;
                        }
                        break;
                        
                    case ConsoleKey.DownArrow:
                        if (directionY != -1) // Если не движемся вверх
                        {
                            directionX = 0;
                            directionY = 1;
                        }
                        break;
                        
                    case ConsoleKey.LeftArrow:
                        if (directionX != 1) // Если не движемся вправо
                        {
                            directionX = -1;
                            directionY = 0;
                        }
                        break;
                        
                    case ConsoleKey.RightArrow:
                        if (directionX != -1) // Если не движемся влево
                        {
                            directionX = 1;
                            directionY = 0;
                        }
                        break;
                }
            }

            // === ДВИЖЕНИЕ ===
            var head = snake[0];
            int newHeadX = head.X + directionX;
            int newHeadY = head.Y + directionY;
            
            // === ПРОВЕРКА НА ПРОИГРЫШ ===
            // Проверка столкновения со стенами
            if (newHeadX <= 0 || newHeadX >= Console.WindowWidth - 1 ||
                newHeadY <= 0 || newHeadY >= Console.WindowHeight - 1)
            {
                gameOver = true;
                break; // Выходим из цикла while
            }
            
            // Проверка столкновения с собственным хвостом
            // Проверяем, не совпадает ли новая голова с каким-либо сегментом змейки
            foreach (var segment in snake)
            {
                if (segment.X == newHeadX && segment.Y == newHeadY)
                {
                    gameOver = true;
                    break;
                }
            }
            
            if (gameOver) break; // Если проиграли, выходим
            
            // Вставляем новую голову
            snake.Insert(0, (newHeadX, newHeadY));
            
            // === ПРОВЕРКА ЕДЫ ===
            if (newHeadX == foodX && newHeadY == foodY)
            {
                score++; // Увеличиваем счет
                GenerateFood(random, snake, out foodX, out foodY);
                // НЕ удаляем хвост - змейка растет
            }
            else
            {
                // Удаляем хвост
                snake.RemoveAt(snake.Count - 1);
            }

            // === ОТРИСОВКА ===
            Console.Clear();

            // Рисуем счет
            Console.SetCursorPosition(2, 1);
            Console.Write($"Score: {score}");

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
                Console.SetCursorPosition(segment.X, segment.Y);
                Console.Write('O');
            }

            // Рисуем еду
            Console.SetCursorPosition(foodX, foodY);
            Console.Write('@');

            Thread.Sleep(100);
        }

        // === КОНЕЦ ИГРЫ ===
        Console.Clear();
        Console.SetCursorPosition(35, 12);
        Console.WriteLine("GAME OVER!");
        Console.SetCursorPosition(33, 13);
        Console.WriteLine($"Your score: {score}");
        Console.SetCursorPosition(30, 14);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
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
