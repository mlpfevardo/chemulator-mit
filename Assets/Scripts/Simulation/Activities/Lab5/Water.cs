using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab5
{
    public class Water : SimulationMixableBehavior
    {
        public Water()
        {
            itemName = "Distilled Water";
            icon = Resources.Load<Sprite>("Simulation/Lab5/Materials/Water");
        }

        public Water(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
