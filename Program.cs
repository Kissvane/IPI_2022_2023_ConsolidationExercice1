using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CorrectionConsolidation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Save");
            string directoryPath2 = Path.Combine(Directory.GetCurrentDirectory(), "Save2");
            Stopwatch stopwatch = new Stopwatch();
            //synchronous mehods
            //is here only to compare speed
            stopwatch.Start();
            List<ClassedCharacter> characters = DeserializeAll(directoryPath);
            File.WriteAllText(Path.Combine(directoryPath2,"SynchronSave.json"),JsonConvert.SerializeObject(characters));
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            //asyncronous mehod start here
            stopwatch.Start();
            Task<List<ClassedCharacter>> tasks = Deserialize(directoryPath);
            await tasks;
            List<ClassedCharacter> characters2 = tasks.Result;
            File.WriteAllText(Path.Combine(directoryPath2, "AsynchronSave.json"), JsonConvert.SerializeObject(characters2));
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        static List<ClassedCharacter> DeserializeAll(string directoryPath)
        {
            List<ClassedCharacter> result = new List<ClassedCharacter>();
            string[] filenames = Directory.GetFiles(directoryPath, "*", SearchOption.TopDirectoryOnly);
            int fCount = filenames.Length;
            List<Task<Character>> tasks = new List<Task<Character>>();
            foreach (string name in filenames)
            {
                string path = Path.Combine(directoryPath, name);
                Character c = DeserializeCharacter(path);
                result.Add(new ClassedCharacter(c));
            }

            return result;
        }

        static async Task<List<ClassedCharacter>> Deserialize(string directoryPath)
        {
            List<ClassedCharacter> result = new List<ClassedCharacter>();
            string[] filenames = Directory.GetFiles(directoryPath, "*", SearchOption.TopDirectoryOnly);
            int fCount = filenames.Length;
            List<Task<Character>> tasks = new List<Task<Character>>();
            //load
            foreach(string name in filenames)
            {
                string path = Path.Combine(directoryPath, name);
                Task<Character> t = Task.Run(() => DeserializeCharacter(path));
                tasks.Add(t);
            }

            await Task.WhenAll<Character>(tasks);
            foreach (Task<Character> task in tasks)
            {
                result.Add(new ClassedCharacter(task.Result));
            }

            return result;
        }

        static Character DeserializeCharacter(string filePath)
        {
            string data =File.ReadAllText(filePath);
            Character result = JsonConvert.DeserializeObject<Character>(data);
            return result;
        }
    }
}
