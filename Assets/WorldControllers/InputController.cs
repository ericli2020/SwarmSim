using Assets.WorldFactories;
using Assets.WorldObjects;
using System.Collections.Generic;
using System.Linq;
using Assets.WorldDefaults;
using UnityEngine;

namespace Assets.WorldControllers
{
    public class InputController : MonoBehaviour
    {
        private ModuleContainer _myModules;
        private WorldSettings _settings;
        private Dictionary<string, KeyCode> _keyMapping;
        private int _settingIndex;

        void Awake()
        {
            _keyMapping = new Dictionary<string, KeyCode> // eventually port this to a state
            {
                {"Up", KeyCode.E },
                {"Down", KeyCode.F },
                {"ToggleDial", KeyCode.C },
                {"ToggleStart", KeyCode.D },
                {"TurnUp", KeyCode.A },
                {"TurnDown", KeyCode.B }
            };
            _settings = new WorldSettings();
            _myModules = new ModuleContainer();
        }

        void Update()
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(_keyMapping["TurnUp"]))
                {
                    _myModules.ChangeSetting("TurnSpeed", 1);
                }
                else if (Input.GetKeyDown(_keyMapping["TurnDown"]))
                {
                    _myModules.ChangeSetting("TurnSpeed", -1);
                }
                else if (Input.GetKeyDown(_keyMapping["Up"]))
                {
                    _myModules.ChangeSetting("MoveSpeed", 1);
                }
                else if (Input.GetKeyDown(_keyMapping["Down"]))
                {
                    _myModules.ChangeSetting("MoveSpeed", -1);
                }
            }

            /*
            if (_worldStart)
            {
                if (_firstIteration)
                {
                    _myModules = new ModuleContainer(_settings);
                    _myAgentFactory = _myModules.AgentFactory;
                    _myWorldGrid = _myModules.WorldGrid;
                    _myWorldGrid.Started = true;
                    _firstIteration = false;
                } else if (Input.GetKeyDown(_keyMapping["ToggleStart"]))
                {
                    Debug.Log("Stopping");
                    _worldStart = false;
                    _myWorldGrid.Started = false;
                    
                    // delete game objects
                }
            }
            else if (Input.anyKeyDown) // SETTINGS CAN ONLY BE SET WHEN THE WORLD IS PAUSED
            {
                if (Input.GetKeyDown(_keyMapping["ToggleStart"]))
                {
                    Debug.Log("Starting");
                    if (!_firstIteration)
                    {
                        _myModules.ClearAll();
                    }
                    _firstIteration = true;
                    _worldStart = true;
                } else if (Input.GetKeyDown(_keyMapping["ToggleDial"]))
                {
                    _settingIndex = (_settingIndex + 1) % _settings.Count;
                    Debug.Log(_settings.Keys.ToList()[_settingIndex]);
                } else if (Input.GetKeyDown(_keyMapping["Up"]))
                {
                    _settings[_settings.Keys.ToList()[_settingIndex]]++;
                    Debug.Log(_settings.Keys.ToList()[_settingIndex] + " " + _settings.Values.ToList()[_settingIndex]);
                } else if (Input.GetKeyDown(_keyMapping["Down"]))
                {
                    _settings[_settings.Keys.ToList()[_settingIndex]]--;
                    if (_settings[_settings.Keys.ToList()[_settingIndex]] < 1)
                    {
                        _settings[_settings.Keys.ToList()[_settingIndex]] = 1;
                    }
                    Debug.Log(_settings.Keys.ToList()[_settingIndex] + " " + _settings.Values.ToList()[_settingIndex]);
                }
            }
            */
        }
    }
}
