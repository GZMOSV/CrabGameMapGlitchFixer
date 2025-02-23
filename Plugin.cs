using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using PlayerMovement = MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique;
using GameManager = MonoBehaviourPublicDi2UIObacspDi2UIObUnique;
using LobbyManager = MonoBehaviourPublicCSDi2UIInstObUIloDiUnique;
using ServerSend = MonoBehaviourPublicInInUnique;
using GameServer = MonoBehaviourPublicObInCoIE85SiAwVoFoCoUnique;
using SteamManager = MonoBehaviourPublicObInUIgaStCSBoStcuCSUnique;

namespace GlitchBoundaryFixer
{
    [BepInPlugin("com.GZMOS.glitchboundaryfixer", "Glitch & Boundary Fixer", "1.0.0")]
    public class GlitchBoundaryFixerPlugin : BasePlugin
    {
        public static int prevMapID = 0;

        public override void Load()
        {
            // Initialize Harmony to patch game methods
            Harmony.CreateAndPatchAll(typeof(GlitchBoundaryFixerPlugin));
            Log.LogInfo("Glitch & Boundary Fixer loaded!");
        }

        // Returns the Rigidbody of the specified player
        public static Rigidbody GetPlayerRigidBody(ulong id)
        {
            // If the player is the local player, return their Rigidbody
            if (id == GetMyID())
                return PlayerMovement.prop_MonoBehaviourPublicGaplfoGaTrorplTrRiBoUnique_0.GetRb();
            // Otherwise, return the Rigidbody of the specified player
            else
                return GameManager.Instance.activePlayers[id].prop_MonoBehaviourPublicObVeSiVeRiSiAnVeanTrUnique_0.field_Private_Rigidbody_0;
        }

        // Returns the local player's Steam ID
        public static ulong GetMyID()
        {
            return SteamManager.Instance.field_Private_CSteamID_0.m_SteamID;
        }

        // Checks if the local player is the lobby host
        public static bool IsHost()
        {
            return SteamManager.Instance.IsLobbyOwner() && !LobbyManager.Instance.Method_Public_Boolean_0();
        }

        // Kills players who go beyond map boundaries
        public static void BoundsCheck(ulong id)
        {
            float maxDistance = 0f;

            // Map-specific distance thresholds
            switch (prevMapID)
            {
                case 5:  // Desert - check disabled due to issues
                case 17: // Plains - check disabled due to issues
                case 22: // Slippery Slope - check disabled due to issues
                    return; // Skip the out-of-bounds check entirely for these maps

                case 0: maxDistance = 100; break;  // Bitter Beach
                case 1: maxDistance = 110; break;  // Blueline
                case 2: maxDistance = 90; break;   // Cocky Containers
                case 3: maxDistance = 100; break;  // Color Climb
                case 4: maxDistance = 100; break;  // Crusty Rocks
                case 6: maxDistance = 100; break;  // Dorm
                case 7: maxDistance = 100; break;  // Funky Field
                case 8: maxDistance = 160; break;  // Glass Jump
                case 9: maxDistance = 100; break;  // Hasty Hill
                case 10: maxDistance = 80; break;  // Icy Crack
                case 11: maxDistance = 170; break; // Icy Islands
                case 12: maxDistance = 170; break; // Icy Rocks
                case 13: maxDistance = 170; break; // Islands
                case 14: maxDistance = 80; break;  // Karlson
                case 15: maxDistance = 90; break;  // Lanky Lava
                case 16: maxDistance = 90; break;  // Lava Lake
                case 18: maxDistance = 70; break;  // Playground
                case 19: maxDistance = 70; break;  // Playground 2
                case 20: maxDistance = 100; break; // Return to Monke
                case 21: maxDistance = 80; break;  // Sandstorm
                case 23: maxDistance = 70; break;  // (S) Color Climb
                case 24: maxDistance = 80; break;  // (S) Glass Jump
                case 25: maxDistance = 70; break;  // (S) Hill
                case 26: maxDistance = 125; break; // (S) Icy Islands
                case 27: maxDistance = 125; break; // (S) Islands
                case 28: maxDistance = 50; break;  // (S) Playground
                case 29: maxDistance = 150; break; // Snowtop
                case 30: maxDistance = 70; break;  // Splat
                case 31: maxDistance = 70; break;  // Splot
                case 32: maxDistance = 70; break;  // Sunny Saloon
                case 33: maxDistance = 150; break; // Toxic Train
                case 34: maxDistance = 80; break;  // Twisted Towers
                case 35: maxDistance = 80; break;  // Mini Monke
                case 36: maxDistance = 100; break; // (S) Beach
                case 37: maxDistance = 60; break;  // (S) Saloon
                case 38: maxDistance = 60; break;  // (S) Containers
                case 39: maxDistance = 60; break;  // Tiny Town 2
                case 40: maxDistance = 60; break;  // Tiny Town
                case 41: maxDistance = 80; break;  // Dodgy Fields
                case 42: maxDistance = 80; break;  // Dodgy Ice
                case 43: maxDistance = 80; break;  // Dodgy Snow
                case 44: maxDistance = 80; break;  // Dodgy Streets
                case 45: maxDistance = 170; break; // Sandy Islands
                case 46: maxDistance = 130; break; // (S) Sandy Islands
                case 47: maxDistance = 100; break; // Cheeky Chamber
                case 48: maxDistance = 100; break; // Lava Drop
                case 49: maxDistance = 100; break; // Lava Dump
                case 50: maxDistance = 100; break; // Peaceful Platform
                case 51: maxDistance = 100; break; // Salty Island
                case 52: maxDistance = 120; break; // Skybox
                case 53: maxDistance = 90; break;  // Saucy Stage
                case 54: maxDistance = 220; break; // Lava Climb
                case 55: maxDistance = 190; break; // Macaroni Mountain
                case 56: maxDistance = 210; break; // Sussy Sandcastle
                case 57: maxDistance = 700; break; // Sussy Slope
                case 58: maxDistance = 80; break;  // Sandy Stones
                case 59: maxDistance = 80; break;  // Crabfields
                case 60: maxDistance = 80; break;  // Crabheat
                case 61: maxDistance = 80; break;  // Crabland
            }

            // If the player is too far from the center, kill them
            if (Vector3.Distance(GetPlayerRigidBody(id).position, Vector3.zero) > maxDistance)
            {
                GameServer.PlayerDied(id, 1, Vector3.zero);
            }
        }

