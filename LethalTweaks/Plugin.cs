using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;
using LethalTools;
using LethalTools.Patches;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LethalTweaks
{
    [BepInPlugin(modGUID, modName, ModVersion)]
    [BepInDependency("ainavt.lc.lethalconfig")]
    [BepInDependency("com.rune580.LethalCompanyInputUtils")]
    public class TweaksBase : BaseUnityPlugin
    {
        public const string modGUID = "net.RB007.LethalTweaks";
        public const string modName = "LethalTweaks";
        public const string ModVersion = "1.1.0";

        private static DateTimeOffset sessionStartTime = DateTimeOffset.UtcNow;
        private static DateTimeOffset lastEng = DateTimeOffset.UtcNow;
        private static int GAHitCount = 1;
        private static int GASessionCount = 1;
        private static string GASessionID = Regex.Replace(System.Guid.NewGuid().ToString(), "[^\\d]", "").Substring(0, 10);

        private readonly Harmony harmony = new Harmony(modGUID);
        public static TweaksBase Instance;

        internal static ManualLogSource mls;

        internal static InputActions IAInstance;
        internal static InputEvents IEInstance;

        public static StartOfRound plyrManager;

        public static bool isChatting = false;
        public static bool isInTerminal = false;
        public static bool isMenuOpen = false;
        public static bool isPlayerControlled = false;

        public static ConfigEntry<bool> enableKeybinds;
        public static ConfigEntry<bool> enableTele;
        public static ConfigEntry<bool> enableCustomBrightness;
        public static ConfigEntry<float> customBrightness;
        public static ConfigEntry<bool> enableInfiniteStamina;
        public static ConfigEntry<bool> enableWeightless;
        public static ConfigEntry<bool> enableSpeed;
        public static ConfigEntry<bool> enableClimbSpeed;
        public static ConfigEntry<float> speedMultiplier;
        public static ConfigEntry<float> climbSpeed;
        public static ConfigEntry<bool> unSink;
        public static ConfigEntry<bool> disableBridgeFall;
        public static ConfigEntry<bool> disableFallDamage;
        public static ConfigEntry<bool> unStun;
        public static ConfigEntry<bool> disableMines;
        public static ConfigEntry<bool> disableTurrets;
        public static ConfigEntry<bool> disableBees;
        public static ConfigEntry<bool> underwaterRoaming;
        public static ConfigEntry<bool> lethalLaunch;
        public static ConfigEntry<float> llVelocity;
        public static ConfigEntry<string> playerSelect;
        public static ConfigEntry<string> moonSelect;
        public static ConfigEntry<bool> universalKeyEnabled; // missing implimentation
        public static ConfigEntry<string> chatMessage;
        public static ConfigEntry<bool> warmUp;
        public static ConfigEntry<bool> fullCharge;
        public static ConfigEntry<bool> oneHanded;
        public static ConfigEntry<bool> unMuffle;
        public static ConfigEntry<bool> nameTags;

        public static bool isHornOn = false;
        public static bool sendHornDown = false;
        public static bool sendHornUp = false;

        public static bool shouldRevertSpeed = false;
        public static bool shouldRevertClimbSpeed = false;

        public static HttpClient HTTPclient = new HttpClient();

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("LethalTweaks is enabling...");

            mls.LogInfo("Reading config...");
            enableKeybinds = Config.Bind("General", "Enable keybinds", true, "When enabled, individual tweaks can be toggled via user input.");
            var cb_eK = new BoolCheckBoxConfigItem(enableKeybinds, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_eK);
            enableTele = Config.Bind("General", "Enable telemetry", true, "When enabled, some anonymous user data/input actions may be uploaded. This helps me understand what features are used the most and what I can improve. :) \n\nWhat's collected?\n- OS, LC, and LethalTweaks version data\n- LTs actions/events (some game data)\n- Your language and region (ex. en-US, Chicago)\n\nCheck the README for more info.");
            var cb_eT = new BoolCheckBoxConfigItem(enableTele, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_eT);

            enableCustomBrightness = Config.Bind("VFX", "Night vision", false, "When enabled, you can further customize the brightness of the factory (default keybind: F).");
            var cb_eCB = new BoolCheckBoxConfigItem(enableCustomBrightness, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_eCB);
            customBrightness = Config.Bind("VFX", "Light intensity", 45f, "Set the brightness level of the factory.");
            var sldr_cB = new FloatSliderConfigItem(customBrightness, new FloatSliderOptions
            {
                RequiresRestart = false,
                Min = 0f,
                Max = 50f
            });
            LethalConfigManager.AddConfigItem(sldr_cB);

            enableInfiniteStamina = Config.Bind("Movement", "Infinite stamina", true, "When enabled, your stamina bar won't deplete (default keybind: R).");
            var cb_eIS = new BoolCheckBoxConfigItem(enableInfiniteStamina, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_eIS);
            enableWeightless = Config.Bind("Movement", "Weightless", true, "When enabled, your movement won't be affected by what you're carrying (default keybind: H).");
            var cb_eW = new BoolCheckBoxConfigItem(enableWeightless, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_eW);
            enableSpeed = Config.Bind("Movement", "Custom sprint", false, "When enabled, you can run at a faster/slower speed (default keybind: Y).");
            var cb_eS = new BoolCheckBoxConfigItem(enableSpeed, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_eS);
            speedMultiplier = Config.Bind("Movement", "Sprint multiplier", 1f, "How fast you can move in various player positions.");
            var sldr_sM = new FloatSliderConfigItem(speedMultiplier, new FloatSliderOptions
            {
                RequiresRestart = false,
                Min = 0.25f,
                Max = 25f
            });
            LethalConfigManager.AddConfigItem(sldr_sM);
            enableClimbSpeed = Config.Bind("Movement", "Custom climb speed", false, "When enabled, you can climb at a faster/slower speed (default keybind: U).");
            var cb_eCS = new BoolCheckBoxConfigItem(enableClimbSpeed, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_eCS);
            climbSpeed = Config.Bind("Movement", "Climb speed", 4f, "How fast you can climb ladders and other objects.");
            var sldr_cS = new FloatSliderConfigItem(climbSpeed, new FloatSliderOptions
            {
                RequiresRestart = false,
                Min = 0.25f,
                Max = 25f
            });
            LethalConfigManager.AddConfigItem(sldr_cS);
            unSink = Config.Bind("Movement", "Let that sink out", false, "When enabled, you won't sink on rainy days (default keybind: I).");
            var cb_uS = new BoolCheckBoxConfigItem(unSink, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_uS);
            disableBridgeFall = Config.Bind("Movement", "Get over it", false, "When enabled, bridges won't fall (other players may, though).");
            var cb_dBF = new BoolCheckBoxConfigItem(disableBridgeFall, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_dBF);

            disableFallDamage = Config.Bind("Damage", "Disable fall damage", false, "When enabled, you won't take fall damage (default keybind: L).");
            var cb_fD = new BoolCheckBoxConfigItem(disableFallDamage, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_fD);
            unStun = Config.Bind("Damage", "Anti-stun", false, "When enabled, stun grenades won't explode (for you, at least).");
            var cb_uStn = new BoolCheckBoxConfigItem(unStun, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_uStn);
            disableMines = Config.Bind("Damage", "Disable mines", false, "When enabled, you won't activate mines (unless you DC or teleport off of one).");
            var cb_dM = new BoolCheckBoxConfigItem(disableMines, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_dM);
            disableTurrets = Config.Bind("Damage", "Disable turrets", false, "When enabled, you won't activate nearby turrets.");
            var cb_dT = new BoolCheckBoxConfigItem(disableTurrets, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_dT);
            disableBees = Config.Bind("Damage", "Disable locust bees", true, "When enabled, bee clusters won't aggro. you at all. Feel free to grab their hives.");
            var cb_dB = new BoolCheckBoxConfigItem(disableBees, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_dB);
            underwaterRoaming = Config.Bind("Damage", "IP67 (water breathing)", false, "You can breathe under water... even longer than 30 mins (default keybind: J)!");
            var cb_uR = new BoolCheckBoxConfigItem(underwaterRoaming, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_uR);

            warmUp = Config.Bind("Items & Scrap", "Warm up", true, "When enabled, item use cooldowns will be disabled (may not work on all items) (default keybind: C).");
            var cb_wU = new BoolCheckBoxConfigItem(warmUp, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_wU);
            fullCharge = Config.Bind("Items & Scrap", "Unlimited power", true, "When enabled, batteries last forever (default keybind: P)!");
            var cb_fC = new BoolCheckBoxConfigItem(fullCharge, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_fC);
            oneHanded = Config.Bind("Items & Scrap", "One-handed", false, "When enabled, two-handed items can be carried normally (default keybind: O).");
            var cb_oH = new BoolCheckBoxConfigItem(oneHanded, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_oH);

            unMuffle = Config.Bind("SFX & Voice", "Safe words", false, "When enabled, monsters won't muffle your voice.");
            var cb_uM = new BoolCheckBoxConfigItem(unMuffle, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_uM);

            nameTags = Config.Bind("Identity", "Always show name tags", false, "When enabled, players' tags will always be shown above their heads (default keybind: T).");
            var cb_nT = new BoolCheckBoxConfigItem(nameTags, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_nT);

            lethalLaunch = Config.Bind("Tweaks", "Lethal launch", true, "When enabled, you can launch and kill yourself by pressing the suicide keybind (default: K) at any time.");
            var cb_lL = new BoolCheckBoxConfigItem(lethalLaunch, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_lL);
            llVelocity = Config.Bind("Tweaks", "Launch velocity", 50f, "How fast/far you fly!");
            var sldr_lV = new FloatSliderConfigItem(llVelocity, new FloatSliderOptions
            {
                RequiresRestart = false,
                Min = 0f,
                Max = 100f
            });
            LethalConfigManager.AddConfigItem(sldr_lV);
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Pull start lever", "This will remotely initiate a game start (works regardless of host status) (default keybind: Z).", "Pull", () =>
            {
                InputEvents.startGame();
            }));
            playerSelect = Config.Bind("Tweaks", "Target", "Zeekerss", "Enter the target's username (must be exact).");
            var ti_pS = new TextInputFieldConfigItem(playerSelect, new TextInputFieldOptions
            {
                RequiresRestart = false,
                CharacterLimit = 50,
                NumberOfLines = 1,
            });
            LethalConfigManager.AddConfigItem(ti_pS);
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Pull ship lever as target", "This will remotely initiate a ship lever pull on behalf of the selected target. Their name will also be mentioned in the chat.", "Pull", () =>
            {
                InputEvents.pullLeverAs();
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Kill target", "This will remotely kill your target via bludgeoning.", "Kill", () =>
            {
                InputEvents.killTarget();
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Nuke entire lobby", "This will remotely kill EVERYONE (ending the level).", "Nuke", () =>
            {
                InputEvents.nukeLobby();
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Toggle ship horn", "Exactly what it sounds like. Toggle the horn without cooldown or restraint... even remotely!", "Toggle", () =>
            {
                if (isHornOn)
                {
                    isHornOn = false;
                    sendHornUp = true;
                    TweaksBase.mls.LogInfo("Sending ship horn end...");
                }
                else
                {
                    isHornOn = true;
                    sendHornDown = true;
                    TweaksBase.mls.LogInfo("Sending ship horn sound...");
                }
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Force Jeb attack", "The tenticles come out in 4k. Don't stand near the desk.", "Summon", () =>
            {
                InputEvents.summonJeb();
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Firing squad", "Remotely fires all shotguns within the scene (on the server... for everyone).", "Fire!", () =>
            {
                InputEvents.activateSchoolSupplies();
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Despawn all enemies", "All current enemies will be despawned (on the server... for everyone).", "Remove", () =>
            {
                InputEvents.killEnemies();
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Despawn all items", "All current items will be despawned (on the server... for everyone).", "Remove", () =>
            {
                InputEvents.killItems();
            }));
            moonSelect = Config.Bind("Tweaks", "Set destination", "Rend", "Enter the name of a moon (must be exact).");
            var ti_mS = new TextInputFieldConfigItem(moonSelect, new TextInputFieldOptions
            {
                RequiresRestart = false,
                CharacterLimit = 50,
                NumberOfLines = 1,
            });
            LethalConfigManager.AddConfigItem(ti_mS);
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Route ship to destination", "This will remotely initiate a ship level change on your behalf.", "Send", () =>
            {
                InputEvents.reRouteShip();
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Brick ship terminal", "This will remotely deactivate the ship terminal for a while. Only works if no one is currently at the terminal.", "Hack", () =>
            {
                InputEvents.brickTerminal();
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Fix ship terminal", "This will remotely fix the ship terminal. Only works if no one is currently at the terminal. This reverses terminal hacks.", "Fix", () =>
            {
                InputEvents.fixTerminal();
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Open ship doors", "This will remotely open the ship doors.", "Open", () =>
            {
                InputEvents.openShip();
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Close ship doors", "This will remotely close the ship doors.", "Close", () =>
            {
                InputEvents.closeShip();
            }));
            universalKeyEnabled = Config.Bind("Tweaks", "Unlock factory door on interact", true, "When enabled, you will unlock any door you interact with (regardless of whether you have a key). Not fully implemented yet.");
            var cb_uKE = new BoolCheckBoxConfigItem(universalKeyEnabled, requiresRestart: false);
            LethalConfigManager.AddConfigItem(cb_uKE);
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Unlock all factory doors/gates", "This will remotely unlock all factory doors and gates (on the server... for everyone). Some gates without power may remain closed for other players.", "Unlock", () =>
            {
                InputEvents.unlockAllDoors();
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Fix all factory steam valves", "This will remotely fix all factory leaky steam valves (on the server... for everyone).", "Unlock", () =>
            {
                InputEvents.fixAllValves();
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Teleport to ship", "This will warp you back to the ship (default keybind: B).", "Warp", () =>
            {
                InputEvents.warpToShip();
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Teleport to entrance", "This will warp you to the main entrance (default keybind: V).", "Warp", () =>
            {
                InputEvents.warpToEntrance();
            }));
            chatMessage = Config.Bind("Tweaks", "Custom message", "the bees are kinda hot", "Enter a message to send without your Username/ID.");
            var ti_cM = new TextInputFieldConfigItem(chatMessage, new TextInputFieldOptions
            {
                RequiresRestart = false,
                CharacterLimit = 50,
                NumberOfLines = 1,
            });
            LethalConfigManager.AddConfigItem(ti_cM);
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Send anon. message", "Send a chat message to everyone anonymously. Be respectful.", "Send", () =>
            {
                InputEvents.sendTextMessage();
            }));
            LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Tweaks", "Scan new enemy message", "\"Scans\" a new enemy and sends a message to all players (creeps them out fr).", "Send", () =>
            {
                InputEvents.scanNewEnemy();
            }));

            mls.LogInfo("Config loaded!");

            mls.LogInfo("Loading input classes...");
            IAInstance = new InputActions();
            IEInstance = new InputEvents();
            IEInstance.Init();
            mls.LogInfo("Done!");

            mls.LogInfo("Patching game...");
            mls.LogInfo("Searching for patches...");
            Type[] classtypes = {typeof(ControllerPatch), typeof(SoRPatches), typeof(LobbyPatch), typeof(LeavePatch), typeof(GrenadesPatch), typeof(TurretsPatch), typeof(MinesPatch), typeof(BeesPatch), typeof(ItemPatch), typeof(HornPatch), typeof(BridgePatch), typeof(AppaPatch), typeof(QMenuPatch), typeof(MenuPatch), typeof(LockPatch)}; // list all class types with harmony patches here
            foreach (Type t in classtypes)
            {
                try
                {
                    harmony.CreateClassProcessor(t).Patch(); // using Reflection because not all patches work all the time
                    mls.LogInfo("Patched " + t.Name + " successfully!");
                }
                catch (Exception ex)
                {
                    mls.LogInfo("Harmony patch failed for " + t.Name + "! This may cause issues... Exception: " + ex.Message);
                }
            }
            mls.LogInfo("Patched all!");

            if (isFirstRun())
            {
                SendUserGA("page_view", "MainMenu", "&_fv=1");
                mls.LogInfo("Finishing initial setup...");
            } else
            {
                SendUserGA("page_view", "MainMenu", "");
                mls.LogInfo("Welcome back!");
            }
        }

        private static void startNewGASession()
        {
            GASessionCount++;
            GASessionID = Regex.Replace(System.Guid.NewGuid().ToString(), "[^\\d]", "").Substring(0, 10);
            sessionStartTime = DateTimeOffset.UtcNow;
            lastEng = DateTime.UtcNow;
            GAHitCount = 1;
            TweaksBase.SendUserGA("page_view", "InGame", "");
        }

        private static bool isFirstRun()
        {
            if (!PlayerPrefs.HasKey("LTs.newUsert"))
            {
                //PlayerPrefs.SetInt("LTs.newUser", 1);
                //PlayerPrefs.Save();
                return true;
            }
            return false;
        }

        internal static bool recentlyUpdated()
        {
            if (PlayerPrefs.GetString("LTs.v") != "0.0.0")
            {
                //PlayerPrefs.SetString("LTs.v", ModVersion);
                //PlayerPrefs.Save();
                return true;
            }
            return false;
        }

        private static string getSystemArch()
        {
            if (Environment.Is64BitProcess)
            {
                return "x86_64";
            } else
            {
                return "x86";
            }
        }

        private static string getSystemBits()
        {
            if (Environment.Is64BitProcess)
            {
                return "64";
            }
            else
            {
                return "32";
            }
        }

        internal static void SendGAEvent(string category, string type, Dictionary<string, string> args)
        {
            if ((bool)enableTele.BoxedValue)
            {
                if ((DateTime.UtcNow - lastEng).TotalMinutes >= 30)
                {
                    startNewGASession();
                }

                type = WebUtility.UrlEncode(type);
                category = WebUtility.UrlEncode(category);

                string e_params = "";
                foreach (KeyValuePair<string, string> kvp in args)
                {
                    if (int.TryParse(kvp.Value, out int value))
                    {
                        e_params += "&epn." + WebUtility.UrlEncode(kvp.Key) + "=" + value;
                    } else
                    {
                        e_params += "&ep." + WebUtility.UrlEncode(kvp.Key) + "=" + WebUtility.UrlEncode(kvp.Value);
                    }
                }

                GAHitCount++;
                _ = HTTPclient.GetAsync("https://www.google-analytics.com/g/collect?v=2&tid=G-73HYZC435K&cid=" + SystemInfo.deviceUniqueIdentifier + "&sid=" + GASessionID + "&ul=" + CultureInfo.CurrentCulture.Name + "&sr=" + Screen.currentResolution.width + "x" + Screen.currentResolution.height + "&uap=" + WebUtility.UrlEncode(Environment.OSVersion.ToString()) + "&uam=" + WebUtility.UrlEncode(Application.version) + "&uapv=" + WebUtility.UrlEncode(TweaksBase.ModVersion) + "&en=" + type + "&ep.category=" + category + e_params + "&_et=" + (long)Math.Floor((DateTime.UtcNow - lastEng).TotalMilliseconds) + "&tfd=" + (long)Math.Floor((DateTime.UtcNow - sessionStartTime).TotalMilliseconds) + "&uamb=0&uaw=0&pscdl=noapi&_s=" + GAHitCount + "&sct=" + GASessionCount + "&seg=1&_ee=1&npa=0&dma=0&frm=0&are=1");
                lastEng = DateTime.UtcNow;
            }
        }
        internal static void SendUserGA(string e, string screenTitle, string args)
        {
            if ((bool)enableTele.BoxedValue)
            {
                _ = HTTPclient.GetAsync("https://www.google-analytics.com/g/collect?v=2&tid=G-73HYZC435K&cid=" + SystemInfo.deviceUniqueIdentifier + "&sid=" + GASessionID + "&ul=" + CultureInfo.CurrentCulture.Name + "&sr=" + Screen.currentResolution.width + "x" + Screen.currentResolution.height + "&uap=" + WebUtility.UrlEncode(Environment.OSVersion.ToString()) + "&uam=" + WebUtility.UrlEncode(Application.version) + "&uapv=" + WebUtility.UrlEncode(TweaksBase.ModVersion) + "&en=" + e + "&ep.cpu=" + SystemInfo.processorType + "&ep.gpu=" + SystemInfo.graphicsDeviceName + "&tfd=" + (long)Math.Floor((DateTime.UtcNow - sessionStartTime).TotalMilliseconds) + "&uaa=" + getSystemArch() + "&uab=" + getSystemBits() + "&uamb=0&uaw=0&dt=" + WebUtility.UrlEncode(screenTitle + " (v" + TweaksBase.ModVersion + ")") + "&are=1&frm=0&pscdl=noapi&seg=1&npa=0&_s=1&sct=" + GASessionCount + "&dma=0&_ss=1&_nsi=1&_ee=1" + args);
            }
        }
    }
}
