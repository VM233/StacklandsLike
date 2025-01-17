﻿using Sirenix.OdinInspector;
using System;
using System.Runtime.CompilerServices;
using VMFramework.OdinExtensions;

namespace VMFramework.Property
{
    [PreviewComposite]
    public struct BaseIntProperty<TOwner> : IFormattable, IComparable<BaseIntProperty<TOwner>>
        where TOwner : class
    {
        private int _value;

        [ShowInInspector]
        [DelayedProperty]
        public int value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _value;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                var oldHealth = _value;
                _value = value;
                OnValueChanged?.Invoke(owner, oldHealth, _value);
            }
        }
        
        public readonly TOwner owner;

        public event Action<TOwner, int, int> OnValueChanged;

        public BaseIntProperty(TOwner owner, int value)
        {
            this.owner = owner;
            _value = value;
            OnValueChanged = null;
        }

        #region To String

        public readonly string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }

        public readonly override string ToString()
        {
            return _value.ToString();
        }

        #endregion

        public static implicit operator int(BaseIntProperty<TOwner> property) => property.value;

        public readonly int CompareTo(BaseIntProperty<TOwner> other)
        {
            return _value.CompareTo(other._value);
        }
    }
    
    [PreviewComposite]
    public struct BaseIntProperty : IFormattable, IComparable<BaseIntProperty>
    {
        private int _value;

        [ShowInInspector]
        [DelayedProperty]
        public int value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => _value;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                var oldHealth = _value;
                _value = value;
                OnValueChanged?.Invoke(oldHealth, _value);
            }
        }

        public event Action<int, int> OnValueChanged;

        public BaseIntProperty(int value)
        {
            _value = value;
            OnValueChanged = null;
        }

        #region To String

        public readonly string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }

        public readonly override string ToString()
        {
            return _value.ToString();
        }

        #endregion

        public static implicit operator int(BaseIntProperty property) => property.value;

        public readonly int CompareTo(BaseIntProperty other)
        {
            return _value.CompareTo(other._value);
        }
    }
}
