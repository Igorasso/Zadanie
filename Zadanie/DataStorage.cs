using System;

namespace ZadanieDomowe
{
    internal class DataStorage
    {
        public string FileName { get; private set; }
        public Uri Uri  { get; private set; }

        public DataStorage(string fileName, Uri uri)
        {
            FileName = fileName;
            Uri = uri;
        }

    }
}
