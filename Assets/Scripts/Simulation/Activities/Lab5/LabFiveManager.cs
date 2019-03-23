using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab5
{
    [System.Serializable]
    public class LabFiveManager : SimulationActivityBehavior
    {
        private Rack rack;
        private MicroTestTube tube;
        private Water water;
        private Kerosene kerosene;

        public enum LabPart
        {
            PartA,
            PartB
        }

        public static LabFiveManager instance = null;

        public LabPart ActivePart { get; private set; }

        public override string ID
        {
            get
            {
                return "lab5";
            }
        }

        public override void Setup()
        {
            Debug.Log("Running lab 5 setup");
            BuildList();

            instance = this;
            ActivePart = LabPart.PartA;
        }

        public void MoveToPartB()
        {
            ActivePart = LabPart.PartB;
            BuildPartB();
        }
        public override void Finish()
        {
            TableDropZone.Instance.RemoveObjects();
            SimulationManager.instance.ClearList(true, true);
            ModalPanel.Instance.ShowModalOK("Activity Finished", "You have successfully finished this activity.");
        }


        private void BuildList()
        {
            this.Reset();

            /* EQUIPMENTS */
            tube = new MicroTestTube();
            SimulationMixtureManager.instance.RegisterMixable(tube);
            SimulationManager.instance.AddEquipmentItem(tube);

            rack = new Rack();
            SimulationMixtureManager.instance.RegisterMixable(rack);
            SimulationManager.instance.AddEquipmentItem(rack);

            /* MATERIALS */
            var nacl = new SodiumChloride();
            SimulationManager.instance.AddMaterial(nacl);

            var naph = new Naphthalene();
            SimulationManager.instance.AddMaterial(naph);

            water = new Water();
            SimulationManager.instance.AddMaterial(water);

            kerosene = new Kerosene();
            SimulationManager.instance.AddMaterial(kerosene);

            /* REGISTRATION */
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(rack, tube);

            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(rack, nacl);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(rack, naph);

            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(rack, water);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(rack, kerosene);

            this.Publish();
        }

        private void BuildPartB()
        {
            this.Reset();

            /* EQUIPMENTS */
            SimulationMixtureManager.instance.RegisterMixable(tube);
            SimulationManager.instance.AddEquipmentItem(tube);

            SimulationMixtureManager.instance.RegisterMixable(rack);
            SimulationManager.instance.AddEquipmentItem(rack);

            var ethyl = new EthylAlcohol();
            SimulationManager.instance.AddMaterial(ethyl);

            var coco = new CoconutOil();
            SimulationManager.instance.AddMaterial(coco);

            SimulationManager.instance.AddMaterial(water);
            SimulationManager.instance.AddMaterial(kerosene);

            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(rack, tube);

            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(rack, ethyl);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(rack, coco);

            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(rack, water);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(rack, kerosene);

            this.Publish();
        }
    }
}
