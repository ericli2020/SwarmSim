using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.WorldDefaults
{
    public class WorldValue
    {
        public string Name { get; set; }
        private int _value;
        private int _min;
        private int _max;
        private float _multiplier;

        public WorldValue(string name, int value, int min, int max, float multiplier)
        {
            Name = name;
            _value = value;
            _min = min;
            _max = max;
            _multiplier = multiplier;
        }

        public float GetValue()
        {
            return _value * _multiplier;
        }

        public void ChangeValueBy(int change)
        {
            _value = _value + change;
            if (_value < _min)
            {
                _value = _min;
            }
            else if (_value > _max)
            {
                _value = _max;
            }
        }

        public void ChangeValue(int? newValue = null, int? newMin = null, int? newMax = null, float? newMultiplier = null)
        {
            if (newValue.HasValue)
            {
                _value = newValue.Value;
            }

            if (newMin.HasValue)
            {
                _min = newMin.Value;
            }

            if (newMax.HasValue)
            {
                _max = newMax.Value;
            }

            if (newMultiplier.HasValue)
            {
                _multiplier = newMultiplier.Value;
            }
        }
    }
}
