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
        /// The method checks if the input was correct
        /// </summary>
        /// <param name="message">The message tells the player to make an input</param>
        /// <param name="minValue">The minimum size of the room</param>
        /// <param name="maxValue">The maximum size of the room.</param>
        /// <returns>The correct room size entered by the player.</returns>
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
        /// This method adds walls and floor to the room.
        /// </summary>
        /// <param name="length">This is the length of the room</param>
        /// <param name="width">This is the width of the room</param>
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
        /// In this method, the player, key and door are placed randomly in the room
        /// </summary>
        #region SetObjects
        static void SetObjects()
        {
            Random random = new Random();
            playerX = random.Next(1, room.GetLength(0) - 1); // A random X coordinate is selected here
            playerY = random.Next(1, room.GetLength(1) - 1); // The same with the Y coordinate
            room[playerX, playerY] = ":)"; // <- To place the little guy here

            keyX = random.Next(1, room.GetLength(0) - 1);
            keyY = random.Next(1, room.GetLength(1) - 1);
            room[keyX, keyY] = "O┤";

            int side = random.Next(4); // A random wall (or a random case) is selected on which the door is then placed at the end
            switch (side)
            {
                case 0: // Wall - Top
                    doorX = 0;
                    doorY = random.Next(1, room.GetLength(1) - 1);
                    break;
                case 1: // Wall - Right
                    doorX = random.Next(1, room.GetLength(0) - 1);
                    doorY = room.GetLength(1) - 1;
                    break;
                case 2: // Wall - Bottom
                    doorX = room.GetLength(0) - 1;
                    doorY = random.Next(1, room.GetLength(1) - 1);
                    break;
                case 3: // Wall - Left
                    doorX = random.Next(1, room.GetLength(0) - 1);
                    doorY = 0;
                    break;
            }
            room[doorX, doorY] = "▓▓";
        }
        #endregion

        /// <summary>
        /// This method ensures that the room is displayed
        /// </summary>
        #region RenderRoom
        static void RenderRoom()
        {
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < room.GetLength(0); i++) // The program runs through all lines
            {
                for (int j = 0; j < room.GetLength(1); j++) // In the same way through all columns
                {
                    if (i == playerX && j == playerY) // This checks if the current position is the same as that of the character
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write(room[i, j]); // The characters are assigned to the respective position here
                }
                Console.WriteLine();
            }
        }
        #endregion

        /// <summary>
        /// This method provides the player movement (key assignment)
        /// </summary>
        /// <param name="direction">The direction in which the player is to be moved is specified here</param>
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
        /// The key is replaced by a floor at the corresponding position
        /// </summary>
        /// <param name="x">The X-coordinate of the key in the room</param>
        /// <param name="y">The Y-coordinate of the key in the room</param>
        #region RemoveKey
        static void RemoveKey(int x, int y)
        {
            if (x >= 0 && x < room.GetLength(0) && y >= 0 && y < room.GetLength(1)) // It is checked whether the coordinate is within the room
            {
                room[x, y] = "  "; // The key is removed from the room and replaced with a floor
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
