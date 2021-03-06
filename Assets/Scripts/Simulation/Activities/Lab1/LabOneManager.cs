﻿using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    public class LabOneManager : SimulationActivityBehavior
    {
        public enum LabPart
        {
            PartA,
            PartB,
            PartC,
            PartD,
            PartE
        }

        public static LabPart ActivePart { get; private set; }

        public override string ID
        {
            get
            {
                return "lab1";
            }
        }

        #region Overriden functions
        public override void Setup()
        {
            ChangePart(LabPart.PartA);
        }

        public override void OnPause()
        {
            base.OnPause();

            SimulationManager.instance.pauseMenuObject.AddButton("Change Activity Part", () =>
            {
                Action<LabPart> a = ChangePart;
                var buttons = new List<PartSelectorPanel.SelectorButton>();

                foreach (var item in Enum.GetValues(typeof(LabPart)).Cast<LabPart>())
                {
                    if (ActivePart == item)
                    {
                        continue;
                    }
                    else
                    {
                        buttons.Add(new PartSelectorPanel.SelectorButton
                        {
                            action = () => a.Invoke(item),
                            title = StringUtils.SplitStringBySpace(item.ToString())
                        });
                    }
                }

                PartSelectorPanel.Instance.ShowSelector(buttons.ToArray());
            });
        }

        #endregion

        public void ChangePart(LabPart part)
        {
            this.Reset();

            switch (part)
            {
                case LabPart.PartA:
                    BuildA();
                    break;
                case LabPart.PartB:
                    BuildB();
                    break;
                case LabPart.PartC:
                    BuildC();
                    break;
                case LabPart.PartD:
                    BuildD();
                    break;
                case LabPart.PartE:
                    BuildE();
                    break;
            }

            this.Publish();

            ActivePart = part;
        }

        private void BuildE()
        {
            /* EQUIPMENTS */
            var beaker = new Beaker(400);
            SimulationManager.instance.AddEquipmentItem(beaker);
            SimulationMixtureManager.instance.RegisterMixable(beaker);

            var beaker150 = new Beaker(150);
            SimulationManager.instance.AddEquipmentItem(beaker150);
            SimulationMixtureManager.instance.RegisterMixable(beaker150);

            /* MATERIALS */
            var water = new Water();
            SimulationManager.instance.AddMaterial(water);

            var dye = new BlueDye();
            SimulationManager.instance.AddMaterial(dye);

            var naph = new Naphthalene();
            SimulationManager.instance.AddMaterial(naph);

            var ice = new Ice();
            SimulationManager.instance.AddMaterial(ice);
        }

        private void BuildD()
        {
            /* EQUIPMENTS */
            var beaker = new Beaker(150);
            SimulationManager.instance.AddEquipmentItem(beaker);
            SimulationMixtureManager.instance.RegisterMixable(beaker);

            var stand = new IronStand();
            SimulationManager.instance.AddEquipmentItem(stand);

            var filter = new Filter();
            SimulationManager.instance.AddEquipmentItem(filter);

            var setup = new BoilSetup();
            SimulationManager.instance.AddEquipmentItem(setup);

            /* MATERIALS */
            var water = new Water();
            SimulationManager.instance.AddMaterial(water);

            var sugar = new Sugar();
            SimulationManager.instance.AddMaterial(sugar);

            var charcoal = new Charcoal();
            SimulationManager.instance.AddMaterial(charcoal);
        }

        private void BuildC()
        {
            /* EQUIPMENTS */
            var beaker = new Beaker(150);
            SimulationManager.instance.AddEquipmentItem(beaker);
            SimulationMixtureManager.instance.RegisterMixable(beaker);

            var setup = new DistillationSetup();
            SimulationManager.instance.AddEquipmentItem(setup);

            /* MATERIALS */
            var water = new Water();
            SimulationManager.instance.AddMaterial(water);

            var dye = new BlueDye();
            SimulationManager.instance.AddMaterial(dye);

            var ammonium = new AmmoniumHydroxide();
            SimulationManager.instance.AddMaterial(ammonium);

            var phenolph = new Phenolphthalein();
            SimulationManager.instance.AddMaterial(phenolph);
        }

        private void BuildB()
        {
            /* EQUIPMENTS */
            var beaker = new Beaker(150);
            SimulationManager.instance.AddEquipmentItem(beaker);
            SimulationMixtureManager.instance.RegisterMixable(beaker);

            var filter = new Filter();
            SimulationManager.instance.AddEquipmentItem(filter);

            var funnel = new Funnel();
            SimulationManager.instance.AddEquipmentItem(funnel);

            var funnelSupport = new FunnelSupport();
            SimulationManager.instance.AddEquipmentItem(funnelSupport);

            var stand = new IronStand();
            SimulationManager.instance.AddEquipmentItem(stand);

            /* MATERIALS */
            var nacl = new SodiumChloride();
            SimulationManager.instance.AddMaterial(nacl);

            var chalk = new Chalk();
            SimulationManager.instance.AddMaterial(chalk);

            var water = new Water();
            SimulationManager.instance.AddMaterial(water);
        }

        private void BuildA()
        {
            /* EQUIPMENTS */
            var beaker = new Beaker(150);
            SimulationManager.instance.AddEquipmentItem(beaker);
            SimulationMixtureManager.instance.RegisterMixable(beaker);

            /* MATERIALS */
            var chalk = new Chalk();
            SimulationManager.instance.AddMaterial(chalk);

            var sand = new Sand();
            SimulationManager.instance.AddMaterial(sand);

            var water = new Water();
            SimulationManager.instance.AddMaterial(water);

            /* REGISTRATION */
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(beaker, water);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(beaker, sand);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(beaker, chalk);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(beaker, beaker);
        }
    }
}
