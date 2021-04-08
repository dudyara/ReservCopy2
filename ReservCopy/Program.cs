using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReservCopy
{
    class Program
    {
        class JSONinfo
        {
            public string SourceRef { get; set; }
            public string EndRef { get; set; }
        }
        static void Main(string[] args)
        {
            string jsonString = File.ReadAllText("settings.json"); //считываем json
            JSONinfo references = JsonSerializer.Deserialize<JSONinfo>(jsonString);

            DirectoryInfo dirInfo = new DirectoryInfo(references.EndRef); //создаем новый каталог
            dirInfo.CreateSubdirectory(references.EndRef);

            string sourceRef = references.SourceRef;
            string endRef = references.EndRef + "/" + references.EndRef; //путь каталога с бэкапами
            string targetRef = references.EndRef + "/" + dirInfo.LastAccessTime.Hour + "." //путь каталога с конкретным бэкапом
                                + dirInfo.LastAccessTime.Minute + "." + dirInfo.LastAccessTime.Second + " " + dirInfo.LastAccessTime.ToShortDateString();
            Directory.Move(endRef, targetRef); //переименовываем каталог

            string[] files = Directory.GetFiles(sourceRef); //получаем файлы для копирования

            try
            {
                foreach (string s in files) //копируем 
                {
                    string fileName = Path.GetFileName(s);
                    string destFile = Path.Combine(targetRef, fileName);
                    File.Copy(s, destFile, true);
                }
                Console.WriteLine("Saved");
            }
            catch (UnauthorizedAccessException) //в случае отказа доступа
            {
                Console.WriteLine("Error");
            }
            Console.ReadLine();
        }
    }
}
