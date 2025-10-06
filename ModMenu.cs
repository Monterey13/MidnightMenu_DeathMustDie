using System;
using BepInEx;
using BepInEx.Logging;
using MidnightMenu_DeathMustDie.Cheats;
using UnityEngine;
using MidnightMenu_DeathMustDie.Inspection;
using MidnightMenu_DeathMustDie.Patches;
using HarmonyLib;

namespace MidnightMenu_DeathMustDie
{
    [BepInPlugin("midnight.modmenu.deathmustdie", "MidnightMenu_DeathMustDie", "0.0.1")]
    public class ModMenu : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private bool _showMenu;
        private Rect _windowRect = new Rect(60, 60, 260, 340);
        public static float DropChanceModifier = 0.0f;
        public static float ItemPowerMultiplier = 1.0f;
        public static bool ClampItemPower = false;

        private void Awake()
        {
            Log = this.Logger;
            Log.LogInfo("[MidnightMenu] Plugin initializing...");

            try
            {
                // Driver to keep Update/OnGUI alive
                var go = new GameObject("Midnight_ModMenu_Driver");
                go.hideFlags = HideFlags.HideAndDontSave;
                DontDestroyOnLoad(go);

                var driver = go.AddComponent<UpdateDriver>();
                driver.OnUpdate += PluginUpdate;
                driver.OnGUIEvent += PluginOnGUI;

                Log.LogInfo("[MidnightMenu] Driver created & hooks attached successfully.");
            }
            catch (Exception ex)
            {
                Log.LogError("[MidnightMenu] Driver init FAILED:\n" + ex);
            }

            try
            {
                ModMenu.Log.LogInfo("[MidnightMenu] Runtime Detours applied successfully.");
            }
            catch (Exception ex)
            {
                Log.LogError("[MidnightMenu] Runtime Detours init FAILED:\n" + ex);
            }

            try
            {
                var harmony = new Harmony("midnight.modmenu.deathmustdie");
                harmony.PatchAll();
                ModMenu.Log.LogInfo("[MidnightMenu] Harmony patches applied successfully.");
            }
            catch (Exception ex)
            {
                Log.LogError("[MidnightMenu] Patching FAILED:\n" + ex);
            }
        }

        private void PluginUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                _showMenu = !_showMenu;
            }
        }

        private void PluginOnGUI()
        {
            if (!_showMenu)
                return;

            _windowRect = GUI.Window(4242, _windowRect, DrawWindow, "Midnight Menu");
        }

        private void DrawWindow(int id)
        {
            GUILayout.BeginVertical();

            if (GUILayout.Button("Replace Loot Table"))
                CheatManager.ReplaceLootTable();
            GUILayout.Space(2);

            ClampItemPower = GUILayout.Toggle(ClampItemPower, "Max Item Power");
            GUILayout.Space(2);

            GUILayout.Label("Drop Chance Modifier");
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{DropChanceModifier:F1}", GUILayout.Width(40));
            float newVal = GUILayout.HorizontalSlider(DropChanceModifier, 0f, 10f, GUILayout.Width(180));
            GUILayout.EndHorizontal();
            GUILayout.Space(2);

            GUILayout.Label("Item Power Multiplier");
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{ItemPowerMultiplier:F1}", GUILayout.Width(40));
            float newVal2 = GUILayout.HorizontalSlider(ItemPowerMultiplier, 0f, 10f, GUILayout.Width(180));
            GUILayout.EndHorizontal();

            DropChanceModifier = Mathf.Round(newVal * 10f) / 10f;
            ItemPowerMultiplier = Mathf.Round(newVal2 * 10f) / 10f;

            GUILayout.EndVertical();
            GUI.DragWindow();
        }
    }

    public class UpdateDriver : MonoBehaviour
    {
        public event Action OnUpdate;
        public event Action OnGUIEvent;

        private void Update()
        {
            try
            {
                OnUpdate?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError("[MidnightMenu] UpdateDriver.Update Exception: " + ex);
            }
        }

        private void OnGUI()
        {
            try
            {
                OnGUIEvent?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError("[MidnightMenu] UpdateDriver.OnGUI Exception: " + ex);
            }
        }
    }
}
