using System;
using System.IO;
using System.Net;
using System.Threading;

namespace ZadanieDomowe
{
    internal class DownloadDataTask
    {
        private static int fileSize = 0;
        public static int TotalFileSize { get; private set; }

        internal static void DownloadDataAsync(Uri uri, string fileName)
        {
            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            using (WebClient client = new WebClient())
            {

                Console.WriteLine($"Rozpoczynam pobieranie pliku {fileName}");


                try
                {
                    //while(client.IsBusy) { }
                    client.DownloadFileAsync(uri, fileName); // await
                    client.DownloadFileCompleted += (sender, e) =>
                    {
                        fileSize = (int)new FileInfo(fileName).Length;
                        TotalFileSize += fileSize;
                        Console.WriteLine($"Zakonczono pobieranie {fileName} i wazy {fileSize} ");

                    };


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd podczas pobierania pliku: {ex.Message}");
                }




            }








        }

        internal static void DownloadData(Uri uri, string fileName)
        {
            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            using (WebClient client = new WebClient())
            {

                Console.WriteLine($"Rozpoczynam pobieranie pliku {fileName}");


                try
                {
                    //while(client.IsBusy) { }
                    client.DownloadFile(uri, fileName); // await
                                    
                        fileSize = (int)new FileInfo(fileName).Length;
                        TotalFileSize += fileSize;
                        Console.WriteLine($"Zakonczono pobieranie {fileName} i wazy {fileSize} ");

                    


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd podczas pobierania pliku: {ex.Message}");
                }

                


            }
        }

        internal static void DownloadData(Object dataStorageObject)
        {
            
            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            using (WebClient client = new WebClient())
            {
                var storage = (DataStorage)dataStorageObject;
                string fileName = storage.FileName;
                Uri uri = storage.Uri;

                Console.WriteLine($"Rozpoczynam pobieranie pliku {fileName}");


                try
                {
                    //while(client.IsBusy) { }
                    client.DownloadFile(uri, fileName); // await

                    fileSize = (int)new FileInfo(fileName).Length;
                    TotalFileSize += fileSize;
                    Console.WriteLine($"Zakonczono pobieranie {fileName} i wazy {fileSize} ");




                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd podczas pobierania pliku: {ex.Message}");
                }
                
                Program.countdownEvent.Signal();
                //Interlocked.Decrement(ref Program.urlCount);



            }
        }
    }
}