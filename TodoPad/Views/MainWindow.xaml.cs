using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;
using TodoPad.Models;
using TodoPad.Task_Parser;

namespace TodoPad.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskFile currentFile;

        public MainWindow()
        {
            InitializeComponent();
            currentFile = new TaskFile(null, "");
        }

        private void DocumentBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            RichTextBox textBox = (RichTextBox)sender;
            //textBox.Document.PageWidth = 5120;

            // Remove this handler so we don't trigger it when formatting.
            textBox.TextChanged -= DocumentBoxTextChanged;

            // Get the line that was changed.
            foreach (TextChange currentChange in e.Changes)
            {
                TextPointer offSet = textBox.Document.ContentStart.GetPositionAtOffset(currentChange.Offset, LogicalDirection.Forward);

                if (offSet != null)
                {
                    Paragraph currentParagraph = offSet.Paragraph;

                    if (offSet.Parent == textBox.Document && currentChange.AddedLength > currentChange.RemovedLength)
                    {
                        FormatDocument();
                    }
                    else if (currentParagraph != null)
                    {
                        FormatParagraph(currentParagraph);
                    }
                }
            }

            // Restore this handler.
            textBox.TextChanged += DocumentBoxTextChanged;
        }

        private void FormatDocument()
        {
            Paragraph currentParagraph = TextBox.Document.Blocks.FirstBlock as Paragraph;

            while (currentParagraph != null)
            {
                FormatParagraph(currentParagraph);
                currentParagraph = currentParagraph.NextBlock as Paragraph;
            }
        }

        private static void FormatParagraph(Paragraph currentParagraph)
        {
            // Get the text on this row.
            TextPointer start = currentParagraph.ContentStart;
            TextPointer end = currentParagraph.ContentEnd;
            TextRange currentTextRange = new TextRange(start, end);

            // Parse the row.
            Row currentRow = new Row(currentTextRange.Text);

            // Format the displayed text.
            TaskParser.FormatTextRange(currentTextRange, currentRow);
        }

        private void OpenMenuItemClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            fileDialog.Filter = "Text Documents (*.txt)|*.txt|All Files (*.*)|*.*";


            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    using (StreamReader reader = new StreamReader(fileDialog.OpenFile()))
                    {
                        String contents = reader.ReadToEnd();
                        String filePath = fileDialog.FileName;

                        currentFile = new TaskFile(filePath, contents);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }

        }
    }
}
