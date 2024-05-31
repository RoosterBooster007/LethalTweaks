using HarmonyLib;
using LethalTweaks;

namespace LethalTools.Patches
{
    [HarmonyPatch(typeof(GrabbableObject))]
    internal class ItemPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void patchItems(ref float ___currentUseCooldown, ref Battery ___insertedBattery)
        {
            if ((bool)TweaksBase.warmUp.BoxedValue)
            {
                ___currentUseCooldown = -0.1f; // cooldown is always set to done
            }
            if ((bool)TweaksBase.fullCharge.BoxedValue)
            {
                ___insertedBattery.charge = 1f; // charge always set to full
            }
        }
    }
    [HarmonyPatch(typeof(ShipAlarmCord))]
    internal class HornPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void patchHorn(ShipAlarmCord __instance)
        {
            if (TweaksBase.sendHornDown)
            {
                TweaksBase.sendHornDown = false;
                __instance.HoldCordDown();
            }
            if (TweaksBase.sendHornUp)
            {
                TweaksBase.sendHornUp = false;
                __instance.StopHorn();
            }
        }
    }
    [HarmonyPatch(typeof(BridgeTrigger))]
    internal class BridgePatch
    {
        [HarmonyPatch("BridgeFallClientRpc")]
        [HarmonyPostfix]
        private static void patchBF(ref bool __result)
        {
            if ((bool)TweaksBase.disableBridgeFall.BoxedValue)
            {
                __result = false;
            }
        }
    }
}
