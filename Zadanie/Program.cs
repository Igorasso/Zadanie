using System;
using System.Collections.Generic;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace ZadanieDomowe
{
    internal class Program
    {

        static int totalFileSize;
        static public int urlCount;
        static public CountdownEvent countdownEvent;
        static void Main(string[] args)
        {
            string url;
            string[] urls, fileNames;
            Uri[] uris;
            SourceManager.WebPrep(out url, out urls, out fileNames, out uris);

            bool state = false;

            while (state == false)
            {
                Console.WriteLine(@"Wybierz metode realizacji zadania:
1 : metoda Thread
2 : metoda ThreadPool
3 : metoda Task");


                int picker = int.Parse(Console.ReadLine());

                if (picker == 1)
                { //Metoda realizująca zadanie za pomocą threadingu.
                    ThreadMethod(url, urls, fileNames, uris);
                    state = true;
                }
                else if (picker == 2)
                {//Metoda realizująca zadanie za pomocą threadPool'a.
                    ThreadPoolMethod(url, urls, fileNames, uris);
                    state = true; ;
                }
                else if (picker == 3)
                {//Metoda realizujaca zadanie za pomoca taskow
                    TaskMethod(url, urls, fileNames, uris);
                    state = true;
                }

            }




            // Console.WriteLine($"Pobrano w sumie {DownloadDataTask.TotalFileSize} bajtów.");


            Console.ReadKey();



        }

        private static void TaskMethod(string url, string[] urls, string[] fileNames, Uri[] uris)
        {
            List<Task> tasks = new List<Task>();
            //wypelniamy listę watkow, thread.add(new thread(() => 
            for (int i = 0; i < urls.Length; i++) // tutaj moznaby uzyc urlCount, ale trzebaby pozmieniac sygnatury itp.
            {
                int j = i;
                fileNames[j] = urls[j].ToString();
                urls[j] = url + fileNames[j];
                uris[j] = new Uri(urls[j]);

                tasks.Add(Task.Run(() =>
                {
                    DownloadDataTask.DownloadData(uris[j], fileNames[j]);
                }));

            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Pobrano w sumie {DownloadDataTask.TotalFileSize} bajtów."); // Dlaczego insturkcja jest realizowana pomimo tego, że wszystkie wątki nie zakończyły pracy? Przy async nie dziala.
        }

        private static void ThreadPoolMethod(string url, string[] urls, string[] fileNames, Uri[] uris)
        {
            //ThreadPoolMethod



            urlCount = urls.Length; //dla ThreadPool'a
            countdownEvent = new CountdownEvent(urlCount);
            for (int i = 0; i < urls.Length; i++)
            {
                int j = i;
                fileNames[j] = urls[j].ToString();
                urls[j] = url + fileNames[j];
                uris[j] = new Uri(urls[j]);
                object data = new DataStorage(fileNames[j], uris[j]);
                ThreadPool.QueueUserWorkItem(new WaitCallback(DownloadDataTask.DownloadData), data);

            }
            countdownEvent.Wait();
            Console.WriteLine($"Pobrano w sumie {DownloadDataTask.TotalFileSize} bajtów.");
            //while (urlCount > 0)
            //{
            //    Thread.Sleep(100);
            //}
        }

        private static void ThreadMethod(string url, string[] urls, string[] fileNames, Uri[] uris)
        {
            List<Thread> threads = new List<Thread>();
            //wypelniamy listę watkow, thread.add(new thread(() => 
            for (int i = 0; i < urls.Length; i++) // tutaj moznaby uzyc urlCount, ale trzebaby pozmieniac sygnatury itp.
            {
                int j = i;
                fileNames[j] = urls[j].ToString();
                urls[j] = url + fileNames[j];
                uris[j] = new Uri(urls[j]);

                threads.Add(new Thread(() => DownloadDataTask.DownloadData(uris[j], fileNames[j])));


            }



            //uruchamiamy watki 
            foreach (Thread thread in threads)
            {
                thread.Start();

            }

            foreach (Thread thread in threads)
            {
                thread.Join(); // czekamy na zakonczenie wszystkich watkow 
            }

            Console.WriteLine($"Pobrano w sumie {DownloadDataTask.TotalFileSize} bajtów."); // Dlaczego insturkcja jest realizowana pomimo tego, że wszystkie wątki nie zakończyły pracy? Przy async nie dziala.
        }

    }


}





//
//Pobrać dane spod URLa http://51.91.120.89/TABLICE/. 
//Zawierają one dane tekstowe, gdzie w każdej linii jest jedna nazwa pliku. 
//Należy pobrać dane spod URLi wg nastepującego schematu : adres_bazwowy / nazwa_pliku.
//Adres bazowy to http://51.91.120.89/TABLICE/ , czyli http://51.91.120.89/TABLICE/AA0571PC.jpg  
//Pliki graficzne pobrać asynchronicznie (sposób zostawiam Wam). 
//Pobrane pliki powinny zostać zapisane na dysku lokalnym, a na zakonczenie pobierania wszystkich plików podana sumaryczna liczba pobranych bajtów.
//Może to być aplikacja konsolowa lub WinForms.
//Rozwiązania udostepniamy jako repozytoria na Github (edited) 

//dorobic w wersji P06TaskExample i P02ThreadDelegate