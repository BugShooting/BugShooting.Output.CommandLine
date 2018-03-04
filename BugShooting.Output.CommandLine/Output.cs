using BS.Plugin.V3.Output;
using System;

namespace BugShooting.Output.CommandLine
{

  public class Output: IOutput 
  {

    string name;
    string application;
    string arguments;
    string fileName;
    Guid fileFormatID;

    public Output(string name,
                  string application,
                  string arguments,
                  string fileName,
                  Guid fileFormatID)
    {
      this.name = name;
      this.application = application;
      this.arguments = arguments;
      this.fileName = fileName;
      this.fileFormatID = fileFormatID;
    }
    
    public string Name
    {
      get { return name; }
    }

    public string Application
    {
      get { return application; }
    }

    public string Arguments
    {
      get { return arguments; }
    }

    public string Information
    {
      get { return application; }
    }

    public string FileName
    {
      get { return fileName; }
    }

    public Guid FileFormatID
    {
      get { return fileFormatID; }
    }

  }
}