using LiesOfPractice.Interfaces;
using LiesOfPractice.Memory;

namespace LiesOfPractice.Services;

public class GameStateService (IMemoryIoService memoryIoService) : IGameStateService
{
    
    public bool IsLoaded()
    {
        var attributesBase = memoryIoService.FollowPointers(Offsets.PlayerBase.Base, Offsets.PlayerBase.Offsets.PlayerAttributesEntity, true);
        return memoryIoService.ReadInt64(attributesBase) != IntPtr.Zero;
    }
}