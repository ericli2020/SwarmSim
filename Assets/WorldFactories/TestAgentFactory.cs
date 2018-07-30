using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.WorldControllers;
using Assets.WorldObjects;
using UnityEngine;

namespace Assets.WorldFactories
{
    public class TestAgentFactory : MonoBehaviour
    {
        private int _currentId;

        public void Initialize(int prevId = 0)
        {
            _currentId = prevId;
        }

        private string NewId()
        {
            return _currentId++.ToString();
        }

        public void CreateAgent(WorldGrid worldGrid)
        {
            WorldAgent myAgent = (Instantiate(Resources.Load("AgentEdge")) as GameObject).GetComponent<WorldAgent>();
            myAgent.Initialize(worldGrid, NewId());
        }
    }
}
