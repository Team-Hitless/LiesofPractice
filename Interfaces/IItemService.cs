using LiesOfPractice.Models;

namespace LiesOfPractice.Interfaces;

public interface IItemService
{
    void ItemSpawn(int itemId, int quantity);
    void WeaponSpawn(int bladeId, int handleId);
}