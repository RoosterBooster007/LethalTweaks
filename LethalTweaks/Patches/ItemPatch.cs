using HarmonyLib;
using LethalTweaks;
using System.Collections.Generic;

namespace LethalTools.Patches
{
    [HarmonyPatch(typeof(GrabbableObject))]
    public class ItemPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void patchItems(ref float ___currentUseCooldown, ref Battery ___insertedBattery)
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

        [HarmonyPatch("OnBroughtToShip")]
        [HarmonyPostfix]
        public static void patchCollect(ref int ___scrapValue, ref Item ___itemProperties)
        {
            TweaksBase.SendGAEvent("event", "collect", new Dictionary<string, string>() { ["item_name"] = ___itemProperties.itemName, ["scrap_value"] = ___scrapValue.ToString(), ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
        }
    }

    [HarmonyPatch(typeof(ShipAlarmCord))]
    public class HornPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void patchHorn(ShipAlarmCord __instance)
        {
            if (TweaksBase.sendHornDown)
            {
                TweaksBase.sendHornDown = false;
                __instance.HoldCordDown();
                TweaksBase.SendGAEvent("client", "honk", new Dictionary<string, string>() { ["sound_playing"] = "true" });
            }
            if (TweaksBase.sendHornUp)
            {
                TweaksBase.sendHornUp = false;
                __instance.StopHorn();
                TweaksBase.SendGAEvent("client", "honk", new Dictionary<string, string>() { ["sound_playing"] = "false" });
            }
        }
    }

    [HarmonyPatch(typeof(BridgeTrigger))]
    public class BridgePatch
    {
        [HarmonyPatch("BridgeFallClientRpc")]
        [HarmonyPrefix]
        public static bool patchBF()
        {
            if ((bool)TweaksBase.disableBridgeFall.BoxedValue)
            {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(DoorLock))]
    public class LockPatch
    {
        [HarmonyPatch("OnHoldInteract")]
        [HarmonyPostfix]
        public static void patchPullAppa(ref DoorLock __instance)
        {
            if (!__instance.isLocked) return;
            __instance.UnlockDoorSyncWithServer();
            TweaksBase.SendGAEvent("event", "unlock", new Dictionary<string, string>() { ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
        }
    }

    [HarmonyPatch(typeof(LungProp))]
    public class AppaPatch
    {
        [HarmonyPatch("DisconnectFromMachinery")]
        [HarmonyPostfix]
        public static void patchPullAppa()
        {
            TweaksBase.SendGAEvent("event", "pull_lung", new Dictionary<string, string>() { ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
        }
    }
}
