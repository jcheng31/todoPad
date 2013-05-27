using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoPad.ViewModels
{
    class SettingsViewModel : ObservableObject
    {
        public List<FontFamily> FontFamilies { get; set; }

        public SettingsViewModel()
        {
            InstalledFontCollection fonts = new InstalledFontCollection();
            FontFamilies = new List<FontFamily>();
            foreach (FontFamily font in fonts.Families)
            {
                FontFamilies.Add(font);
            }
        }
    }
}
