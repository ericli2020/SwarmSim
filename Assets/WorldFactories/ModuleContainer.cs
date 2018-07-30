using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.WorldControllers;
using Assets.WorldDefaults;
using Assets.WorldObjects;

namespace Assets.WorldFactories
{
    public class ModuleContainer
    {
        private WorldGrid MyWorldGrid { get; set; } // eventually a dictionary of world grids
        private WorldSettings Settings { get; set; }

        public ModuleContainer(params WorldValue[] newValues)
        {
            Settings = new WorldSettings();
            TestAgentFactory AgentFactory = new TestAgentFactory();
            MyWorldGrid = new WorldGrid(Settings.GetSettings());
            for (int i = 0; i < Settings.GetRealValue("AgentCount"); i++)
            {
                AgentFactory.CreateAgent(MyWorldGrid);
            }

            MyWorldGrid.Started = true;
        }

        public void ChangeSetting(string name, int change)
        {
            Settings.ChangeValueBy(name, change);
            MyWorldGrid.SetValue(name, Settings[name].GetValue());
        }

        public void SetSetting(string name, int? newValue = null, int? newMin = null, int? newMax = null, float? newMultiplier = null)
        {
            Settings.SetWorldValue(name, newValue, newMin, newMax, newMultiplier);
            MyWorldGrid.SetValue(name, Settings[name].GetValue());
        }

        public void ClearAll()
        {
            MyWorldGrid.ClearAll();
        }
    }
}
