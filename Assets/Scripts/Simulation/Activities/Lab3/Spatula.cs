﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab3
{
    public class Spatula : SimulationMixableBehavior
    {
        public Spatula()
        {
            itemName = "Spatula";
            icon = Resources.Load<Sprite>("Simulation/Lab3/Equipments/Spatula");
        }

        public Spatula(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
