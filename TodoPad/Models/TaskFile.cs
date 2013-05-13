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
        public String[] Rows { get; set; }

        public TaskFile(String path, String contents)
        {
            Path = path;
            Rows = contents.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }
    }
}
