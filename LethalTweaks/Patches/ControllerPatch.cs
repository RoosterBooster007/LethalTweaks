using GameNetcodeStuff;
using HarmonyLib;
using LethalTweaks;
using Unity.Netcode;
using UnityEngine;

namespace LethalTools.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class ControllerPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void patchLighting(ref Light ___nightVision)
        {
            ___nightVision.type = LightType.Point;
            ___nightVision.intensity = 45000f;
            ___nightVision.range = 99999f;
            ___nightVision.shadowStrength = 0f;
            ___nightVision.bounceIntensity = 5555f;
            ___nightVision.innerSpotAngle = 999f;
            ___nightVision.spotAngle = 9999f;
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void patchController(PlayerControllerB __instance, ref float ___sprintMultiplier)
        {
            TweaksBase.plyrManager = __instance.playersManager;

            TweaksBase.isChatting = __instance.isTypingChat;
            TweaksBase.isInTerminal = __instance.inTerminalMenu;
            TweaksBase.isMenuOpen = __instance.quickMenuManager.isMenuOpen;

            if (!__instance.isTypingChat && !__instance.inTerminalMenu && __instance.quickMenuManager.isMenuOpen && (bool)TweaksBase.enableKeybinds.BoxedValue) // do not allow keybinds when chatting or in terminal
            {
                if (TweaksBase.IAInstance.SuicideKey.WasPressedThisFrame() && NetworkManager.Singleton.LocalClientId == __instance.playerClientId && !__instance.isPlayerDead)
                {
                    __instance.KillPlayer(Vector3.up * 50f, true, CauseOfDeath.Gravity, 0); // launches killed player when keybind pressed
                }
            }

            if ((bool)TweaksBase.enableCustomBrightness.BoxedValue && __instance.isInsideFactory)
            {
                __instance.nightVision.enabled = true;
                __instance.nightVision.type = LightType.Point;
                __instance.nightVision.intensity = (float)TweaksBase.speedMultiplier.BoxedValue * 1000f;
                __instance.nightVision.range = 99999f;
                __instance.nightVision.shadowStrength = 0f;
                __instance.nightVision.bounceIntensity = 5555f;
                __instance.nightVision.innerSpotAngle = 999f;
                __instance.nightVision.spotAngle = 9999f;
                __instance.nightVision.gameObject.SetActive(true); // enable and set vision level

            } else if (!__instance.isPlayerDead && __instance.isInsideFactory)
            {
                __instance.nightVision.gameObject.SetActive(false);
                __instance.nightVision.enabled = false;
            }
            if ((bool)TweaksBase.enableInfiniteStamina.BoxedValue)
            {
                __instance.sprintMeter = 1f; // stamina always set to full
            }
            if ((bool)TweaksBase.enableWeightless.BoxedValue)
            {
                __instance.carryWeight = 1f; // weight always set to none
            }
            if ((float)TweaksBase.speedMultiplier.BoxedValue > 0 && (bool)TweaksBase.enableSpeed.BoxedValue)
            {
                TweaksBase.shouldRevertSpeed = true;
                ___sprintMultiplier = (float)TweaksBase.speedMultiplier.BoxedValue; // custom user speed
            } else if (TweaksBase.shouldRevertSpeed)
            {
                TweaksBase.shouldRevertSpeed = false;
                ___sprintMultiplier = 1f;
            }
            if ((float)TweaksBase.climbSpeed.BoxedValue > 0 && (bool)TweaksBase.enableClimbSpeed.BoxedValue)
            {
                TweaksBase.shouldRevertClimbSpeed = true;
                __instance.climbSpeed = (float)TweaksBase.climbSpeed.BoxedValue; // custom user speed
            }
            else if (TweaksBase.shouldRevertClimbSpeed)
            {
                TweaksBase.shouldRevertSpeed = false;
                __instance.climbSpeed = 4f;
            }
            if ((bool)TweaksBase.disableFallDamage.BoxedValue)
            {
                __instance.takingFallDamage = false; // never taking fall damage
            }
            if ((bool)TweaksBase.underwaterRoaming.BoxedValue)
            {
                StartOfRound.Instance.drowningTimer = 1f; // reset drowning timer
            }
            if ((bool)TweaksBase.unSink.BoxedValue)
            {
                __instance.sourcesCausingSinking = 0; // set sink surfaces to 0... may need to be a prefix
            }
            if ((bool)TweaksBase.unMuffle.BoxedValue)
            {
                if (__instance.currentVoiceChatIngameSettings != null)
                {
                    __instance.currentVoiceChatIngameSettings.voiceAudio.GetComponent<OccludeAudio>().overridingLowPass = false; // force disable lowpass filter all the time
                }
            }
            if ((bool)TweaksBase.nameTags.BoxedValue)
            {
                for (int i = 0; i < __instance.playersManager.allPlayerObjects.Length; i++)
                {
                    PlayerControllerB component = __instance.playersManager.allPlayerObjects[i].GetComponent<PlayerControllerB>();
                    component.ShowNameBillboard();
                }
            }
            if ((bool)TweaksBase.oneHanded.BoxedValue)
            {
                __instance.twoHanded = false; // player is always in one-handed mode (may need fix with twoHandedAnimation)
            }
        }

        [HarmonyPatch("ConnectClientToPlayerObject")]
        [HarmonyPrefix]
        private static void patchUser(ref ulong ___playerClientId)
        {
            if ((bool)TweaksBase.userPrefixEnabled.BoxedValue && NetworkManager.Singleton.LocalClientId == ___playerClientId)
            {
                GameNetworkManager.Instance.username = TweaksBase.userPrefix.BoxedValue + " " + GameNetworkManager.Instance.username; // adds prefix
            }
        }

        [HarmonyPatch("NoPunctuation")]
        [HarmonyPostfix]
        private static void patchUsername(ref ulong ___playerClientId, ref string __result)
        {
            if ((bool) TweaksBase.userPrefixEnabled.BoxedValue && NetworkManager.Singleton.LocalClientId == ___playerClientId)
            {
                __result = TweaksBase.userPrefix.BoxedValue + " " + __result; // also adds prefix
            }
        }
    }
}
