// Service1.asmx.cs in project SolveMazeClient in solution HolmanMazeSolver
// Craig Holman

// Note that this file was copied from an earlier solution in which the web
// service worked properly. I managed to break that solution while attempting
// to change the name of the endpoint and was not able to repair it. After
// creating a new solution and copying the files that I wrote to it, I was
// unable to create a web reference successfully. I commented out the lines
// that depended on this web reference.

using System;
using System.Diagnostics;
using System.IO;
//using ConsoleApplication1.localhost;

namespace MazeSolverWebServiceConsumer
{
    // This program is a console application that attempts to solve the maze 
    // contained in the file that is specified in the command line. It calls
    // a web service to accomplish this.

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
                        Console.WriteLine();

                        Stopwatch stopwatch = new Stopwatch();

                        /*
                        Service1 myApp = new Service1();
                        
                        stopwatch.Start();

                        string jsonResult = myApp.SolveMaze(mazeAsString);
                        
                        stopwatch.Stop();

                        Console.WriteLine(jsonResult);
                        */

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

            Console.WriteLine();
            Console.Write("Press any key to exit the program... ");
            Console.ReadKey();
        }
    }
}