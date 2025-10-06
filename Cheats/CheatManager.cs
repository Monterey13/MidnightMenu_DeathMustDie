using Claw.Core.Structures;
using Death.Data;
using Death.Items;
using MidnightMenu_DeathMustDie.Tables;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace MidnightMenu_DeathMustDie.Cheats
{
    public class CheatManager
    {
        public static void ReplaceLootTable()
        {
            var db = Death.Data.Database.Current;
            if (db == null)
            {
                ModMenu.Log.LogError("[MidnightMenu] Database.Current is null, cannot replace loot table.");
                return;
            }

            var field = typeof(Database).GetField("<ItemDropsPerMinTable>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
            {
                ModMenu.Log.LogError("[MidnightMenu] Could not find ItemDropsPerMinTable field via reflection.");
                return;
            }

            var table = field.GetValue(db);
            if (table == null)
            {
                ModMenu.Log.LogError("[MidnightMenu] ItemDropsPerMinTable is null, cannot modify.");
                return;
            }

            var entriesField = table.GetType().GetField("_entries", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (entriesField == null)
            {
                ModMenu.Log.LogError("[MidnightMenu] Could not find _entries field on ItemDropsPerMinTable.");
                return;
            }

            var list = entriesField.GetValue(table) as IList;
            if (list == null)
            {
                ModMenu.Log.LogError("[MidnightMenu] _entries field was not a list — cannot modify.");
                return;
            }

            // --- Clear and repopulate with our Mythic-only entries ---
            list.Clear();
            var newEntries = CustomItemDropsPerMinTable.CreateDefault().ToList();
            foreach (var e in newEntries)
                list.Add(e);

            ModMenu.Log.LogInfo($"[MidnightMenu] Replaced existing ItemDropsPerMinTable contents with {newEntries.Count} custom entries (Mythic only).");
        }
    }
}
