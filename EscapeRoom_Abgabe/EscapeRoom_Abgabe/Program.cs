namespace EscapeRoom_Abgabe
{
    class Program
    {
        static string[,] room;
        static int playerX, playerY, keyX, keyY, doorX, doorY;
        static bool hasKey = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the game!");
            Console.ReadLine();
            Console.Clear();

            Console.WriteLine("Movement: ");
            Console.WriteLine("");
            Console.Write("Up    = ↑ or W");
            Console.Write("Left  = ← or A");
            Console.Write("Down  = ↓ or S");
            Console.Write("Right = → or D");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("Instructions: ");
            Console.WriteLine();
            Console.WriteLine("Move the Character ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("':)' ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Collect the Key ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("'O┤' ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("and open the door to win.");
            Console.ReadLine();
            Console.Clear();

            int length = GetValidRoomDimension("Enter the length of the room (5-30):", 5, 30);
            int width = GetValidRoomDimension("Enter the width of the room (5-30):", 5, 30);

            InitializeRoom(length, width);
            PlaceObjects();

            DrawRoom();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                MovePlayer(keyInfo.Key);
                DrawRoom();

                if (playerX == doorX && playerY == doorY && hasKey)
                {
                    DisplayWinningMessage();
                    break;
                }
            }
        }

        /// <summary>
        /// Fordert den Spieler auf, die Länge oder Breite des Raums einzugeben und überprüft die Gültigkeit der Eingabe.
        /// </summary>
        /// <param name="message">Die Meldung, die den Spieler zur Eingabe auffordert.</param>
        /// <param name="minValue">Der minimale Wert für die Raumdimension.</param>
        /// <param name="maxValue">Der maximale Wert für die Raumdimension.</param>
        /// <returns>Die gültige Raumdimension, die vom Spieler eingegeben wurde.</returns>

        #region GetValidRoomDimension
        static int GetValidRoomDimension(string message, int minValue, int maxValue)
        {
            int dimension;
            while (true)
            {
                Console.WriteLine(message);
                if (!int.TryParse(Console.ReadLine(), out dimension) || dimension < minValue || dimension > maxValue)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please read the request and then enter your answer again!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press a button to try again!");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    return dimension;
                }
            }
        }
        #endregion

        static void InitializeRoom(int length, int width)
        {
            room = new string[length, width];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    room[i, j] = (i == 0 || i == length - 1 || j == 0 || j == width - 1) ? "██" : "  ";
                }
            }
        }

        static void PlaceObjects()
        {
            Random random = new Random();
            playerX = random.Next(1, room.GetLength(0) - 1);
            playerY = random.Next(1, room.GetLength(1) - 1);
            room[playerX, playerY] = ":)"; // Player

            keyX = random.Next(1, room.GetLength(0) - 1);
            keyY = random.Next(1, room.GetLength(1) - 1);
            room[keyX, keyY] = "O┤"; // Key

            int side = random.Next(4); // 0: top, 1: right, 2: bottom, 3: left
            switch (side)
            {
                case 0: doorX = 0; doorY = random.Next(1, room.GetLength(1) - 1); break; // top wall
                case 1: doorX = random.Next(1, room.GetLength(0) - 1); doorY = room.GetLength(1) - 1; break; // right wall
                case 2: doorX = room.GetLength(0) - 1; doorY = random.Next(1, room.GetLength(1) - 1); break; // bottom wall
                case 3: doorX = random.Next(1, room.GetLength(0) - 1); doorY = 0; break; // left wall
            }
            room[doorX, doorY] = "▓▓"; // Door
        }

        static void DrawRoom()
        {
            Console.Clear();
            for (int i = 0; i < room.GetLength(0); i++)
            {
                for (int j = 0; j < room.GetLength(1); j++)
                {
                    Console.Write(room[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void MovePlayer(ConsoleKey direction)
        {
            int newX = playerX, newY = playerY;
            switch (direction)
            {
                case ConsoleKey.UpArrow: newX--; break;
                case ConsoleKey.DownArrow: newX++; break;
                case ConsoleKey.LeftArrow: newY--; break;
                case ConsoleKey.RightArrow: newY++; break;
                default: return;
            }
            if (newX < 0 || newX >= room.GetLength(0) || newY < 0 || newY >= room.GetLength(1)) return;
            if (room[newX, newY] == "██") return;
            if (room[newX, newY] == "▓▓" && !hasKey) return;
            if (room[newX, newY] == "O┤") { hasKey = true; room[newX, newY] = "  "; RemoveDoor(); }
            room[playerX, playerY] = "  ";
            playerX = newX;
            playerY = newY;
            room[playerX, playerY] = ":)";
        }

        static void RemoveDoor()
        {
            for (int i = 0; i < room.GetLength(0); i++)
            {
                for (int j = 0; j < room.GetLength(1); j++)
                {
                    if (room[i, j] == "▓▓")
                    {
                        room[i, j] = "  ";
                        return;
                    }
                }
            }
        }

        static void DisplayWinningMessage()
        {
            Console.Clear();
            Console.WriteLine("Herzlichen Glückwunsch! Sie haben das Spiel gewonnen!");
            Console.WriteLine("Drücken Sie eine Taste, um das Spiel zu beenden.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
