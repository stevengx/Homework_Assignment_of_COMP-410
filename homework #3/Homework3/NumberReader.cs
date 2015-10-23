using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Homework3 {
    class NumberReader : IDisposable {
        private readonly TextReader _reader;
        string content = string.Empty;

        public NumberReader(FileInfo file) {
            _reader = new StreamReader(new MemoryStream(Encoding.GetEncoding("utf-8").GetBytes(new StreamReader(new BufferedStream(new FileStream(
                file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan), 65536)).ReadToEnd())));
        }


        public IEnumerable<long> ReadIntegers() {
            string line;
            while ((line = _reader.ReadLine()) != null) {
                var value = long.Parse(line);
                yield return value;
            }
        } 

        public void Dispose() {
            _reader.Dispose();
        }
    }
}