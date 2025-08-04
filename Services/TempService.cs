using LiesOfPractice.Memory;
using LiesOfPractice.Utilities;

namespace LiesOfPractice.Services;

public class TempService(MemoryIo memoryIo)
{
    public void GiveErgo(int amount)
    {
        var entity = memoryIo.FollowPointers(Offsets.GiveErgoEntity.Base, [
            (int)Offsets.GiveErgoEntity.ErgoEntity.Ptr1,
            (int)Offsets.GiveErgoEntity.ErgoEntity.Ptr2
        ], true);
        var bytes = AsmLoader.GetAsmBytes("GiveErgo");
        AsmHelper.WriteAbsoluteAddresses(bytes, [
            (entity, 0x4 + 2),
            (amount, 0xE + 2),
            (Offsets.Funcs.GiveErgo, 0x18 + 2)
        ]);
        memoryIo.AllocateAndExecute(bytes);
    }

    public void ToggleAllTeleports(bool isEnabled) =>
        memoryIo.WriteByte((IntPtr)memoryIo.ReadInt64(Offsets.ActivateAllTeleports.Base), isEnabled ? 1 : 0);
}