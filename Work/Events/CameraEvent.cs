using EventSystems;

namespace Events
{
    public static class CameraEvent
    {
        public static readonly ImpulseEvent ImpulseEvent = new ImpulseEvent();
    }
    public class ImpulseEvent : GameEvent { }
}
