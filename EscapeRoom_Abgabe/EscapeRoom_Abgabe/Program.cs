using System;

namespace EscapeRoom_Abgabe
{
    class Program
    {
        static string[,] room;
        static int playerX, playerY, keyX, keyY, doorX, doorY;
        static bool hasKey = false;
        static bool shouldRender = true;

        static void Main(string[] args)
        {
            #region Welcome Message - Text
            Console.WriteLine("Welcome to the game!");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Press a button to continue");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
            Console.Clear();
            #endregion

            #region Key assignment - Text
            Console.WriteLine("Movement: ");
            Console.WriteLine("Up    = W or ↑");
            Console.WriteLine("Left  = A or Computer says no... Think of the left arrow at this point)");
            Console.WriteLine("Down  = S or ↓");
            Console.WriteLine("Right = D or →");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Press a button to continue");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
            Console.Clear();
            #endregion

            #region Instructíons - Text
            Console.WriteLine("Instructions: ");
            Console.WriteLine();
            Console.Write("Move the Character ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("':)' ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Collect the Key ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("'O┤' ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("and open the door to win.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Press a button to continue");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
            Console.Clear();
            #endregion

            #region Enter Room Size
            int length = GetValidRoomSize("Enter the length of the room (5-25):", 5, 25);
            Console.Clear();
            int width = GetValidRoomSize("Enter the width of the room (5-25):", 5, 25);
            Console.WriteLine();
            #endregion

            CreateRoom(length, width);
            SetObjects();

            RenderRoom();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    PlayerMovement(keyInfo.Key);
                    shouldRender = true;
                }

                if (shouldRender)
                {
                    RenderRoom();
                    shouldRender = false;
                }

                if (playerX == doorX && playerY == doorY && hasKey)
                {
                    WinningMessage();
                    break;
                }

                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Die Methode überprüft ob die Eingabe Richtig war.
        /// </summary>
        /// <param name="message">Die Meldung die den Spieler zur Eingabe auffordert.</param>
        /// <param name="minValue">Die mind. Größe des Raums.</param>
        /// <param name="maxValue">Der max. Größe des Raums.</param>
        /// <returns>Die Richtige Raum Größe die vom Spieler eingegeben wird.</returns>
        #region GetValidRoomDimension
        static int GetValidRoomSize(string message, int minValue, int maxValue)
        {
            int size;
            while (true)
            {
                Console.WriteLine(message);
                if (!int.TryParse(Console.ReadLine(), out size) || size < minValue || size > maxValue)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please read the request and then enter your answer again!");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Press a button to try again");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    return size;
                }
            }
        }
        #endregion

