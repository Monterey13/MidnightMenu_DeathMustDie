using Death.Items;
using HarmonyLib;

namespace MidnightMenu_DeathMustDie.Patches
{
    [HarmonyPatch(typeof(ItemAffix), nameof(ItemAffix.GetMaxVal))]
    public static class ItemAffix_MaxValPatch
    {
        static void Postfix(ref int __result)
        {
            try
            {
                if (ModMenu.ClampItemPower)
                {
                    __result = (int)(__result * ModMenu.ItemPowerMultiplier);
                }
                else
                {
                    __result = (int)(__result * ModMenu.ItemPowerMultiplier);
                }
            }
            catch (System.Exception ex)
            {
                ModMenu.Log.LogError("[ItemAffix_MaxValPatch] " + ex);
            }
        }
    }
}