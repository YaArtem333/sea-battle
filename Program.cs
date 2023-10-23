using System;
using System.Collections.Generic;
using System.IO;


namespace sea_battle
{
    class Program // Presentation Layer
    {
        public static void Main()
        {
            Console.Clear();
            Console.WriteLine("Введите имя пользователя:");
            string username = Console.ReadLine();

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Привет, {username}");
                Console.WriteLine("\nВыберите опцию:\n1. Начать игру\n2. Посмотреть историю игр\n3. Выход");
                //string choice = Console.ReadLine();
                char choice = char.ToUpper(Console.ReadKey().KeyChar);


                if (choice == '1')
                {
                    Game(username); 
                }
                else if (choice == '2')
                {
                    Console.Clear();
                    Data dataHandler = new Data();
                    List<PlayerData> playerRecords = dataHandler.GetRecordsByName(username); 
                    Console.WriteLine();
                    foreach(var record in playerRecords)
                    {
                        Console.WriteLine($"Name: {record.Name}, " +
                                        $"Result: {record.Result}, " +
                                        $"Date: {record.Date}, " +
                                        $"Time: {record.Time}");
                    }
                    Console.WriteLine("\nНажмите на любую кнопку, чтобы выйти...");
                    char any_btn = char.ToUpper(Console.ReadKey().KeyChar);
                }
                else if (choice == '3')
                {
                    Main();
                }
                else
                {
                    Console.WriteLine("Некорректный выбор. Пожалуйста, попробуйте снова.");
                }
            }
        }

        public static void Game(string username)
        {
            Functional.InitializeGrids();
            Functional.PlaceMyShips(Functional.grid);
            Functional.PlaceEnemyShips(); // Новая функция для размещения кораблей противника

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
                            Data dataHandler = new Data();
                            PlayerData playerData = new PlayerData(username, DateTime.Now, "Win");
                            dataHandler.AddRecord(playerData);
                            break;
                        }
                    }
                    else if (Functional.enemyGrid[row, col] == 'O' || Functional.enemyGrid[row, col] == 'X')
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
                if (!Functional.HasShipsLeft(Functional.grid))
                {
                    Console.Clear();
                    Functional.DisplayMyGrid(Functional.grid, "Ваше поле:");
                    Console.WriteLine("Печально! Вы проиграли!");

                    Data dataHandler = new Data();
                    PlayerData playerData = new PlayerData(username, DateTime.Now, "Loss");
                    dataHandler.AddRecord(playerData);
                    break;
                }
            }
            while (true);
        }
    }
}