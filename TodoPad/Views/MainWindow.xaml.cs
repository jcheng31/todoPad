using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TodoPad.Task_Parser;

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
            
            // Remove this handler so we don't trigger it when formatting.
            textBox.TextChanged -= DocumentBoxTextChanged;

            Paragraph currentLine = textBox.Document.Blocks.FirstBlock as Paragraph;
            while (currentLine != null)
            {
                TextRange currentLineRange = new TextRange(currentLine.ContentStart, currentLine.ContentEnd);
                TaskParser.ParseTask(currentLineRange);

                currentLine = currentLine.NextBlock as Paragraph;
            }

            //foreach (Paragraph currentLine in textBox.Document.Blocks)
            //{
            //    TextRange currentLineRange = new TextRange(currentLine.ContentStart, currentLine.ContentEnd);
            //    TaskParser.ParseTask(currentLineRange);
            //}

            // Restore this handler.
            textBox.TextChanged += DocumentBoxTextChanged;
        }
    }
}
