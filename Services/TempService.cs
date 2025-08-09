using LiesOfPractice.Interfaces;
using LiesOfPractice.Memory;
using LiesOfPractice.Utilities;
using static LiesOfPractice.Memory.Offsets;

namespace LiesOfPractice.Services;

public class TempService(IMemoryIoService memoryIo)
{

    public void ToggleAllTeleports(bool isEnabled) =>
        memoryIo.WriteByte((IntPtr)memoryIo.ReadInt64(ActivateAllTeleports.Base), isEnabled ? 1 : 0);

    public void ToggleChrDebugFlagA(bool isEnabled, int debugFlagOffset) =>
        memoryIo.WriteByte(DebugFlagsBaseA.Base + debugFlagOffset, isEnabled ? 1 : 0);
    
    public void ToggleAiDisable(bool isDisabled) =>
        memoryIo.WriteByte(DebugFlagsBaseA.Base + (int)DebugFlagsBaseA.Flags.EnableAi, isDisabled ? 0 : 1);

    

}