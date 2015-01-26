namespace FogBugz.Api.Domain
{
    public struct AttachmentIconId
    {
        readonly int _value;

        private AttachmentIconId(int val)
        {
            _value = val;
        }

        public static explicit operator AttachmentIconId(int value)
        {
            return new AttachmentIconId(value);
        }

        public static implicit operator int(AttachmentIconId me)
        {
            return me._value;
        }
    }
}