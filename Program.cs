using System;
using System.Collections.Generic;
using System.IO;


class Program // User Layer
{
    public static void Main()
    {
        Functional.InitializeGrids(); // Инициализация двух пустых полей grid и enemyGrid
        Functional.PlaceMyShips(Functional.grid);
        Functional.PlaceEnemyShips(); // Функция для размещения кораблей противника

        Console.WriteLine("Добро пожаловать в игру 'Морской бой'!");
        Console.WriteLine("Вы готовы к битве!");

        do
        {
            Console.Clear();
            Functional.DisplayMyGrid(Functional.grid, "Ваше поле:");
            Functional.DisplayEnemyGrid();

            Console.WriteLine("Введите координаты выстрела (например, A5 или A10):");
            string input = Console.ReadLine().ToUpper();
            if (input.Length >= 2 && input[0] >= 'A' && input[0] <= 'J')
            {
                int row;
                int col;
                if (input.Length == 2)
                {
                    row = input[1] - '1';
                    col = input[0] - 'A';
                }
                else if (input.Length == 3 && input[1] == '1' && input[2] == '0')
                {
                    row = 9; // "10" соответствует индексу 9
                    col = input[0] - 'A';
                }
                else
                {
                    Console.WriteLine("Неверный формат координат.");
                    continue;
                }

                if (Functional.enemyGrid[row, col] == ' ')
                {
                    Console.WriteLine("Промах!");
                    Functional.enemyGrid[row, col] = 'O';
                }
                else if (Functional.enemyGrid[row, col] == 'S')
                {
                    Console.WriteLine("Попадание!");
                    Functional.enemyGrid[row, col] = 'X';

                    if (Functional.IsShipDestroyed(Functional.enemyGrid, row, col))
                    {
                        Console.WriteLine("Корабль уничтожен!");
                        Functional.DestroySurroundingArea(Functional.enemyGrid, row, col);
                    }

                    if (!Functional.HasShipsLeft(Functional.enemyGrid))
                    {
                        Console.Clear();
                        Functional.DisplayMyGrid(Functional.grid, "Ваше поле:");
                        Console.WriteLine("Поздравляем! Вы победили!");
                        break;
                    }
                }
                else if (Functional.enemyGrid[row, col] == 'O' ||
                 Functional.enemyGrid[row, col] == 'X')
                {
                    Console.WriteLine("Вы уже стреляли в это место.");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат координат.");
            }

            Console.WriteLine("Нажмите Enter для продолжения...");
            Console.ReadLine();

            // Противник делает случайный выстрел
            Functional.EnemyTurn();
        }
        while (true);
    }
}

class Functional
{
    public static char[,] grid = new char[10, 10];
    public static char[,] enemyGrid = new char[10, 10];
    static Random random = new Random();

