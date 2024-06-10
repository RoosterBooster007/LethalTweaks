using GameNetcodeStuff;
using LethalTweaks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LethalTools
{
    public class InputEvents
    {
        public void Init()
        {
            TweaksBase.IAInstance.NightVisionKey.performed += OnNightVisionInput;
            TweaksBase.IAInstance.NameTagKey.performed += OnNameTagInput;
            TweaksBase.IAInstance.StaminaKey.performed += OnStaminaInput;
            TweaksBase.IAInstance.WeightlessKey.performed += OnWeightlessInput;
            TweaksBase.IAInstance.SpeedKey.performed += OnSpeedInput;
            TweaksBase.IAInstance.ClimbSpeedKey.performed += OnClimbSpeedInput;
            TweaksBase.IAInstance.SinkKey.performed += OnSinkInput;
            TweaksBase.IAInstance.FallDamageKey.performed += OnFallDamageInput;
            TweaksBase.IAInstance.WaterBKey.performed += OnWaterBInput;
            TweaksBase.IAInstance.CooldownKey.performed += OnCooldownInput;
            TweaksBase.IAInstance.BatteryKey.performed += OnBatteryInput;
            TweaksBase.IAInstance.OneHKey.performed += OnOneHInput;
            TweaksBase.IAInstance.ShipKey.performed += OnShipInput;
            TweaksBase.IAInstance.EntranceKey.performed += OnEntranceInput;
            TweaksBase.IAInstance.StartKey.performed += OnStartInput;
        }
        public static bool isValidInput()
        {
            if (!TweaksBase.isChatting && !TweaksBase.isInTerminal && !TweaksBase.isMenuOpen && (bool)TweaksBase.enableKeybinds.BoxedValue && TweaksBase.isPlayerControlled)
            {
                return true;
            }
            return false;
        }
        public void OnNightVisionInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.enableCustomBrightness.BoxedValue = !(bool)TweaksBase.enableCustomBrightness.BoxedValue;
            TweaksBase.SendGAEvent("toggle", "night_vision", new Dictionary<string, string>() { ["activated"] = TweaksBase.enableCustomBrightness.BoxedValue.ToString()} );
        }
        public void OnNameTagInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.nameTags.BoxedValue = !(bool)TweaksBase.nameTags.BoxedValue;
            TweaksBase.SendGAEvent("toggle", "name_tags", new Dictionary<string, string>() { ["activated"] = TweaksBase.nameTags.BoxedValue.ToString() });
        }
        public void OnStaminaInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.enableInfiniteStamina.BoxedValue = !(bool)TweaksBase.enableInfiniteStamina.BoxedValue;
            TweaksBase.SendGAEvent("toggle", "stamina", new Dictionary<string, string>() { ["activated"] = TweaksBase.enableInfiniteStamina.BoxedValue.ToString() });
        }
        public void OnWeightlessInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.enableWeightless.BoxedValue = !(bool)TweaksBase.enableWeightless.BoxedValue;
            TweaksBase.SendGAEvent("toggle", "weightless", new Dictionary<string, string>() { ["activated"] = TweaksBase.enableWeightless.BoxedValue.ToString() });
        }
        public void OnSpeedInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.enableSpeed.BoxedValue = !(bool)TweaksBase.enableSpeed.BoxedValue;
            TweaksBase.SendGAEvent("toggle", "speed", new Dictionary<string, string>() { ["activated"] = TweaksBase.enableSpeed.BoxedValue.ToString() });
        }
        public void OnClimbSpeedInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.enableClimbSpeed.BoxedValue = !(bool)TweaksBase.enableClimbSpeed.BoxedValue;
            TweaksBase.SendGAEvent("toggle", "climb_speed", new Dictionary<string, string>() { ["activated"] = TweaksBase.enableClimbSpeed.BoxedValue.ToString() });
        }
        public void OnSinkInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.unSink.BoxedValue = !(bool)TweaksBase.unSink.BoxedValue;
            TweaksBase.SendGAEvent("toggle", "anti_sink", new Dictionary<string, string>() { ["activated"] = TweaksBase.unSink.BoxedValue.ToString() });
        }
        public void OnFallDamageInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.disableFallDamage.BoxedValue = !(bool)TweaksBase.disableFallDamage.BoxedValue;
            TweaksBase.SendGAEvent("toggle", "anti_fall_damage", new Dictionary<string, string>() { ["activated"] = TweaksBase.disableFallDamage.BoxedValue.ToString() });
        }
        public void OnWaterBInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.underwaterRoaming.BoxedValue = !(bool)TweaksBase.underwaterRoaming.BoxedValue;
            TweaksBase.SendGAEvent("toggle", "water_breathing", new Dictionary<string, string>() { ["activated"] = TweaksBase.underwaterRoaming.BoxedValue.ToString() });
        }
        public void OnCooldownInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.warmUp.BoxedValue = !(bool)TweaksBase.warmUp.BoxedValue;
            TweaksBase.SendGAEvent("toggle", "anti_cooldowns", new Dictionary<string, string>() { ["activated"] = TweaksBase.warmUp.BoxedValue.ToString() });
        }
        public void OnBatteryInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.fullCharge.BoxedValue = !(bool)TweaksBase.fullCharge.BoxedValue;
            TweaksBase.SendGAEvent("toggle", "unlimited_power", new Dictionary<string, string>() { ["activated"] = TweaksBase.fullCharge.BoxedValue.ToString() });
        }
        public void OnOneHInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.oneHanded.BoxedValue = !(bool)TweaksBase.oneHanded.BoxedValue;
            TweaksBase.SendGAEvent("toggle", "one_handed", new Dictionary<string, string>() { ["activated"] = TweaksBase.oneHanded.BoxedValue.ToString() });
        }
        public void OnShipInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            warpToShip();
        }
        public void OnEntranceInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            warpToEntrance();
        }
        public void OnStartInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            startGame();
        }
        public static void startGame()
        {
            Object.FindObjectOfType<StartMatchLever>().PlayLeverPullEffectsServerRpc(true);
            StartOfRound.Instance.StartGameServerRpc();
            TweaksBase.mls.LogInfo("Lever pulled! Starting game...");
            TweaksBase.SendGAEvent("event", "remote_start", new Dictionary<string, string>() { ["online_players"] = StartOfRound.Instance.connectedPlayersAmount.ToString() });
        }
        public static void pullLeverAs()
        {
            if (TweaksBase.plyrManager != null)
            {
                for (int i = 0; i < TweaksBase.plyrManager.allPlayerScripts.Length; i++)
                {
                    PlayerControllerB target = TweaksBase.plyrManager.allPlayerScripts[i];
                    if (target.playerUsername.Equals((string)TweaksBase.playerSelect.BoxedValue))
                    {
                        StartOfRound.Instance.EndGameServerRpc(i);
                        TweaksBase.mls.LogInfo("EndGameServerRpc sent \"from\": " + (string)TweaksBase.playerSelect.BoxedValue);
                        TweaksBase.SendGAEvent("event", "remote_pull", new Dictionary<string, string>() { ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
                    }
                }
            }
        }
        public static void killTarget()
        {
            if (TweaksBase.plyrManager != null)
            {
                for (int i = 0; i < TweaksBase.plyrManager.allPlayerScripts.Length; i++)
                {
                    PlayerControllerB target = TweaksBase.plyrManager.allPlayerScripts[i];
                    if (target.playerUsername.Equals((string)TweaksBase.playerSelect.BoxedValue))
                    {
                        target.DamagePlayerFromOtherClientServerRpc(999, UnityEngine.Vector3.zero, i);
                        TweaksBase.mls.LogInfo("DamagePlayerFromOtherClientServerRpc sent from \"self\": " + (string)TweaksBase.playerSelect.BoxedValue);
                        TweaksBase.SendGAEvent("event", "kill", new Dictionary<string, string>() { ["living_players"] = StartOfRound.Instance.livingPlayers.ToString() });
                    }
                }
            }
        }
        public static void nukeLobby()
        {
            if (TweaksBase.plyrManager != null)
            {
                for (int i = 0; i < TweaksBase.plyrManager.allPlayerScripts.Length; i++)
                {
                    PlayerControllerB target = TweaksBase.plyrManager.allPlayerScripts[i];
                    target.DamagePlayerFromOtherClientServerRpc(999, UnityEngine.Vector3.zero, i);
                    TweaksBase.mls.LogInfo("Nuked player: " + target.playerUsername);
                }
                TweaksBase.mls.LogInfo("Nuked entire lobby - game (should be) ending...");
                TweaksBase.SendGAEvent("event", "nuke", new Dictionary<string, string>() { ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
            }
        }
        public static void reRouteShip()
        {
            if (StartOfRound.Instance.levels != null)
            {
                for (int i = 0; i < StartOfRound.Instance.levels.Length; i++)
                {
                    SelectableLevel level = StartOfRound.Instance.levels[i];
                    if (Regex.Replace(level.PlanetName, @"\d", string.Empty).Trim().Equals((string)TweaksBase.moonSelect.BoxedValue))
                    {
                        StartOfRound.Instance.ChangeLevelServerRpc(i, Object.FindObjectOfType<Terminal>().groupCredits);
                        TweaksBase.mls.LogInfo("Setting final moon destination to: " + (string)TweaksBase.moonSelect.BoxedValue);
                        TweaksBase.SendGAEvent("event", "route", new Dictionary<string, string>() { ["moon"] = (string)TweaksBase.moonSelect.BoxedValue });
                    }
                }
            }
        }
        public static void openShip()
        {
            StartOfRound.Instance.shipDoorsAnimator.SetBool("Closed", false);
            StartOfRound.Instance.hangarDoorsClosed = false;
            TweaksBase.mls.LogInfo("Opening ship doors...");
            TweaksBase.SendGAEvent("event", "doors", new Dictionary<string, string>() { ["closed"] = "false" });
        }
        public static void closeShip()
        {
            StartOfRound.Instance.shipDoorsAnimator.SetBool("Closed", true);
            StartOfRound.Instance.hangarDoorsClosed = true;
            TweaksBase.mls.LogInfo("Closing ship doors...");
            TweaksBase.SendGAEvent("event", "doors", new Dictionary<string, string>() { ["closed"] = "true" });
        }
        public static void warpToShip()
        {
            GameNetworkManager.Instance.localPlayerController.TeleportPlayer(StartOfRound.Instance.playerSpawnPositions[0].position, false, 0f, false, true);
            TweaksBase.mls.LogInfo("Warping player to ship...");
            TweaksBase.SendGAEvent("event", "warp_ship", new Dictionary<string, string>() { ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
        }
        public static void warpToEntrance()
        {
            if (RoundManager.FindMainEntrancePosition(true, true) != Vector3.zero)
            {
                GameNetworkManager.Instance.localPlayerController.TeleportPlayer(RoundManager.FindMainEntrancePosition(true, true), false, 0f, false, true);
                TweaksBase.mls.LogInfo("Warping player to entrance...");
                TweaksBase.SendGAEvent("event", "warp_entrance", new Dictionary<string, string>() { ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
            }
        }
        public static void sendTextMessage()
        {
            HUDManager.Instance.AddTextToChatOnServer((string)TweaksBase.chatMessage.BoxedValue);
            TweaksBase.mls.LogInfo("Sent anon. message: " + (string)TweaksBase.chatMessage.BoxedValue);
            TweaksBase.SendGAEvent("event", "anon_msg", new Dictionary<string, string>() { ["length"] = ((string)TweaksBase.chatMessage.BoxedValue).Length.ToString() });
        }
        public static void killEnemies()
        {
            EnemyAI[] array = Object.FindObjectsOfType<EnemyAI>();
            foreach (EnemyAI ai in array)
            {
                RoundManager.Instance.DespawnEnemyOnServer(ai.NetworkObject);
                TweaksBase.mls.LogInfo("Despawned EnemyAI on server - Type: " + ai.gameObject.name);
            }
            TweaksBase.SendGAEvent("event", "despawn_enemies", new Dictionary<string, string>() { ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
        }
        public static void killItems()
        {
            GrabbableObject[] array = Object.FindObjectsOfType<GrabbableObject>();
            int presize = array.Length;
            foreach (GrabbableObject item in array)
            {
                item.GetComponent<MeshRenderer>().gameObject.SetActive(false);
                item.NetworkObject.Despawn();
                item.NetworkObject.transform.SetParent(null, false);
                Object.Destroy(item);
                TweaksBase.mls.LogInfo("Despawned GrabbableObject on server - Type: " + item.gameObject.name);
            }
            TweaksBase.SendGAEvent("event", "despawn_items", new Dictionary<string, string>() { ["count"] = presize.ToString() });
        }
        public static void unlockAllDoors()
        {
            DoorLock[] array = Object.FindObjectsOfType<DoorLock>();
            foreach (DoorLock dlock in array)
            {
                dlock.UnlockDoorSyncWithServer();
                dlock.OpenDoorAsEnemyServerRpc();
                TweaksBase.mls.LogInfo("Unlocked factory door on server - ID: " + dlock.gameObject.name);
            }
            TerminalAccessibleObject[] array2 = Object.FindObjectsOfType<TerminalAccessibleObject>();
            foreach (TerminalAccessibleObject tlock in array2)
            {
                tlock.OnPowerSwitch(true);
                tlock.SetDoorLocalClient(true);
                TweaksBase.mls.LogInfo("Unlocked factory gate on server - ID: " + tlock.gameObject.name);
            }
            TweaksBase.SendGAEvent("event", "unlock_doors", new Dictionary<string, string>() { ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
        }
        public static void fixAllValves()
        {
            SteamValveHazard[] array = Object.FindObjectsOfType<SteamValveHazard>();
            foreach (SteamValveHazard valve in array)
            {
                valve.FixValveServerRpc();
                TweaksBase.mls.LogInfo("Factory steam valve repaired - Type: " + valve.gameObject.name);
            }
            TweaksBase.SendGAEvent("event", "fix_valves", new Dictionary<string, string>() { ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
        }
        public static void scanNewEnemy()
        {
            if (Object.FindObjectOfType<Terminal>() != null)
            {
                for (int i = 0; i < Object.FindObjectOfType<Terminal>().enemyFiles.Count; i++)
                {
                    if (!Object.FindObjectOfType<Terminal>().scannedEnemyIDs.Contains(i))
                    {
                        HUDManager.Instance.ScanNewCreatureServerRpc(i);
                        TweaksBase.mls.LogInfo("Server scanned next new creature - Type: " + Object.FindObjectOfType<Terminal>().enemyFiles[i].creatureName);
                        TweaksBase.SendGAEvent("event", "enemy_scan", new Dictionary<string, string>() { ["type"] = Object.FindObjectOfType<Terminal>().enemyFiles[i].creatureName });
                        return;
                    }
                }
            }
        }
        public static void summonJeb()
        {
            if (Object.FindObjectOfType<DepositItemsDesk>() != null)
            {
                Object.FindObjectOfType<DepositItemsDesk>().AttackPlayersServerRpc();
                TweaksBase.mls.LogInfo("Sending server Jeb attack. Glhf.");
                TweaksBase.SendGAEvent("event", "jeb_attack", new Dictionary<string, string>() { ["elapsed"] = StartOfRound.Instance.timeSinceRoundStarted.ToString() });
            }
        }
        public static void brickTerminal()
        {
            if (Object.FindObjectOfType<Terminal>() != null)
            {
                Object.FindObjectOfType<Terminal>().SetTerminalInUseServerRpc(true);
                TweaksBase.mls.LogInfo("Sending server terminal in use. No one will be able to use the terminal.");
                TweaksBase.SendGAEvent("event", "brick_terminal", new Dictionary<string, string>() { ["bricked"] = "true" });
            }
        }
        public static void fixTerminal()
        {
            if (Object.FindObjectOfType<Terminal>() != null)
            {
                Object.FindObjectOfType<Terminal>().SetTerminalInUseServerRpc(false);
                TweaksBase.mls.LogInfo("Sending server terminal out of use. This should fix the terminal.");
                TweaksBase.SendGAEvent("event", "brick_terminal", new Dictionary<string, string>() { ["bricked"] = "false" });
            }
        }
        public static void activateSchoolSupplies()
        {
            ShotgunItem[] array = Object.FindObjectsOfType<ShotgunItem>();
            int presize = array.Length;
            foreach (ShotgunItem gun in array)
            {
                gun.ShootGunAndSync(false);
                TweaksBase.mls.LogInfo("Fired shotgun on server: " + gun.gameObject.name);
            }
            TweaksBase.SendGAEvent("event", "fire_weapons", new Dictionary<string, string>() { ["count"] = presize.ToString() });
        }
    }
}
