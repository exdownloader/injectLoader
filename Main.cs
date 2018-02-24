using dnlib.DotNet;
using IllusionPlugin;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace injectLoader
{
    public class m : IPlugin
    {
        public string Name => "injectLoader";
        public string Version => "1.0";

        public void OnApplicationQuit() { }
        public void OnApplicationStart() { }
        public void OnFixedUpdate() { }
        public void OnLevelWasInitialized(int level) { }
        public void OnLevelWasLoaded(int level) { }

        private void LoadDLL(string path)
        {
            var fi = new FileInfo(path);
            if (!fi.Exists)
            {
                Console.WriteLine("File \"{0}\" does not exist.", fi.Name);
                return;
            }

            var b = File.ReadAllBytes(fi.FullName);
            var ad = AssemblyDef.Load(b);
            var newName = new string(ad.Name.ToString().Take(6).ToArray()) + DateTime.Now.Ticks.ToString(); //Arbitrary, take first 6 chars.
            ad.Name = new UTF8String(newName);

            using (var ms = new MemoryStream())
            {
                ad.Write(ms);
                var ass = Assembly.Load(ms.ToArray());
                var t = ass.GetType("script.Main");
                var m = t.GetMethod("Bootstrap", BindingFlags.Static | BindingFlags.Public);
                m.Invoke(null, null);
            }
        }

        public void OnUpdate() {
            if (!Input.GetKeyDown(KeyCode.LeftAlt)) return;
            
            var path = ModPrefs.GetString("injectLoader", "path", "C:\\Mod.dll", true);
            LoadDLL(path);
        }
    }
}
