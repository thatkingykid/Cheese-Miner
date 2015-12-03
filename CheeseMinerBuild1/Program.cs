/*
Space Cheese Mining v0.3.5
James King
08101

Implemented in this build:
- Player selection
- Cheese placement
- Methodisation and improvement of system structure
- Movement System
- Cheese checker
- Battle system
- Scoring the game
- Bug squashing


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
            char test;

            Console.WriteLine("Welcome to Space Cheese Miner!");
            Console.WriteLine("Do you wish to run the game in test mode? Y or N");
            test = char.Parse(Console.ReadLine());

            player_data = PlayerCollection(playerData: ref player_data, numberOfPlayers: ref playerAmount); //begin method that collects player data

            do //begin main execution block of the game
            {
                int currentPlayer;
                List<int> winningPlayer = new List<int>();
                bool gameWon = false;

                cheeseBoard = CheesePlacer(cheesePosition: ref cheeseBoard, playerNumber: playerAmount); //begin method that collects data on where the cheese will be placed
                player_data = PlayerResetter(playerInfo: ref player_data); //reset the player's position on the board
                int startingPlayer = CheeseHead(playerList: player_data); //begin method which finds out who starts the game
                currentPlayer = startingPlayer; //get the index of the player who starts the game
                char direction; //initialise the direction character
                if (test.ToString().ToLower() == "n") //run the game in release mode
                {
                    while (gameWon == false)
                    {
                        List<int> collisionIndex = new List<int>();
                        char battleDecision = char.Parse("n");
                        int targetPlayer = 0;

                        Console.WriteLine("Player " + (currentPlayer + 1) + ", " + player_data[currentPlayer].playerName);
                        Console.WriteLine("You are on X Position: " + player_data[currentPlayer].xCoordinates);
                        Console.WriteLine("You are on Y Position: " + player_data[currentPlayer].yCoordinates);
                        int diceRoll = RollDice(); //find what number they rolled using the roll dice method
                        Console.WriteLine("You rolled a " + diceRoll);
                        direction = MovementCatcher(); //find out what direction the user wants to move in using the movement catcher method
                        Console.WriteLine("You are moving " + diceRoll + " spaces in a " + direction + " direction"); //display their chosen move
                        MovementCalculator(playerList: ref player_data[currentPlayer], roll: diceRoll, movement: direction); //run the movement calculation method 
                        Console.WriteLine("You are now at X: " + player_data[currentPlayer].xCoordinates + " and Y: " + player_data[currentPlayer].yCoordinates); //display new position
                        cheeseBoard = CheeseCollector(board: ref cheeseBoard, playerDetails: ref player_data[currentPlayer]); //check if the user landed on any cheese
                        bool collision = CollisionDetector(playerList: player_data, currentIndex: currentPlayer, detectedPlayer: ref collisionIndex); //run the method which checks for player collision

                        if (collision == true) //check if we had a collision
                        {
                            battleDecision = BattleDecisionMaker(opponentPlayer: collisionIndex, finalTarget: targetPlayer); //if so, run the method which gets user input if they want to battle
                        }

                        if (battleDecision.ToString().ToLower() == "y") //check the user wants to battle
                        {
                            BattleSystem(playerData: ref player_data, activePlayer: currentPlayer, targetPlayer: targetPlayer, cheeseData: ref cheeseBoard); //run the method which battles players
                        }
                        gameWon = FindAWinner(playerList: player_data, winnerIndex: ref winningPlayer); //run the method that checks if anyone has won the game

                        if (currentPlayer == playerAmount - 1) //checks if we're on the last player in the index
                        {
                            currentPlayer = 0; //if so, restart our loop
                        }
                        else
                        {
                            currentPlayer++; //if not, add one
                        }
                    }
                }
                else //run the game in test mode
                {
                    while (gameWon == false)
                    {
                        List<int> collisionIndex = new List<int>();
                        char battleDecision = char.Parse("n");
                        int targetPlayer = 0;

                        Console.WriteLine("Player " + (currentPlayer + 1) + ", " + player_data[currentPlayer].playerName);
                        Console.WriteLine("You are on X Position: " + player_data[currentPlayer].xCoordinates);
                        Console.WriteLine("You are on Y Position: " + player_data[currentPlayer].yCoordinates);
                        int diceRoll = ReadDice(); //find what number they rolled using the roll dice method
                        Console.WriteLine("You rolled a " + diceRoll);
                        direction = MovementCatcher(); //find out what direction the user wants to move in using the movement catcher method
                        Console.WriteLine("You are moving " + diceRoll + " spaces in a " + direction + " direction"); //display their chosen move
                        MovementCalculator(playerList: ref player_data[currentPlayer], roll: diceRoll, movement: direction); //run the movement calculation method 
                        Console.WriteLine("You are now at X: " + player_data[currentPlayer].xCoordinates + " and Y: " + player_data[currentPlayer].yCoordinates); //display new position
                        cheeseBoard = CheeseCollector(board: ref cheeseBoard, playerDetails: ref player_data[currentPlayer]); //check if the user landed on any cheese
                        bool collision = CollisionDetector(playerList: player_data, currentIndex: currentPlayer, detectedPlayer: ref collisionIndex); //run the method which checks for player collision

                        if (collision == true) //check if we had a collision
                        {
                            battleDecision = BattleDecisionMaker(opponentPlayer: collisionIndex, finalTarget: targetPlayer); //if so, run the method which gets user input if they want to battle
                        }

                        if (battleDecision.ToString().ToLower() == "y") //check the user wants to battle
                        {
                            BattleSystemTest(playerData: ref player_data, activePlayer: currentPlayer, targetPlayer: targetPlayer, cheeseData: ref cheeseBoard); //run the method which battles players
                        }
                        gameWon = FindAWinner(playerList: player_data, winnerIndex: ref winningPlayer); //run the method that checks if anyone has won the game

                        if (currentPlayer == playerAmount - 1) //checks if we're on the last player in the index
                        {
                            currentPlayer = 0; //if so, restart our loop
                        }
                        else
                        {
                            currentPlayer++; //if not, add one
                        }
                    }
                }

                PrintWinner(winnerList: winningPlayer); //run the method which prints out the winners

                Console.WriteLine("Do you wish to end the game? [Y]es or [N]o"); //get input if the user wants to end the session
                finalInput = Console.ReadLine();
                if (finalInput.ToUpper() == "Y" || finalInput.ToUpper() == "YES") //if the user is finished
                {
                    break; //exit the main method
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
                catch (System.FormatException) //when a user inputs an incorrect data, do this to stop the system from crashing
                {
                    Console.WriteLine("That's not a number!"); //throw error
                    Console.WriteLine(Environment.NewLine); //render a new line
                    continue; //restart chunk
                }


                if (numberOfPlayers < 0) //check user hasn't entered a negative value
                {
                    Console.WriteLine("Please input a number between 2 and 4!" + Environment.NewLine);
                    continue;
                }
                else if (numberOfPlayers == 0) //check user hasn't set input to 0
                {
                    Console.WriteLine("Please input a number between 2 and 4!" + Environment.NewLine);
                    continue;
                }
                else if (numberOfPlayers == 1) //check user hasn't set input to 1
                {
                    Console.WriteLine("Please input a number between 2 and 4!");
                    Console.WriteLine(Environment.NewLine);
                    continue;
                }
                else if (numberOfPlayers > 4) //check there aren't too many players
                {
                    Console.WriteLine("Please input a number between 2 and 4!", Environment.NewLine);
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
                playerInfo[0].playerStash = 0;
                //place them at opposite board ends
                playerInfo[1].xCoordinates = 7;
                playerInfo[1].yCoordinates = 7;
                playerInfo[1].playerStash = 0;
            }
            else if (playerInfo.Length == 3) //check if we have 3 players
            {
                playerInfo[0].xCoordinates = 0;
                playerInfo[0].yCoordinates = 0;
                playerInfo[0].playerStash = 0;
                //place them in an L shape across the board
                playerInfo[1].xCoordinates = 7;
                playerInfo[1].yCoordinates = 0;
                playerInfo[1].playerStash = 0;

                playerInfo[2].xCoordinates = 0;
                playerInfo[2].yCoordinates = 7;
                playerInfo[2].playerStash = 0;
            }
            else
            {
                playerInfo[0].xCoordinates = 0;
                playerInfo[0].yCoordinates = 0;
                playerInfo[0].playerStash = 0;
                //place all four players in the corners of the board
                playerInfo[1].xCoordinates = 7;
                playerInfo[1].yCoordinates = 0;
                playerInfo[1].playerStash = 0;

                playerInfo[2].xCoordinates = 0;
                playerInfo[2].yCoordinates = 7;
                playerInfo[2].playerStash = 0;

                playerInfo[3].xCoordinates = 7;
                playerInfo[3].yCoordinates = 7;
                playerInfo[3].playerStash = 0;
            }
            return playerInfo;
        }
        static int CheeseHead(player_info[] playerList)
        {
            string CheeseFace; //initialise the method variables
            bool found = false;
            int playerIndex = 0;


            do //make one big outer loop
            {
                Console.WriteLine("Who has the head which looks most like a block of cheese? "); //ask who goes first
                CheeseFace = Console.ReadLine(); //read in the input

                for (int i = 0; i < playerList.Length; i++) //loop through array
                {
                    if (playerList[i].playerName.ToLower() == CheeseFace.ToLower()) //check if the input exists in the array
                    {
                        playerIndex = i; //move it over to our catch variable
                        found = true; //set found to true
                    }
                }

                if (found == false) //check if the input was invalid
                {
                    Console.WriteLine("Error, player not found! " + Environment.NewLine); //throw error
                    continue; //restart outer loop
                }
            } while (found == false);

            return playerIndex; //return the index of the player
        }
        static int RollDice()
        {
            Random dice = new Random(); //declare a new Random Number Generator
            int spots = dice.Next(1, 7); //declare an int based in between 1 and 6
            Console.WriteLine("You rolled a " + spots);
            return spots; //return the roll
        }
        static int ReadDice()
        {
            Console.WriteLine("Input your dice roll: ");
            int diceRoll = int.Parse(Console.ReadLine()); //get the user's roll input
            return diceRoll;
        }
        static char MovementCatcher()
        {
            char direction = char.Parse("n"); //set our default value to North in case the system fails for some reason

            for (int i = 0; i < 1; i++) //initialise an outer loop
            {
                Console.WriteLine("Which direction do you want to move? N, E, S or W? "); //print available options
                try
                {
                    direction = char.Parse(Console.ReadLine()); //attempt to catch input
                }
                catch (System.FormatException) //catch any invalid formats
                {
                    Console.WriteLine("Please input a singular character. "); //throw an error
                    i = i - 1;
                    continue; //restart our loop
                }

                switch (direction.ToString().ToLower()) //check the data in our valid input
                {
                    case "n": //if one of our inputs is a valid direction, return it to our main method
                        return direction;
                    case "e":
                        return direction;
                    case "s":
                        return direction;
                    case "w":
                        return direction;
                    default: //if not
                        break;
                }
                Console.WriteLine("Error, invalid input!" + Environment.NewLine); //throw error
                i = i - 1; //restart loop
                continue;
            }
            return direction;
        }
        static void MovementCalculator(ref player_info playerList, int roll, char movement)
        {
            switch (movement.ToString().ToLower()) //check our movement direction
            {
                case "n":
                    playerList.yCoordinates = playerList.yCoordinates + roll; //if north, move up by roll amount

                    if (playerList.yCoordinates > 7) //if we go off the board
                    {
                        playerList.yCoordinates = playerList.yCoordinates - 8; //wrap around to (y + roll) - 8
                    }
                    break;
                case "s": //check if we're moving south
                    playerList.yCoordinates = playerList.yCoordinates - roll; //if so, move down on the y axis by roll

                    if (playerList.yCoordinates < 0)
                    {
                        playerList.yCoordinates = playerList.yCoordinates + 8; //wrap around to (y - roll) + 8 if we go off the board
                    }
                    break;
                case "e":
                    playerList.xCoordinates = playerList.xCoordinates + roll; //if we go east, move up the x axis by roll

                    if (playerList.xCoordinates > 7)
                    {
                        playerList.xCoordinates = playerList.xCoordinates - 8; //wrap to (x + roll) - 8 if we go off the x axis
                    }
                    break;
                case "w":
                    playerList.xCoordinates = playerList.xCoordinates - roll; //if we move west, go down the x axis by roll amount

                    if (playerList.xCoordinates < 0)
                    {
                        playerList.xCoordinates = playerList.xCoordinates + 8; //wrap to (x - roll) + 8 if we fall off the board
                    }
                    break;
            }

        }
        static bool[,] CheeseCollector(ref bool[,] board, ref player_info playerDetails)
        {
            if (board[playerDetails.xCoordinates, playerDetails.yCoordinates] == false) //check if we've landed on an empty square
            {
                Console.WriteLine("There is no cheese on this space.");
                Console.WriteLine("You do not collect any cheese this turn. "); //tell them it's empty
                return board; //return an unedited board
            }
            else //if there's cheese on this board
            {
                Console.WriteLine("There is a cheese on this space."); //tell them
                playerDetails.playerStash = playerDetails.playerStash + 1; //move the player stash up by one
                board[playerDetails.xCoordinates, playerDetails.yCoordinates] = false; //set the position to empty
                Console.WriteLine("You collect the cheese and have a new stash counter of " + playerDetails.playerStash); //tell them their new stash amount
                return board; //return our edited board
            }
        }
        static bool CollisionDetector(player_info[] playerList, int currentIndex, ref List<int> detectedPlayer)
        {
            for (int i = 0; i < playerList.Length; i++) //loop through our array
            {
                if (i == currentIndex) //check we're not on the current player
                {
                    continue; //if so, skip this iteration
                }
                else
                {
                    if (playerList[i].xCoordinates == playerList[currentIndex].xCoordinates) //check the x co-ordinates between the two players match
                    {
                        if (playerList[i].yCoordinates == playerList[currentIndex].yCoordinates) //if so, check that the y co-ordinates match
                        {
                            detectedPlayer.Add(i); //if so, move our loop iteration into our list
                        }
                    }
                }
            }

            if (detectedPlayer.Count == 0)
            {
                return false; //return that there were no collisions
            }
            else
            {
                return true; //return that there was a collision
            }
        }
        static char BattleDecisionMaker(List<int> opponentPlayer, int finalTarget)
        {
            char decision = char.Parse("u"); //initalise our variable with a default value
            do //intialise a big outer loop to get a valid input
            {
                if (opponentPlayer.Count == 1)
                {
                    Console.WriteLine("Do you wish to perform battle with Player " + (opponentPlayer[0] + 1) + " Y or N? "); //ask player if they wish to do battle
                    try
                    {
                        decision = char.Parse(Console.ReadLine()); //attempt to catch an input
                    }
                    catch (System.FormatException)
                    {
                        Console.WriteLine("Invalid input, please try again! " + Environment.NewLine); //throw an error and restart if their input is invalid
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("There are " + (opponentPlayer.Count + 1) + " players to battle."); //tell the user there were multiple ships on the same space
                    Console.Write("Please pick from Players: ");
                    for (int i = 0; i < opponentPlayer.Count; i++) //loop through and print out the players on the space
                    {
                        Console.Write(opponentPlayer[i] + 1);
                        Console.Write(", ");
                    }
                    try
                    {
                        finalTarget = int.Parse(Console.ReadLine()); //attempt to catch which player they want to battle
                    }
                    catch (System.FormatException) //catch any strings
                    {
                        Console.WriteLine("Invalid input, please try again! " + Environment.NewLine);
                        continue; //restart the chunk if the input is invalid
                    }
                    finalTarget--; //take one off so we have the actual user they wish to battle
                    bool broken = false; //set the variable that checks if we've broken out of the loop to false

                    for (int i = 0; i < opponentPlayer.Count; i++)
                    {
                        if (finalTarget - 1 == opponentPlayer[i]) //loop through and check the user has selected a valid target
                        {
                            broken = true; //break out of the loop if we have
                            break;
                        }
                    }

                    if (broken == false) //check we broke out of the loop
                    {
                        Console.WriteLine("Player selected is invalid, please try again! " + Environment.NewLine); //if not, we didn't find a user
                        continue; //so we restart the loop
                    }

                    Console.WriteLine("Are you sure you wish to do battle with Player " + (finalTarget + 1) + "?"); //check if they wish to do battle with this player
                    try
                    {
                        decision = char.Parse(Console.ReadLine()); //attempt to catch an input
                    }
                    catch (System.FormatException)
                    {
                        Console.WriteLine("Invalid input, please try again! " + Environment.NewLine); //throw an error and restart if their input is invalid
                        continue;
                    }
                }
            }
            while (decision.ToString().ToLower() != "n" && decision.ToString().ToLower() != "y"); //only break when our input is valid
            return decision; //return our input
        }
        static void BattleSystem(ref player_info[] playerData, int activePlayer, int targetPlayer, ref bool[,] cheeseData)
        {
            bool lostBattle = false; //initialise our variables
            int cheeseChange;
            int activeRoll = RollDice(); //active player rolls the dice
            Console.WriteLine("Player " + (activePlayer + 1) + " rolled a " + activeRoll);
            int targetRoll = RollDice(); //target rolls the dice
            Console.WriteLine("Player " + (targetPlayer + 1) + " rolled a " + targetRoll);
            cheeseChange = BattleLogic(playerRoll: activeRoll, opponentRoll: targetRoll, larger: ref lostBattle); //run the method which runs through the logic of the battle
            if (lostBattle == true) //if the player lost the battle, switch over the victor in the decision making
            {
                int temp = activePlayer;
                activePlayer = targetPlayer;
                targetPlayer = temp;
            }

            switch (cheeseChange)
            {
                case -2: //check if it was a poor win
                    {
                        Console.WriteLine("Poor win! Surrender a cheese to your target!");
                        if (playerData[activePlayer].playerStash == 0) //check the loser has cheese to give
                        {
                            Console.WriteLine("No cheese to be given! You escape this time!"); //if not spit out an error
                            break;
                        }
                        else
                        { //if so, fork over cheese
                            playerData[activePlayer].playerStash = playerData[activePlayer].playerStash - 1;
                            playerData[targetPlayer].playerStash = playerData[targetPlayer].playerStash + 1;
                            Console.WriteLine("Player " + (activePlayer + 1) + "'s new stash amount: " + playerData[activePlayer].playerStash);
                            Console.WriteLine("Player " + (targetPlayer + 1) + "'s new stash amount: " + playerData[targetPlayer].playerStash); //print new stash amounts
                            break; //exit out of the switch
                        }
                    }
                case -1: //check if it was an honorable defeat
                    {
                        int selectedMove;

                        Console.WriteLine("Honorable defeat! Player " + (targetPlayer + 1) + " must move to any of these adjacent spaces:");
                        Console.WriteLine("1. " + (playerData[targetPlayer].xCoordinates) + ", " + (playerData[targetPlayer].yCoordinates + 1));
                        Console.WriteLine("2. " + (playerData[targetPlayer].xCoordinates) + ", " + (playerData[targetPlayer].yCoordinates - 1));
                        Console.WriteLine("3. " + (playerData[targetPlayer].xCoordinates + 1) + ", " + (playerData[targetPlayer].yCoordinates)); //print the available moves
                        Console.WriteLine("4. " + (playerData[targetPlayer].xCoordinates - 1) + ", " + (playerData[targetPlayer].yCoordinates));
                        for (int i = 0; i < 1; i++)
                        {
                            Console.WriteLine("Please select move 1, 2, 3 or 4");
                            try
                            {
                                selectedMove = int.Parse(Console.ReadLine()); //attempt to get an integer input
                            }
                            catch (System.FormatException) //catch any exceptions
                            {
                                Console.WriteLine("Invalid format, please try again! " + Environment.NewLine); //throw an error if nessecary
                                i--; //restart the loop
                                continue;
                            }
                            if (selectedMove == 1) //check if we're moving north
                            {
                                MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("n")); //move the player up
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>();
                                occupied = CollisionDetector(playerList: playerData, currentIndex: targetPlayer, detectedPlayer: ref playersOccupying); //check for a collision
                                if (occupied == true) //if we have a collision
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine); //inform the player
                                    MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("s")); //move them back one
                                    i--; //restart the loop
                                    continue;
                                }
                                else
                                {
                                    break; //exit out of the loop
                                }
                            }
                            else if (selectedMove == 2) //if we're moving south
                            {
                                MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("s")); //move them back one
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>();
                                occupied = CollisionDetector(playerList: playerData, currentIndex: targetPlayer, detectedPlayer: ref playersOccupying); //check collisions
                                if (occupied == true) //if we have a collision
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("n")); //move them forwards and restart
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 3) //if we're moving east
                            {
                                MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("e")); //move them right
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>();
                                occupied = CollisionDetector(playerList: playerData, currentIndex: targetPlayer, detectedPlayer: ref playersOccupying); //check the space isn't occupied
                                if (occupied == true) //if it is
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("w")); //move them back and try restart the loop
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 4) //check we're moving them west
                            {
                                MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("w")); //move them west
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check if we collide with anyone in their western position
                                occupied = CollisionDetector(playerList: playerData, currentIndex: targetPlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we do
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("e")); //tell them the space is invalid and restart
                                    i--;
                                    continue;
                                }
                                else //if not
                                {
                                    break;
                                }
                            }
                            else //if the input is invalid
                            {
                                Console.WriteLine("Invalid move selected! Please try again!" + Environment.NewLine); //throw an error
                                i--; //restart the loop
                                continue;
                            }
                        }
                        cheeseData = CheeseCollector(board: ref cheeseData, playerDetails: ref playerData[targetPlayer]); //check if there's any cheese on the new space
                        break; //break out of the switch
                    }
                case 0: //check if we have a solid victory
                    {
                        int selectedMove;

                        Console.WriteLine("Solid victory! Player " + (activePlayer + 1) + " must move to any of these adjacent spaces:"); //inform the player of available moves
                        Console.WriteLine("1. " + (playerData[activePlayer].xCoordinates) + ", " + (playerData[activePlayer].yCoordinates + 1));
                        Console.WriteLine("2. " + (playerData[activePlayer].xCoordinates) + ", " + (playerData[activePlayer].yCoordinates - 1));
                        Console.WriteLine("3. " + (playerData[activePlayer].xCoordinates + 1) + ", " + (playerData[activePlayer].yCoordinates));
                        Console.WriteLine("4. " + (playerData[activePlayer].xCoordinates - 1) + ", " + (playerData[activePlayer].yCoordinates));
                        for (int i = 0; i < 1; i++)
                        {
                            Console.WriteLine("Please select move 1, 2, 3 or 4");
                            try
                            {
                                selectedMove = int.Parse(Console.ReadLine()); //check for integer input
                            }
                            catch (System.FormatException)
                            {
                                Console.WriteLine("Invalid format, please try again! " + Environment.NewLine); //throw an error and restart the loop if we don't get one
                                i--;
                                continue;
                            }
                            if (selectedMove == 1) //if we're moving north
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("n")); //move the user north
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("s"));
                                    i--; //move them back and restart the loop
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 2) //if they're moving south
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("s")); //move them south
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check if we have a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we have a collision
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("n")); //move them back and try again
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 3) //if we're moving east
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("e")); //move them east
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("w")); //move them back and restart
                                    i--;
                                    continue;
                                }
                                else //if we don't
                                {

                                    break;
                                }
                            }
                            else if (selectedMove == 4) //if we're moving west
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("w")); //move them west
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("e")); //move them back and restart
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else //if our input isn't valid
                            {
                                Console.WriteLine("Invalid move selected! Please try again!" + Environment.NewLine);
                                i--; //throw an error and restart
                                continue;
                            }
                        }
                        cheeseData = CheeseCollector(board: ref cheeseData, playerDetails: ref playerData[activePlayer]); //collect any cheese on the new position
                        break; //exit the switch case
                    }
                case 1:
                    {
                        int selectedMove;

                        Console.WriteLine("Glorious victory!");
                        if (playerData[targetPlayer].playerStash < 1)
                        {
                            Console.WriteLine("Player " + (targetPlayer + 1) + " is unable to give you any cheese, he escapes this time!");
                        }
                        else
                        {
                            playerData[activePlayer].playerStash++;
                            playerData[targetPlayer].playerStash--;
                            Console.WriteLine("Player " + (activePlayer + 1) + "'s new stash amount: " + playerData[activePlayer].playerStash);
                            Console.WriteLine("Player " + (targetPlayer + 1) + "'s new stash amount: " + playerData[targetPlayer].playerStash);
                        }
                        Console.WriteLine("Now Player " + (activePlayer + 1) + "must move to any of these co-ordinates: ");
                        Console.WriteLine("1. " + (playerData[activePlayer].xCoordinates) + ", " + (playerData[activePlayer].yCoordinates + 1));
                        Console.WriteLine("2. " + (playerData[activePlayer].xCoordinates) + ", " + (playerData[activePlayer].yCoordinates - 1));
                        Console.WriteLine("3. " + (playerData[activePlayer].xCoordinates + 1) + ", " + (playerData[activePlayer].yCoordinates));
                        Console.WriteLine("4. " + (playerData[activePlayer].xCoordinates - 1) + ", " + (playerData[activePlayer].yCoordinates));
                        for (int i = 0; i < 1; i++)
                        {
                            Console.WriteLine("Please select move 1, 2, 3 or 4");
                            try
                            {
                                selectedMove = int.Parse(Console.ReadLine());
                            }
                            catch (System.FormatException)
                            {
                                Console.WriteLine("Invalid format, please try again! " + Environment.NewLine);
                                i--;
                                continue;
                            }
                            if (selectedMove == 1) //if we're moving north
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("n")); //move the user north
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("s"));
                                    i--; //move them back and restart the loop
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 2) //if they're moving south
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("s")); //move them south
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check if we have a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we have a collision
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("n")); //move them back and try again
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 3) //if we're moving east
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("e")); //move them east
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("w")); //move them back and restart
                                    i--;
                                    continue;
                                }
                                else //if we don't
                                {

                                    break;
                                }
                            }
                            else if (selectedMove == 4) //if we're moving west
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("w")); //move them west
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("e")); //move them back and restart
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else //if our input isn't valid
                            {
                                Console.WriteLine("Invalid move selected! Please try again!" + Environment.NewLine);
                                i--; //throw an error and restart
                                continue;
                            }
                        }
                        cheeseData = CheeseCollector(board: ref cheeseData, playerDetails: ref playerData[activePlayer]); //check if there's cheese on the new space
                        break; //break out of the case switch
                    }
                case 2:
                    {
                        int selectedMove;

                        Console.WriteLine("Infamous victory!");
                        if (playerData[targetPlayer].playerStash < 2)
                        {
                            Console.WriteLine("Player " + (targetPlayer + 1) + " is unable to give you any cheese, he escapes this time!");
                        }
                        else
                        {
                            playerData[activePlayer].playerStash = playerData[activePlayer].playerStash + 2;
                            playerData[targetPlayer].playerStash = playerData[targetPlayer].playerStash - 2;
                            Console.WriteLine("Player " + (activePlayer + 1) + "'s new stash amount: " + playerData[activePlayer].playerStash);
                            Console.WriteLine("Player " + (targetPlayer + 1) + "'s new stash amount: " + playerData[targetPlayer].playerStash);
                        }
                        Console.WriteLine("Now Player " + (activePlayer + 1) + "must move to any of these co-ordinates: ");
                        Console.WriteLine("1. " + (playerData[activePlayer].xCoordinates) + ", " + (playerData[activePlayer].yCoordinates + 1));
                        Console.WriteLine("2. " + (playerData[activePlayer].xCoordinates) + ", " + (playerData[activePlayer].yCoordinates - 1));
                        Console.WriteLine("3. " + (playerData[activePlayer].xCoordinates + 1) + ", " + (playerData[activePlayer].yCoordinates));
                        Console.WriteLine("4. " + (playerData[activePlayer].xCoordinates - 1) + ", " + (playerData[activePlayer].yCoordinates));
                        for (int i = 0; i < 1; i++)
                        {
                            Console.WriteLine("Please select move 1, 2, 3 or 4");
                            try
                            {
                                selectedMove = int.Parse(Console.ReadLine());
                            }
                            catch (System.FormatException)
                            {
                                Console.WriteLine("Invalid format, please try again! " + Environment.NewLine);
                                i--;
                                continue;
                            }
                            if (selectedMove == 1) //if we're moving north
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("n")); //move the user north
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("s"));
                                    i--; //move them back and restart the loop
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 2) //if they're moving south
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("s")); //move them south
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check if we have a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we have a collision
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("n")); //move them back and try again
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 3) //if we're moving east
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("e")); //move them east
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("w")); //move them back and restart
                                    i--;
                                    continue;
                                }
                                else //if we don't
                                {

                                    break;
                                }
                            }
                            else if (selectedMove == 4) //if we're moving west
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("w")); //move them west
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("e")); //move them back and restart
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else //if our input isn't valid
                            {
                                Console.WriteLine("Invalid move selected! Please try again!" + Environment.NewLine);
                                i--; //throw an error and restart
                                continue;
                            }
                        }
                        cheeseData = CheeseCollector(board: ref cheeseData, playerDetails: ref playerData[activePlayer]); //check if there's cheese in the new position
                        break; //break out of the case switch
                    }
            }
        }
        static void BattleSystemTest(ref player_info[] playerData, int activePlayer, int targetPlayer, ref bool[,] cheeseData)
        {
            bool lostBattle = false; //initialise our variables
            int cheeseChange;
            int activeRoll = ReadDice(); //active player rolls the dice
            Console.WriteLine("Player " + (activePlayer + 1) + " rolled a " + activeRoll);
            int targetRoll = ReadDice(); //target rolls the dice
            Console.WriteLine("Player " + (targetPlayer + 1) + " rolled a " + targetRoll);
            cheeseChange = BattleLogic(playerRoll: activeRoll, opponentRoll: targetRoll, larger: ref lostBattle); //run the method which runs through the logic of the battle
            if (lostBattle == true) //if the player lost the battle, switch over the victor in the decision making
            {
                int temp = activePlayer;
                activePlayer = targetPlayer;
                targetPlayer = temp;
            }

            switch (cheeseChange)
            {
                case -2: //check if it was a poor win
                    {
                        Console.WriteLine("Poor win! Surrender a cheese to your target!");
                        if (playerData[activePlayer].playerStash == 0) //check the loser has cheese to give
                        {
                            Console.WriteLine("No cheese to be given! You escape this time!"); //if not spit out an error
                            break;
                        }
                        else
                        { //if so, fork over cheese
                            playerData[activePlayer].playerStash = playerData[activePlayer].playerStash - 1;
                            playerData[targetPlayer].playerStash = playerData[targetPlayer].playerStash + 1;
                            Console.WriteLine("Player " + (activePlayer + 1) + "'s new stash amount: " + playerData[activePlayer].playerStash);
                            Console.WriteLine("Player " + (targetPlayer + 1) + "'s new stash amount: " + playerData[targetPlayer].playerStash); //print new stash amounts
                            break; //exit out of the switch
                        }
                    }
                case -1: //check if it was an honorable defeat
                    {
                        int selectedMove;

                        Console.WriteLine("Honorable defeat! Player " + (targetPlayer + 1) + " must move to any of these adjacent spaces:");
                        Console.WriteLine("1. " + (playerData[targetPlayer].xCoordinates) + ", " + (playerData[targetPlayer].yCoordinates + 1));
                        Console.WriteLine("2. " + (playerData[targetPlayer].xCoordinates) + ", " + (playerData[targetPlayer].yCoordinates - 1));
                        Console.WriteLine("3. " + (playerData[targetPlayer].xCoordinates + 1) + ", " + (playerData[targetPlayer].yCoordinates)); //print the available moves
                        Console.WriteLine("4. " + (playerData[targetPlayer].xCoordinates - 1) + ", " + (playerData[targetPlayer].yCoordinates));
                        for (int i = 0; i < 1; i++)
                        {
                            Console.WriteLine("Please select move 1, 2, 3 or 4");
                            try
                            {
                                selectedMove = int.Parse(Console.ReadLine()); //attempt to get an integer input
                            }
                            catch (System.FormatException) //catch any exceptions
                            {
                                Console.WriteLine("Invalid format, please try again! " + Environment.NewLine); //throw an error if nessecary
                                i--; //restart the loop
                                continue;
                            }
                            if (selectedMove == 1) //check if we're moving north
                            {
                                MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("n")); //move the player up
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>();
                                occupied = CollisionDetector(playerList: playerData, currentIndex: targetPlayer, detectedPlayer: ref playersOccupying); //check for a collision
                                if (occupied == true) //if we have a collision
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine); //inform the player
                                    MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("s")); //move them back one
                                    i--; //restart the loop
                                    continue;
                                }
                                else
                                {
                                    break; //exit out of the loop
                                }
                            }
                            else if (selectedMove == 2) //if we're moving south
                            {
                                MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("s")); //move them back one
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>();
                                occupied = CollisionDetector(playerList: playerData, currentIndex: targetPlayer, detectedPlayer: ref playersOccupying); //check collisions
                                if (occupied == true) //if we have a collision
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("n")); //move them forwards and restart
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 3) //if we're moving east
                            {
                                MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("e")); //move them right
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>();
                                occupied = CollisionDetector(playerList: playerData, currentIndex: targetPlayer, detectedPlayer: ref playersOccupying); //check the space isn't occupied
                                if (occupied == true) //if it is
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("w")); //move them back and try restart the loop
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 4) //check we're moving them west
                            {
                                MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("w")); //move them west
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check if we collide with anyone in their western position
                                occupied = CollisionDetector(playerList: playerData, currentIndex: targetPlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we do
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[targetPlayer], roll: 1, movement: char.Parse("e")); //tell them the space is invalid and restart
                                    i--;
                                    continue;
                                }
                                else //if not
                                {
                                    break;
                                }
                            }
                            else //if the input is invalid
                            {
                                Console.WriteLine("Invalid move selected! Please try again!" + Environment.NewLine); //throw an error
                                i--; //restart the loop
                                continue;
                            }
                        }
                        cheeseData = CheeseCollector(board: ref cheeseData, playerDetails: ref playerData[targetPlayer]); //check if there's any cheese on the new space
                        break; //break out of the switch
                    }
                case 0: //check if we have a solid victory
                    {
                        int selectedMove;

                        Console.WriteLine("Solid victory! Player " + (activePlayer + 1) + " must move to any of these adjacent spaces:"); //inform the player of available moves
                        Console.WriteLine("1. " + (playerData[activePlayer].xCoordinates) + ", " + (playerData[activePlayer].yCoordinates + 1));
                        Console.WriteLine("2. " + (playerData[activePlayer].xCoordinates) + ", " + (playerData[activePlayer].yCoordinates - 1));
                        Console.WriteLine("3. " + (playerData[activePlayer].xCoordinates + 1) + ", " + (playerData[activePlayer].yCoordinates));
                        Console.WriteLine("4. " + (playerData[activePlayer].xCoordinates - 1) + ", " + (playerData[activePlayer].yCoordinates));
                        for (int i = 0; i < 1; i++)
                        {
                            Console.WriteLine("Please select move 1, 2, 3 or 4");
                            try
                            {
                                selectedMove = int.Parse(Console.ReadLine()); //check for integer input
                            }
                            catch (System.FormatException)
                            {
                                Console.WriteLine("Invalid format, please try again! " + Environment.NewLine); //throw an error and restart the loop if we don't get one
                                i--;
                                continue;
                            }
                            if (selectedMove == 1) //if we're moving north
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("n")); //move the user north
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("s"));
                                    i--; //move them back and restart the loop
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 2) //if they're moving south
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("s")); //move them south
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check if we have a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we have a collision
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("n")); //move them back and try again
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 3) //if we're moving east
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("e")); //move them east
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("w")); //move them back and restart
                                    i--;
                                    continue;
                                }
                                else //if we don't
                                {

                                    break;
                                }
                            }
                            else if (selectedMove == 4) //if we're moving west
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("w")); //move them west
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("e")); //move them back and restart
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else //if our input isn't valid
                            {
                                Console.WriteLine("Invalid move selected! Please try again!" + Environment.NewLine);
                                i--; //throw an error and restart
                                continue;
                            }
                        }
                        cheeseData = CheeseCollector(board: ref cheeseData, playerDetails: ref playerData[activePlayer]); //collect any cheese on the new position
                        break; //exit the switch case
                    }
                case 1:
                    {
                        int selectedMove;

                        Console.WriteLine("Glorious victory!");
                        if (playerData[targetPlayer].playerStash < 1)
                        {
                            Console.WriteLine("Player " + (targetPlayer + 1) + " is unable to give you any cheese, he escapes this time!");
                        }
                        else
                        {
                            playerData[activePlayer].playerStash++;
                            playerData[targetPlayer].playerStash--;
                            Console.WriteLine("Player " + (activePlayer + 1) + "'s new stash amount: " + playerData[activePlayer].playerStash);
                            Console.WriteLine("Player " + (targetPlayer + 1) + "'s new stash amount: " + playerData[targetPlayer].playerStash);
                        }
                        Console.WriteLine("Now Player " + (activePlayer + 1) + "must move to any of these co-ordinates: ");
                        Console.WriteLine("1. " + (playerData[activePlayer].xCoordinates) + ", " + (playerData[activePlayer].yCoordinates + 1));
                        Console.WriteLine("2. " + (playerData[activePlayer].xCoordinates) + ", " + (playerData[activePlayer].yCoordinates - 1));
                        Console.WriteLine("3. " + (playerData[activePlayer].xCoordinates + 1) + ", " + (playerData[activePlayer].yCoordinates));
                        Console.WriteLine("4. " + (playerData[activePlayer].xCoordinates - 1) + ", " + (playerData[activePlayer].yCoordinates));
                        for (int i = 0; i < 1; i++)
                        {
                            Console.WriteLine("Please select move 1, 2, 3 or 4");
                            try
                            {
                                selectedMove = int.Parse(Console.ReadLine());
                            }
                            catch (System.FormatException)
                            {
                                Console.WriteLine("Invalid format, please try again! " + Environment.NewLine);
                                i--;
                                continue;
                            }
                            if (selectedMove == 1) //if we're moving north
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("n")); //move the user north
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("s"));
                                    i--; //move them back and restart the loop
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 2) //if they're moving south
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("s")); //move them south
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check if we have a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we have a collision
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("n")); //move them back and try again
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 3) //if we're moving east
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("e")); //move them east
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("w")); //move them back and restart
                                    i--;
                                    continue;
                                }
                                else //if we don't
                                {

                                    break;
                                }
                            }
                            else if (selectedMove == 4) //if we're moving west
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("w")); //move them west
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("e")); //move them back and restart
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else //if our input isn't valid
                            {
                                Console.WriteLine("Invalid move selected! Please try again!" + Environment.NewLine);
                                i--; //throw an error and restart
                                continue;
                            }
                        }
                        cheeseData = CheeseCollector(board: ref cheeseData, playerDetails: ref playerData[activePlayer]); //check if there's cheese on the new space
                        break; //break out of the case switch
                    }
                case 2:
                    {
                        int selectedMove;

                        Console.WriteLine("Infamous victory!");
                        if (playerData[targetPlayer].playerStash < 2)
                        {
                            Console.WriteLine("Player " + (targetPlayer + 1) + " is unable to give you any cheese, he escapes this time!");
                        }
                        else
                        {
                            playerData[activePlayer].playerStash = playerData[activePlayer].playerStash + 2;
                            playerData[targetPlayer].playerStash = playerData[targetPlayer].playerStash - 2;
                            Console.WriteLine("Player " + (activePlayer + 1) + "'s new stash amount: " + playerData[activePlayer].playerStash);
                            Console.WriteLine("Player " + (targetPlayer + 1) + "'s new stash amount: " + playerData[targetPlayer].playerStash);
                        }
                        Console.WriteLine("Now Player " + (activePlayer + 1) + "must move to any of these co-ordinates: ");
                        Console.WriteLine("1. " + (playerData[activePlayer].xCoordinates) + ", " + (playerData[activePlayer].yCoordinates + 1));
                        Console.WriteLine("2. " + (playerData[activePlayer].xCoordinates) + ", " + (playerData[activePlayer].yCoordinates - 1));
                        Console.WriteLine("3. " + (playerData[activePlayer].xCoordinates + 1) + ", " + (playerData[activePlayer].yCoordinates));
                        Console.WriteLine("4. " + (playerData[activePlayer].xCoordinates - 1) + ", " + (playerData[activePlayer].yCoordinates));
                        for (int i = 0; i < 1; i++)
                        {
                            Console.WriteLine("Please select move 1, 2, 3 or 4");
                            try
                            {
                                selectedMove = int.Parse(Console.ReadLine());
                            }
                            catch (System.FormatException)
                            {
                                Console.WriteLine("Invalid format, please try again! " + Environment.NewLine);
                                i--;
                                continue;
                            }
                            if (selectedMove == 1) //if we're moving north
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("n")); //move the user north
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("s"));
                                    i--; //move them back and restart the loop
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 2) //if they're moving south
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("s")); //move them south
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check if we have a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we have a collision
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("n")); //move them back and try again
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (selectedMove == 3) //if we're moving east
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("e")); //move them east
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("w")); //move them back and restart
                                    i--;
                                    continue;
                                }
                                else //if we don't
                                {

                                    break;
                                }
                            }
                            else if (selectedMove == 4) //if we're moving west
                            {
                                MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("w")); //move them west
                                bool occupied = false;
                                List<int> playersOccupying = new List<int>(); //check for a collision
                                occupied = CollisionDetector(playerList: playerData, currentIndex: activePlayer, detectedPlayer: ref playersOccupying);
                                if (occupied == true) //if we get one
                                {
                                    Console.WriteLine("Space is already occupied, please try again! " + Environment.NewLine);
                                    MovementCalculator(playerList: ref playerData[activePlayer], roll: 1, movement: char.Parse("e")); //move them back and restart
                                    i--;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else //if our input isn't valid
                            {
                                Console.WriteLine("Invalid move selected! Please try again!" + Environment.NewLine);
                                i--; //throw an error and restart
                                continue;
                            }
                        }
                        cheeseData = CheeseCollector(board: ref cheeseData, playerDetails: ref playerData[activePlayer]); //check if there's cheese in the new position
                        break; //break out of the case switch
                    }
            }
        }
        static int BattleLogic(int playerRoll, int opponentRoll, ref bool larger)
        {
            int rollDifference;
            int score = 0;

            if (playerRoll < opponentRoll) //check if the opponent scored more
            {
                larger = true; //set our larger boolean to true
            }

            if (larger == true) //if the opponent rolled more
            {
                rollDifference = opponentRoll; //use the opponent score
            }
            else
            {
                rollDifference = playerRoll; //if not, use the player score
            }

            switch (rollDifference) //set up a case switch for the different scores
            {
                case 2: //if it's a poor win
                    score = -2; //set our score to -2
                    break;
                case 3: //if it's an honourable defeat
                    score = -1; //set the score to -1
                    break;
                case 4: //if it's a solid victory
                    score = 0; //set the score to 0
                    break;
                case 5: //if it's a glorious victory
                    score = 1; //set the score to 1
                    break;
                case 6: //if it's an infamous victory
                    score = 2; //set the score to 2
                    break;
            }
            return score; //return the score
        }
        static bool FindAWinner(player_info[] playerList, ref List<int> winnerIndex)
        {
            for (int i = 0; i < playerList.Length; i++) //loop through our structure
            {
                if (playerList[i].playerStash >= 6) //if a player has a stash of equal or more than 6
                {
                    winnerIndex.Add(i); //add them to the winner's list
                }
            }
            if (winnerIndex.Count == 0) //if there's no one in the list
            {
                return false; //no one won
            }
            else //if there is
            {
                return true; //someone won
            }
        }
        static void PrintWinner(List<int> winnerList)
        {
            Console.WriteLine("Game over!"); //tell the users the game is over
            if (winnerList.Count == 1) //if there was just one winner
            {
                Console.WriteLine("Your winner is: "); //print the winner
                Console.Write("Player " + (winnerList[0] + 1));
            }
            else //if there was more than one winner
            {
                Console.WriteLine("There has been a tie!"); //inform the users who tied
                Console.WriteLine("Your winners are: ");
                for (int i = 0; i < winnerList.Count; i++)
                {
                    Console.Write("Player " + (winnerList[i] + 1));
                    Console.Write(", ");
                }
            }
        }
    }
}

