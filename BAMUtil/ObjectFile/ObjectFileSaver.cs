using System;
using System.IO;

using Newtonsoft.Json;

namespace BAMUtil.ObjectFile {
    public static class ObjectFileSaver {
        public static void SaveFile<T>(this T obj, string path) where T : new() {
            var textToSave = JsonConvert.SerializeObject(obj);
            File.WriteAllText(path, textToSave);
        }

        public static T LoadFile<T>(this T obj, string path) {
            var objectText = File.ReadAllText(path);
            obj = JsonConvert.DeserializeObject<T>(objectText);

            return obj;
        }
    }
}