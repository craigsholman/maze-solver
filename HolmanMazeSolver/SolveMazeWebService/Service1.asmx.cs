// Service1.asmx.cs in project SolveMazeWebService in solution HolmanMazeSolver
// Craig Holman

using System;
using System.Text;
using System.Web;
using System.Web.Services;
using HolmanMazeSolver;

namespace MazeSolverWebService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "HolmanMazeSolver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {
        [WebMethod]
        public string SolveMaze(string mazeAsString)
        {
            MazeSolver solver = new MazeSolver();

            // Set the symbols that may occur in the maze

            solver.GoalSymbol = 'B';
            solver.OpenSymbol = '.';
            solver.StartSymbol = 'A';
            solver.WallSymbol = '#';

            // Set the additional symbol that may occur in a solution
            // to the maze

            solver.PathSymbol = '@';

            string errorMessage = "";

            try
            {
                // Information on the solution attempt may retrieved
                // from properties of the solver object after the
                // SolveMaze() call completes

                solver.SolveMaze(mazeAsString);

                errorMessage = solver.ErrorMessage;
            }
            catch (Exception exception)
            {
                errorMessage = String.Format("Exception: {0} {1}", exception.Message,
                                             exception.StackTrace);
            }

            // Construct the JSON string that is to be returned

            StringBuilder jsonBuilder = new StringBuilder();

            // Since the task specification didn't request any error
            // reporting, I extended the JSON response to include four
            // items related to error reporting.

            jsonBuilder.AppendLine("{");
            jsonBuilder.Append("   \"error message\": \"");
            jsonBuilder.Append(errorMessage);
            jsonBuilder.AppendLine("\"");
            jsonBuilder.Append("   \"symbols are valid\": ");
            jsonBuilder.AppendLine(solver.SymbolsAreValid.ToString());
            jsonBuilder.Append("   \"maze is valid\": ");
            jsonBuilder.AppendLine(solver.MazeIsValid.ToString());
            jsonBuilder.Append("   \"solution found\": ");
            jsonBuilder.AppendLine(solver.SolutionFound.ToString());
            jsonBuilder.Append("   \"steps\": ");
            jsonBuilder.AppendLine(solver.NumSolutionSteps.ToString());
            jsonBuilder.Append("   \"solution\": \"");
            jsonBuilder.Append(solver.Solution);
            jsonBuilder.AppendLine("\"");
            jsonBuilder.AppendLine("}");

            return jsonBuilder.ToString();
        }
    }
}