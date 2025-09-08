using LiesOfPractice.Enums;
using LiesOfPractice.Interfaces;

namespace LiesOfPractice.Services;

public class EventService : IEventService
{
    private readonly Dictionary<GameEvent, List<Action>> _eventHandlers = new();
    
    public void Publish(GameEvent eventType)
    {
        if (_eventHandlers.ContainsKey(eventType))
        {
            foreach (var handler in _eventHandlers[eventType])
                handler.Invoke();
        }
    }

    public void Subscribe(GameEvent eventType, Action handler)
    {
        if (!_eventHandlers.ContainsKey(eventType))
            _eventHandlers[eventType] = new List<Action>();
   
        _eventHandlers[eventType].Add(handler);
    }

    public void Unsubscribe(GameEvent eventType, Action handler)
    {
        if (_eventHandlers.ContainsKey(eventType))
            _eventHandlers[eventType].Remove(handler);
    }
}