        /// <summary>
        /// Diese Methode fügt dem Raum Wände und Boden hinzu.
        /// </summary>
        /// <param name="length">Das ist die Länge des Raums.</param>
        /// <param name="width">Das ist die Breite des Raums.</param>
        /// 
        #region InitalizeRoom
        static void CreateRoom(int length, int width)
        {
            Console.Clear();
            room = new string[length, width]; // Es wird ein neues Raum Array mit Länge und Breite erstellt

            for (int i = 0; i < length; i++) // Das Programm läuft durch jede Zeile des Codes
            {
                for (int j = 0; j < width; j++) // Und Spalte...
                {
                    if (i == 0 || i == length - 1 || j == 0 || j == width - 1) // Hier wird überprüft ob die Aktuelle Position eine Wand ist
                    {
                        room[i, j] = "██"; // Wenn dem so ist wird sie als Wand ausgegeben
                    }
                    else
                    {
                        room[i, j] = "  "; // Wenn nicht dann nicht = Boden
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Hier werden Spieler, Schlüssel und Tür zufällig im Raum platziert.
        /// </summary>
        #region SetObjects
        static void SetObjects()
        {
            Random random = new Random();
            playerX = random.Next(1, room.GetLength(0) - 1); // Hier wird eine Zufällige X-Koordinate gewählt
            playerY = random.Next(1, room.GetLength(1) - 1); // Das selbe mit der Y Koordinate
            room[playerX, playerY] = ":)"; // <- Um den kleinen Kerl hier zu Platzieren

            keyX = random.Next(1, room.GetLength(0) - 1);
            keyY = random.Next(1, room.GetLength(1) - 1);
            room[keyX, keyY] = "O┤";

            int side = random.Next(4); // Es wird eine Zufällige Wand (bzw ein zufälliger Case) gewählt auf diese am ende dann die Tür platziert wird
            switch (side)
            {
                case 0: // Wand - Oben
                    doorX = 0;
                    doorY = random.Next(1, room.GetLength(1) - 1);
                    break;
                case 1: // Wand - Rechts
                    doorX = random.Next(1, room.GetLength(0) - 1);
                    doorY = room.GetLength(1) - 1;
                    break;
                case 2: // Wand - Unten
                    doorX = room.GetLength(0) - 1;
                    doorY = random.Next(1, room.GetLength(1) - 1);
                    break;
                case 3: // Wand - Links
                    doorX = random.Next(1, room.GetLength(0) - 1);
                    doorY = 0;
                    break;
            }
            room[doorX, doorY] = "▓▓";
        }
        #endregion

        /// <summary>
        /// Diese Methode sorgt dafür das der Raum dargestellt wird.
        /// </summary>
        #region RenderRoom
        static void RenderRoom()
        {
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < room.GetLength(0); i++) // Das Programm läuft durch alle Zeilen
            {
                for (int j = 0; j < room.GetLength(1); j++) // Genauso durch alle Spalten
                {
                    if (i == playerX && j == playerY) // Hier wird überprüft ob die Aktuelle Position die selbe ist wie vom Character
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write(room[i, j]); // Hier werden die Zeichen an der jeweiligen Position vergeben
                }
                Console.WriteLine();
            }
        }
        #endregion

        /// <summary>
        /// Diese Methode sorgt für die Spieler Bewegung (Tastenbelegung)
        /// </summary>
        /// <param name="direction">Hier wird angegeben in welche Richtung der Spieler bewegt werden soll.</param>
        #region PlayerMovement
        static void PlayerMovement(ConsoleKey direction)
        {
            int newX = playerX, newY = playerY;
            switch (direction)
            {
                case ConsoleKey.UpArrow:
                    newX--;
                    break;
                case ConsoleKey.DownArrow:
                    newX++;
                    break;
                case ConsoleKey.LeftArrow:
                    newY--;
                    break;
                case ConsoleKey.RightArrow:
                    newY++;
                    break;
                default:
                    return;
            }
            if (newX < 0 || newX >= room.GetLength(0) || newY < 0 || newY >= room.GetLength(1))
                return;
            if (room[newX, newY] == "██")
                return;
            if (room[newX, newY] == "▓▓" && !hasKey)
                return;
            if (room[newX, newY] == "O┤")
            {
                hasKey = true;
                RemoveKey(newX, newY);
            }

            room[playerX, playerY] = "  ";
            playerX = newX;
            playerY = newY;
            room[playerX, playerY] = ":)";
        }
        #endregion

        /// <summary>
        /// Der Schlüssel wird an der ensprechenden Stelle durch einen Boden ersetzt.
        /// </summary>
        /// <param name="x">Die X-Koordinate vom Schlüssels im Raum.</param>
        /// <param name="y">Die Y-Koordinate vom Schlüssels im Raum.</param>
        #region RemoveKey
        static void RemoveKey(int x, int y)
        {
            if (x >= 0 && x < room.GetLength(0) && y >= 0 && y < room.GetLength(1)) // Es wird gecheckt ob die Koordinate innerhalb des Raumes liegt
            {
                room[x, y] = "  "; // Der Schlüssel wird aus dem Raum entfernt und durch einen Boden ersetzt
            }
        }
        #endregion

        #region WinningMessage - Text
        static void WinningMessage()
        {
            Console.Clear();
            Console.WriteLine("Congratulations! You have won the Game!");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Press a button to exit the Game");
            Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion
    }
}
