using System;
using System.Collections.Generic;
using System.Linq;
using Assets.WorldDefaults;
using UnityEngine;
using Random = System.Random;

namespace Assets.WorldObjects
{
    // DIFFERENT WORLD GRID AND WORLD AGENT FOR EACH TYPE
    public class WorldGrid
    {
        private float multiplier { get; set; }
        private int testNum { get; set; }
        private float testOffset { get; set; }
        private float testAngle { get; set; }
        public string Type { get; set; }
        private Random RandomGen { get; set; }
        private int GridX { get; set; }
        private int GridY { get; set; }
        public bool Started { get; set; }
        private Dictionary<string, float> settings { get; set; }
        private List<WorldAgent>[,] _agentGrid;
        private Dictionary<string, Pair<int, int>> _coordsOfAgent;

        public WorldGrid(Dictionary<string, float> defaultSettings)
        {
            testNum = -1;
            testAngle = 0F;
            testOffset = 0.5F;
            multiplier = 2F;
            settings = defaultSettings;
            GridX = (int)Math.Ceiling(GetSettingFloat("XDim") / GetSettingFloat("ViewRange"));
            GridY = (int)Math.Ceiling(GetSettingFloat("YDim") / GetSettingFloat("ViewRange"));
            RandomGen = new Random();
            _agentGrid = new List<WorldAgent>[GridX, GridY];

            for (int i = 0; i < GridX; i++)
            {
                for (int j = 0; j < GridY; j++)
                {
                    _agentGrid[i, j] = new List<WorldAgent>();
                }
            }

            _coordsOfAgent = new Dictionary<string, Pair<int, int>>();
        }

        public void AddAgent(WorldAgent newAgent)
        {
            // newAgent.transform.position = GetTestLocation();
            newAgent.transform.position = GetNewLocation();
            // newAgent.transform.position = GetCenterLocation();
            newAgent.transform.eulerAngles = GetNewAngle();
            // newAgent.transform.eulerAngles = GetTestAngle();
            newAgent.BaseTurn = GetRandomTurnSpeed();
            newAgent.BaseSpeed = GetRandomSpeed();
            // newAgent.BaseSpeed = 0;
            float newScale = GetTurnScale(newAgent.BaseTurn);
            newAgent.transform.localScale = new Vector3(newScale, newScale, 0);
            newAgent.GetComponent<SpriteRenderer>().color = GetSpeedColor(newAgent.BaseSpeed);

            int newX = (int)Math.Floor(newAgent.transform.position.x / GetSettingFloat("ViewRange"));
            int newY = (int)Math.Floor(newAgent.transform.position.y / GetSettingFloat("ViewRange"));
            _coordsOfAgent[newAgent.Id] = new Pair<int, int>(newX, newY);
            _agentGrid[newX, newY].Add(newAgent);
        }

        private Color GetSpeedColor(float currSpeed)
        {
            return new Color(0.2F + (float) (RandomGen.NextDouble() * 0.4 - 0.2), currSpeed - 0.5F, 0.2F + (float)(RandomGen.NextDouble() * 0.4 - 0.2));
        }

        private float GetTurnScale(float currTurn)
        {
            return currTurn;
        }

        private Vector3 GetTestLocation()
        {
            testNum++;
            if (testNum == 0)
            {
                return new Vector3(1 * multiplier + testOffset, 1 * multiplier + testOffset);
            } else if (testNum == 1)
            {
                return new Vector3(2 * multiplier + testOffset, 1 * multiplier + testOffset);
            } else if (testNum == 2)
            {
                return new Vector3(3 * multiplier + testOffset, 1 * multiplier + testOffset);
            } else if (testNum == 3)
            {
                return new Vector3(1 * multiplier + testOffset, 2 * multiplier + testOffset);
            } else if (testNum == 4)
            {
                return new Vector3(2 * multiplier + testOffset, 2 * multiplier + testOffset);
            } else if (testNum == 5)
            {
                return new Vector3(3 * multiplier + testOffset, 2 * multiplier + testOffset);
            } else if (testNum == 6)
            {
                return new Vector3(1 * multiplier + testOffset, 3 * multiplier + testOffset);
            } else if (testNum == 7)
            {
                return new Vector3(2 * multiplier + testOffset, 3 * multiplier + testOffset);
            }
            return new Vector3(3 * multiplier + testOffset, 3 * multiplier + testOffset);
        }

        private Vector3 GetTestAngle()
        {
            return new Vector3(0, 0, testAngle);
        }

