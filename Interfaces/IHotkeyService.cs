using H.Hooks;
using LiesOfPractice.Enums;

namespace LiesOfPractice.Interfaces;

public interface IHotkeyService
{
    void ClearHotkey(ActionTag actionTag);
    ActionTag? GetActionTagByKeys(Keys keys);
    Keys? GetHotkey(ActionTag actionTag);
    void RegisterAction(ActionTag actionTag, Action action);
    void SetHotkey(ActionTag actionTag, Keys keys);
    void Start();
    void Stop();
}