using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab4
{
    [System.Serializable]
    public class GlassTube : SimulationMixableBehavior
    {
        public GlassTube()
        {
            this.itemName = "Glass Tube";
            this.icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab4/Equipments/GlassTube");
        }

        public GlassTube(SimulationMixableBehavior other) : base(other)
        {

        }
    }
}
