using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace centerVirtualJoystickWithFreelook
{

[BepInPlugin("com.vento.centerVirtualJoystickWithFreelook", "centerVirtualJoystickWithFreelook", "1.0.0")]
public class CenterVirtualJoystickWithFreelook_Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        var harmony = new Harmony("com.vento.centerVirtualJoystickWithFreelook");
        harmony.PatchAll();
        Logger.LogInfo("centerVirtualJoystickWithFreelook Loaded Successfully!");
    }
}

[HarmonyPatch(typeof(PilotPlayerState), "PlayerAxisControls")]
class CenterVirtualJoystickWithFreelook
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
