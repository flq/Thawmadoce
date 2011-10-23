using System.Windows.Input;

namespace Thawmadoce.Extensibility
{
    /// <summary>
    /// Describes a Key combination to press
    /// </summary>
    public struct KeyCombo
    {
        public static readonly KeyCombo Default = new KeyCombo(default(Key), default(ModifierKeys));
        private readonly Key _key;
        private readonly ModifierKeys _modifier;

        public KeyCombo(Key key) : this(key, ModifierKeys.None)
        {
        }

        public KeyCombo(Key key, ModifierKeys modifier)
        {
            _key = key;
            _modifier = modifier;
        }
        
        public Key Key
        {
            get { return _key; }
        }
        
        public ModifierKeys Modifier
        {
            get { return _modifier; }
        }

        public bool Equals(KeyCombo other)
        {
            return Equals(other._key, _key) && Equals(other._modifier, _modifier);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (KeyCombo)) return false;
            return Equals((KeyCombo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_key.GetHashCode()*397) ^ _modifier.GetHashCode();
            }
        }

        public static bool operator ==(KeyCombo left, KeyCombo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(KeyCombo left, KeyCombo right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            if (_modifier == ModifierKeys.None)
                return _key.ToString();
            return _modifier + "+" + _key;
        }
    }
}