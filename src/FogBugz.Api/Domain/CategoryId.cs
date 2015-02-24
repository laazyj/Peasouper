namespace FogBugz.Api.Domain
{
    public struct CategoryId
    {
        readonly int _value;

        private CategoryId(int val)
        {
            _value = val;
        }

        public static explicit operator CategoryId(int value)
        {
            return new CategoryId(value);
        }

        public static implicit operator int(CategoryId me)
        {
            return me._value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}