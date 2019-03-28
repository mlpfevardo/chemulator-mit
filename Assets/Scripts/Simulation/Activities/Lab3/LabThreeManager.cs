using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab3
{
    [System.Serializable]
    public class LabThreeManager : SimulationActivityBehavior
    {
        public override string ID
        {
            get
            {
                return "lab3";
            }
        }

        public override void Setup()
        {
            Build();
        }

        private void Build()
        {
            this.Reset();

            /* EQUIPMENTS */
            var beaker = new Beaker(400);
            SimulationManager.instance.AddEquipmentItem(beaker);

            var beaker150 = new Beaker(150);
            SimulationManager.instance.AddEquipmentItem(beaker150);
            SimulationMixtureManager.instance.RegisterMixable(beaker150);

            var cylinder = new Cylinder();
            SimulationManager.instance.AddEquipmentItem(cylinder);
            SimulationMixtureManager.instance.RegisterMixable(cylinder);

            var balance = new ElectronicBalance();
            SimulationManager.instance.AddEquipmentItem(balance);

            var spatula = new Spatula();
            SimulationManager.instance.AddEquipmentItem(spatula);

            var glass = new WatchGlass();
            SimulationManager.instance.AddEquipmentItem(glass);

            /* MATERIALS */
            var cacl = new CalciumChloride();
            SimulationManager.instance.AddMaterial(cacl);

            var naco = new SodiumCarbonate();
            SimulationManager.instance.AddMaterial(naco);

            /* REGISTRATION */
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(cylinder, cacl);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(cylinder, naco);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(beaker, cylinder);

            this.Publish();
        }
    }
}
