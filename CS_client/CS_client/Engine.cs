using System;
using System.IO;
using System.Collections.Generic;

namespace Ants
{
    public static class Engine
    {
        private const string READY = "ready";
        private const string GO = "go";
        private const string END = "end";

        private static GameState state;

        public static void PlayGame(Bot bot)
        {
            List<string> input = new List<string>();

            try
            {
                while (true)
                {
                    string line = System.Console.In.ReadLine().Trim().ToLower();

                    switch (line)
                    {
                        case READY:
                            ParseSetup(input);
                            bot.Init();
                            input.Clear();
                            FinishTurn();
                            break;
                        case GO:
                            state.StartNewTurn();
                            ParseUpdate(input);
                            bot.DoTurn(state);
                            input.Clear();
                            FinishTurn();
                            break;
                        case END:
                            return;
                        default:
                            input.Add(line);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
#if DEBUG
                StreamWriter sw = File.CreateText("Exception_Debug_" + DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss") + ".log");
                sw.WriteLine(e);
                sw.Close();
#endif
            }

        }

        // parse initial input and setup starting game state
        private static void ParseSetup(List<string> input)
        {
            int mapWidth = 0, mapHeight = 0;
            int totalNumberOfTurns = 0, turntime = 0, loadtime = 0;
            int viewradius2 = 0, attackradius2 = 0, spawnradius2 = 0;
            int playerSeed = 0;

            foreach (string line in input)
            {
                if (line.Length <= 0) continue;

                string[] tokens = line.Split();
                string key = tokens[0];

                switch (key)
                {
                    case "cols":
                        mapWidth = int.Parse(tokens[1]);
                        break;
                    case "rows":
                        mapHeight = int.Parse(tokens[1]);
                        break;
                    case "turn":
                        // We don't care about the first turn during setup
                        break;
                    case "turns":
                        totalNumberOfTurns = int.Parse(tokens[1]);
                        break;
                    case "turntime":
                        turntime = int.Parse(tokens[1]);
                        break;
                    case "loadtime":
                        loadtime = int.Parse(tokens[1]);
                        break;
                    case "viewradius2":
                        viewradius2 = int.Parse(tokens[1]);
                        break;
                    case "attackradius2":
                        attackradius2 = int.Parse(tokens[1]);
                        break;
                    case "spawnradius2":
                        spawnradius2 = int.Parse(tokens[1]);
                        break;
                    case "player_seed":
                        playerSeed = int.Parse(tokens[1]);
                        break;
                    default:
                        throw new Exception("Unknown setup input token: '" + key + "'");
                }
            }

            state = new GameState(mapWidth, mapHeight, totalNumberOfTurns, turntime, loadtime, viewradius2, attackradius2, spawnradius2, playerSeed);
        }

        // parse engine input and update the game state
        private static void ParseUpdate(List<string> input)
        {
            foreach (string line in input)
            {
                if (line.Length <= 0) continue;

                string[] tokens = line.Split();

                if (tokens.Length >= 3)
                {
                    int row = int.Parse(tokens[1]);
                    int col = int.Parse(tokens[2]);

                    switch (tokens[0])
                    {
                        case "a":
                            state.AddAnt(row, col, int.Parse(tokens[3]));
                            break;
                        case "f":
                            state.AddFood(row, col);
                            break;
                        case "r":
                            state.RemoveFood(row, col);
                            break;
                        case "w":
                            state.AddWater(row, col);
                            break;
                        case "d":
                            state.DeadAnt(row, col);
                            break;
                        case "h":
                            state.AntHill(row, col, int.Parse(tokens[3]));
                            break;
                        default:
                            throw new Exception("Unknown update input token: '" + tokens[0] + "'");
                    }
                }
                else if (tokens[0] == "turn")
                    state.SetTurn(int.Parse(tokens[1]));

                else
                    throw new Exception("Unknown or invalid update input token: '" + line + "'");
            }
        }

        private static void FinishTurn()
        {
            System.Console.Out.WriteLine(GO);
        }
    }
}