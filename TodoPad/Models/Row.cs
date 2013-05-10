using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TodoPad.Models
{
    public class Row
    {
        // The actual text contained on this row.
        public String RowText;

        // Ranges for various properties.
        // These are the indices of the start and end of each characteristic.
        public Tuple<int, int> PriorityRange;

        public Tuple<int, int> CompletionDateRange;

        public Tuple<int, int> CreationDateRange;

        public List<Tuple<int, int>> ContextRanges;
        public List<Tuple<int, int>> ProjectRanges;

        // Actual characteristics found within the task.
        public DateTime CompletionDate;
        public DateTime CreationDate;
        public List<String> Projects;
        public List<String> Contexts;

        public bool IsCompleted
        {
            get { return CompletionDateRange != null; }
        }

        public bool HasText
        {
            get { return RowText != ""; }
        }

        public bool HasPriority
        {
            get { return PriorityRange != null; }
        }

        public bool HasProjects
        {
            get { return Projects.Count > 0; }
        }

        public bool HasContexts
        {
            get { return Contexts.Count > 0; }
        }

        public Row(String task)
        {
            if (task == "") return;

            RowText = task;

            String[] components = task.Split();

            // Check if the task is completed.
            const string dateFormat = "yyyy-MM-dd";
            if (task.StartsWith("x") && components.Length > 1)
            {
                // There must be a completion date.
                // (If there isn't, the task isn't properly formatted.)
                bool isCompletionPresent = DateTime.TryParseExact(components[1], dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out CompletionDate);
                if (isCompletionPresent)
                {
                    CompletionDateRange = new Tuple<int, int>(2, 12);
                }
            }
            else
            {
                // Check if there's a priority.
                Regex priorityRegex = new Regex("^\\([A-Z]\\)");
                Match priorityMatch = priorityRegex.Match(task);
                if (priorityMatch.Success)
                {
                    PriorityRange = new Tuple<int, int>(priorityMatch.Index, priorityMatch.Index + priorityMatch.Length);
                }

                // Check for a creation date. We just match the first date 
                // (since a creation date is supposed to come immediately after 
                // priority, or is the first thing on the line.
                Regex dateRegex = new Regex("\\d{4}-[0|1][0-9]-[0-3][1-9]");
                Match creationMatch = dateRegex.Match(task);
                if (creationMatch.Success)
                {
                    CreationDateRange = new Tuple<int, int>(creationMatch.Index, creationMatch.Index + creationMatch.Length);
                    DateTime.TryParseExact(task.Substring(creationMatch.Index, creationMatch.Length), dateFormat,
                                           CultureInfo.InvariantCulture, DateTimeStyles.None, out CreationDate);
                }

                // Check if we have any projects/contexts.
                Contexts = new List<string>();
                ContextRanges = new List<Tuple<int, int>>();
                Projects = new List<string>();
                ProjectRanges = new List<Tuple<int, int>>();

                Regex projectRegex = new Regex("@\\w+"); // Match any word beginning with "@"
                Regex contextRegex = new Regex("\\+\\w+"); // Match any word beginning with "+";

                foreach (Match match in projectRegex.Matches(task))
                {
                    string project = match.Value;
                    Tuple<int, int> indices = new Tuple<int, int>(match.Index, match.Index + match.Length);

                    Projects.Add(project);
                    ProjectRanges.Add(indices);
                }

                foreach (Match match in contextRegex.Matches(task))
                {
                    string context = match.Value;
                    Tuple<int, int> indices = new Tuple<int, int>(match.Index, match.Index + match.Length);

                    Contexts.Add(context);
                    ContextRanges.Add(indices);
                }
            }
        }
    }
}
