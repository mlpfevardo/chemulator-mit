using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab5
{
    [System.Serializable]
    public class Naphthalene : SimulationMixableBehavior
    {
        public Naphthalene()
        {
            itemName = "0.15g Naphthalene";
            icon = Resources.Load<Sprite>("Simulation/Lab5/Materials/Napthalene");
        }

        public Naphthalene(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
