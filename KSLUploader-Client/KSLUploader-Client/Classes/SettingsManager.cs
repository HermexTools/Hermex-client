﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KSLUploader.Classes
{
    public class SettingsManager
    {
        private static string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".KSLU");
        private static string file = "ksluconfig.json";

        public static void Inizialize(string key, object value)
        {
            if(!Contains(key))
            {
                Set(key, value);
            }
        }

        public static void Set(string key, object value)
        {
            var file = ReadSettingsFile();

            if(Contains(key))
            {
                file[key] = value;
            }
            else
            {
                file.Add(key, value);
            }
            
            SaveSettingsFile(file);
        }

        public static object Get(string key)
        {
            var file = ReadSettingsFile();
            return file[key];
        }

        public static void Remove(string key)
        {
            var file = ReadSettingsFile();
            file.Remove(key);
            SaveSettingsFile(file);
        }

        public static void RemoveAll()
        {
            var file = ReadSettingsFile();
            foreach(var item in file)
            {
                Remove(item.Key);
            }
        }

        public static bool Contains(string key)
        {
            var file = ReadSettingsFile();
            return file.ContainsKey(key);
        }



        private static Dictionary<string, object> ReadSettingsFile()
        {
            bool error = false;
            Dictionary<string, object> list = new Dictionary<string, object>();

            try
            {
                var file = GetSettingFile();
                string content = File.ReadAllText(file.FullName);
                list = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
            }
            catch
            {
                error = true;
            }

            if(error)
            {
                var file = GetSettingFile();
                File.WriteAllText(file.FullName, JsonConvert.SerializeObject(new Dictionary<string, object>()));
                string content = File.ReadAllText(file.FullName);
                list = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
            }

            return list;
        }

        private static FileInfo GetSettingFile()
        {
            //create folder
            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            
            //create file
            if(!File.Exists(folder + "\\" + file))
            {
                using(var fs = File.Create(folder + "\\" + file))
                {
                    fs.Close();
                }
                File.WriteAllText(folder + "\\" + file, JsonConvert.SerializeObject(new Dictionary<string, object>(), Formatting.Indented));                
            }

            return new FileInfo(folder + "\\" + file);
        }

        private static void SaveSettingsFile(Dictionary<string,object> settings)
        {
            var file = GetSettingFile();
            File.WriteAllText(file.FullName, JsonConvert.SerializeObject(settings, Formatting.Indented));
        }

    }
}