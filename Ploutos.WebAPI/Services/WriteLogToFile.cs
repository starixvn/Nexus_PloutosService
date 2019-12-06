namespace Ploutos.WebAPI.Services
{
    using System;
    using System.IO;
    using Newtonsoft.Json;

    public static class WriteLogToFile
    {
        public static void Writelog(object content)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "log.txt");
            if (!File.Exists(path))
            {
                File.CreateText(path);
            }
            using (StreamWriter writer = File.AppendText(path))
            {
                Log(content, writer);
            }
        }

        private static void Log(object content, TextWriter writer)
        {
            writer.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()} :");
            writer.WriteLine(JsonConvert.SerializeObject(content));
            writer.WriteLine("-------------------------------");
        }
    }
}