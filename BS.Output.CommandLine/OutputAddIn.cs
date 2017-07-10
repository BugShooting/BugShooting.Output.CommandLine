using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BS.Output.CommandLine
{
  public class OutputAddIn: V3.OutputAddIn<Output>
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

    protected override OutputValueCollection SerializeOutput(Output Output)
    {

      OutputValueCollection outputValues = new OutputValueCollection();

      outputValues.Add(new OutputValue("Name", Output.Name));
      outputValues.Add(new OutputValue("Application", Output.Application));
      outputValues.Add(new OutputValue("Arguments", Output.Arguments));
      outputValues.Add(new OutputValue("FileName", Output.FileName));
      outputValues.Add(new OutputValue("FileFormat", Output.FileFormat));

      return outputValues;
      
    }

    protected override Output DeserializeOutput(OutputValueCollection OutputValues)
    {
      return new Output(OutputValues["Name", this.Name].Value,
                        OutputValues["Application", ""].Value,
                        OutputValues["Arguments", ""].Value,
                        OutputValues["FileName", "Screenshot"].Value,
                        OutputValues["FileFormat", ""].Value);
    }

    protected async override Task<V3.SendResult> Send(IWin32Window Owner, Output Output, V3.ImageData ImageData)
    {
      try
      {

        string fileFormat = Output.FileFormat;
        string fileName = V3.FileHelper.GetFileName(Output.FileName, fileFormat, ImageData); ;
        string filePath = Path.Combine(Path.GetTempPath(), fileName + "." + V3.FileHelper.GetFileExtention(fileFormat));
        
        Byte[] fileBytes = V3.FileHelper.GetFileBytes(fileFormat, ImageData);

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

        return new V3.SendResult(V3.Result.Success, filePath);

      }
      catch (Exception ex)
      {
        return new V3.SendResult(V3.Result.Failed, ex.Message);
      }

    }
      
  }

}