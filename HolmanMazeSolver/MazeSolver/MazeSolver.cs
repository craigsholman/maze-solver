// MazeSolver.cs in project MazeSolver in solution HolmanMazeSolver
// Craig Holman

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HolmanMazeSolver
{
    // This class is used to solve two-dimensional mazes. The five symbols that may
    // participate in a maze or in the representation of its solution are properties
    // of the class that must be set prior to a call to SolveMaze(). This set of 
    // symbols are checked for distinctness prior to an attempt to solve the maze. 

    // The maze is checked for being valid prior toan attempt to solve the maze.
    // When the maze is solved, a solution to the maze is set as a property having 
    // the same format as the maze but using the specified Path symbol to mark the
    // solution path from the Start symbol to a Goal sysmbol. 
    
    // The number of steps on the solution path may be retrieved as a property. 
    // Information on the validity of the set of symbols and the validity of the maze, 
    // as well as any error message, are also available as properties.

    public class MazeSolver
    {
        // The following properties need to be set before SolveMaze() is called

        public char GoalSymbol { get; set; }
        public char OpenSymbol { get; set; }
        public char PathSymbol { get; set; }
        public char StartSymbol { get; set; }
        public char WallSymbol { get; set; }

        // The following properties are given values by calls to SolveMaze() 

        public string ErrorMessage { get; set; }
        public bool MazeIsValid { get; private set; }
        public string Solution { get; private set; }
        public bool SolutionFound { get; private set; }
        public bool SymbolsAreValid { get; private set; }

        public int NumSolutionSteps
        {
            get
            {
                return SolutionFound ? Solution.Count(x => x == PathSymbol) + 1 : 0;
            }
        }

        // Constructors

        public MazeSolver()
        {
            GoalSymbol = ' ';
            OpenSymbol = ' ';
            PathSymbol = ' ';
            StartSymbol = ' ';
            WallSymbol = ' ';
        }

        // Public methods

        // This method attempts to solve a maze. The maze is specified by the
        // parameter string. Prior to this method being called, the following
        // five properties must have been set by the caller: GoalSymbol,
        // Open Symbol. PathSymbol, StallSymbol, WallSymbol. The PathSymbol
        // may not occur in the maze but is used to represent a path from
        // the StartSymbol to a GoalSymbol in the solved maze.

        public void SolveMaze(string mazeAsString)
        {
            ErrorMessage = "";
            Solution = "";
            SolutionFound = false;

            try
            {
                if ( SymbolsValidated() ) 
                {
                    ExtractMazeFromString(mazeAsString);

                    if ( MazeValidated(mazeAsString) && 
                         GoalFound(_startRow, _startColumn) )
                    {
                        Solution = SolutionStringFromMaze(_maze);
                        SolutionFound = true;
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorMessage = String.Format("An internal error occurred: {0} {1}",
                                             exception.Message, exception.StackTrace);
            }
        }

        // Private methods

        // This function determines whether a cell location is on the maze

        private bool CellIsOnMaze(int cellRow, int cellColumn)
        {
            return 0 <= cellRow && cellRow < _numRows &&
                   0 <= cellColumn && cellColumn < _numColumns;
        }

        // This function determines whether a cell contains a visitable symbol,
        // one of StartSymbol, GoalSymbol, or OpenSymbol

        private bool CellIsVisitable(int cellRow, int cellColumn)
        {
            return _visitableSymbols.Contains(_maze[cellRow][cellColumn]);
        }

        // This method parses a string and constructs an object representing a
        // maze. An array of char is used to represent a row of the maze rather
        // than a string because cells in the row may need to be modified and
        // strings are immutable. The location of the Start symbol is recorded
        // while the maze is being constructed.

        private void ExtractMazeFromString(string mazeAsString)
        {
            _maze = new List<char[]>();

            foreach ( string mazeRowString in Regex.Split(mazeAsString, "\r\n|\r|\n") )
            {
                int startSymbolIndex = mazeRowString.IndexOf(StartSymbol);

                if ( startSymbolIndex >= 0 )
                {
                    _startRow = _maze.Count();
                    _startColumn = startSymbolIndex;
                }

                _maze.Add(mazeRowString.ToArray());
            }

            _numColumns = _maze[0].GetLength(0);
            _numRows = _maze.Count();
        }

        // This function recursively searches for a cell containing the Goal symbol.
        // It is first called on the cell containing the Start symbol. If it
        // discovers a path from the Start symbol to a Goal symbol, all cells on
        // that path are marked with the Path symbol. Cells containing the Open
        // symbol that are visited by the function are marked by the Visited symbol
        // so that they won't be visited again.

        // This function returns true if the cell contains the Goal symbol or if
        // any of the recursive GoalFound() calls returns true.

        private bool GoalFound(int cellRow, int cellColumn)
        {
            // Cells are visitable if they contain StartSymbol, OpenSymbol, 
            // or Goal symbol

            if ( CellIsOnMaze(cellRow, cellColumn) &&
                 CellIsVisitable(cellRow, cellColumn) )
            {
                if ( _maze[cellRow][cellColumn] == GoalSymbol )
                {
                    return true;
                }
                else
                {
                    if ( _maze[cellRow][cellColumn] == OpenSymbol )
                    {
                        // Mark the cell as visited so that it won't be 
                        // visited again

                        _maze[cellRow][cellColumn] = _openVisitedSymbol;
                    }

                    // This condition short circuits as soon as a cell with the
                    // Goal symbol has been found

                    if ( GoalFound(cellRow - 1, cellColumn) ||
                         GoalFound(cellRow, cellColumn + 1) ||
                         GoalFound(cellRow + 1, cellColumn) ||
                         GoalFound(cellRow, cellColumn - 1) )
                    {
                        if ( _maze[cellRow][cellColumn] == _openVisitedSymbol )
                        {
                            // mark the cell as being on the solution path

                            _maze[cellRow][cellColumn] = PathSymbol;
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        // This function validates that the maze-as-string satisfies several 
        // requirements of a maze.

        private bool MazeValidated(string mazeAsString)
        {
            if ( _numRows >= 1 &&
                 _numColumns >= 1 &&
                 mazeAsString.Count(x => x == GoalSymbol) > 0 &&
                 mazeAsString.Count(x => x == OpenSymbol) > 0 &&
                 mazeAsString.Count(x => x == StartSymbol) == 1 &&
                 mazeAsString.Count(x => x == WallSymbol) > 0 )
            {
                MazeIsValid = true;
            }
            else
            {
                ErrorMessage = "The maze must contain at least one Goal, Open, and Wall symbol, and exactly 1 Start symbol.";
                MazeIsValid = false;
            }

            return MazeIsValid;
        }

        // This function constructs a string representing a solution of the maze
        // from the solved maze.

        private string SolutionStringFromMaze(List<char[]> maze)
        {
            StringBuilder solutionStringBuilder = new StringBuilder();

            int row = 0;

            foreach (char[] mazeRow in maze)
            {
                foreach (char cell in mazeRow)
                {
                    solutionStringBuilder.Append(cell);
                }

                if ( ++row < maze.Count )
                {
                    solutionStringBuilder.AppendLine();
                }
            }

            return solutionStringBuilder.ToString().Replace(_openVisitedSymbol, OpenSymbol);
        }

        // This function verifies that the five maze symbols specified by the caller
        // are distinct. It also selects another symbol that is not one of these five
        // to use as the OpenVisited symbol, used to mark a cell that contained the
        // Open symbol as having been visited.

        private bool SymbolsValidated()
        {
            List<char> symbols = new List<char>() { GoalSymbol, OpenSymbol, PathSymbol, StartSymbol, WallSymbol };

            if ( symbols.Distinct().Count() == symbols.Count() )
            {
                char symbol;

                for ( symbol = 'v'; symbols.Contains(symbol); symbol = (char)((int)symbol + 1) )
                    ;

                _openVisitedSymbol = symbol;

                _visitableSymbols = new List<char>() { GoalSymbol, OpenSymbol, StartSymbol };

                SymbolsAreValid = true;
            }
            else
            {
                ErrorMessage = "The Goal, Open, Path, Start, and Wall symbols must be distinct";
                SymbolsAreValid = false;
            }

            return SymbolsAreValid;
        }

        // Private data members

        private int _numColumns;
        private int _numRows;
        private char _openVisitedSymbol;
        private List<char[]> _maze;
        private int _startColumn;
        private int _startRow;
        private List<char> _visitableSymbols;
    }
}