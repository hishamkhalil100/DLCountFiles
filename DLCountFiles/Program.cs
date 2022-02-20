using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Odbc;
using System.Data;

namespace DLCountFiles
{
    class Program
    {
        //  static OdbcConnection con1 = new OdbcConnection("Driver=Sybase ASE ODBC Driver;SRVR=production;DB=kfnl;UID=sa;PWd=sybase1;");
        static void Main(string[] args)
        {
            using (OdbcConnection con1 = new OdbcConnection("Driver=Sybase ASE ODBC Driver;SRVR=production;DB=kfnl;UID=sa;PWd=sybase1;"))
            {
                con1.Open();
                string folderPath = @"\\kfnl-fs01\g$\TempDLCovers\New folder\7-New\7";
                DirectoryInfo di = new DirectoryInfo(folderPath);
                // This path is a file
                ProcessDirectory(di, con1);
            }

        }
        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(DirectoryInfo targetDirectory, OdbcConnection con1)
        {
            // Process the list of files found in the directory.
            FileInfo[] fileEntries = targetDirectory.GetFiles();
            foreach (FileInfo fileName in fileEntries)
            {
                Console.WriteLine($"get File{fileName}");
                // if (fileName.Extension.Equals(".pdf") || fileName.Extension.Equals(".PDF"))
                ProcessFile(fileName, con1);
            }
            // Recurse into subdirectories of this directory.
            DirectoryInfo[] subdirectoryEntries = targetDirectory.GetDirectories();
            foreach (DirectoryInfo subdirectory in subdirectoryEntries)
            {
                Console.WriteLine("get Direct");
                ProcessDirectory(subdirectory, con1);
            }
        }
        // Insert logic for processing found files here.
        public static void ProcessFile(FileInfo file, OdbcConnection con1)
        {
            string s = string.Empty;
            try
            {
                int bibNo = 0;
                s = file.Name;
                //string fileName = file.Name;
                if (file.Name.Contains("-"))
                {
                    bibNo = int.Parse(file.Name.Substring(0, file.Name.IndexOf('-')));
                }
                else if (file.Name.ToLower().Contains("jpg"))
                {
                    bibNo = int.Parse(file.Name.ToLower().Replace(".jpg", ""));
                }
                else if (file.Name.ToLower().Contains("png"))
                {
                    bibNo = int.Parse(file.Name.ToLower().Replace(".png", ""));
                }
                else if (file.Name.ToLower().Contains("tif"))
                {
                    return;
                }
                else
                {
                    bibNo = int.Parse(file.Name.ToLower().Replace(".pdf", ""));
                }

                Console.WriteLine(bibNo);
                OdbcCommand commandItem = new OdbcCommand(@"insert into dbo.tempDlCoversFromAbst (bib#,FileName,FilePath) values (?,?,?)", con1);
                commandItem.Parameters.Add(new OdbcParameter("@bib#", bibNo));
                commandItem.Parameters.Add(new OdbcParameter("@bib#", file.Name));
                commandItem.Parameters.Add(new OdbcParameter("@bib#", Path.GetFullPath(file.DirectoryName)));
                commandItem.ExecuteNonQuery();
                Console.WriteLine("Finish!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
