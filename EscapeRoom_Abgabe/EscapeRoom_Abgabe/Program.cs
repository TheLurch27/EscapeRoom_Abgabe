namespace EscapeRoom_Abgabe
{
    class Program
    {
        static string[,] room;
        static int playerX, playerY, keyX, keyY, doorX, doorY;
        static bool hasKey = false;

        static void Main(string[] args)
        {
            #region Welcome Message
            Console.WriteLine("Welcome to the game!");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Press a button to continue");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
            Console.Clear();
            #endregion

            #region Key Assignment
            Console.WriteLine("Movement: ");
            Console.WriteLine("");
            Console.WriteLine("Up    = W or ↑");
            Console.WriteLine("Left  = A or Computer says no... Think of the left arrow at this point)");
            Console.WriteLine("Down  = S or ↓");
            Console.WriteLine("Right = D or →");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Press a button to continue");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
            Console.Clear();
            #endregion

            #region Instructions
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
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Press a button to continue");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
            Console.Clear();
            #endregion

            int length = GetValidRoomSize("Enter the length of the room (5-30):", 5, 30);
            int width = GetValidRoomSize("Enter the width of the room (5-30):", 5, 30);

            CreateRoom(length, width);
            SetObjects();

            RenderRoom();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                PlayerMovement(keyInfo.Key);
                RenderRoom();

                if (playerX == doorX && playerY == doorY && hasKey)
                {
                    WinningMessage();
                    break;
                }
            }
        }

        /// <summary>
        /// Hier wird man aufgefordert die Größe des Raums einzugeben. Das Programm überprüft ob die Eingabe Richtig war.
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
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press a button to try again!");
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
            room = new string[length, width];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    room[i, j] = (i == 0 || i == length - 1 || j == 0 || j == width - 1) ? "██" : "  ";
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
        #endregion

        /// <summary>
        /// Diese Methode löscht die Konsole und zeichnet den aktuellen Raumzustand auf dem Bildschirm.
        /// </summary>
        #region RenderRoom
        static void RenderRoom()
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
                case ConsoleKey.UpArrow: newX--; break;
                case ConsoleKey.DownArrow: newX++; break;
                case ConsoleKey.LeftArrow: newY--; break;
                case ConsoleKey.RightArrow: newY++; break;
                default: return;
            }
            if (newX < 0 || newX >= room.GetLength(0) || newY < 0 || newY >= room.GetLength(1)) return;
            if (room[newX, newY] == "██") return;
            if (room[newX, newY] == "▓▓" && !hasKey) return;
            if (room[newX, newY] == "O┤") { hasKey = true; room[newX, newY] = "  "; } //RemoveDoor(); }
            room[playerX, playerY] = "  ";
            playerX = newX;
            playerY = newY;
            room[playerX, playerY] = ":)";
        }
        #endregion

        /// <summary>
        /// Nachdem der Spieler den Schlüssel eingesammelt hat wird die Tür aus dem Raum entfernt.
        /// </summary>
        #region RemoveDoor
        //static void RemoveDoor()
        //{
        //    for (int i = 0; i < room.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < room.GetLength(1); j++)
        //        {
        //            if (room[i, j] == "▓▓")
        //            {
        //                room[i, j] = "  ";
        //                return;
        //            }
        //        }
        //    }
        //}
        #endregion

        /// <summary>
        /// Hier wird eine Nachricht angezeigt dass das Spiel gewonnen wurde. Das Spiel wird danach beendet.
        /// </summary>
        #region WinningMessage
        static void WinningMessage()
        {
            Console.Clear();
            Console.WriteLine("Herzlichen Glückwunsch! Sie haben das Spiel gewonnen!");
            Console.WriteLine("Drücken Sie eine Taste, um das Spiel zu beenden.");
            Console.ReadKey();
            Environment.Exit(0);
        }
        #endregion
    }
}
