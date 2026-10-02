using System;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace ResetJoystickMod
{
    [BepInPlugin("com.yourname.resetjoystick", "Reset Joystick On Freelook", "1.0.0")]
    public class ResetJoystickPlugin : BaseUnityPlugin
    {
        void Awake()
        {
            // Initialize Harmony and apply all patches defined in this assembly
            var harmony = new Harmony("com.yourname.resetjoystick");
            harmony.PatchAll();
            Logger.LogInfo("Reset Joystick Mod Loaded Successfully!");
        }
    }

    [HarmonyPatch(typeof(PilotPlayerState), "PlayerAxisControls")]
    public class PlayerAxisControlsPatch
    {
        // Метод Prefix выполняется перед оригинальным методом
        static bool Prefix(PilotPlayerState __instance)
        {
            Debug.Log("ResetJoystickMod: Method executed.");
            
            // Use reflection to access the 'player' field or property
            var playerField = typeof(PilotPlayerState).GetField("player", BindingFlags.NonPublic | BindingFlags.Instance);
            var playerProperty = typeof(PilotPlayerState).GetProperty("player", BindingFlags.NonPublic | BindingFlags.Instance);

            object player = null;

            // Check if 'player' is a field or property and retrieve its value
            if (playerField != null)
            {
                player = playerField.GetValue(__instance);
            }
            else if (playerProperty != null)
            {
                player = playerProperty.GetValue(__instance);
            }

            if (player != null)
            {
                Debug.Log("Player object retrieved successfully.");

                // Call the 'GetButton' method on the 'player' object
                var getButtonMethod = player.GetType().GetMethod("GetButton", new Type[] { typeof(string) });
                if (getButtonMethod != null)
                {
                    Debug.Log("GetButton method found.");

                    bool isFreeLookPressed = (bool)getButtonMethod.Invoke(player, new object[] { "Free Look" });
                    Debug.Log($"Is 'Free Look' pressed: {isFreeLookPressed}");

                    // Only reset the joystick when "Free Look" is pressed
                    if (isFreeLookPressed)
                    {
                        Debug.Log("Free Look is pressed. Resetting joystick.");
                        SceneSingleton<FlightHud>.i.SetVirtualJoystick(Vector3.zero);
                    }
                    else
                    {
                        Debug.Log("Free Look is not pressed. Joystick not reset.");
                    }
                }
                else
                {
                    Debug.LogError("GetButton method not found on player object.");
                }
            }
            else
            {
                Debug.LogError("Player object is null.");
            }
            
            // Allow the original method to run
            return true;
        }
    }
}
