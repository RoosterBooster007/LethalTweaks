using HarmonyLib;
using LethalTweaks;
using UnityEngine;

namespace LethalTools.Patches
{
    [HarmonyPatch(typeof(StunGrenadeItem))]
    public class GrenadesPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void patchGrenades(ref bool ___hasExploded)
        {
            if ((bool) TweaksBase.unStun.BoxedValue)
            {
                ___hasExploded = true; // stops grenades from exploding
            }
        }
    }

    [HarmonyPatch(typeof(Turret))]
    public class TurretsPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void patchTurrets(ref bool ___turretActive, ref Animator ___turretAnimator)
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
    public class MinesPatch
    {
        [HarmonyPatch("TriggerMineOnLocalClientByExiting")]
        [HarmonyPrefix]
        public static bool patchMines()
        {
            if ((bool)TweaksBase.disableMines.BoxedValue)
            {
                return false; // deactivates all mines
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(RedLocustBees))]
    public class BeesPatch
    {
        [HarmonyPatch("OnCollideWithPlayer")]
        [HarmonyPrefix]
        public static bool patchBees()
        {
            if ((bool)TweaksBase.disableBees.BoxedValue)
            {
                return false; // disables all bee cluster collisions
            }
            return true;
        }
    }
}
