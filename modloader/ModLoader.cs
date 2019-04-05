using System.Reflection;
using UnityEngine;
using Harmony;
using System;
using System.IO;
using System.Linq;

namespace RoR2
{
    public class ModLoader
    {
        public static void Init()
        {
            RoR2Application.isModded = true;
            using (StreamWriter log = File.CreateText("./ror2-modloader.log"))
            {
                log.WriteLine("Hello!");
                try
                {
                    var mods = Directory.GetFiles("./Mods/");
                    foreach (var mod in mods)
                    {
                        log.WriteLine("Found mod file " + mod);
                        var module = Assembly.LoadFile(mod);
                        foreach (var method in module.GetTypes().SelectMany(t => t.GetMethods()).Where(m => m.IsStatic && m.GetCustomAttributes(typeof(Mods.ModEntry), false).Length != 0))
                        {
                            var attr = method.GetCustomAttribute<Mods.ModEntry>(false);
                            log.WriteLine("Loading mod " + attr.Name + " " + attr.Version + " by " + attr.Author);
                            try
                            {
                                method.Invoke(null, new object[] { });
                            }
                            catch (Exception e)
                            {
                                log.WriteLine(e.ToString());
                            }
                        }


                    }
                    log.WriteLine("ModLoader initiated!");

                }
                catch (Exception e)
                {
                    log.WriteLine(e.ToString());
                }
            }
        }
    }

}