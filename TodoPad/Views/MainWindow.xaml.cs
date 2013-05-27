using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using TodoPad.Models;
using TodoPad.Task_Parser;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using RichTextBox = System.Windows.Controls.RichTextBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace TodoPad.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskFile _currentFile;
        
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand OpenCommand = new RoutedCommand();

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            _currentFile = new TaskFile(null, "");

            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            OpenCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
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

                    // We're intentionally checking reference equality here.
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

        private void OpenCommandHandler(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void OpenFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Filter = "Text Documents (*.txt)|*.txt|All Files (*.*)|*.*"
                };


            if (fileDialog.ShowDialog() != true) return;
            try
            {
                using (StreamReader reader = new StreamReader(fileDialog.OpenFile()))
                {
                    String contents = reader.ReadToEnd();
                    String filePath = fileDialog.FileName;

                    _currentFile = new TaskFile(filePath, contents);
                    UpdateDocumentContents();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void UpdateDocumentContents()
        {
            FlowDocument document = new FlowDocument();

            foreach (string row in _currentFile.Rows)
            {
                Paragraph currentParagraph = new Paragraph(new Run(row));
                document.Blocks.Add(currentParagraph);
            }

            TextBox.Document = document;
            FormatDocument();
        }

        private void SaveCommandHandler(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            if (_currentFile.Path == null)
            {
                ShowSaveFileDialog();
            }
            SaveContentsToDisk();
        }

        private void SaveContentsToDisk()
        {
            if (_currentFile.Path != null)
            {
                using (StreamWriter writer = new StreamWriter(new FileStream(_currentFile.Path, FileMode.OpenOrCreate)))
                {
                    foreach (Block currentBlock in TextBox.Document.Blocks)
                    {
                        String currentLine = GetTextFromBlock(currentBlock);
                        writer.WriteLine(currentLine);
                    }
                }
            }
        }

        private string GetTextFromBlock(Block currentBlock)
        {
            TextPointer start = currentBlock.ContentStart;
            TextPointer end = currentBlock.ContentEnd;
            TextRange range = new TextRange(start, end);

            string textFromBlock = range.Text;
            return textFromBlock;
        }

        private void SaveAsMenuItemClick(object sender, RoutedEventArgs e)
        {
            SaveFileAs();
        }

        private void SaveFileAs()
        {
            bool isNewFilePathSet = ShowSaveFileDialog();
            if (isNewFilePathSet)
            {
                SaveContentsToDisk();
            }
        }

        private bool ShowSaveFileDialog()
        {
            SaveFileDialog fileDialog = new SaveFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Filter = "Text Documents (*.txt)|*.txt|All Files (*.*)|*.*"
                };

            if (fileDialog.ShowDialog() == true)
            {
                _currentFile.Path = fileDialog.FileName;
                return true;
            }
            return false;
        }

        private void PreferencesMenuItemClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
            settingsWindow.ShowDialog();
        }

        private void FontMenuItemClick(object sender, RoutedEventArgs e)
        {
            FontDialog fontDialog = new FontDialog
                {
                    AllowVerticalFonts = false,
                    FontMustExist = true,
                    ShowColor = false,
                    ShowApply = false,
                    ShowEffects = false
                };
            fontDialog.ShowDialog();
        }
    }
}
