namespace KooFrame
{
    public interface ISystem : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetUtility,
        ICanRegisterEvent, ICanTriggerEvent, ICanGetSystem, ICanInit
    {
    }
}