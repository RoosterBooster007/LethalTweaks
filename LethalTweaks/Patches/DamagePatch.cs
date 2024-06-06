using HarmonyLib;
using LethalTweaks;
using UnityEngine;

namespace LethalTools.Patches
{
    [HarmonyPatch(typeof(StunGrenadeItem))]
    internal class GrenadesPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void patchGrenades(ref bool ___hasExploded)
        {
            if ((bool) TweaksBase.unStun.BoxedValue)
            {
                ___hasExploded = true; // stops grenades from exploding
            }
        }
    }

    [HarmonyPatch(typeof(Turret))]
    internal class TurretsPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void patchTurrets(ref bool ___turretActive, ref Animator ___turretAnimator)
        {
            if ((bool)TweaksBase.disableTurrets.BoxedValue)
            {
                if (___turretActive)
                {
                    ___turretActive = false; // deactivates all turrets
                    ___turretAnimator.SetBool("turretActive", false);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Landmine))]
    internal class MinesPatch
    {
        [HarmonyPatch("TriggerMineOnLocalClientByExiting")]
        [HarmonyPrefix]
        private static void patchMines(ref bool __result)
        {
            if ((bool)TweaksBase.disableMines.BoxedValue)
            {
                __result = false; // deactivates all mines
            }
        }
    }

    [HarmonyPatch(typeof(RedLocustBees))]
    internal class BeesPatch
    {
        [HarmonyPatch("OnCollideWithPlayer")]
        [HarmonyPrefix]
        private static void patchBees(ref bool __result)
        {
            if ((bool)TweaksBase.disableBees.BoxedValue)
            {
                __result = false; // disables all bee cluster collisions
            }
        }
    }
}
