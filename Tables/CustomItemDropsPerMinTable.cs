using Death.Items;
using Claw.Core.Structures;
using System.Collections;
using System.Collections.Generic;
using Death.Data.Tables;

namespace MidnightMenu_DeathMustDie.Tables
{
    public class CustomItemDropsPerMinTable : ItemDropsPerMinTable, IEnumerable<ItemDropsPerMin>, IReadOnlyList<ItemDropsPerMin>, IReadOnlyCollection<ItemDropsPerMin>
    {
        private readonly List<ItemDropsPerMin> _entries = new List<ItemDropsPerMin>();

        public ItemDropsPerMin this[int index] => _entries[index];
        public new int Count => _entries.Count;
        public new IEnumerator<ItemDropsPerMin> GetEnumerator() => _entries.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public new void Add(ItemDropsPerMin entry) => _entries.Add(entry);

        public static CustomItemDropsPerMinTable CreateDefault()
        {
            var table = new CustomItemDropsPerMinTable();

            float[] minutes = { 0, 6, 12, 20 };
            foreach (var minute in minutes)
                table.Add(CreateEntry(minute));

            return table;
        }

        private static ItemDropsPerMin CreateEntry(float minute)
        {
            var rarity = new ItemRarityArray<float>();

            rarity.Set(ItemRarity.Broken, 0.00f);
            rarity.Set(ItemRarity.Common, 0.00f);
            rarity.Set(ItemRarity.Rare, 0.00f);
            rarity.Set(ItemRarity.Epic, 0.00f);
            rarity.Set(ItemRarity.Mythic, 1.00f);
            rarity.Set(ItemRarity.Immortal, 1.00f); //Immortal treated as Mythic for drops.  An immortal item will never drop, but triggers a unique drop roll.

            IReadOnlyWeighedRandomSet<ItemType> typeSet = CreateTypeSet();
            return new ItemDropsPerMin(minute, typeSet, rarity);
        }

        private static IReadOnlyWeighedRandomSet<ItemType> CreateTypeSet()
        {
            var weightedSet = new WeighedRandomSet<ItemType>();

            weightedSet.Add(ItemType.Weapon, 10f);
            weightedSet.Add(ItemType.Head, 10f);
            weightedSet.Add(ItemType.Torso, 10f);
            weightedSet.Add(ItemType.Hands, 10f);
            weightedSet.Add(ItemType.Waist, 10f);
            weightedSet.Add(ItemType.Feet, 10f);
            weightedSet.Add(ItemType.Ring, 10f);
            weightedSet.Add(ItemType.Amulet, 10f);
            weightedSet.Add(ItemType.Relic, 10f);
            weightedSet.Add(ItemType.Jewel, 10f);
            weightedSet.Add(ItemType.Lore, 0f);

            return weightedSet;
        }
    }
}
