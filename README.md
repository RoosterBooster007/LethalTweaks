# LethalTweaks
Allows you to tweak many aspects of the game. Most work client side! Full bright, custom sprint/climb, inf. stamina, weightless, anti-sink/turrets, water breathing, disable cooldowns, disable falling, unltd. power, one-handed, remote start, and more!

## Dependencies
| Required | Name | Version |
|:--------:|:-------------|:------|
| ✅ | [LethalConfig](https://thunderstore.io/c/lethal-company/p/AinaVT/LethalConfig/versions/#1.4.2:~:text=1.4.2) | 1.4.2 |
| ✅ | [LethalCompany InputUtils](https://thunderstore.io/c/lethal-company/p/Rune580/LethalCompany_InputUtils/versions/#:~:text=0.7.4) | 0.7.4 |
| ✅ | [BepInExPack](https://thunderstore.io/c/lethal-company/p/BepInEx/BepInExPack/versions/#:~:text=5.4.2100) | 5.4.2100 |

## Features & Tweaks
This mod will always support the latest version of Lethal Company (and its beta branch). Compatibility with older versions isn't guaranteed. If something isn't right, open an issue.

(🧑‍💻 = Client, 🖥️ = Server, ⏳ = Temporarily broken, 🧪 = Experimental, ⭐ = Spotlight)
| Perms | Section | Tweak | Notes |
|:---------:|:------:|:-------------|:----------------|
| 🧑‍💻 | VFX | **Night vision (full bright)** | Allows you to customize the brightness of the factory with a slider. |
|||||
| 🧑‍💻 | Movement | **Infinite stamina** | Disables vanilla stamina depletion. You can run infinitely. |
| 🧑‍💻 | Movement | **Weightless** | Your movement won't be affected by what you're carrying (now 0 lbs). |
| 🧑‍💻 | Movement | **Custom speed** | Allows you to customize your walk/run speed with a slider. |
| 🧑‍💻 | Movement | **Custom climb speed** | Allows you to customize your climb speed with a slider. |
| 🧑‍💻 | Movement | **Anti-sink (let that sink out)** | Disables sinkholes entirely. You can slowly walk across them. |
| ⏳ | Movement | **No fall (get over it)** | Prevents bridges from falling (locally). Not fully implemented yet. |
|||||
| 🧑‍💻 | Damage | **Disable fall damage** | Disables fall damage 💀. |
| 🧑‍💻 | Damage | **Anti-stun** | Disables stun grenade explosion effects. |
| ⏳ | Damage | **Disable mines** | Disables mine explosions. Not fully implemented yet. |
| 🧑‍💻 | Damage | **Disable turrets** | Deactivates all turrets (locally). You're bullet-proof... your friends? not so much. |
| ⏳ | Damage | **Disable locust bees** | Exactly what it sounds like. Not fully implemented yet. |
| 🧑‍💻 | Damage | **Water breathing (IP67)** | You can breathe underwater indefinitely. Go for a swim! |
|||||
| 🧑‍💻 | Items & Scrap | **Disable item cooldowns (warm up)** | All item use cooldowns will be disabled (may not work on all items). Try the airhorn lol. |
| 🧑‍💻 | Items & Scrap | **Unlimited battery** | Batteries last forever. |
| 🧑‍💻 | Items & Scrap | **One-handed** | Two-handed items can be carried normally (scroll normally to swap items). This may not work on all items. |
|||||
| 🧑‍💻 | SFX & Voice | **Safe words** | Monsters won't muffle your voice. |
|||||
| 🧑‍💻 | Identity | **Show name tags** | Always show name tags above all players. |
| ⏳ | Identity | **Prefix** | Lets you customize your name tag by adding a cool prefix. Not fully implemented yet. |
|||||
| 🧑‍💻 | Tweaks | **Suicide (lethal launch)** | Blast off and leave your friends behind. You can even customize the initial velocity! |
| 🧑‍💻🧪⭐ | Tweaks | **Pull ship lever** | This will remotely initiate a game start (works regardless of host status)! Start the game without the host requirement! |
| 🧑‍💻 | Tweaks | **Target** | Set a player target to modify or impersonate (for the below actions). |
| 🧑‍💻🧪 | Tweaks | **Pull ship lever as target** | Remotely initiates a ship lever pull on behalf of the selected target. Their name will also be mentioned in the chat. |
| 🧑‍💻🧪 | Tweaks | **Kill target** | Remotely kill your target via bludgeoning. May not work if certain player protection mods are present. |
| 🧑‍💻🧪 | Tweaks | **Nuke entire lobby** | This will remotely kill EVERYONE (ending the level). You're clearly a sadist. |
| 🧑‍💻🧪 | Tweaks | **Toggle ship horn** | Exactly what it sounds like. Toggle the horn without cooldown or restraint... even remotely! |
| 🧑‍💻🧪⭐ | Tweaks | **Force Jeb attack** | The tenticles come out in 4k. Don't stand near the desk. |
| 🧑‍💻🧪 | Tweaks | **Firing squad** | Remotely fires all shotguns within the scene (for everyone). |
| 🧑‍💻🧪 | Tweaks | **Despawn all enemies** | All current enemies will be despawned (on the server... for everyone). |
| ⏳🖥️🧪 | Tweaks | **Despawn all items** | Very broken atm. Bugs scrap on your client and may lag others'. |
| 🧑‍💻🧪 | Tweaks | **Route ship to destination** | This will remotely initiate a ship level change on your behalf. |
| 🧑‍💻🧪 | Tweaks | **Brick ship terminal** | This will remotely deactivate the ship terminal for a while. Only works if no one is currently at the terminal. |
| 🧑‍💻🧪 | Tweaks | **Fix ship terminal** | This will remotely fix the ship terminal. Only works if no one is currently at the terminal. This reverses terminal hacks. |
| 🧑‍💻 | Tweaks | **Open ship doors** | This will remotely open the ship doors (just for the client). |
| 🧑‍💻 | Tweaks | **Close ship doors** | This will remotely close the ship doors (just for the client). |
| ⏳🧪 | Tweaks | **Unlock factory door on interact** | You will unlock any door you interact with (regardless of whether you have a key). |
| 🧑‍💻🧪 | Tweaks | **Unlock all factory doors/gates** | This will remotely unlock all factory doors and gates (on the server... for everyone). Some gates without power may remain closed. |
| 🧑‍💻🧪 | Tweaks | **Fix all factory steam valves** | This will remotely fix all factory leaky steam valves (on the server... for everyone) |
| 🧑‍💻🧪 | Tweaks | **Teleport to ship** | Warps you back to the ship. |
| 🧑‍💻🧪 | Tweaks | **Teleport to entrance** | Warps you to the main entrance. |
| 🧑‍💻🧪 | Tweaks | **Send custom anon. message** | Send a chat message to everyone anonymously. Be respectful. |
| 🧑‍💻🧪⭐ | Tweaks | **Scan new enemy message** | "Scans" a new enemy and sends a message to all players (creeps them out fr). |

## Default Keybinds
The below actions can be rebound to other keys or HIDs through InputUtils (they aren't keyboard-exclusive).

| Key | Action | Type |
|:--------:|:-------------|:------|
| F | **Night vision (full bright)** | Toggle |
| R | **Infinite stamina** | Toggle |
| H | **Weightless** | Toggle |
| Y | **Custom speed** | Toggle |
| U | **Custom climb speed** | Toggle |
| I | **Anti-sink** | Toggle |
| L | **Disable fall damage** | Toggle |
| J | **Water breathing** | Toggle |
| C | **Disable item cooldowns** | Toggle |
| P | **Unlimited battery** | Toggle |
| O | **One-handed** | Toggle |
| K | **Suicide** | Toggle |
| T | **Name tags** | Toggle |

## Issues & Contributions
Please follow the available issue templates when submitting requests. Use the discussions tab when wanting to chat. Pull requests and other user contributions are encouraged and always welcome.

## Licencing (GPLv3)
Read the **LICENSE.md** file for more information. The **Contributor Agreement** can be found there too. By downloading or contributing to this repo, you agree to the terms stated in the **LICENSE.md**.

```Copyright (C) 2024 RoosterBooster007```

Please include the above copyright notice when distributing or modifying any code.
