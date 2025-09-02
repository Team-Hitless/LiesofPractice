using LiesOfPractice.Enums;

namespace LiesOfPractice.Core;

[AttributeUsage(AttributeTargets.Property)]
public class ActionTagAttribute(ActionTag tag) : Attribute
{
    public ActionTag Tag { get; } = tag;
}
