﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    public class Funnel : SimulationMixableBehavior
    {
        public Funnel()
        {
            itemName = "Funnel";
            icon = Resources.Load<Sprite>("Simulation/Lab1/Equipments/Funnel");
        }

        public Funnel(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
