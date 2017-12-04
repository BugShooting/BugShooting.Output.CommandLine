using BS.Plugin.V3.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BugShooting.Output.CommandLine
{
  partial class Edit : Window
  {
    public Edit(Output output)
    {
      InitializeComponent();

      foreach (string fileNameReplacement in FileHelper.GetFileNameReplacements())
      {
        MenuItem item = new MenuItem();
        item.Header = fileNameReplacement;
        item.Click += FileNameReplacementItem_Click;
        FileNameReplacementList.Items.Add(item);
      }

      IEnumerable<string> fileFormats = FileHelper.GetFileFormats();
      foreach (string fileFormat in fileFormats)
      {
        ComboBoxItem item = new ComboBoxItem();
        item.Content = fileFormat;
        item.Tag = fileFormat;
        FileFormatComboBox.Items.Add(item);
      }

      NameTextBox.Text = output.Name;
      ApplicationTextBox.Text = output.Application;
      ArgumentsTextBox.Text = output.Arguments;
      FileNameTextBox.Text = output.FileName;
      FileFormatComboBox.SelectedValue = output.FileFormat;

      if (fileFormats.Contains(output.FileFormat))
      {
        FileFormatComboBox.SelectedValue = output.FileFormat;
      }
      else
      {
        FileFormatComboBox.SelectedValue = fileFormats.First();
      }

      NameTextBox.TextChanged += ValidateData;
      ApplicationTextBox.TextChanged += ValidateData;
      FileFormatComboBox.SelectionChanged += ValidateData;
      ValidateData(null, null);

      ApplicationTextBox.Focus();

    }

    public string OutputName
    {
      get { return NameTextBox.Text; }
    }

    public string Application
    {
      get { return ApplicationTextBox.Text; }
    }

    public string Arguments
    {
      get { return ArgumentsTextBox.Text; }
    }

    public string FileName
    {
      get { return FileNameTextBox.Text; }
    }

    public string FileFormat
    {
      get { return (string)FileFormatComboBox.SelectedValue; }
    }

    private void SelectApplication_Click(object sender, RoutedEventArgs e)
    {

      using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
      {

        openFileDialog.Filter = "Applications (*.exe)|*.exe|All files (*.*)|*.*";

        if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          ApplicationTextBox.Text = openFileDialog.FileName;

          NameTextBox.Text = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);

        }
      }

    }

    private void ArgumentsReplacement_Click(object sender, RoutedEventArgs e)
    {
      ArgumentsReplacement.ContextMenu.IsEnabled = true;
      ArgumentsReplacement.ContextMenu.PlacementTarget = ArgumentsReplacement;
      ArgumentsReplacement.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
      ArgumentsReplacement.ContextMenu.IsOpen = true;
    }

    private void ArgumentsReplacementItemt_Click(object sender, RoutedEventArgs e)
    {

      MenuItem item = (MenuItem)sender;

      int selectionStart = ArgumentsTextBox.SelectionStart;

      ArgumentsTextBox.Text = ArgumentsTextBox.Text.Substring(0, ArgumentsTextBox.SelectionStart) + item.Header.ToString() + ArgumentsTextBox.Text.Substring(ArgumentsTextBox.SelectionStart, ArgumentsTextBox.Text.Length - ArgumentsTextBox.SelectionStart);

      ArgumentsTextBox.SelectionStart = selectionStart + item.Header.ToString().Length;
      ArgumentsTextBox.Focus();

    }

    private void FileNameReplacement_Click(object sender, RoutedEventArgs e)
    {
      FileNameReplacement.ContextMenu.IsEnabled = true;
      FileNameReplacement.ContextMenu.PlacementTarget = FileNameReplacement;
      FileNameReplacement.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
      FileNameReplacement.ContextMenu.IsOpen = true;
    }

    private void FileNameReplacementItem_Click(object sender, RoutedEventArgs e)
    {

      MenuItem item = (MenuItem)sender;

      int selectionStart = FileNameTextBox.SelectionStart;

      FileNameTextBox.Text = FileNameTextBox.Text.Substring(0, FileNameTextBox.SelectionStart) + item.Header.ToString() + FileNameTextBox.Text.Substring(FileNameTextBox.SelectionStart, FileNameTextBox.Text.Length - FileNameTextBox.SelectionStart);

      FileNameTextBox.SelectionStart = selectionStart + item.Header.ToString().Length;
      FileNameTextBox.Focus();

    }

    private void ValidateData(object sender, EventArgs e)
    {
      OK.IsEnabled = Validation.IsValid(NameTextBox) &&
                     Validation.IsValid(ApplicationTextBox) &&
                     Validation.IsValid(FileFormatComboBox);
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = true;
    }
    
  }
}
