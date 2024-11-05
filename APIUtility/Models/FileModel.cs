namespace APIUtility.Models
{
    public class FileModel 
    {
        public string FileName { get; set; }
        public string FileServerPath { get; set; }
        public int ContentLength { get; set; }
        public byte[] Content { get; set; }
        public string LastFileDateTime { get; set; }
        public string LastFileUser { get; set; }
        public string LastFileTerminal { get; set; }
        public string Active { get; set; }
        public string RecordFound { get; set; }
        public string RecordMessage { get; set; }

        public FileModel()
        {
        }

        public FileModel(string fName, string fServerPath, int fContentLength, byte[] fContent)
        {
            FileName = fName;
            FileServerPath = fServerPath;
            ContentLength = fContentLength;
            Content = fContent;
        }

        public FileModel(string fileName,  byte[] content, int contentLength)
        {
            FileName = fileName;
            Content = content;
            ContentLength = contentLength;
        }
  }
}