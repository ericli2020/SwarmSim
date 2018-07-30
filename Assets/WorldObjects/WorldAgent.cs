using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.WorldControllers;
using UnityEngine;

namespace Assets.WorldObjects
{
    public class WorldAgent : MonoBehaviour, IEquatable<WorldAgent>
    {
        private WorldGrid _myWorld;
        public string Id { get; set; }
        public float BaseSpeed { get; set; }
        public float BaseTurn { get; set; }

        public void Initialize(WorldGrid newWorld, string id)
        {
            _myWorld = newWorld;
            Id = id;
            BaseSpeed = 1;
            BaseTurn = 1;
            _myWorld.AddAgent(this);
        }
        
        void Update()
        {
            if (_myWorld.Started)
            {
                Vector3 heading = _myWorld.GetNextHeading(this);
                this.transform.eulerAngles = heading;
                float xForward = _myWorld.GetSettingFloat("MoveSpeed") * BaseSpeed * (float) Math.Cos(heading.z * Math.PI / 180);
                float yForward = _myWorld.GetSettingFloat("MoveSpeed") * BaseSpeed * (float) Math.Sin(heading.z * Math.PI / 180);
                float currY = this.transform.position.y;
                float currX = this.transform.position.x;
                float nextX = ((currX + xForward/* + _myWorld.xWind*/) % _myWorld.GetSettingFloat("XDim") + _myWorld.GetSettingFloat("XDim")) % _myWorld.GetSettingFloat("XDim");
                float nextY = ((currY + yForward/* + _myWorld.yWind*/) % _myWorld.GetSettingFloat("YDim") + _myWorld.GetSettingFloat("YDim")) % _myWorld.GetSettingFloat("YDim");
                this.transform.position = new Vector3(nextX, nextY);
                
                _myWorld.reportNewLocation(this);
            }
        }

        public bool Equals(WorldAgent other)
        {
            if (other == null)
            {
                return false;
            }
            return Id == other.Id;
        }

        public double FindDistance(WorldAgent other) // FIX REFERENCES
        {
            if (other.Id == this.Id)
            {
                return double.PositiveInfinity;
            }
            else
            {
                return Math.Sqrt((other.transform.position.x - this.transform.position.x) * (other.transform.position.x - this.transform.position.x) + (other.transform.position.y - this.transform.position.y) * (other.transform.position.y - this.transform.position.y));
            }
        }

        public void End()
        {
            Destroy(this.gameObject);
        }
    }
}
