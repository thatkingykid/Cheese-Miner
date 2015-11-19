/*
Space Cheese Mining v0.1.5
James King
08101

Implemented in this build:
- Player selection
- Cheese placement
- Methodisation and improvement of system structure
- Movement System
- Cheese checker

Next build will implement:
- Battle system
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheeseMinerBuild1
{
    class Program
    {
        public enum player_colour //declare an enum for player colour for general tidyness
        { blue, red, yellow, green }

        struct player_info //declare a structure for holding all player data
        {
            public int playerID;
            public player_colour playerColour;
            public string playerName;
            public int playerStash;
            public int xCoordinates;
            public int yCoordinates;
        }
        static void Main(string[] args)
        {
            string finalInput = null; //declare a string to catch the input that terminates the program
            player_info[] player_data = null;
            int playerAmount = 0;
            bool[,] cheeseBoard = null;

            Console.WriteLine("Welcome to Space Cheese Miner!");
            player_data = PlayerCollection(playerData: ref player_data, numberOfPlayers: ref playerAmount); //begin method that collects player data

            do //begin main execution block of the game
            {
                int currentPlayer;
                int winningPlayer;
                bool gameWon = false;

                cheeseBoard = CheesePlacer(cheesePosition: ref cheeseBoard, playerNumber: playerAmount); //begin method that collects data on where the cheese will be placed
                player_data = PlayerResetter(playerInfo: ref player_data); //reset the player's position on the board
                int startingPlayer = CheeseHead(playerList: player_data);
                currentPlayer = startingPlayer;
                char direction;

                while (gameWon == false)
                {
                    Console.WriteLine("Player " + (currentPlayer + 1) + ", " + player_data[currentPlayer].playerName);
                    Console.WriteLine("You are on X Position: " + player_data[currentPlayer].xCoordinates);
                    Console.WriteLine("You are on Y Position: " + player_data[currentPlayer].yCoordinates);
                    int diceRoll = RollDice();
                    Console.WriteLine("You rolled a " + diceRoll);
                    direction = MovementCatcher();
                    Console.WriteLine("You are moving " + diceRoll + " spaces in a " + direction + " direction");
                    MovementCalculator(playerList: ref player_data[currentPlayer], roll: diceRoll, movement: direction);
                    Console.WriteLine("You are now at X: " + player_data[currentPlayer].xCoordinates + " and Y: " + player_data[currentPlayer].yCoordinates);
                    cheeseBoard = CheeseCollector(board: ref cheeseBoard, playerDetails: ref player_data[currentPlayer]);
                }


                Console.WriteLine("Do you wish to end the game? [Y]es or [N]o"); //get input if the user wants to end the session
                finalInput = Console.ReadLine();
                if (finalInput.ToUpper() == "Y" || finalInput.ToUpper() == "YES")
                {
                    break;
                }
            } while (finalInput.ToUpper() != "N" || finalInput.ToUpper() != "NO"); //finish execution block when players are done
        }
        static player_info[] PlayerCollection(ref player_info[] playerData, ref int numberOfPlayers)
        {

            for (int i = 0; i < 100000000; i++) //start loop for handling exceptions
            {
                Console.WriteLine("How many players are there? ");
                try //attempt to execute this code
                {
                    numberOfPlayers = int.Parse(Console.ReadLine());
                }
                catch (System.FormatException) //when a user cocks up, do this to stop the system from crashing
                {
                    Console.WriteLine("That's not a number!"); //throw error
                    Console.WriteLine(Environment.NewLine); //render a new line
                    continue; //restart chunk
                }


                if (numberOfPlayers < 0) //check user hasn't entered a negative value
                {
                    Console.WriteLine("Either you've entered an alternate dimension,", Environment.NewLine, "where man can exist in negative form, or you're trying to break the system.", Environment.NewLine);
                    continue;
                }
                else if (numberOfPlayers == 0) //check user hasn't set input to 0
                {
                    Console.WriteLine("Den who was input??????", Environment.NewLine);
                    continue;
                }
                else if (numberOfPlayers == 1) //check user hasn't set input to 1
                {
                    Console.WriteLine("Ah yes, the lonely nights spent playing cheese miner with youself.");
                    Console.WriteLine(Environment.NewLine);
                    continue;
                }
                else if (numberOfPlayers > 4) //check there aren't too many players
                {
                    Console.WriteLine("Though time is infinite, sadly RAM is not, please select 4 or less players.", Environment.NewLine);
                    continue;
                }
                else
                {
                    break; //exit out of the loop when the inputs are valid
                }
            }


            playerData = new player_info[numberOfPlayers]; //make a new structure for the number of players
            string colourCatcher;
            for (int i = 0; i < playerData.Length; i = i + 1) //loop around and get player info
            {
                bool broken = false;

                Console.WriteLine("Player " + (i + 1)); //write which player we are collecting info for
                playerData[i].playerID = i; //write ID based on index
                Console.WriteLine("What is your name? ");
                playerData[i].playerName = Console.ReadLine();
                Console.WriteLine("What colour would you like to be? You can pick from: Red, Blue, Green, Yellow"); //ask for colour selection
                colourCatcher = Console.ReadLine();

                if (colourCatcher.ToLower() == "red" || colourCatcher.ToLower() == "blue" || colourCatcher.ToLower() == "green" || colourCatcher.ToLower() == "yellow")
                //ensure we got a valid input
                {

                    for (int j = 0; j < i; j++) //nest a new loop
                    {
                        if (playerData[j].playerColour.ToString() == colourCatcher.ToLower()) //check selected colour hasn't been picked
                        {
                            Console.WriteLine("Selected colour is unavailable, please pick again!");
                            broken = true; //set a break variable to true if so
                            break; //break out of nested loop
                        }

                    }

                    if (broken == true) //check if we broke out of the nested loop
                    {
                        i = i - 1; //move the index back one
                        continue; //restart
                    }
                    else
                    {
                        playerData[i].playerColour = (player_colour)Enum.Parse(typeof(player_colour), colourCatcher, true); //messy code, but here we check if our Colour Catcher matches an Enum state, then put the state it matches into our structure.
                    }
                    Console.WriteLine("Player details saved!");
                    Console.WriteLine(Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("Due to printing issues, the colours selected are unavailable, please try again!");
                    i = i - 1; //move index one back
                    continue; //restart our loop
                }


            }

            return playerData;
        }
        static bool[,] CheesePlacer(ref bool[,] cheesePosition, int playerNumber)
        {
            cheesePosition = new bool[8, 8]; //make a new boolean array for the position of the cheese
            int cheeseCatcherX;
            int cheeseCatcherY;
            int cheesePerPlayer;

            if (playerNumber == 3) //check if we have an odd number of players
            {
                cheesePerPlayer = 5; //if so, remove one of the cheeses so it divides evenly
            }
            else
            {
                cheesePerPlayer = (16 / playerNumber); //otherwise, divide up the cheeses
            }

            Console.WriteLine("Now to place the cheese on the board. ");

            for (int i = 0; i < playerNumber; i++) //loop through each player for their turn adding cheese
            {
                Console.WriteLine("Player " + (i + 1) + " place your cheese using x and y co-ordinates.");
                for (int j = 0; j < cheesePerPlayer; j++) //loop through the player's cheese pieces
                {
                    Console.WriteLine("Cheese piece " + (j + 1));
                    Console.WriteLine("Enter X co-ordinate ");
                    try //attempt to catch an X co-ordinate
                    {
                        cheeseCatcherX = int.Parse(Console.ReadLine());
                    }
                    catch (System.FormatException) //throw an error if the format is invalid
                    {
                        Console.WriteLine("Sadly, that is not an X co-ordinate, please try again.", Environment.NewLine);
                        j = j - 1; //restart the current iteration of J
                        continue;
                    }

                    if (cheeseCatcherX > 7 || cheeseCatcherX < 0) //check it is a position on the board
                    {
                        Console.WriteLine("Space is infinite, unlike our board, please input a value between 0 and 8.", Environment.NewLine);
                        j = j - 1; //restart if not
                        continue;
                    }
                    Console.WriteLine("Enter Y co-ordinates");
                    try //attempt to catch the Y co-ordinate
                    {
                        cheeseCatcherY = int.Parse(Console.ReadLine());
                    }
                    catch (System.FormatException) //catch any incorrect format exceptions
                    {
                        Console.WriteLine("Sadly, that is not a Y co-ordinate, please try again.", Environment.NewLine);
                        j = j - 1; //restart iteration
                        continue;
                    }

                    if (cheeseCatcherY > 7 || cheeseCatcherY < 0) //check if it isn't a position on the board
                    {
                        Console.WriteLine("Space is infinite, unlike our board, please input a value between 0 and 8.", Environment.NewLine);
                        j = j - 1;
                        continue; //restart iteration
                    }

                    switch (cheeseCatcherX)
                    {
                        case 0:
                            if (cheeseCatcherY == 0 || cheeseCatcherY == 7)
                            {
                                Console.WriteLine("Cannot place cheese here as it is a spawning space. ");
                                j = j - 1;
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        case 7:
                            if (cheeseCatcherY == 0 || cheeseCatcherY == 7)
                            {
                                Console.WriteLine("Cannot place cheese here as it is a spawning space. ");
                                j = j - 1;
                                continue;
                            }
                            else
                            {
                                break;
                            }
                    }

                    if (cheesePosition[cheeseCatcherX, cheeseCatcherY] == true) //check position isn't currently occupied
                    {
                        Console.WriteLine("Position already occupied, please select a new value.", Environment.NewLine);
                        j = j - 1; //restart if it is
                        continue;
                    }
                    else //if not
                    {
                        cheesePosition[cheeseCatcherX, cheeseCatcherY] = true; //set boolean flag for the cheese at given x,y to true
                        Console.WriteLine("Cheese successfully added.");
                        Console.WriteLine(Environment.NewLine);
                    }
                }
            }
            return cheesePosition;
        }
        static player_info[] PlayerResetter(ref player_info[] playerInfo)
        {
            if (playerInfo.Length == 2) //check if we have 2 players
            {
                playerInfo[0].xCoordinates = 0;
                playerInfo[0].yCoordinates = 0;
                //place them at opposite board ends
                playerInfo[1].xCoordinates = 7;
                playerInfo[1].yCoordinates = 7;
            }
            else if (playerInfo.Length == 3) //check if we have 3 players
            {
                playerInfo[0].xCoordinates = 0;
                playerInfo[0].yCoordinates = 0;
                //place them in an L shape across the board
                playerInfo[1].xCoordinates = 7;
                playerInfo[1].yCoordinates = 0;

                playerInfo[2].xCoordinates = 0;
                playerInfo[2].yCoordinates = 7;
            }
            else
            {
                playerInfo[0].xCoordinates = 0;
                playerInfo[0].yCoordinates = 0;
                //place all four players in the corners of the board
                playerInfo[1].xCoordinates = 7;
                playerInfo[1].yCoordinates = 0;

                playerInfo[2].xCoordinates = 0;
                playerInfo[2].yCoordinates = 7;

                playerInfo[2].xCoordinates = 7;
                playerInfo[2].yCoordinates = 7;
            }
            return playerInfo;
        }
        static int CheeseHead(player_info[] playerList)
        {
            string CheeseFace;
            bool found = false;
            int playerIndex = 0;


            do
            {
                Console.WriteLine("Who has the head which looks most like a block of cheese? ");
                CheeseFace = Console.ReadLine();

                for (int i = 0; i < playerList.Length; i++)
                {
                    if (playerList[i].playerName.ToLower() == CheeseFace.ToLower())
                    {
                        playerIndex = i;
                        found = true;
                    }
                }

                if (found == false)
                {
                    Console.WriteLine("Error, player not found! " + Environment.NewLine);
                    continue;
                }
            } while (found == false);

            return playerIndex;
        }
        static int RollDice()
        {
            Console.WriteLine("Input your dice roll: ");
            int diceRoll = int.Parse(Console.ReadLine());
            return diceRoll;
        }
        static char MovementCatcher()
        {
            char direction = char.Parse("n");

            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine("Which direction do you want to move? N, E, S or W? ");
                try
                {
                    direction = char.Parse(Console.ReadLine());
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Please input a singular character. ");
                    i = i - 1;
                    continue;
                }

                switch (direction.ToString().ToLower())
                {
                    case "n":
                        return direction;
                    case "e":
                        return direction;
                    case "s":
                        return direction;
                    case "w":
                        return direction;
                    default:
                        break;
                }
                i = i - 1;
                continue;
            }
            return direction;
        }
        static void MovementCalculator (ref player_info playerList, int roll, char movement)
        {
            switch (movement.ToString().ToLower())
            {
                case "u":
                    playerList.yCoordinates = playerList.yCoordinates + roll;

                    if (playerList.yCoordinates > 7)
                    {
                        playerList.yCoordinates = playerList.yCoordinates - 8;
                    }
                    break;
                case "s":
                    playerList.yCoordinates = playerList.yCoordinates - roll;

                    if (playerList.yCoordinates < 0)
                    {
                        playerList.yCoordinates = playerList.yCoordinates + 8;
                    }
                    break;
                case "e":
                    playerList.xCoordinates = playerList.xCoordinates + roll;

                    if (playerList.xCoordinates > 7)
                    {
                        playerList.xCoordinates = playerList.xCoordinates - 8;
                    }
                    break;
                case "w":
                    playerList.xCoordinates = playerList.xCoordinates - roll;

                    if (playerList.xCoordinates < 0)
                    {
                        playerList.xCoordinates = playerList.xCoordinates + 8;
                    }
                    break;
            }

        }
        static bool[,] CheeseCollector(ref bool[,] board, ref player_info playerDetails)
        {
            if (board[playerDetails.xCoordinates, playerDetails.yCoordinates] == false)
            {
                Console.WriteLine("There is no cheese on this space.");
                Console.WriteLine("You do not collect any cheese this turn. ");
                return board;
            }
            else
            {
                Console.WriteLine("There is a cheese on this space.");
                playerDetails.playerStash = playerDetails.playerStash + 1;
                board[playerDetails.xCoordinates, playerDetails.yCoordinates] = false;
                Console.WriteLine("You collect the cheese and have a new stash counter of " + playerDetails.playerStash);
                return board;
            }
        }
    }
}

