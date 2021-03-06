﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    public class FunnelSupport : SimulationMixableBehavior
    {
        public FunnelSupport()
        {
            itemName = "Funnel Support";
            icon = Resources.Load<Sprite>("Simulation/Lab1/Equipments/FunnelSupport");
        }

        public FunnelSupport(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
