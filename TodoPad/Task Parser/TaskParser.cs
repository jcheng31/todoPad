using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using TodoPad.Models;

namespace TodoPad.Task_Parser
{
    class TaskParser
    {
        public static void FormatTextRange(TextRange range, Row row)
        {
            range.ClearAllProperties();

            if (!row.HasText)
            {
                return;
            }
            
            if (row.IsCompleted)
            {
                range.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Gray);
                range.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
                return;
            }

            TextPointer start = range.Start;
            
            if (row.HasPriority)
            {
                TextPointer priorityStart = GetTextPointAtOffset(start, row.PriorityRange.Item1);
                TextPointer priorityEnd = GetTextPointAtOffset(start, row.PriorityRange.Item2);
                TextRange priorityRange = new TextRange(priorityStart, priorityEnd);

                priorityRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                priorityRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
            }

            if (row.HasCreationDate)
            {
                TextPointer dateStart = GetTextPointAtOffset(start, row.CreationDateRange.Item1);
                TextPointer dateEnd = GetTextPointAtOffset(start, row.CreationDateRange.Item2);
                TextRange dateRange = new TextRange(dateStart, dateEnd);

                dateRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                dateRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.DarkGreen);
            }

            if (row.HasContexts)
            {
                foreach (Tuple<int, int> contextRange in row.ContextRanges)
                {
                    TextPointer contextStart = GetTextPointAtOffset(start, contextRange.Item1);
                    TextPointer contextEnd = GetTextPointAtOffset(start, contextRange.Item2);
                    TextRange contextTextRange = new TextRange(contextStart, contextEnd);

                    contextTextRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                    contextTextRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.DarkCyan);
                }
            }

            if (row.HasProjects)
            {
                foreach (Tuple<int, int> projectRange in row.ProjectRanges)
                {
                    TextPointer projectStart = GetTextPointAtOffset(start, projectRange.Item1);
                    TextPointer projectEnd = GetTextPointAtOffset(start, projectRange.Item2);
                    TextRange projectTextRange = new TextRange(projectStart, projectEnd);

                    projectTextRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                    projectTextRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.SaddleBrown);
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
