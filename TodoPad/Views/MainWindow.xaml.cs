using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace TodoPad.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DocumentBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            RichTextBox textBox = (RichTextBox) sender;
            foreach (Paragraph currentLine in textBox.Document.Blocks)
            {
                TextRange currentLineRange = new TextRange(currentLine.ContentStart, currentLine.ContentEnd);
                String currentLineText = currentLineRange.Text;

            }
        }
    }
}
