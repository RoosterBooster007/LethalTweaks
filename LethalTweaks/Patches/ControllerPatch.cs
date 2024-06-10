using GameNetcodeStuff;
using HarmonyLib;
using LethalTweaks;
using Steamworks;
using Steamworks.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace LethalTools.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    public class ControllerPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void patchLighting(ref Light ___nightVision)
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
        public static void patchController(PlayerControllerB __instance, ref float ___sprintMultiplier)
        {
            TweaksBase.plyrManager = __instance.playersManager;

            TweaksBase.isChatting = __instance.isTypingChat;
            TweaksBase.isInTerminal = __instance.inTerminalMenu;
            TweaksBase.isMenuOpen = __instance.quickMenuManager.isMenuOpen;
            TweaksBase.isPlayerControlled = __instance.isPlayerControlled;

            if (!__instance.isTypingChat && !__instance.inTerminalMenu && __instance.quickMenuManager.isMenuOpen && (bool)TweaksBase.enableKeybinds.BoxedValue) // do not allow keybinds when chatting or in terminal
            {
                if (TweaksBase.IAInstance.SuicideKey.WasPressedThisFrame() && NetworkManager.Singleton.LocalClientId == __instance.playerClientId && !__instance.isPlayerDead)
                {
                    __instance.KillPlayer(Vector3.up * (float)TweaksBase.llVelocity.BoxedValue, true, CauseOfDeath.Gravity, 0); // launches killed player when keybind pressed
                    TweaksBase.SendGAEvent("event", "suicide", new Dictionary<string, string>() { ["initial_velocity"] = ((float)TweaksBase.llVelocity.BoxedValue).ToString(), ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
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

            }
            else if (!__instance.isPlayerDead && __instance.isInsideFactory)
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
            }
            else if (TweaksBase.shouldRevertSpeed)
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

        [HarmonyPatch("BreakLegsSFXServerRpc")]
        [HarmonyPostfix]
        public static void patchStartEmote()
        {
            TweaksBase.SendGAEvent("event", "fall", new Dictionary<string, string>() { ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
        }

        [HarmonyPatch("StartPerformingEmoteServerRpc")]
        [HarmonyPostfix]
        public static void patchStartEmote(ref Animator ___playerBodyAnimator)
        {
            TweaksBase.SendGAEvent("event", "emote", new Dictionary<string, string>() { ["emote_id"] = ___playerBodyAnimator.GetInteger("emoteNumber").ToString(), ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
        }
    }

    [HarmonyPatch(typeof(LobbySlot))]
    public class LobbyPatch
    {
        [HarmonyPatch("JoinLobbyAfterVerifying")]
        [HarmonyPostfix]
        public static void patchJoining(Lobby lobby, SteamId lobbyId)
        {
            TweaksBase.SendGAEvent("client", "join", new Dictionary<string, string>() { ["lobby_id"] = lobbyId.ToString(), ["lobby_name"] = lobby.GetData("name") });
        }
    }

    [HarmonyPatch(typeof(GameNetworkManager))]
    public class LeavePatch
    {
        [HarmonyPatch("StartHost")]
        [HarmonyPostfix]
        public static void patchHosting(ref HostSettings ___lobbyHostSettings)
        {
            TweaksBase.SendGAEvent("client", "host", new Dictionary<string, string>() { ["lobby_name"] = ___lobbyHostSettings.lobbyName });
        }

        [HarmonyPatch("DisconnectProcess")]
        [HarmonyPostfix]
        public static void patchLeaving(ref int ___disconnectReason, ref bool ___isHostingGame)
        {
            TweaksBase.SendGAEvent("client", "leave", new Dictionary<string, string>() { ["reason_id"] = ___disconnectReason.ToString(), ["hosting"] = ___isHostingGame.ToString(), ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
        }

        [HarmonyPatch("OnDisable")]
        [HarmonyPrefix]
        public static void patchQuit()
        {
            TweaksBase.SendGAEvent("state", "session_end", new Dictionary<string, string>() { ["closed"] = "true" });
        }
    }

    [HarmonyPatch(typeof(StartOfRound))]
    public class SoRPatches
    {
        [HarmonyPatch("FirePlayersAfterDeadlineClientRpc")]
        [HarmonyPrefix]
        public static void patchJoining(ref EndOfGameStats ___gameStats, ref bool ___firingPlayersCutsceneRunning)
        {
            if (___firingPlayersCutsceneRunning) return;
            ___firingPlayersCutsceneRunning = true;
            TweaksBase.SendGAEvent("event", "fired", new Dictionary<string, string>() { ["days_spent"] = ___gameStats.daysSpent.ToString(), ["scrap_value"] = ___gameStats.scrapValueCollected.ToString(), ["steps_taken"] = ___gameStats.allStepsTaken.ToString() });
        }

        [HarmonyPatch("openingDoorsSequence")]
        [HarmonyPostfix]
        public static void patchLandShip()
        {
            TweaksBase.SendGAEvent("event", "land", new Dictionary<string, string>() { ["moon"] = Regex.Replace(StartOfRound.Instance.currentLevel.PlanetName, @"\d", string.Empty).Trim(), ["weather"] = StartOfRound.Instance.currentLevel.currentWeather.ToString() });
        }
    }

    [HarmonyPatch(typeof(QuickMenuManager))]
    public class QMenuPatch
    {
        [HarmonyPatch("OpenQuickMenu")]
        [HarmonyPostfix]
        public static void patchOpen()
        {
            TweaksBase.SendGAEvent("event", "menu", new Dictionary<string, string>() { ["open"] = "true" });
        }

        [HarmonyPatch("CloseQuickMenu")]
        [HarmonyPostfix]
        public static void patchClose()
        {
            TweaksBase.SendGAEvent("event", "menu", new Dictionary<string, string>() { ["open"] = "false" });
        }
    }

    [HarmonyPatch(typeof(MenuManager))]
    public class MenuPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void patchVersionText(MenuManager __instance, ref TextMeshProUGUI ___versionNumberText)
        {
            if (__instance != null && ___versionNumberText != null)
            {
                ___versionNumberText.enableWordWrapping = false;
                ___versionNumberText.text = "v" + GameNetworkManager.Instance.gameVersionNum + " - LTs v" + TweaksBase.ModVersion;

                if (TweaksBase.recentlyUpdated())
                {
                    __instance.DisplayMenuNotification("LethalTweaks has been updated to v" + TweaksBase.ModVersion + "!\n\nSee CHANGELOG.md for more info.", "Cool");
                    TweaksBase.SendGAEvent("state", "update", new Dictionary<string, string>() { ["version"] = TweaksBase.ModVersion });
                }
            }
        }
    }
}
