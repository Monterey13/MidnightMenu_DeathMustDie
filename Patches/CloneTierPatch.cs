using Death.Items;
using HarmonyLib;

namespace MidnightMenu_DeathMustDie.Patches
{
    [HarmonyPatch(typeof(ItemArchetype), nameof(ItemArchetype.GetTier))]
    public static class ItemArchetypeTierPatch
    {
        static void Postfix(ref ItemArchetype.Tier __result)
        {
            if (__result == null)
                return;

            try
            {
                int newMin = ModMenu.ClampItemPower
                    ? __result.TotalValueMax
                    : __result.TotalValueMin;

                int newMax = __result.TotalValueMax;

                newMin = (int)(newMin * ModMenu.ItemPowerMultiplier);
                newMax = (int)(newMax * ModMenu.ItemPowerMultiplier);

                var modified = new ItemArchetype.Tier(
                    newMin,
                    newMax,
                    __result.BaseValueMin,
                    __result.BaseValueMax
                );

                __result = modified;
            }
            catch (System.Exception ex)
            {
                ModMenu.Log.LogError("[TierPatch] Failed to modify tier: " + ex);
            }
        }
    }
}
