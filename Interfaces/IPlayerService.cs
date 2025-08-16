namespace LiesOfPractice.Interfaces;

public interface IPlayerService
{
    void GiveErgo(int amount);
    void ToggleChrDebugFlagA(bool isEnabled, int debugFlagOffset);
    void ToggleActivateAllTeleports(bool isEnabled);
    void SavePos(int index);
    void RestorePos(int index);
    void ToggleInfiniteConsumables(bool isEnabled);
    void ToggleNoErgoLossOnDeath(bool isEnabled);
    void Rest();
}