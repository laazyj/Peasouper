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
    }
}
