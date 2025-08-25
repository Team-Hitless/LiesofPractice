using LiesOfPractice.Interfaces;
using LiesOfPractice.Models;
using LiesOfPractice.Utilities;
using static LiesOfPractice.Memory.Offsets;

namespace LiesOfPractice.Services;

public class ItemService (IMemoryIoService memoryIo) : IItemService
{
    public void ItemSpawn(int itemId, int quantity)
    {
        var bytes = AsmLoader.GetAsmBytes("ItemSpawn");
        AsmHelper.WriteAbsoluteAddresses(bytes, [
            (Funcs.GetGiveItemEntity, 0x04 + 2),
            (itemId, 0x17 + 2),
            (quantity, 0x21 + 2),
            (Funcs.GiveItem, 0x2B + 2)
        ]);
        memoryIo.AllocateAndExecute(bytes);
    }

    public void WeaponSpawn(Weapon weapon)
    {
        var bytes = AsmLoader.GetAsmBytes("WeaponSpawn");
        
        AsmHelper.WriteAbsoluteAddresses(bytes, [
            (Funcs.GetGiveItemEntity, 0x04 + 2),
            (weapon.BladeId, 0x17 + 2),
            (weapon.HandleId, 0x21 + 2),
            (Funcs.GiveWeapon, 0x32 + 2)
        ]);
        memoryIo.AllocateAndExecute(bytes);
    }
}