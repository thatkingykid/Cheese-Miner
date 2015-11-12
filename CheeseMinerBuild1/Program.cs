/*
Space Cheese Mining v0.1.0
James King
08101

Implemented in this build:
- Player selection
- Cheese placement

Next build will implement:
- Movement system
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
        enum player_colour //declare an enum for player colour for general tidyness
        { blue, red, yellow, green }

        struct player_info //declare a structure for holding all player data
        {
            public int playerID;
            public player_colour playerColour;
            public string playerName;
            public int playerStash;
        }
        static void Main(string[] args)
        {
            string finalInput = null; //declare a string to catch the input that terminates the program
            int numberOfPlayers = 0;
            do //begin main execution block
            {
                Console.WriteLine("Welcome to Space Cheese Miner!");

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

                player_info[] playerData = new player_info[numberOfPlayers]; //make a new structure for the number of players
                string colourCatcher;
                bool broken = false;
                for (int i = 0; 0 < playerData.Length; i++) //loop around and get player info
                {
                    Console.WriteLine("Player " + (i + 1)); //write which player we are collecting info for
                    playerData[i].playerID = i; //write ID based on index
                    Console.WriteLine("What is your name? ");
                    playerData[i].playerName = Console.ReadLine();
                    Console.WriteLine("What colour would you like to be? You can pick from: Red, Blue, Green, Yellow"); //ask for colour selection
                    colourCatcher = Console.ReadLine();

                    if (colourCatcher.ToLower() != "red" || colourCatcher.ToLower() != "blue" || colourCatcher.ToLower() != "green" || colourCatcher.ToLower() != "yellow")
                    //ensure we got a valid input
                    {
                        Console.WriteLine("Due to printing issues, the colours selected are unavailable, please try again!");
                        i = i - 1; //move index one back
                        continue; //restart our loop
                    }

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

                bool[,] cheesePosition = new bool[8, 8]; //make a new boolean array for the position of the cheese
                int cheeseCatcherX;
                int cheeseCatcherY;
                Console.WriteLine("Now to place the cheese on the board. ");

                for (int i = 0; i < playerData.Length; i++) //loop through each player for their turn adding cheese
                {
                    Console.WriteLine("Player ", i, "place your cheese using x and y co-ordinates.");
                    for (int j = 0; j < 4; j++) //loop through the player's four cheese pieces
                    {
                        Console.WriteLine("Cheese piece ", (j + 1));
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

                        if (cheeseCatcherX > 8 || cheeseCatcherX < 0) //check it is a position on the board
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

                        if (cheeseCatcherY > 8 || cheeseCatcherY < 0) //check if it isn't a position on the board
                        {
                            Console.WriteLine("Space is infinite, unlike our board, please input a value between 0 and 8.", Environment.NewLine);
                            j = j - 1;
                            continue; //restart iteration
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

                Console.WriteLine("Do you wish to end the game? [Y]es or [N]o"); //get input if the user wants to end the session
                finalInput = Console.ReadLine();
                if (finalInput.ToUpper() == "Y" || finalInput.ToUpper() == "YES")
                {
                    break;
                }
            } while (finalInput.ToUpper() != "N" || finalInput.ToUpper() != "NO"); //finish execution block when players are done
        }
    }
}
