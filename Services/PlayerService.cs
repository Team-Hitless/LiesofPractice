using LiesOfPractice.Interfaces;
using LiesOfPractice.Memory;
using LiesOfPractice.Utilities;
using static LiesOfPractice.Memory.Offsets;

namespace LiesOfPractice.Services;

public class PlayerService(IMemoryIoService memoryIo) : IPlayerService
{
    public void GiveErgo(int amount)
    {
        var playerEntity = memoryIo.FollowPointers(PlayerBase.Base, new[]
        {
            PlayerBase.Offsets.PlayerEntityPtr,
            PlayerBase.Offsets.PlayerEntity,
        }, true);
        var bytes = AsmLoader.GetAsmBytes("GiveErgo");
        AsmHelper.WriteAbsoluteAddresses(bytes, [
            (playerEntity, 0x4 + 2),
            (amount, 0xE + 2),
            (Funcs.GiveErgo, 0x18 + 2)
        ]);
        memoryIo.AllocateAndExecute(bytes);
    }

    public void ToggleChrDebugFlagA(bool isEnabled, int debugFlagOffset) =>
        memoryIo.WriteByte(DebugFlagsBaseA.Base + debugFlagOffset, isEnabled ? 1 : 0);

    public void ToggleActivateAllTeleports(bool isEnabled) =>
        memoryIo.WriteByte((IntPtr)memoryIo.ReadInt64(ActivateAllTeleports.Base), isEnabled ? 1 : 0);

    public void SavePos(int index)
    {
        var pos = memoryIo.FollowPointers(PlayerBase.Base, PlayerBase.Offsets.PlayerPosPtrChain, false);
        byte[] positionBytes = memoryIo.ReadBytes(pos, 12);
        if (index == 0) memoryIo.WriteBytes(CodeCaveOffsets.Base + CodeCaveOffsets.SavePos1, positionBytes);
        else memoryIo.WriteBytes(CodeCaveOffsets.Base + CodeCaveOffsets.SavePos2, positionBytes);
    }

    public void RestorePos(int index)
    {
        var pos = memoryIo.FollowPointers(PlayerBase.Base, PlayerBase.Offsets.PlayerPosPtrChain, false);
        byte[] positionBytes;
        if (index == 0) positionBytes = memoryIo.ReadBytes(CodeCaveOffsets.Base + CodeCaveOffsets.SavePos1, 12);
        else positionBytes = memoryIo.ReadBytes(CodeCaveOffsets.Base + CodeCaveOffsets.SavePos2, 12);

        memoryIo.WriteBytes(pos, positionBytes);
    }

    public void ToggleInfiniteConsumables(bool isEnabled) =>
        memoryIo.WriteByte(InfiniteConsumablesFlag.Base, isEnabled ? 1 : 0);

    public void ToggleNoErgoLossOnDeath(bool isEnabled)
    {
        if (isEnabled) memoryIo.WriteBytes(Patches.NoErgoLoss, [0x90, 0x90, 0x90, 0x90, 0x90, 0x90]);
        else memoryIo.WriteBytes(Patches.NoErgoLoss, [0x89, 0x99, 0xA4, 0x00, 0x00, 0x00]);
    }

    public void Rest()
    {
        var playerEntity = memoryIo.FollowPointers(PlayerBase.Base, [
            PlayerBase.Offsets.PlayerEntityPtr,
            PlayerBase.Offsets.PlayerEntity
        ], true);
        var bytes = AsmLoader.GetAsmBytes("Rest");
        AsmHelper.WriteAbsoluteAddresses(bytes, [
            (playerEntity, 0x0 + 2),
            (Funcs.Rest, 0xA + 2)
        ]);
        memoryIo.AllocateAndExecute(bytes);
    }
}