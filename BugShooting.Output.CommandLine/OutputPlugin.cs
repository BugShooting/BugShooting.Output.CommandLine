using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using BS.Plugin.V3.Output;
using BS.Plugin.V3.Common;
using BS.Plugin.V3.Utilities;

namespace BugShooting.Output.CommandLine
{
  public class OutputPlugin: OutputPlugin<Output>
  {

    protected override string Name
    {
      get { return "Command line"; }
    }

    protected override Image Image64
    {
      get  { return Properties.Resources.logo_64; }
    }

    protected override Image Image16
    {
      get { return Properties.Resources.logo_16 ; }
    }

    protected override bool Editable
    {
      get { return true; }
    }

    protected override string Description
    {
      get { return "Share screenshots to every application by using the command line."; }
    }
    
    protected override Output CreateOutput(IWin32Window Owner)
    {

      Output output = new Output(Name,
                                 string.Empty,
                                 "%file%",
                                 "Screenshot",
                                 String.Empty);

      return EditOutput(Owner, output);

    }

    protected override Output EditOutput(IWin32Window Owner, Output Output)
    {

      Edit edit = new Edit(Output);

      var ownerHelper = new System.Windows.Interop.WindowInteropHelper(edit);
      ownerHelper.Owner = Owner.Handle;

      if (edit.ShowDialog() == true)
      {

        return new Output(edit.OutputName,
                          edit.Application,
                          edit.Arguments,
                          edit.FileName,
                          edit.FileFormat);
      }
      else
      {
        return null;
      }

    }

    protected override OutputValues SerializeOutput(Output Output)
    {

      OutputValues outputValues = new OutputValues();

      outputValues.Add("Name", Output.Name);
      outputValues.Add("Application", Output.Application);
      outputValues.Add("Arguments", Output.Arguments);
      outputValues.Add("FileName", Output.FileName);
      outputValues.Add("FileFormat", Output.FileFormat);

      return outputValues;
      
    }

    protected override Output DeserializeOutput(OutputValues OutputValues)
    {
      return new Output(OutputValues["Name", this.Name],
                        OutputValues["Application", ""],
                        OutputValues["Arguments", ""],
                        OutputValues["FileName", "Screenshot"],
                        OutputValues["FileFormat", ""]);
    }

    protected async override Task<SendResult> Send(IWin32Window Owner, Output Output, ImageData ImageData)
    {
      try
      {

        string fileName = AttributeHelper.ReplaceAttributes(Output.FileName,  ImageData); ;
        string filePath = Path.Combine(Path.GetTempPath(), fileName + "." + FileHelper.GetFileExtension(Output.FileFormat));
        
        Byte[] fileBytes = FileHelper.GetFileBytes(Output.FileFormat, ImageData);

        using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
        {
          file.Write(fileBytes, 0, fileBytes.Length);
          file.Close();
        }

        string arguments;
        if (Output.Arguments.IndexOf("%file%", StringComparison.InvariantCultureIgnoreCase) == -1)
        {
          arguments = "\"" + filePath + "\"";         
        }
        else
        {
          arguments = Output.Arguments;
          arguments = arguments.Replace("%file%", "\"" + filePath + "\"");
          arguments = arguments.Replace("%FILE%", "\"" + filePath + "\"");
          arguments = arguments.Replace("%File%", "\"" + filePath + "\"");
        }

        Process.Start(Output.Application, arguments);

        return new SendResult(Result.Success, filePath);

      }
      catch (Exception ex)
      {
        return new SendResult(Result.Failed, ex.Message);
      }

    }
      
  }

}