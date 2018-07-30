using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.WorldDefaults
{
    public class WorldSettings : Dictionary<string, WorldValue>
    {
        public WorldSettings()
        {
            AddNewValue("TurnSpeed", 10, 0, 20, 0.1F);
            AddNewValue("MoveSpeed", 10, 0, 20, 0.005F);
            AddNewValue("ViewRange", 10, 1, Int32.MaxValue, 0.3F);
            AddNewValue("ViewAngle", 10, 0, 180, 4.5F);
            AddNewValue("AgentCount", 50, 0, Int32.MaxValue, 1);
            AddNewValue("AgentSetup", 0, 0, 0, 1);
            AddNewValue("XDim", 18, 1, Int32.MaxValue, 1);
            AddNewValue("YDim", 10, 1, Int32.MaxValue, 1);
        }

        public void ChangeValueBy(string newkey, int change)
        {
            this[newkey].ChangeValueBy(change);
        }

        public void AddNewValue(string newkey, int newvalue, int min, int max, float multiplier)
        {
            this[newkey] = new WorldValue(newkey, newvalue, min, max, multiplier);
        }

        public float GetRealValue(string newkey)
        {
            return this[newkey].GetValue();
        }

        public void SetWorldValue(string name, int? newValue = null, int? newMin = null, int? newMax = null,
            float? newMultiplier = null)
        {
            this[name].ChangeValue(newValue, newMin, newMax, newMultiplier);
        }

        public Dictionary<string, float> GetSettings()
        {
            Dictionary<string, float> toReturn = new Dictionary<string, float>();
            foreach (string name in this.Keys)
            {
                toReturn[name] = this[name].GetValue();
            }
            return toReturn;
        }
    }
}
