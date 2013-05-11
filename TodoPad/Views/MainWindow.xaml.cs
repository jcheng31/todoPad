using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TodoPad.Models;
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
            RichTextBox textBox = (RichTextBox)sender;

            // Remove this handler so we don't trigger it when formatting.
            textBox.TextChanged -= DocumentBoxTextChanged;

            Paragraph currentParagraph = textBox.Document.Blocks.FirstBlock as Paragraph;
            while (currentParagraph != null)
            {
                // Get the text on this row.
                TextPointer start = currentParagraph.ContentStart;
                TextPointer end = currentParagraph.ContentEnd;
                TextRange currentTextRange = new TextRange(start, end);

                // Parse the row.
                Row currentRow = new Row(currentTextRange.Text);

                // Format the displayed text.
                TaskParser.FormatTextRange(currentTextRange, currentRow);

                currentParagraph = currentParagraph.NextBlock as Paragraph;
            }

            // Restore this handler.
            textBox.TextChanged += DocumentBoxTextChanged;
        }
    }
}
