﻿using Simulation.World.Specimens.Attributes;
using System;
using System.Collections.Generic;

namespace Simulation.World.Specimens
{
    internal interface ISpecimenInternal : IWorldObject
    {
        void ApplyForce(double force);
        void ApplyAngularForce(double force);

        double Weight { get; }
        double MaxSpeed { get; }
        double MaxAngularSpeed { get; }

        double ActualSpeed { get; }
        double ActualAngularSpeed { get; }

        IEnumerable<ISpecimenAttributeInternal> Attributes { get; }

        void Update(TimeSpan lastUpdateDuration);
    }
}