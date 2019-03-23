using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab4
{
    [System.Serializable]
    public class LabFourManager : SimulationActivityBehavior
    {
        public override string ID
        {
            get
            {
                return "lab4";
            }
        }

        public override void Setup()
        {
            BuildMaterials();
        }

        private void BuildMaterials()
        {
            this.Reset();

            /* EQUIPMENTS */
            var tube = new TestTube();
            SimulationManager.instance.AddEquipmentItem(tube);

            var rack = new Rack();
            SimulationMixtureManager.instance.RegisterMixable(rack);
            SimulationManager.instance.AddEquipmentItem(rack);

            var glass = new GlassTube();
            SimulationManager.instance.AddEquipmentItem(glass);

            /* MATERIALS */
            var hcl = new HydrochloricAcid();
            SimulationManager.instance.AddMaterial(hcl);

            var nhoh = new AmmoniumHydroxide();
            SimulationManager.instance.AddMaterial(nhoh);

            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(rack, hcl);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(rack, nhoh);

            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(rack, tube);

            this.Publish();
        }
    }
}
