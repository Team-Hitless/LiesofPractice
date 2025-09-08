using LiesOfPractice.Enums;

namespace LiesOfPractice.Interfaces;

public interface IEventService
{
    void Publish(GameEvent eventType);
    void Subscribe(GameEvent eventType, Action handler);
    void Unsubscribe(GameEvent eventType, Action handler);
}