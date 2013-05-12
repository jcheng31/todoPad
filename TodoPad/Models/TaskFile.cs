using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoPad.Models
{
    class TaskFile
    {
        public String Path { get; set; }
        public List<Row> Rows { get; set; }

        public TaskFile(String path, String contents)
        {
            Path = path;

            String[] lines = contents.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            Rows = new List<Row>(lines.Length);

            foreach (string currentLine in lines)
            {
                Row currentRow = new Row(currentLine);
                Rows.Add(currentRow);
            }
        }
    }
}
