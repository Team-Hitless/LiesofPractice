using LiesOfPractice.Interfaces;
using LiesOfPractice.Memory;
using LiesOfPractice.Utilities;
using static LiesOfPractice.Memory.Offsets;

namespace LiesOfPractice.Services;

public class TempService(IMemoryIoService memoryIo)
{
    public void GiveErgo(int amount)
    {
        var entity = memoryIo.FollowPointers(GiveErgoEntity.Base, [
            (int)GiveErgoEntity.ErgoEntity.Ptr1,
            (int)GiveErgoEntity.ErgoEntity.Ptr2
        ], true);
        var bytes = AsmLoader.GetAsmBytes("GiveErgo");
        AsmHelper.WriteAbsoluteAddresses(bytes, [
            (entity, 0x4 + 2),
            (amount, 0xE + 2),
            (Funcs.GiveErgo, 0x18 + 2)
        ]);
        memoryIo.AllocateAndExecute(bytes);
    }

    public void ToggleAllTeleports(bool isEnabled) =>
        memoryIo.WriteByte((IntPtr)memoryIo.ReadInt64(ActivateAllTeleports.Base), isEnabled ? 1 : 0);

    public void ToggleChrDebugFlagA(bool isEnabled, int debugFlagOffset) =>
        memoryIo.WriteByte(DebugFlagsBaseA.Base + debugFlagOffset, isEnabled ? 1 : 0);
    
    public void ToggleAiDisable(bool isDisabled) =>
        memoryIo.WriteByte(DebugFlagsBaseA.Base + (int)DebugFlagsBaseA.Flags.EnableAi, isDisabled ? 0 : 1);

    public void SavePos(int index)
    {
        var pos = memoryIo.FollowPointers(PlayerPosEntity.Base, new[]
        {
            PlayerPosEntity.Ptr1,
            PlayerPosEntity.Ptr2,
            PlayerPosEntity.Ptr3,
            PlayerPosEntity.PosX,
        }, false);

        byte[] positionBytes = memoryIo.ReadBytes(pos, 12);
        if (index == 0) memoryIo.WriteBytes(CodeCaveOffsets.Base + CodeCaveOffsets.SavePos1, positionBytes);
        else memoryIo.WriteBytes(CodeCaveOffsets.Base + CodeCaveOffsets.SavePos2, positionBytes);
    }

    public void RestorePos(int index)
    {
        var pos = memoryIo.FollowPointers(PlayerPosEntity.Base, new[]
        {
            PlayerPosEntity.Ptr1,
            PlayerPosEntity.Ptr2,
            PlayerPosEntity.Ptr3,
            PlayerPosEntity.PosX,
        }, false);
        
        byte[] positionBytes;
        if (index == 0) positionBytes = memoryIo.ReadBytes(CodeCaveOffsets.Base + CodeCaveOffsets.SavePos1, 12);
        else positionBytes = memoryIo.ReadBytes(CodeCaveOffsets.Base + CodeCaveOffsets.SavePos2, 12);
        
        memoryIo.WriteBytes(pos, positionBytes);
    }

}