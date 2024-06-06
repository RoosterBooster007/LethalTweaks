using GameNetcodeStuff;
using LethalTweaks;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

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
        }
        public static bool isValidInput()
        {
            TweaksBase.mls.LogInfo("Checking validity of input... - " + !TweaksBase.isChatting + " | " + !TweaksBase.isInTerminal + " | " + !TweaksBase.isMenuOpen + " | " + (bool)TweaksBase.enableKeybinds.BoxedValue);
            if (!TweaksBase.isChatting && !TweaksBase.isInTerminal && !TweaksBase.isMenuOpen && (bool)TweaksBase.enableKeybinds.BoxedValue)
            {
                return true;
            }
            return false;
        }
        public void OnNightVisionInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.enableCustomBrightness.BoxedValue = !(bool)TweaksBase.enableCustomBrightness.BoxedValue;
        }
        public void OnNameTagInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.nameTags.BoxedValue = !(bool)TweaksBase.nameTags.BoxedValue;
        }
        public void OnStaminaInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.enableInfiniteStamina.BoxedValue = !(bool)TweaksBase.enableInfiniteStamina.BoxedValue;
        }
        public void OnWeightlessInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.enableWeightless.BoxedValue = !(bool)TweaksBase.enableWeightless.BoxedValue;
        }
        public void OnSpeedInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.enableSpeed.BoxedValue = !(bool)TweaksBase.enableSpeed.BoxedValue;
        }
        public void OnClimbSpeedInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.enableClimbSpeed.BoxedValue = !(bool)TweaksBase.enableClimbSpeed.BoxedValue;
        }
        public void OnSinkInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.unSink.BoxedValue = !(bool)TweaksBase.unSink.BoxedValue;
        }
        public void OnFallDamageInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.disableFallDamage.BoxedValue = !(bool)TweaksBase.disableFallDamage.BoxedValue;
        }
        public void OnWaterBInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.underwaterRoaming.BoxedValue = !(bool)TweaksBase.underwaterRoaming.BoxedValue;
        }
        public void OnCooldownInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.warmUp.BoxedValue = !(bool)TweaksBase.warmUp.BoxedValue;
        }
        public void OnBatteryInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.fullCharge.BoxedValue = !(bool)TweaksBase.fullCharge.BoxedValue;
        }
        public void OnOneHInput(InputAction.CallbackContext c)
        {
            if (!isValidInput()) return;
            TweaksBase.oneHanded.BoxedValue = !(bool)TweaksBase.oneHanded.BoxedValue;
        }
        public static void startGame()
        {
            Object.FindObjectOfType<StartMatchLever>().PlayLeverPullEffectsServerRpc(true);
            StartOfRound.Instance.StartGameServerRpc();
            TweaksBase.mls.LogInfo("Lever pulled! Starting game...");
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
            }
        }
        public static string toUTF8String(string s)
        {
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(s));
        }
        public static void reRouteShip()
        {
            if (StartOfRound.Instance.levels != null)
            {
                for (int i = 0; i < StartOfRound.Instance.levels.Length; i++)
                {
                    SelectableLevel level = StartOfRound.Instance.levels[i];
                    TweaksBase.mls.LogInfo("Value: " + (string)TweaksBase.moonSelect.BoxedValue + " | " + level.PlanetName + " - " + level.sceneName + " - " + level.levelID);
                    if (level.PlanetName.Equals(toUTF8String((string)TweaksBase.moonSelect.BoxedValue)))
                    {
                        StartOfRound.Instance.ChangeLevelServerRpc(i, Object.FindObjectOfType<Terminal>().groupCredits);
                        TweaksBase.mls.LogInfo("Setting final moon destination to: " + (string)TweaksBase.moonSelect.BoxedValue);
                    }
                }
            }
        }
        public static void openShip()
        {
            StartOfRound.Instance.shipDoorsAnimator.SetBool("Closed", false);
            StartOfRound.Instance.hangarDoorsClosed = false;
            TweaksBase.mls.LogInfo("Opening ship doors...");
        }
        public static void closeShip()
        {
            StartOfRound.Instance.shipDoorsAnimator.SetBool("Closed", true);
            StartOfRound.Instance.hangarDoorsClosed = true;
            TweaksBase.mls.LogInfo("Closing ship doors...");
        }
        public static void warpToShip()
        {
            GameNetworkManager.Instance.localPlayerController.TeleportPlayer(StartOfRound.Instance.playerSpawnPositions[0].position, false, 0f, false, true);
            TweaksBase.mls.LogInfo("Warping player to ship...");
        }
        public static void warpToEntrance()
        {
            if (RoundManager.FindMainEntrancePosition(true, true) != Vector3.zero)
            {
                GameNetworkManager.Instance.localPlayerController.TeleportPlayer(RoundManager.FindMainEntrancePosition(true, true), false, 0f, false, true);
                TweaksBase.mls.LogInfo("Warping player to entrance...");
            }
        }
        public static void sendTextMessage()
        {
            HUDManager.Instance.AddTextToChatOnServer((string)TweaksBase.chatMessage.BoxedValue);
            TweaksBase.mls.LogInfo("Sent anon. message: " + (string)TweaksBase.chatMessage.BoxedValue);
        }
        public static void killEnemies()
        {
            EnemyAI[] array = Object.FindObjectsOfType<EnemyAI>();
            foreach (EnemyAI ai in array)
            {
                RoundManager.Instance.DespawnEnemyOnServer(ai.NetworkObject);
                TweaksBase.mls.LogInfo("Despawned EnemyAI on server - Type: " + ai.gameObject.name);
            }
        }
        public static void killItems()
        {
            GrabbableObject[] array = Object.FindObjectsOfType<GrabbableObject>();
            foreach (GrabbableObject item in array)
            {
                item.GetComponent<MeshRenderer>().gameObject.SetActive(false);
                //item.NetworkObject.Despawn();
                item.NetworkObject.transform.SetParent(null, false);
                Object.Destroy(item);
                TweaksBase.mls.LogInfo("Despawned GrabbableObject on server - Type: " + item.gameObject.name);
            }
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
        }
        public static void fixAllValves()
        {
            SteamValveHazard[] array = Object.FindObjectsOfType<SteamValveHazard>();
            foreach (SteamValveHazard valve in array)
            {
                valve.FixValveServerRpc();
                TweaksBase.mls.LogInfo("Factory steam valve repaired - Type: " + valve.gameObject.name);
            }
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
            }
        }
        public static void brickTerminal()
        {
            if (Object.FindObjectOfType<Terminal>() != null)
            {
                Object.FindObjectOfType<Terminal>().SetTerminalInUseServerRpc(true);
                TweaksBase.mls.LogInfo("Sending server terminal in use. No one will be able to use the terminal.");
            }
        }
        public static void fixTerminal()
        {
            if (Object.FindObjectOfType<Terminal>() != null)
            {
                Object.FindObjectOfType<Terminal>().SetTerminalInUseServerRpc(false);
                TweaksBase.mls.LogInfo("Sending server terminal out of use. This should fix the terminal.");
            }
        }
        public static void activateSchoolSupplies()
        {
            ShotgunItem[] array = Object.FindObjectsOfType<ShotgunItem>();
            foreach (ShotgunItem gun in array)
            {
                gun.ShootGunAndSync(false);
                TweaksBase.mls.LogInfo("Fired shotgun on server: " + gun.gameObject.name);
            }
        }
    }
}
