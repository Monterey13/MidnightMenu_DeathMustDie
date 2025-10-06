using HarmonyLib;
using System.Reflection;
using Claw.Core.Types;

namespace MidnightMenu_DeathMustDie.Patches
{
    [HarmonyPatch]
    public static class LootDropperCoroutinePatch
    {
        static MethodBase TargetMethod()
        {
            var type = AccessTools.TypeByName("Death.Run.Systems.System_LootDropper+<GenerateLoot>d__15");
            return AccessTools.Method(type, "MoveNext");
        }

        [HarmonyPrefix]
        public static void Prefix(object __instance)
        {
            var t = __instance.GetType();
            var dropChanceField = t.GetField("dropChance", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (dropChanceField == null)
            {
                ModMenu.Log.LogWarning("[LootDropperCoroutine] dropChance field not found");
                return;
            }

            try
            {
                var currentValue = dropChanceField.GetValue(__instance);
                ModMenu.Log.LogInfo($"[LootDropperCoroutine] Current dropChance = {currentValue}");

                // Create a new Optional<float> instance with our slider value
                var optionalType = currentValue.GetType();
                var ctor = optionalType.GetConstructor(new[] { typeof(float) });
                if (ctor != null)
                {
                    var newOptional = ctor.Invoke(new object[] { ModMenu.DropChanceModifier });
                    dropChanceField.SetValue(__instance, newOptional);
                    ModMenu.Log.LogInfo($"[LootDropperCoroutine] dropChance updated → {ModMenu.DropChanceModifier}");
                }
                else
                {
                    ModMenu.Log.LogWarning("[LootDropperCoroutine] Could not locate Optional<float> constructor");
                }
            }
            catch (System.Exception ex)
            {
                ModMenu.Log.LogError($"[LootDropperCoroutine] Failed to modify dropChance: {ex}");
            }
        }
    }
}
