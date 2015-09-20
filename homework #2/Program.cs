using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filecopy
{
    class Program
    {
        static void Main(string[] args)
        {
            var bufferSize = int.Parse(args[0]);
            var sourceFile = new System.IO.FileInfo(args[1]);
            var destFile = new System.IO.FileInfo(args[2]);
            var start = DateTime.Now;
            CopyFile(sourceFile, destFile, bufferSize);
            var finish = DateTime.Now;

            var totalMilliSeconds = (finish - start).TotalMilliseconds;
            var fileSize = sourceFile.Length;

            var bytesPerMIlliSecond = fileSize / totalMilliSeconds;
            var bytesPerSeconds = bytesPerMIlliSecond / 1000;

            Console.WriteLine("Wrote {0} bytes in {1} MS, at a rate of {2} bytes/second", fileSize, totalMilliSeconds, bytesPerMIlliSecond);

        }

        private static void CopyFile(FileInfo sourceFile, FileInfo destFile, int bufferSize)
        {
            using (var source = OpenSourceFile(sourceFile))
            {
                using (var dest = OpenDestFile(destFile))
                {
                    CopyData(source, dest, bufferSize);
                }
            }
        }

        private static void CopyData(Stream source, Stream dest, int bufferSize)
        {
            var buffer = new byte[bufferSize];
            int readLen;
            while ((readLen = source.Read(buffer, 0, buffer.Length)) !=0)
            {
                dest.Write(buffer, 0, readLen);
            }

        }

        private static  Stream OpenSourceFile(FileInfo file)
        {
            return new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
        }

        private static Stream OpenDestFile(FileInfo file)
        {
            char ch;
            if (File.Exists(@"newfile.txt"))
            {
                Console.WriteLine("The destination file is already exist, do you want to override it? Y/N");
                ch = (char)Console.Read();

                while (!((ch.Equals('Y'))||(ch.Equals('N'))))
                {
                    Console.WriteLine("Input invaild, please try again");
                    ch = (char)Console.Read();
                }


                if ((ch.Equals('Y')))
                {
                    return new FileStream(file.FullName, FileMode.Open, FileAccess.Write);
                }
                if ((ch.Equals('N')))
                {
                    Console.WriteLine("Progress terminated");
                    Environment.Exit(0);
                }

            }

            return new FileStream(file.FullName, FileMode.Create, FileAccess.Write);

        }
    }
}
