using System;

namespace LiesOfPractice.Memory;

public static class Offsets
{
    public static class GiveErgoEntity
    {
        public static IntPtr Base;

        public enum ErgoEntity
        {
            Ptr1 = 0,
            Ptr2 = 0x20
        }
    }

    public static class ActivateAllTeleports
    {
        public static IntPtr Base;

    }

    public static class DebugFlagsBaseA
    {
        public static IntPtr Base;

        public enum Flags
        {
            InfiniteHealth = 0x0,
            InfiniteFable = 0x1,
            InfiniteStamFable = 0x2, //TODO check if this affects anything else
            OneShot = 0x4,
            EnableAi = 0x5, //Set to 1 by default, set to 0 to disable AI
            ChrNoDeath = 0x6,
            UnknownFlag = 0x7, //Setting this to 1 when ChrNoDeath is enabled still kills the player, test more to find out what it does
        }
 
    }

    public static class PlayerPosEntity
    {
        public static IntPtr Base;

        public const int Ptr1 = 0x140;
        public const int Ptr2 = 0xC0;
        public const int Ptr3 = 0xC0;
        public const int PosX = 0x1C0;
    }

    public static class DebugFlags
    {
        public static IntPtr Base;
        public const int NoDeath = 0x0;
        public const int OneShot = 0x1;
        public const int InfiniteStam = 0x2;
        public const int InfiniteFp = 0x3;
        public const int InfiniteArrows = 0x4;
        public const int Invisible = 0x6;
        public const int Silent = 0x7;
        public const int AllNoDeath = 0x8;
        public const int AllNoDamage = 0x9;
        public const int DisableAllAi = 0xD;
    }

    public static class DebugEvent
    {
        public static IntPtr Base;
        public const int EventDraw = 0xA8;
        public const int DisableEvent = 0xD4;
    }

    public static class SoloParamRepo
    {
        public static IntPtr Base;
        public const int SpEffectParamResCap = 0x4A8;
        public const int SpEffectPtr1 = 0x68;
        public const int SpEffectPtr2 = 0x68;
        public const int CinderSoulmass = 0x126730;

        public const int CamParamResCap = 0x928;
        public const int CamPtr1 = 0x68;
        public const int CamPtr2 = 0x68;
        public const int CamFov = 0xD8C;
    }

    public static class MenuMan
    {
        public static IntPtr Base;

        public enum MenuManOffsets
        {
            QuitOut = 0x250,
        }
    }

    public static class LuaEventMan
    {
        public static IntPtr Base;
    }


    public static class AiTargetingFlags
    {
        public static IntPtr Base;
        public const int Height = 0x4;
        public const int Width = 0x5;
    }

    public static class MapItemMan
    {
        public static IntPtr Base;
    }

    public static class HitIns
    {
        public static IntPtr Base;
        public const int LowHit = 0x0;
        public const int HighHit = 0x1;
        public const int ChrRagdoll = 0x3;
    }

    public static class WorldObjMan
    {
        public static IntPtr Base;
    }

    public static class EnemyIns
    {
        public const int ComManipulator = 0x58;

        public enum ComManipOffsets
        {
            EnemyId = 0x390,
            AiIns = 0x320
        }

        public enum AiInsOffsets
        {
            AiFunc = 0x8,
            SpEffectPtr = 0x20,
            LuaNumbers = 0x6BC,
            TargetingSystem = 0x7AB8,
        }

        public const int ForceActPtr = 0x8;
        public const int ForceActOffset = 0xB681;

        public const int TargetingView = 0x3C;

        public enum SpEffectOffsets
        {
            Stagger = 0x50,
            Poison = 0x60,
            Toxic = 0x64,
            Bleed = 0x170,
            FrostBite = 0x178
        }

        public enum LuaNumbers

        {
            Gwyn5HitComboNumberIndex = 0,
            GwynLightningRainNumberIndex = 1,
            PhaseTransitionCounterNumberIndex = 2
        }

        public const int CurrentPhaseOffset = 0x1FF0;
    }

    public static class FieldArea
    {
        public static IntPtr Base;
        public const int GameRend = 0x18;
        public const int DbgFreeCamMode = 0xE0;
        public const int DbgFreeCam = 0xE8;
        public const int DbgFreeCamCoords = 0x40;
        public const int ChrCam = 0x28;
        public const int ChrExFollowCam = 0x60;
        public const int CameraDownLimit = 0x200;
    }

    public static class GroupMask
    {
        public static IntPtr Base;
        public const int Map = 0x0;
        public const int Obj = 0x1;
        public const int Chr = 0x2;
        public const int Sfx = 0x3;
    }

    public static class UserInputManager
    {
        public static IntPtr Base;
        public const int SteamInputEnum = 0x24B;
    }

    public static class SprjFlipper
    {
        public static IntPtr Base;
        public const int Fps = 0x354;
        public const int DebugFpsToggle = 0x358;
    }

    public static class Patches
    {
        public static IntPtr NoLogo;
        public static IntPtr RepeatAct;
        public static IntPtr GameSpeed;
        public static IntPtr InfiniteDurability;
        public static IntPtr PlayerSoundView;
        public static IntPtr DebugFont;
        public static IntPtr NoRoll;
        public static IntPtr DbgDrawFlag;
        public static IntPtr FreeCam;
        public static IntPtr AccessFullShop;
    }


    public static class Hooks
    {
        public static long LastLockedTarget;
        public static long WarpCoordWrite;
        public static long AddSubGoal;
        public static long InAirTimer;
        public static long NoClipKeyboard;
        public static long NoClipTriggers;
        public static long NoClipTriggers2;
        public static long NoClipUpdateCoords;
        public static long CameraUpLimit;
        public static long ItemLotBase;
        public static long ArgoSpeed;
    }

    public static class Funcs
    {
        public static long GiveErgo;
        public static long ItemSpawn;
        public static long SetEvent;
        public static long Travel;
        public static long LevelUp;
        public static long ReinforceWeapon;
        public static long InfuseWeapon;
        public static long Repair;
        public static long AllotEstus;
        public static long Attunement;
        public static long RegularShop;
        public static long Transpose;
        public static long CombineMenuFlagAndEventFlag;
        public static long BreakAllObjects;
        public static long RestoreAllObjects;
        public static long GetEvent;
        public static long SetSpEffect;
    }
}
