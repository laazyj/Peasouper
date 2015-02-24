namespace FogBugz.Api.Domain
{
    public struct DiscussionId
    {
        readonly int _value;

        private DiscussionId(int val)
        {
            _value = val;
        }

        public static explicit operator DiscussionId(int value)
        {
            return new DiscussionId(value);
        }

        public static implicit operator int(DiscussionId me)
        {
            return me._value;
        }

        public static DiscussionId? FromInt(int val)
        {
            return (val == 0) ? (DiscussionId?)null : (DiscussionId)val;
        }
    }
}