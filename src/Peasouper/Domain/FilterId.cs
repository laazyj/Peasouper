namespace Peasouper.Domain
{
    public class FilterId
    {
        readonly string _value;

        private FilterId(string val)
        {
            _value = val;
        }

        public override string ToString()
        {
            return _value;
        }

        public static explicit operator FilterId(string value)
        {
            return new FilterId(value);
        }

        public static implicit operator string(FilterId me)
        {
            return me._value;
        }

        public bool Equals(FilterId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other._value == _value;
        }

        public bool Equals(string other)
        {
            if (ReferenceEquals(null, other)) return false;
            return other == _value;
        }

        public override bool Equals(object obj)
        {
            if (obj is string) return Equals((string)obj);
            return Equals(obj as FilterId);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            if (_value != null)
                hash = (hash * 7) + _value.GetHashCode();
            return hash;  
        }

        public static bool operator ==(FilterId filter1, FilterId filter2)
        {
            if (ReferenceEquals(filter1, null)) return false;
            return filter1.Equals(filter2);
        }

        public static bool operator !=(FilterId filter1, FilterId filter2)
        {
            if (ReferenceEquals(filter1, null)) return false;
            return !filter1.Equals(filter2);
        }

        public static bool operator ==(FilterId filter1, string filter2)
        {
            if (ReferenceEquals(filter1, null)) return false;
            return filter1.Equals(filter2);
        }

        public static bool operator !=(FilterId filter1, string filter2)
        {
            if (ReferenceEquals(filter1, null)) return false;
            return !filter1.Equals(filter2);
        }
    }
}
