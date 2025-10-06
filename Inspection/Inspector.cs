using System.Reflection;
using BepInEx.Logging;

namespace MidnightMenu_DeathMustDie.Inspection
{
    public class Inspector
    {
        public static void FindItemDropsPerMinReferences()
        {
            var asm = typeof(Death.Data.Database).Assembly;
            foreach (var type in asm.GetTypes())
            {
                foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    if (field.FieldType == typeof(Death.Data.Tables.ItemDropsPerMinTable))
                    {
                        ModMenu.Log.LogInfo($"[Reference] {type.FullName}.{field.Name} ({field.FieldType})");
                    }
                }
            }
        }
    }
}
