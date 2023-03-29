using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZadanieDomowe
{
    internal class SourceManager
    {
        /// <summary>
        /// Metoda przygotowująca listę odnośników do plików 
        /// </summary>
        /// <param name="url">To tak naprawde chyba nawet nie jest potrzebne tutaj</param>
        /// <param name="urls">tablica odnosnikow</param>
        /// <param name="fileNames">nazwa odpowiednich plikow splitowana po adresach</param>
        /// <param name="uris">przekonwertowane urls string na typ Uri</param>
        public static void WebPrep(out string url, out string[] urls, out string[] fileNames, out Uri[] uris)
        {
            url = "http://51.91.120.89/TABLICE/";
            Uri uri = new Uri(url);

            Task<string> dane = new WebClient().DownloadStringTaskAsync(uri);
            string[] separatory = { "\r\n" };
            urls = dane.Result.ToString().Split(separatory, StringSplitOptions.RemoveEmptyEntries);
            fileNames = new string[urls.Length];
            uris = new Uri[urls.Length];
        }
    }
}
