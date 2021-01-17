using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace kpyap_laba_15_2
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("1. Начать новую игру \n2. Загрузить игру");
                string choice;
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        {
                            Console.WriteLine("Введите начальное количество очков.");
                            int points = int.Parse(Console.ReadLine());
                            Console.WriteLine("Введите количество ходов.");
                            int steps = int.Parse(Console.ReadLine());
                            Console.WriteLine("Введите количество очков, которое нужно набрать для выйгрыша.");
                            int winPoints = int.Parse(Console.ReadLine());
                            Console.WriteLine("Введите количество посетителей за ход.");
                            int visitors = int.Parse(Console.ReadLine());
                            Game game = new Game(steps, visitors, points, winPoints);
                            game.Notify += DisplayMessage;
                            Gameplay(game);
                            break;
                        }

                    case "2":
                        {
                            Game game = LoadGame();
                            game.Notify += DisplayMessage;
                            Gameplay(game);
                            break;
                        }
                    default:
                        break;
                }

            }
        }

        static Game LoadGame()
        {
            // сериализя
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream("SavedGame.txt", FileMode.Open))
            {
               return (Game)bf.Deserialize(fs);
            }
        }

        static void Gameplay(Game game)
        {
            while (game.GameStep())
            {
                bool Step = true;
                while (Step)
                {
                    Console.WriteLine(game.CurrentInfo());
                    Console.WriteLine("1. Реклама (-100 очков, удвоение посетителей)");
                    Console.WriteLine("2. Снять мерки (-15 очков, посетителей х 1.25)");
                    Console.WriteLine("3. Сшить костюмы (-20 очков\\костюм за пошив, +50 очков\\костюм за продажу)");
                    Console.WriteLine("4. Завершить ход.");
                    string choice;
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            game.Ad();
                            break;
                        case "2":
                            game.TakeMeasurements();
                            break;
                        case "3":
                            Console.WriteLine("Введите количество костюмов.");
                            int amount = int.Parse(Console.ReadLine());
                            game.SewCostumes(amount);
                            break;
                        case "4":
                            Step = false;
                            break;
                        default:
                            break;
                    }
                }

            }
        }

        private static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }


    }
}
// Игра Ателье. Задача получить n очков, имея m, за t ходов. Если очков
// меньше 0 игрок проигрывает. Количество посетителей в день p.
// - провести рекламу. Стоит 100 очков х2 посетителей.
// - снять мерки. Стоит 15 очков +0.25х посетителей.
// - Сшить костюм. Указывается количество. Костюмы покупаются не в
// большем количестве, чем приходят посетители в ход. Стоит 20 очков. При
// покупке 50 очков.
// Случайно может прийти банда в начале хода и украсть все костюмы.
// Вероятность этого 20%. Генерироваться событие, когда это происходит