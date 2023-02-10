using System;
using System.Collections;
using System.IO;

namespace FurryMes_V1._2
{
    internal class FileService
    {
        readonly static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Furry Notification");
        static string watchers = Path.Combine(path, "watchers.txt");
        static string notifiedWatchers = Path.Combine(path, "notifiedWatchers.txt");
        static string unannouncedWatchers = Path.Combine(path, "unannouncedWatchers.txt");
        static string[] list = new string[] { watchers, notifiedWatchers, unannouncedWatchers };

        public static void createDirectory()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public static void createDataFiles()
        {
            if (Directory.Exists(path))
            {
                foreach (string s in list)
                {
                    if (!File.Exists(s))
                    {
                        File.Create(s);
                    }
                }
            }
        }

        public static async void writeUser(ArrayList list)
        {
            using StreamWriter file = new(watchers, append: true);
            foreach (string l in list)
            {
                await file.WriteLineAsync(l);
            }
        }

        public static async void writeUserWithMessage(ArrayList list)
        {
            using StreamWriter file = new(notifiedWatchers, append: true);
            foreach (string l in list)
            {
                await file.WriteLineAsync(l);
            }
        }
        public static async void writeUserWithoutMessage(ArrayList list)
        {
            using StreamWriter file = new(unannouncedWatchers, false);
            foreach (string l in list)
            {
                await file.WriteLineAsync(l);
            }
        }

        public static string[] GetAllWatchers()
        {
            string[] readText = File.ReadAllLines(unannouncedWatchers);
            return readText;
        }
    }
}
