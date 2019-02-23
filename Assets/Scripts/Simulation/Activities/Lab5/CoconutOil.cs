using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab5
{
    public class CoconutOil : SimulationMixableBehavior
    {
        public CoconutOil()
        {
            itemName = "Coconut Oil";
            icon = Resources.Load<Sprite>("Simulation/Lab5/Materials/CocoOil");
        }

        public CoconutOil(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
