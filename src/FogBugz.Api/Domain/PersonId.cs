namespace FogBugz.Api.Domain
{
    public struct PersonId
    {
        readonly int _value;

        private PersonId(int val)
        {
             _value = val;
        }

        public static explicit operator PersonId(int value)
        {
            return new PersonId(value);
        }

        public static implicit operator int(PersonId me)
        {
            return me._value;
        }    
    }
}
