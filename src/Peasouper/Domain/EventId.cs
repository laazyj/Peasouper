namespace Peasouper.Domain
{
    public struct EventId
    {
        readonly int _value;

        private EventId(int val)
        {
            _value = val;
        }

        public static explicit operator EventId(int value)
        {
            return new EventId(value);
        }

        public static implicit operator int(EventId me)
        {
            return me._value;
        }

        public static EventId? FromInt(int val)
        {
            return (val == 0) ? (EventId?)null : (EventId)val;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}