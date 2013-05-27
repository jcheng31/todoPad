using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TodoPad.ViewModels
{
    class SettingsViewModel : ObservableObject
    {
        public List<FontFamily> FontFamilies { get; set; }

        public SettingsViewModel()
        {
            InstalledFontCollection fonts = new InstalledFontCollection();
            FontFamilies = fonts.Families;
        }
    }
}
