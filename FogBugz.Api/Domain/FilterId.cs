namespace FogBugz.Api.Domain
{
    public class FilterId
    {
        readonly string _value;

        private FilterId(string val)
        {
            _value = val;
        }

        public static explicit operator FilterId(string value)
        {
            return new FilterId(value);
        }

        public static implicit operator string(FilterId me)
        {
            return me._value;
        }
    }
}