        private Vector3 GetNewLocation()
        {
            return new Vector3((float)RandomGen.NextDouble() * GetSettingFloat("XDim"), (float)RandomGen.NextDouble() * GetSettingFloat("YDim"));
        }

        private Vector3 GetCenterLocation()
        {
            return new Vector3(GetSettingFloat("XDim") / 2, GetSettingFloat("YDim") / 2);
        }

        private float GetRandomSpeed()
        {
            return (float)(RandomGen.NextDouble() * 0.4 + 0.8);
        }

        private float GetRandomTurnSpeed()
        {
            return (float)(RandomGen.NextDouble() * 0.4 + 0.8);

        }

        private float GetScale()
        {
            return (float)(RandomGen.NextDouble() * 0.4 + 0.8);
        }

        private Vector3 GetNewAngle()
        {
            return new Vector3(0, 0, (float)RandomGen.NextDouble() * 360);
        }

        private Color GetNewColor()
        {
            return new Color((float)RandomGen.NextDouble(), (float)RandomGen.NextDouble(), (float)RandomGen.NextDouble());
        }

        public void reportNewLocation(WorldAgent newAgent)
        {
            int newX = (int)Math.Floor(newAgent.transform.position.x / GetSettingFloat("ViewRange"));
            int newY = (int)Math.Floor(newAgent.transform.position.y / GetSettingFloat("ViewRange"));
            Pair<int, int> currPos = _coordsOfAgent[newAgent.Id];
            _coordsOfAgent[newAgent.Id] = new Pair<int, int>(newX, newY);
            _agentGrid[currPos.First, currPos.Second].Remove(newAgent);
            _agentGrid[newX, newY].Add(newAgent);
        }

