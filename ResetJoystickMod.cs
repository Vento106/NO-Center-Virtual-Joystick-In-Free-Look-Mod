using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace ResetJoystickMod
{

[BepInPlugin("com.vento.resetjoystick", "Auto-Center Joystick In Freelook", "1.0.0")]
public class ResetJoystickPlugin : BaseUnityPlugin
{
    private void Awake()
    {
        var harmony = new Harmony("com.vento.resetjoystick");
        harmony.PatchAll();
        Logger.LogInfo("Reset Joystick Mod Loaded Successfully!");
    }
}

[HarmonyPatch(typeof(PilotPlayerState), "PlayerAxisControls")]
class ResetJoystickPatch
{
    [HarmonyPrefix]
    private static void Prefix(PilotPlayerState __instance)
    {
        if (!PlayerSettings.virtualJoystickEnabled)
            return;

        if (__instance.player.GetButton("Free Look"))
            SceneSingleton<FlightHud>.i.SetVirtualJoystick(Vector3.zero);
    }
}
}
