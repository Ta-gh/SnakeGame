using System;
using System.Threading; // Нужен для задержки Thread.Sleep

class Program
{
    static void Main()
    {
        // 1. НАСТРОЙКА КОНСОЛИ
        Console.Title = "Змейка"; // Называем окно
        Console.CursorVisible = false; // Прячем курсор, чтобы не мешал
        Console.SetWindowSize(80, 25); // Устанавливаем размер окна (ширина, высота)
        Console.SetBufferSize(80, 25); // Размер буфера делаем таким же, чтобы не было скролла
        
        // 2. ПЕРЕМЕННЫЕ ДЛЯ ЗМЕЙКИ
        // Голова змейки будет иметь координаты X и Y
        // Ставим её примерно в центр экрана
        int headX = 40;
        int headY = 12;
        
        // Направление движения
        // По умолчанию движемся вправо: X увеличивается, Y не меняется
        int directionX = 1;
        int directionY = 0;
        
        // 3. ИГРОВОЙ ЦИКЛ
        // while (true) - значит "делать вечно"
        while (true)
        {
            // === ЧАСТЬ 1: УПРАВЛЕНИЕ ===
            // Проверяем, нажал ли пользователь клавишу
            if (Console.KeyAvailable)
            {
                // Считываем нажатую клавишу (true - не показывать её в консоли)
                ConsoleKeyInfo key = Console.ReadKey(true);
                
                // Определяем, какая стрелка нажата
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        // Запрещаем движение вверх, если движемся вниз
                        if (directionY != 1)
                        {
                            directionX = 0;   // По X не движемся
                            directionY = -1;  // Движемся вверх (Y уменьшается)
                        }
                        break;
                        
                    case ConsoleKey.DownArrow:
                        // Запрещаем движение вниз, если движемся вверх
                        if (directionY != -1)
                        {
                            directionX = 0;
                            directionY = 1;   // Движемся вниз (Y увеличивается)
                        }
                        break;
                        
                    case ConsoleKey.LeftArrow:
                        // Запрещаем движение влево, если движемся вправо
                        if (directionX != 1)
                        {
                            directionX = -1;  // Движемся влево (X уменьшается)
                            directionY = 0;
                        }
                        break;
                        
                    case ConsoleKey.RightArrow:
                        // Запрещаем движение вправо, если движемся влево
                        if (directionX != -1)
                        {
                            directionX = 1;   // Движемся вправо (X увеличивается)
                            directionY = 0;
                        }
                        break;
                        
                    case ConsoleKey.Escape:
                        // Выход из игры по нажатию Escape
                        return;
                }
            }
            
            // === ЧАСТЬ 2: ДВИЖЕНИЕ ===
            // Изменяем координаты головы согласно направлению
            headX = headX + directionX;
            headY = headY + directionY;
            
            // === ПРОВЕРКА СТОЛКНОВЕНИЯ СО СТЕНАМИ ===
            // Проверяем, не врезалась ли змейка в стены
            if (headX <= 0 || headX >= Console.WindowWidth - 1 || 
                headY <= 0 || headY >= Console.WindowHeight - 1)
            {
                // Игра окончена
                Console.Clear();
                Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2);
                Console.WriteLine("GAME OVER! Press any key to exit...");
                Console.ReadKey(true);
                break; // Выходим из игрового цикла
            }
            
            // === ЧАСТЬ 3: ОТРИСОВКА ===
            Console.Clear(); // Очищаем экран, чтобы старые позиции стерлись
            
            // --- Рисуем границы (рамку) ---
            // Верхняя и нижняя граница
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                // Верхняя граница (Y = 0)
                Console.SetCursorPosition(i, 0);
                Console.Write('#');
                
                // Нижняя граница (Y = высота экрана - 1)
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write('#');
            }
            
            // Левая и правая граница
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                // Левая граница (X = 0)
                Console.SetCursorPosition(0, i);
                Console.Write('#');
                
                // Правая граница (X = ширина экрана - 1)
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write('#');
            }
            
            // --- Рисуем голову змейки ---
            Console.SetCursorPosition(headX, headY);
            Console.Write('O'); // Рисуем голову как букву O

            // === ЧАСТЬ 4: ЗАДЕРЖКА ===
            // Без задержки змейка будет двигаться слишком быстро
            // 100 миллисекунд = 0.1 секунды
            Thread.Sleep(100);
        }
    }
}

