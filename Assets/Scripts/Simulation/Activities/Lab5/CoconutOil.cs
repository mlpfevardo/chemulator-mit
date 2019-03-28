using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab5
{
    [System.Serializable]
    public class CoconutOil : SimulationMixableBehavior
    {
        public CoconutOil()
        {
            itemName = "Coconut Oil";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab5/Materials/CocoOil");
        }

        public CoconutOil(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
