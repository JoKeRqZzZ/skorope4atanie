using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using alenadast;
using Newtonsoft.Json;



namespace FastTypingChallenge
{
    internal class SpeedTyper
    {
        static void Main()
        {
            Console.WriteLine("Всем привет в тесте на быстрые руки, вам придется показать пальцы рук и знение текста оксимирона");
            Console.WriteLine("Введите ваш ник:");
            string nick = Console.ReadLine();

            Console.WriteLine("по нажатию enter начнется тест и запустится таймер 2 минуты");
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                Console.Clear();

                string songLyrics = "На небе звезды как огоньки гирлянд, " +
                    "В городе тишина, словно вечер у моря. " +
                    "Сквозь ночной туман пробиваются лучи, " +
                    "Мы пройдем через тень, как сквозь ворота времени. " +
                    "Трава шелестит под ногами, словно нежные слова, " +
                    "В воздухе аромат свежести, как после летнего дождя. " +
                    "Сердце бьется в унисон с ритмом природы, " +
                    "Мы плывем по волнам вечности, как корабль в океане.";


                DateTime endTime = DateTime.Now.AddMinutes(2);
                int correctCharacters = 0;
                int totalCharacters = 0;
                int cursorX = 0;
                int cursorY = 7;
                int typingMistakes = 0;
                List<SpeedRecords> playerRecords = new List<SpeedRecords>();

                Thread timerThread = new Thread(_ =>
                {
                    while (DateTime.Now < endTime)
                    {
                        Console.SetCursorPosition(0, 16);
                        Console.WriteLine("Time left: 00:{00:ss} ", (endTime - DateTime.Now).Seconds);
                        Console.ResetColor();

                        Thread.Sleep(1000);
                    }
                });

                Console.SetCursorPosition(0, cursorY);
                Console.WriteLine(songLyrics);

                timerThread.Start();

                while (totalCharacters < songLyrics.Length && DateTime.Now < endTime)
                {
                    char inputCharacter = Console.ReadKey(true).KeyChar;

                    if (inputCharacter == songLyrics[totalCharacters])
                    {
                        totalCharacters++;
                        correctCharacters++;
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(cursorX, cursorY);
                        if (cursorX >= 118)
                        {
                            cursorX = 0;
                            cursorY++;
                        }
                        else
                        {
                            Console.Write(inputCharacter);
                            cursorX++;
                        }
                    }
                    else
                    {
                        typingMistakes++;
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                }

                Console.Clear();
                Console.ResetColor();
                Console.WriteLine("Congratulations! You've completed the challenge.");
                Console.WriteLine("Your chosen handle: " + nick);
                Console.WriteLine("Typing speed: " + correctCharacters + " characters per minute");

                SpeedRecords newRecord = new SpeedRecords();
                newRecord.nick = nick;
                newRecord.Speed = correctCharacters;
                playerRecords.Add(newRecord);

                string json = JsonConvert.SerializeObject(playerRecords);
                File.WriteAllText("D:\\SpeedTyperLeaderboard.json", json);

                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
