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
            string folderPath = @"\\kfnl-sf01\مخرجات التشغيل\غلافات العناوين - كتب الملك سلمان - 10-11-2021";
            DirectoryInfo di = new DirectoryInfo(folderPath);

            
                // This path is a file
                ProcessDirectory(di);
        
            //DirectoryInfo di = new DirectoryInfo(folderPath);
            //DirectoryInfo[] directoryInfos = di.GetDirectories();
            //foreach (var dir in directoryInfos)
            //{
            //    OdbcConnection con1 = new OdbcConnection("Driver=Sybase ASE ODBC Driver;SRVR=production;DB=kfnl;UID=sa;PWd=sybase1;");
            //    FileInfo[] files = dir.GetFiles();
            //    string s = string.Empty;
            //    try
            //    {

            //        con1.Open();
            //        foreach (FileInfo file in files)
            //        {

            //            int bibNo = 0;
            //            s = file.Name;
            //            //string fileName = file.Name;
            //            if (file.Name.Contains("-"))
            //            {
            //                bibNo = int.Parse(file.Name.Substring(0, file.Name.IndexOf('-')));
            //            }
            //            else
            //            {
            //                bibNo = int.Parse(file.Name.ToLower().Replace(".pdf", ""));
            //            }
            //            try
            //            {
            //                Console.WriteLine(bibNo);
            //                OdbcCommand commandItem = new OdbcCommand(@"insert into DLDEPTCD25112020 (bib#,FileName,FilePath) values (?,?,?)", con1);
            //                commandItem.Parameters.Add(new OdbcParameter("@bib#", bibNo));
            //                commandItem.Parameters.Add(new OdbcParameter("@bib#", file.Name));
            //                commandItem.Parameters.Add(new OdbcParameter("@bib#", Path.GetFullPath(file.DirectoryName)));
            //                commandItem.ExecuteNonQuery();
            //            }
            //            catch
            //            {
            //            }
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);

            //    }
            //    finally
            //    {
            //        con1.Dispose();
            //        con1.Close();

            //    }
            //}
        }


        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(DirectoryInfo targetDirectory)
        {
            // Process the list of files found in the directory.
            FileInfo[] fileEntries = targetDirectory.GetFiles();
            foreach (FileInfo fileName in fileEntries)
            {
               // if (fileName.Extension.Equals(".pdf") || fileName.Extension.Equals(".PDF"))
                    ProcessFile(fileName);
            }

            // Recurse into subdirectories of this directory.
            DirectoryInfo[] subdirectoryEntries = targetDirectory.GetDirectories();
            foreach (DirectoryInfo subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(FileInfo file)
        {


            OdbcConnection con1 = new OdbcConnection("Driver=Sybase ASE ODBC Driver;SRVR=production;DB=kfnl;UID=sa;PWd=sybase1;");

            string s = string.Empty;
            try
            {

                con1.Open();


                int bibNo = 0;
                s = file.Name;
                //string fileName = file.Name;
                if (file.Name.Contains("-"))
                {
                    bibNo = int.Parse(file.Name.Substring(0, file.Name.IndexOf('-')));
                }
                else
                {
                    bibNo = int.Parse(file.Name.ToLower().Replace(".pdf", ""));
                }
                try
                {
                    Console.WriteLine(bibNo);
                    OdbcCommand commandItem = new OdbcCommand(@"insert into dbo.tempKingSalman_10_11_2021 (bib#,FileName,FilePath) values (?,?,?)", con1);
                    commandItem.Parameters.Add(new OdbcParameter("@bib#", bibNo));
                    commandItem.Parameters.Add(new OdbcParameter("@bib#", file.Name));
                    commandItem.Parameters.Add(new OdbcParameter("@bib#", Path.GetFullPath(file.DirectoryName)));
                    commandItem.ExecuteNonQuery();
                }
                catch
                {
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
                con1.Dispose();
                con1.Close();

            }
        }
    }
}