        public Vector3 GetNextHeading(WorldAgent newAgent)
        {
            Vector3 heading = newAgent.transform.eulerAngles;
            Vector3 testForward = new Vector3((float)Math.Cos(newAgent.transform.eulerAngles.z * Math.PI / 180), (float)Math.Sin(newAgent.transform.eulerAngles.z * Math.PI / 180));
            float highAngle = (heading.z + GetSettingFloat("ViewAngle") / 2) % 360;
            float highAngleX = (float) Math.Cos(highAngle * Math.PI / 180);
            float highAngleY = (float) Math.Sin(highAngle * Math.PI / 180);
            float lowAngle = (360 + (heading.z - GetSettingFloat("ViewAngle") / 2)) % 360;
            float lowAngleX = (float) Math.Cos(lowAngle * Math.PI / 180);
            float lowAngleY = (float) Math.Sin(lowAngle * Math.PI / 180);
            Vector3 highForward = new Vector3(highAngleX, highAngleY);
            Vector3 lowForward = new Vector3(lowAngleX, lowAngleY);
            float actualX = newAgent.transform.position.x;
            float actualY = newAgent.transform.position.y;
            int newX = (int)Math.Floor(actualX / GetSettingFloat("ViewRange"));
            int newY = (int)Math.Floor(actualY / GetSettingFloat("ViewRange"));
            double currClosestDistance = double.PositiveInfinity;
            Vector3 targetDirection = Vector3.negativeInfinity;

            // search current grid
            Pair<double, Vector3> currPair = SearchGrid(newAgent, newX, newY, currClosestDistance, targetDirection, testForward);
            currClosestDistance = currPair.First;
            targetDirection = currPair.Second;

            if (newX != 0)
            {
                if (newY != 0 && Math.Sqrt((actualX - newX * GetSettingFloat("ViewRange")) * (actualX - newX * GetSettingFloat("ViewRange")) + (actualY - newY * GetSettingFloat("ViewRange")) * (actualY - newY * GetSettingFloat("ViewRange"))) < currClosestDistance)
                {
                    // search bottom left grid
                    Vector3 topLeft = new Vector3((newX - 1) * GetSettingFloat("ViewRange"), newY * GetSettingFloat("ViewRange"));
                    Vector3 bottomRight = new Vector3(newX* GetSettingFloat("ViewRange"), (newY - 1) * GetSettingFloat("ViewRange"));
                    Vector3 targetTopLeft = topLeft - newAgent.transform.position;
                    Vector3 targetBottomRight = bottomRight - newAgent.transform.position;

                    float highCrossTopLeft = Vector3.Cross(highForward, targetTopLeft).z;
                    float highCrossBottomRight = Vector3.Cross(highForward, targetBottomRight).z;
                    float lowCrossTopLeft = Vector3.Cross(lowForward, targetTopLeft).z;
                    float lowCrossBottomRight = Vector3.Cross(lowForward, targetBottomRight).z;

                    if (GetSettingFloat("ViewAngle") >= 360 || (highCrossTopLeft <= 0 && highCrossBottomRight >= 0) || (lowCrossTopLeft <= 0 && lowCrossBottomRight >= 0))
                    {
                        currPair = SearchGrid(newAgent, newX - 1, newY - 1, currClosestDistance, targetDirection, testForward);
                        currClosestDistance = currPair.First;
                        targetDirection = currPair.Second;
                    }


                }
                if (newY != GridY - 1 && Math.Sqrt((actualX - newX * GetSettingFloat("ViewRange")) * (actualX - newX * GetSettingFloat("ViewRange")) + ((newY + 1) * GetSettingFloat("ViewRange") - actualY) * ((newY + 1) * GetSettingFloat("ViewRange") - actualY)) <
                    currClosestDistance)
                {
                    // search top left grid
                    Vector3 topRight = new Vector3(newX * GetSettingFloat("ViewRange"), (newY + 2) * GetSettingFloat("ViewRange"));
                    Vector3 bottomLeft = new Vector3((newX - 1) * GetSettingFloat("ViewRange"), (newY + 1) * GetSettingFloat("ViewRange"));
                    Vector3 targetTopRight = topRight - newAgent.transform.position;
                    Vector3 targetBottomLeft = bottomLeft - newAgent.transform.position;

                    float highCrossTopRight = Vector3.Cross(highForward, targetTopRight).z;
                    float highCrossBottomLeft = Vector3.Cross(highForward, targetBottomLeft).z;
                    float lowCrossTopRight = Vector3.Cross(lowForward, targetTopRight).z;
                    float lowCrossBottomLeft = Vector3.Cross(lowForward, targetBottomLeft).z;
                    
                    if (GetSettingFloat("ViewAngle") >= 360 || (highCrossTopRight <= 0 && highCrossBottomLeft >= 0) || (lowCrossTopRight <= 0 && lowCrossBottomLeft >= 0))
                    {
                        currPair = SearchGrid(newAgent, newX - 1, newY + 1, currClosestDistance, targetDirection, testForward);
                        currClosestDistance = currPair.First;
                        targetDirection = currPair.Second;
                    }


                }
                if (actualX - newX * GetSettingFloat("ViewRange") < currClosestDistance)
                {
                    // search left grid
                    Vector3 topRight = new Vector3(newX * GetSettingFloat("ViewRange"), (newY + 1) * GetSettingFloat("ViewRange"));
                    Vector3 bottomRight = new Vector3(newX * GetSettingFloat("ViewRange"), newY * GetSettingFloat("ViewRange"));
                    Vector3 targetTopRight = topRight - newAgent.transform.position;
                    Vector3 targetBottomRight = bottomRight - newAgent.transform.position;

                    float highCrossTopRight = Vector3.Cross(highForward, targetTopRight).z;
                    float highCrossBottomRight = Vector3.Cross(highForward, targetBottomRight).z;
                    float lowCrossTopRight = Vector3.Cross(lowForward, targetTopRight).z;
                    float lowCrossBottomRight = Vector3.Cross(lowForward, targetBottomRight).z;

                    if (GetSettingFloat("ViewAngle") >= 360 || (highCrossTopRight <= 0 && highCrossBottomRight >= 0) || (lowCrossTopRight <= 0 && lowCrossBottomRight >= 0))
                    {
                        currPair = SearchGrid(newAgent, newX - 1, newY, currClosestDistance, targetDirection, testForward);
                        currClosestDistance = currPair.First;
                        targetDirection = currPair.Second;
                    }


                }
            }
            if (newY != 0 && actualY - newY * GetSettingFloat("ViewRange") < currClosestDistance)
            {
                // search bottom grid
                Vector3 topLeft = new Vector3(newX * GetSettingFloat("ViewRange"), newY * GetSettingFloat("ViewRange"));
                Vector3 topRight = new Vector3((newX + 1) * GetSettingFloat("ViewRange"), newY * GetSettingFloat("ViewRange"));
                Vector3 targetTopLeft = topLeft - newAgent.transform.position;
                Vector3 targetTopRight = topRight - newAgent.transform.position;

                float highCrossTopRight = Vector3.Cross(highForward, targetTopRight).z;
                float highCrossTopLeft = Vector3.Cross(highForward, targetTopLeft).z;
                float lowCrossTopRight = Vector3.Cross(lowForward, targetTopRight).z;
                float lowCrossTopLeft = Vector3.Cross(lowForward, targetTopLeft).z;

                if (GetSettingFloat("ViewAngle") >= 360 || (highCrossTopLeft <= 0 && highCrossTopRight >= 0) || (lowCrossTopLeft <= 0 && lowCrossTopRight >= 0))
                {
                    currPair = SearchGrid(newAgent, newX, newY - 1, currClosestDistance, targetDirection, testForward);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
            }
            if (newY != GridY - 1 && (newY + 1) * GetSettingFloat("ViewRange") - actualY < currClosestDistance)
            {
                // search top grid
                Vector3 bottomRight = new Vector3((newX + 1) * GetSettingFloat("ViewRange"),
                    (newY + 1) * GetSettingFloat("ViewRange"));
                Vector3 bottomLeft = new Vector3(newX * GetSettingFloat("ViewRange"), (newY + 1) * GetSettingFloat("ViewRange"));
                Vector3 targetBottomRight = bottomRight - newAgent.transform.position;
                Vector3 targetBottomLeft = bottomLeft - newAgent.transform.position;

                float highCrossBottomRight = Vector3.Cross(highForward, targetBottomRight).z;
                float highCrossBottomLeft = Vector3.Cross(highForward, targetBottomLeft).z;
                float lowCrossBottomRight = Vector3.Cross(lowForward, targetBottomRight).z;
                float lowCrossBottomLeft = Vector3.Cross(lowForward, targetBottomLeft).z;

                if (GetSettingFloat("ViewAngle") >= 360 || (highCrossBottomRight <= 0 && highCrossBottomLeft >= 0) || (lowCrossBottomRight <= 0 && lowCrossBottomLeft >= 0))
                {
                    currPair = SearchGrid(newAgent, newX, newY + 1, currClosestDistance, targetDirection, testForward);
                    currClosestDistance = currPair.First;
                    targetDirection = currPair.Second;
                }
            }
            if (newX != GridX - 1)
            {
                if (newY != 0 && Math.Sqrt(((newX + 1) * GetSettingFloat("ViewRange") - actualX) * ((newX + 1) * GetSettingFloat("ViewRange") - actualX) + (actualY - newY * GetSettingFloat("ViewRange")) * (actualY - newY * GetSettingFloat("ViewRange"))) < currClosestDistance)
                {
                    // search bottom right grid
                    Vector3 bottomLeft = new Vector3((newX + 1) * GetSettingFloat("ViewRange"), (newY - 1) * GetSettingFloat("ViewRange"));
                    Vector3 topRight = new Vector3((newX + 2) * GetSettingFloat("ViewRange"), newY * GetSettingFloat("ViewRange"));
                    Vector3 targetBottomLeft = bottomLeft - newAgent.transform.position;
                    Vector3 targetTopRight = topRight - newAgent.transform.position;

                    float highCrossBottomLeft = Vector3.Cross(highForward, targetBottomLeft).z;
                    float highCrossTopRight = Vector3.Cross(highForward, targetTopRight).z;
                    float lowCrossBottomLeft = Vector3.Cross(lowForward, targetBottomLeft).z;
                    float lowCrossTopRight = Vector3.Cross(lowForward, targetTopRight).z;

                    if (GetSettingFloat("ViewAngle") >= 360 || (highCrossBottomLeft <= 0 && highCrossTopRight >= 0) || (lowCrossBottomLeft <= 0 || lowCrossTopRight >= 0))
                    {
                        currPair = SearchGrid(newAgent, newX + 1, newY - 1, currClosestDistance, targetDirection, testForward);
                        currClosestDistance = currPair.First;
                        targetDirection = currPair.Second;
                    }
                }
                if (newY != GridY - 1 && Math.Sqrt(((newX + 1) * GetSettingFloat("ViewRange") - actualX) * ((newX + 1) * GetSettingFloat("ViewRange") - actualX) + ((newY + 1) * GetSettingFloat("ViewRange") - actualY) * ((newY + 1) * GetSettingFloat("ViewRange") - actualY)) <
                    currClosestDistance)
                {
                    // search top right grid
                    Vector3 bottomRight = new Vector3((newX + 2) * GetSettingFloat("ViewRange"), (newY + 1) * GetSettingFloat("ViewRange"));
                    Vector3 topLeft = new Vector3((newX + 1) * GetSettingFloat("ViewRange"), (newY + 2) * GetSettingFloat("ViewRange"));
                    Vector3 targetBottomRight = bottomRight - newAgent.transform.position;
                    Vector3 targetTopLeft = topLeft - newAgent.transform.position;

                    float highCrossBottomRight = Vector3.Cross(highForward, targetBottomRight).z;
                    float highCrossTopLeft = Vector3.Cross(highForward, targetTopLeft).z;
                    float lowCrossBottomRight = Vector3.Cross(lowForward, targetBottomRight).z;
                    float lowCrossTopLeft = Vector3.Cross(lowForward, targetTopLeft).z;

                    if (GetSettingFloat("ViewAngle") >= 360 || (highCrossBottomRight <= 0 && highCrossTopLeft >= 0) || (lowCrossBottomRight <= 0 && lowCrossTopLeft >= 0))
                    {
                        currPair = SearchGrid(newAgent, newX + 1, newY + 1, currClosestDistance, targetDirection, testForward);
                        currClosestDistance = currPair.First;
                        targetDirection = currPair.Second;
                    }


                }
                if ((newX + 1) * GetSettingFloat("ViewRange") - actualX < currClosestDistance)
                {
                    // search right grid
                    Vector3 bottomLeft = new Vector3((newX + 1) * GetSettingFloat("ViewRange"), newY * GetSettingFloat("ViewRange"));
                    Vector3 topLeft = new Vector3((newX + 1) * GetSettingFloat("ViewRange"), (newY + 1) * GetSettingFloat("ViewRange"));
                    Vector3 targetBottomLeft = bottomLeft - newAgent.transform.position;
                    Vector3 targetTopLeft = topLeft - newAgent.transform.position;

                    float highCrossBottomLeft = Vector3.Cross(highForward, targetBottomLeft).z;
                    float highCrossTopLeft = Vector3.Cross(highForward, targetTopLeft).z;
                    float lowCrossBottomLeft = Vector3.Cross(lowForward, targetBottomLeft).z;
                    float lowCrossTopLeft = Vector3.Cross(lowForward, targetTopLeft).z;

                    if (GetSettingFloat("ViewAngle") >= 360 || (highCrossBottomLeft <= 0 && highCrossTopLeft >= 0) || (lowCrossBottomLeft <= 0 && lowCrossTopLeft >= 0))
                    {
                        currPair = SearchGrid(newAgent, newX + 1, newY, currClosestDistance, targetDirection, testForward);
                        currClosestDistance = currPair.First;
                        targetDirection = currPair.Second;
                    }
                }
            }

            if (Double.IsPositiveInfinity(currClosestDistance))
            {
                float newZ = (float)(heading.z + Math.PI * GetSettingFloat("TurnSpeed") * newAgent.BaseTurn * (RandomGen.NextDouble() - 0.5));
                heading = new Vector3(0, 0, newZ);
            }
            else
            {
                Vector3 targetDir = targetDirection - newAgent.transform.position;
                float endAngle = Vector3.Angle(testForward, targetDir);
                Vector3 crossProduct = Vector3.Cross(testForward, targetDir);

                if (endAngle > GetSettingFloat("TurnSpeed") * newAgent.BaseTurn)
                {
                    endAngle = GetSettingFloat("TurnSpeed") * newAgent.BaseTurn;
                }
                if (crossProduct.z < 0)
                {
                    endAngle = -endAngle;
                }

                float endHeading = (heading.z + endAngle + 360) % 360;
                return new Vector3(0, 0, endHeading);
            }

            return heading;
        }

        public Pair<Double, Vector3> SearchGrid(WorldAgent newAgent, int newX, int newY, double currClosestDistance, Vector3 targetDirection, Vector3 simForward)
        {
            foreach (WorldAgent nearestNeighbor in _agentGrid[newX, newY])
            {
                double newDistance = newAgent.FindDistance(nearestNeighbor);
                if (newDistance < currClosestDistance && newDistance < GetSettingFloat("ViewRange"))
                {
                    Vector3 targDirection = nearestNeighbor.transform.position - newAgent.transform.position;
                    float angleToNearestNeighbor = Vector3.Angle(simForward, targDirection);
                    if (angleToNearestNeighbor < GetSettingFloat("ViewAngle") / 2)
                    {
                        currClosestDistance = newDistance;
                        targetDirection = nearestNeighbor.transform.position;
                    }
                }
            }

            return new Pair<Double, Vector3>(currClosestDistance, targetDirection);
        }

        public void ClearAll()
        {
            for (int i = 0; i < GridX; i++)
            {
                for (int j = 0; j < GridY; j++)
                {
                    foreach (WorldAgent currAgent in _agentGrid[i, j])
                    {
                        currAgent.End();
                    }
                }
            }
        }

        public float GetSettingFloat(string key)
        {
            return settings[key];
        }

        public void SetValue(string key, float value)
        {
            settings[key] = value;
        }
    }

    public class Pair<T1, T2>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }
        public Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }
}
