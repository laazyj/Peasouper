namespace FogBugz.Api.Domain
{
    public struct CaseId
    {
        readonly int _value;

        private CaseId(int val)
        {
            _value = val;
        }

        public static explicit operator CaseId(int value)
        {
            return new CaseId(value);
        }

        public static implicit operator int(CaseId me)
        {
            return me._value;
        }

        public static CaseId? FromInt(int val)
        {
            return val == 0 ? (CaseId?)null : (CaseId)val;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
