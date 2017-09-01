// Program.cs in project <azeSolverTester in solution HolmanMazeSolver
// Craig Holman

using System;
using System.Diagnostics;
using System.IO;
using HolmanMazeSolver;

namespace MazeSolverTester
{
    // This program is a console application that attempts to solve the maze 
    // contained in the file that is specified in the command line. It makes
    // a call to an instance of the MazeSolver class to accomplish this.

    class Program
    {
        static void Main(string[] args)
        {
            // The command line must contain one argument, the full
            // path and name of a file. It is recommended to
            // wrap this argument in double-quotes to avoid misparsing.

            if ( args.LongLength == 1 )
            {
                try
                {
                    string filePathName = args[0];

                    if ( File.Exists(filePathName) )
                    {
                        string mazeAsString = File.ReadAllText(filePathName);

                        Console.WriteLine(mazeAsString);

                        MazeSolver solver = new MazeSolver();

                        // Set the symbols that may occur in the maze

                        solver.GoalSymbol = 'B';
                        solver.OpenSymbol = '.';
                        solver.StartSymbol = 'A';
                        solver.WallSymbol = '#';

                        // Set the additional symbol that may occur in a solution
                        // to the maze

                        solver.PathSymbol = '@';

                        Stopwatch stopwatch = new Stopwatch();

                        stopwatch.Start();

                        solver.SolveMaze(mazeAsString);

                        stopwatch.Stop();

                        Console.WriteLine();
                        Console.WriteLine(solver.Solution);
                        Console.WriteLine();

                        if ( solver.ErrorMessage.Length > 0 )
                        {
                            Console.WriteLine("Error message: {0}", solver.ErrorMessage);
                        }

                        Console.WriteLine("Symbols are {0}", solver.SymbolsAreValid
                                                             ? "valid" : "not valid");

                        if ( solver.SymbolsAreValid )
                        {
                            Console.WriteLine("Maze is {0}", solver.MazeIsValid 
                                                             ? "valid" : "not valid");

                            if ( solver.MazeIsValid )
                            {
                                Console.WriteLine("A solution was {0}", solver.SolutionFound 
                                                                        ? "found" : "not found");
                                if ( solver.SolutionFound )
                                {
                                    Console.WriteLine("Number of solution steps = {0}", 
                                                      solver.NumSolutionSteps);
                                }
                            }
                        }

                        Console.WriteLine("Elapsed time: {0} milliseconds", 
                                          stopwatch.ElapsedMilliseconds);
                    }
                    else
                    {
                        Console.WriteLine("File '{0}' does not exist", filePathName);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Exception: {0} {1}",
                                      exception.Message, exception.StackTrace);
                }

                Console.WriteLine();
                Console.Write("Press any key to exit the program... ");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Usage: mazeFilePathName");
            }
        }
    }
}
