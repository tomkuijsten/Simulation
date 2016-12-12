﻿using Simulation.Contracts;
using Simulation.World.Physics;
using Simulation.Interface.Models;
using System.Collections.Generic;
using System.Linq;

namespace Simulation.World.Specimen.Attributes.Sensors
{
    internal abstract class RayCastingSensor : SpecimenSensorBase, IRotateable
    {
        public RayCastingSensor(ISpecimenInternal specimen, double initialDirectionRadians) : base(specimen)
        {
            RotationRadians = initialDirectionRadians;
        }

        public abstract double RangePixels { get; }

        public double RotationRadians { get; set; }

        public sealed override void Update()
        {
            Line ray = new Line()
            {
                Start = Specimen.Position,
                End = Specimen.Position.Translate(RotationRadians, RangePixels)
            };

            List<IWorldObject> rayCastResults = new List<IWorldObject>();

            foreach (var obj in SimulationWorld.Objects.Where(o => o != Specimen))
            {
                var points = obj.PolygonPoints.ToArray();
                for (int i = 0; i < points.Length; i++)
                {
                    Line edge;
                    if (i == 0)
                    {
                        edge = new Line()
                        {
                            Start = points.Last(),
                            End = points[i]
                        };
                    }
                    else
                    {
                        edge = new Line()
                        {
                            Start = points[i - 1],
                            End = points[i]
                        };
                    }

                    if (CollisionDetector.AreColliding(ray, edge))
                    {
                        rayCastResults.Add(obj);
                        break;
                    }
                }
            }

            HandleRayCastResult(rayCastResults);
        }

        protected IWorldObject GetNearestCollision(IEnumerable<IWorldObject> collisions)
        {
            var nearest = collisions.First();
            var nearestDistance = (Specimen.Position - nearest.Position).Length();
            foreach (var obj in collisions.Skip(1))
            {
                var distance = (Specimen.Position - obj.Position).Length();
                if (distance < nearestDistance)
                {
                    nearest = obj;
                    nearestDistance = distance;
                }
            }

            return nearest;
        }

        protected abstract void HandleRayCastResult(IEnumerable<IWorldObject> collisions);
    }
}
