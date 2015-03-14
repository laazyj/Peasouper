namespace Peasouper.Domain
{
    public struct MilestoneId
    {
        readonly int _value;

        private MilestoneId(int val)
        {
            _value = val;
        }

        public static explicit operator MilestoneId(int value)
        {
            return new MilestoneId(value);
        }

        public static implicit operator int(MilestoneId me)
        {
            return me._value;
        }    
    }
}