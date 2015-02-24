namespace FogBugz.Api.Domain
{
    public struct StatusId
    {
        readonly int _value;

        private StatusId(int val)
        {
            _value = val;
        }

        public static explicit operator StatusId(int value)
        {
            return new StatusId(value);
        }

        public static implicit operator int(StatusId me)
        {
            return me._value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}