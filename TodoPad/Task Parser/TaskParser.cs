using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace TodoPad.Task_Parser
{
    class TaskParser
    {
        public static void ParseTask(TextRange taskRange)
        {
            taskRange.ClearAllProperties();
            // Get the text contained on this line, and parse it naively.
            String[] taskComponents = taskRange.Text.Split();
            TextPointer start = taskRange.Start;

            if (taskComponents.Length > 0)
            {
                // (Rules from https://github.com/ginatrapani/todo.txt-cli/wiki/The-Todo.txt-Format)

                // Rule 1: If priority exists, it ALWAYS appears first.
                Regex priorityRegex = new Regex("^\\([A-Z]\\)$");
                if (priorityRegex.IsMatch(taskComponents[0]))
                {
                    TextRange priorityRange = new TextRange(start.GetPositionAtOffset(0), GetTextPointAtOffset(start, 2));
                    priorityRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                } 
                else if (taskComponents[0] == "x")
                {
                    taskRange.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
                    taskRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Gray);
                }
            }
        }

        private static TextPointer GetTextPointAtOffset(TextPointer start, int characterOffset)
        {
            TextPointer returnValue = start;
            int count = 0;
            
            while (count < characterOffset && returnValue != null)
            {
                if (returnValue.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text ||
                    returnValue.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.None)
                {
                    count++;
                }

                if (returnValue.GetPositionAtOffset(1, LogicalDirection.Forward) == null)
                {
                    return returnValue;
                }
                else
                {
                    returnValue = returnValue.GetPositionAtOffset(1, LogicalDirection.Forward);
                }
            }

            return returnValue;
        }
    }
}
