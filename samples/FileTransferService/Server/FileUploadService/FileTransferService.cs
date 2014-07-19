/// Notice : Code written by Dimitris Papadimitriou - http://www.papadi.gr
/// Code is provided to be used freely but without any warranty of any kind
using System;

namespace FileService
{

    public class FileTransferService : IFileTransferService
    {
        public RemoteFileInfo DownloadFile(DownloadRequest request)
        {
            // get some info about the input file
            string filePath = System.IO.Path.Combine("Upload", request.FileName);
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);

            // report start
            Console.WriteLine("Sending stream " + request.FileName + " to client");
            Console.WriteLine("Size " + fileInfo.Length);

            // check if exists
            if (!fileInfo.Exists) throw new System.IO.FileNotFoundException("File not found", request.FileName);

            // open stream
            System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            // return result
            RemoteFileInfo result = new RemoteFileInfo();
            result.FileName = request.FileName;
            result.Length = fileInfo.Length;
            result.FileByteStream = stream;
            return result;

            // after returning to the client download starts. Stream remains open and on server and the client reads it, although the execution of this method is completed.
        }

        public void UploadFile(RemoteFileInfo request)
        {
            // report start
            Console.WriteLine("Start uploading " + request.FileName);
            Console.WriteLine("Size " + request.Length);

            // create output folder, if does not exist
            if (!System.IO.Directory.Exists("Upload")) System.IO.Directory.CreateDirectory("Upload");

            // kill target file, if already exists
            string filePath = System.IO.Path.Combine("Upload", request.FileName);
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

            int chunkSize = 2048;
            byte[] buffer = new byte[chunkSize];

            using (System.IO.FileStream writeStream = new System.IO.FileStream(filePath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write))
            {
                do
                {
                    // read bytes from input stream
                    int bytesRead = request.FileByteStream.Read(buffer, 0, chunkSize);
                    if (bytesRead == 0) break;

                    // write bytes to output stream
                    writeStream.Write(buffer, 0, bytesRead);
                } while (true);

                // report end
                Console.WriteLine("Done!");

                writeStream.Close();
            }
        }
    }

}