        // Teleports players from known glitch spots
        public static void GlitchingCheck(ulong id)
        {
            Vector3 playerPos = GetPlayerRigidBody(id).position;

            // Color Climb (Map ID 3)
            if (prevMapID == 3)
            {
                if (Vector3.Distance(playerPos, new Vector3(9.4f, -25.1f, -9.4f)) < 1f)
                {
                    ServerSend.RespawnPlayer(id, new Vector3(13.0f, -25.2f, -7.4f));
                }
                else if (Vector3.Distance(playerPos, new Vector3(-9.4f, -28.1f, 9.4f)) < 1f)
                {
                    ServerSend.RespawnPlayer(id, new Vector3(-10.6f, -22.1f, 10.6f));
                }
                else if (Vector3.Distance(playerPos, new Vector3(-9.4f, -28.1f, -9.4f)) < 1f)
                {
                    ServerSend.RespawnPlayer(id, new Vector3(-9f, -28.1f, -13f));
                }
            }
            // Snowtop (Map ID 29)
            else if (prevMapID == 29)
            {
                if (Vector3.Distance(playerPos, new Vector3(10.4f, 69.9f, -6.4f)) < 1f)
                {
                    ServerSend.RespawnPlayer(id, new Vector3(14.8f, 69.9f, -5.1f));
                }
                else if (Vector3.Distance(playerPos, new Vector3(-40.6f, 59.9f, 21.5f)) < 1f)
                {
                    ServerSend.RespawnPlayer(id, new Vector3(-44.8f, 59.9f, 21.4f));
                }
                else if (Vector3.Distance(playerPos, new Vector3(54.5f, 79.0f, 14.6f)) < 1f)
                {
                    ServerSend.RespawnPlayer(id, new Vector3(56.0f, 68.7f, 15.0f));
                }
            }
            // (S) Beach (Map ID 36)
            else if (prevMapID == 36)
            {
                if (Vector3.Distance(playerPos, new Vector3(20.8f, -1.1f, -15.8f)) < 1f)
                {
                    ServerSend.RespawnPlayer(id, new Vector3(19.2f, -1.1f, -17.3f));
                }
                else if (Vector3.Distance(playerPos, new Vector3(-10.6f, -4.1f, 14.4f)) < 1f)
                {
                    ServerSend.RespawnPlayer(id, new Vector3(-14.4f, -4.1f, 14.4f));
                }
            }
            // Bitter Beach (Map ID 0)
            else if (prevMapID == 0)
            {
                if (Vector3.Distance(playerPos, new Vector3(25.3f, -1.1f, -20.8f)) < 1f)
                {
                    ServerSend.RespawnPlayer(id, new Vector3(23.2f, -1.1f, -22.2f));
                }
                else if (Vector3.Distance(playerPos, new Vector3(-6.6f, -4.1f, 9.4f)) < 1f)
                {
                    ServerSend.RespawnPlayer(id, new Vector3(-11.3f, -4.1f, 9.4f));
                }
            }
        }

        // Runs every frame to check all players
        [HarmonyPatch(typeof(GameMode), nameof(GameMode.Update))]
        [HarmonyPostfix]
        public static void OnGameUpdate()
        {
            // Only the host can enforce rules
            if (!IsHost()) return;

            // Update the current map ID
            prevMapID = LobbyManager.Instance.map.id;

            // Check all alive players
            foreach (ulong id in GameManager.Instance.activePlayers.Keys)
            {
                if (!GameManager.Instance.activePlayers[id].dead)
                {
                    BoundsCheck(id);
                    GlitchingCheck(id);
                }
            }
        }
    }
}