    public static void InitializeGrids()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                grid[i, j] = ' ';
                enemyGrid[i, j] = ' ';
            }
        }
    }

    public static void DisplayMyGrid(char[,] grid, string title)
    {
        Console.WriteLine(title);
        Console.WriteLine("  A B C D E F G H I J");
        for (int i = 0; i < 10; i++)
        {
            Console.Write((i + 1).ToString() + " ");
            for (int j = 0; j < 10; j++)
            {
                Console.Write(grid[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    public static void DisplayEnemyGrid()
    {
        Console.WriteLine("Поле противника:");
        Console.WriteLine("  A B C D E F G H I J");
        for (int i = 0; i < 10; i++)
        {
            Console.Write((i + 1).ToString() + " ");
            for (int j = 0; j < 10; j++)
            {
                // Если на поле противника есть корабль ('S'), то заменяем его на пробел (' ')
                if (enemyGrid[i, j] == 'S')
                {
                    Console.Write(' ' + " ");
                }
                else
                {
                    Console.Write(enemyGrid[i, j] + " ");
                }
            }
            Console.WriteLine();
        }
    }

    public static void EnemyTurn()
    {
        int row, col;
        do
        {
            row = random.Next(10);
            col = random.Next(10);
        }
        while (grid[row, col] == 'X' || grid[row, col] == 'O');

        if (grid[row, col] == 'S')
        {
            Console.WriteLine("Противник попал!");
            grid[row, col] = 'X';
            if (!HasShipsLeft(grid))
            {
                Console.Clear();
                DisplayMyGrid(grid, "Ваше поле:");
                Console.WriteLine("Противник победил!");
                Environment.Exit(0);
            }
        }
        else
        {
            Console.WriteLine("Противник промахнулся!");
            grid[row, col] = 'O';
        }
    }

    public static bool IsShipDestroyed(char[,] targetGrid, int row, int col)
    {
        // Проверяем, есть ли ещё точки корабля вокруг данной точки
        if ((row - 1 >= 0 && targetGrid[row - 1, col] == 'S') ||
            (row + 1 < 10 && targetGrid[row + 1, col] == 'S') ||
            (col - 1 >= 0 && targetGrid[row, col - 1] == 'S') ||
            (col + 1 < 10 && targetGrid[row, col + 1] == 'S'))
        {
            return false; // Корабль не уничтожен
        }

        // Если не найдены другие точки корабля вокруг данной, то корабль уничтожен
        return true;
    }

    public static void DestroySurroundingArea(char[,] targetGrid, int row, int col)
    {
        int gridSize = targetGrid.GetLength(0);

        // Проверяем, находится ли текущая клетка в пределах поля и не является ли она уже обработанной (0)
        if (row >= 0 && row < gridSize && col >= 0 &&
         col < gridSize && targetGrid[row, col] != '0')
        {
            // Перед тем как заменить текущую клетку, проверим, была ли она 'X'
            bool isX = targetGrid[row, col] == 'X';

            targetGrid[row, col] = 'O';
            // Если текущая клетка была 'X', вызываем функцию рекурсивно для соседних клеток
            if (isX)
            {
                DestroySurroundingArea(targetGrid, row - 1, col);
                DestroySurroundingArea(targetGrid, row + 1, col);
                DestroySurroundingArea(targetGrid, row, col - 1);
                DestroySurroundingArea(targetGrid, row, col + 1);
                DestroySurroundingArea(targetGrid, row - 1, col - 1);
                DestroySurroundingArea(targetGrid, row - 1, col + 1);
                DestroySurroundingArea(targetGrid, row + 1, col - 1);
                DestroySurroundingArea(targetGrid, row + 1, col + 1);
                targetGrid[row, col] = 'X';
            }
        }
    }

    public static bool HasShipsLeft(char[,] targetGrid)
    {
        foreach (var cell in targetGrid)
        {
            if (cell == 'S')
            {
                return true;
            }
        }
        return false;
    }


    public static void PlaceMyShips(char[,] targetGrid)
    {
        Console.Clear();
        DisplayMyGrid(targetGrid, "Расстановка ваших кораблей:");

        int numberOfShips = 1; // Количество кораблей с разными палубами.

        // Сначала ставится корабль 4 палубы, затем переменная shipLength уменьшается, а numberOfShips увеличивается
        for (int shipLength = 4; shipLength >= 1; shipLength--)
        {
            for (int i = 0; i < numberOfShips; i++)
            {
                Console.WriteLine($"Поставьте корабль длиной {shipLength}:");
                Console.WriteLine($"Введите координату верхнего угла корабля (например, A5 или A10):");

                string input; // Ввод координаты

                // Инициализируем row и col значениями по умолчанию
                int row = -1; 
                int col = -1;

                // Проверка на возможность размещения корабля
                bool isValidPlacement = false;

                while (!isValidPlacement) // пока пользователь не введет верные координаты, мы будем спрашивать его
                {
                    input = Console.ReadLine().ToUpper();
                    if (input.Length >= 2 && input[0] >= 'A' && input[0] <= 'J') // Проверка на корректность
                    {
                        if (input.Length == 2) // Если нет координаты 10
                        {
                            // Преобразование введенной координаты в индекс массива
                            row = input[1] - '1';
                            col = input[0] - 'A';
                        }
                        else if (input.Length == 3 && input[1] == '1' && input[2] == '0') // Если введена координата 10
                        {
                            row = 9; // "10" соответствует индексу 9
                            col = input[0] - 'A';
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат координат.");
                            continue;
                        }

                        Console.WriteLine("Выберите направление корабля (V - вертикально, H - горизонтально):");

                        // Для вычисления нажатой клавиши на клавиатуре
                        char direction = char.ToUpper(Console.ReadKey().KeyChar); 
                        Console.WriteLine();

                        if ((direction == 'V'
                        && IsVerticalShipPlacementValid(row, col, shipLength, targetGrid)) ||
                        (direction == 'H'
                        && IsHorizontalShipPlacementValid(row, col, shipLength, targetGrid)))
                        {
                            // Размещаем корабль
                            isValidPlacement = true; 
                            PlaceShip(row, col, shipLength, direction, grid);
                        }
                        else
                        {
                            Console.WriteLine("Невозможно разместить корабль в выбранной позиции.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверный формат координат.");
                    }
                }

                Console.Clear();

                // После постановки корабля вызывается метод показа поля
                DisplayMyGrid(targetGrid, "Расстановка ваших кораблей:"); 
            }
            numberOfShips += 1;
        }
    }

    public static void PlaceShip(int startRow, int startCol, int length, char direction, char[,] targetGrid)
    {
        if (direction == 'V')
        {
            for (int i = 0; i < length; i++)
            {
                targetGrid[startRow + i, startCol] = 'S';
            }
        }
        else if (direction == 'H')
        {
            for (int i = 0; i < length; i++)
            {
                targetGrid[startRow, startCol + i] = 'S';
            }
        }
    }

    public static bool IsVerticalShipPlacementValid(int startRow, int startCol, int length, char[,] targetGrid)
    {
        if (startRow + length > 10)
        {
            return false;
        }

        // Проверка клеток перед и после корабля
        for (int i = startRow - 1; i <= startRow + length; i++)
        {
            for (int j = startCol - 1; j <= startCol + 1; j++)
            {
                if (i >= 0 && i < 10 && j >= 0 && j < 10)
                {
                    if (targetGrid[i, j] != ' ')
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public static bool IsHorizontalShipPlacementValid(int startRow, int startCol, int length, char[,] targetGrid)
    {
        if (startCol + length > 10)
        {
            return false;
        }

        // Проверка клеток перед и после корабля
        for (int i = startRow - 1; i <= startRow + 1; i++)
        {
            for (int j = startCol - 1; j <= startCol + length; j++)
            {
                if (i >= 0 && i < 10 && j >= 0 && j < 10)
                {
                    if (targetGrid[i, j] != ' ')
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }


    public static void PlaceEnemyShips()
    {
        PlaceRandomEnemyShip(4, enemyGrid);
        PlaceRandomEnemyShip(3, enemyGrid);
        PlaceRandomEnemyShip(3, enemyGrid);
        PlaceRandomEnemyShip(2, enemyGrid);
        PlaceRandomEnemyShip(2, enemyGrid);
        PlaceRandomEnemyShip(2, enemyGrid);
        PlaceRandomEnemyShip(1, enemyGrid);
        PlaceRandomEnemyShip(1, enemyGrid);
        PlaceRandomEnemyShip(1, enemyGrid);
        PlaceRandomEnemyShip(1, enemyGrid);
    }

    public static void PlaceRandomEnemyShip(int length, char[,] targetGrid)
    {
        int row, col;
        char direction;

        do
        {
            row = random.Next(10);
            col = random.Next(10);
            direction = random.Next(2) == 0 ? 'H' : 'V'; // Случайное определение направления
        }
        while (!IsEnemyShipPlacementValid(row, col, length, direction));

        PlaceShip(row, col, length, direction, enemyGrid);
    }

    public static bool IsEnemyShipPlacementValid(int startRow, int startCol, int length, char direction)
    {
        if (direction == 'H')
        {
            if (!IsHorizontalShipPlacementValid(startRow, startCol, length, enemyGrid))
            {
                return false;
            }
        }

        else if (direction == 'V')
        {
            if (!IsVerticalShipPlacementValid(startRow, startCol, length, enemyGrid))
            {
                return false;
            }
        }
        return true;
    }
}