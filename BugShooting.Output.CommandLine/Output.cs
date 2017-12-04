using BS.Plugin.V3.Output;

namespace BugShooting.Output.CommandLine
{

  public class Output: IOutput 
  {

    string name;
    string application;
    string arguments;
    string fileName;
    string fileFormat;

    public Output(string name,
                  string application,
                  string arguments,
                  string fileName,
                  string fileFormat)
    {
      this.name = name;
      this.application = application;
      this.arguments = arguments;
      this.fileName = fileName;
      this.fileFormat = fileFormat;
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

    public string FileFormat
    {
      get { return fileFormat; }
    }

  }
}
