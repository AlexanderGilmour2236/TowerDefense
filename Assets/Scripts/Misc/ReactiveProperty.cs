using System;

namespace Misc
{
    public class ReactiveProperty<T>
    {
        private T _value = default;

        public event Action<T> ValueChanged; 
        
        public T Value
        {
            get => _value;
            set
            {
                _value = value; 
                ValueChanged?.Invoke(_value);
            }
        }
    }
}