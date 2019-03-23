using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    [System.Serializable]
    public class Naphthalene : SimulationMixableBehavior
    {
        public Naphthalene()
        {
            itemName = "Powdered Naphthalene";
            icon = Resources.Load<Sprite>("Simulation/Lab1/Materials/Naphthalene");
        }

        public Naphthalene(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
