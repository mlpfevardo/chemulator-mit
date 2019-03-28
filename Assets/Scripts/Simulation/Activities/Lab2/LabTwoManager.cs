using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Simulation.Activities.Lab2
{
    [System.Serializable]
    public class LabTwoManager : SimulationActivityBehavior
    {
        Coins coins = new Coins();
        ElectronicBalance balance = new ElectronicBalance();
        Cylinder cylinder = new Cylinder();

        public enum LabPart
        {
            Part1A,
            Part1B,
            Part2
        }

        public LabPart ActivePart { get; private set; }

        public override string ID
        {
            get
            {
                return "lab2";
            }
        }

        #region Overriden Functions
        public override void Finish()
        {
        }

        public override void Setup()
        {
            ChangePart(LabPart.Part1A);
        }

        public override void OnPause(object sender, EventArgs e)
        {
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
            switch(part)
            {
                case LabPart.Part1A:
                    BuildA();
                    break;
                case LabPart.Part1B:
                    BuildB();
                    break;
                case LabPart.Part2:
                    BuildC();
                    break;
            }

            ActivePart = part;
        }

        private void BuildA()
        {
            this.Reset();

            /* EQUIPMENTS */
            SimulationManager.instance.AddEquipmentItem(balance);
            SimulationMixtureManager.instance.RegisterMixable(balance);

            var ruler = new Ruler();
            SimulationManager.instance.AddEquipmentItem(ruler);

            /* MATERIALS */
            SimulationManager.instance.AddMaterial(coins);
            SimulationMixtureManager.instance.RegisterMixable(coins);

            /* REGISTRATION */
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(balance, coins);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(coins, ruler);

            this.Publish();
        }

        private void BuildB()
        {
            this.Reset();

            /* EQUIPMENTS */
            SimulationManager.instance.AddEquipmentItem(cylinder);
            SimulationMixtureManager.instance.RegisterMixable(cylinder);

            SimulationManager.instance.AddEquipmentItem(balance);
            SimulationMixtureManager.instance.RegisterMixable(balance);

            /* MATERIALS */
            var water = new Water();
            water.itemName = "50mL Water";
            water.Volume = 50;
            SimulationManager.instance.AddMaterial(water);

            SimulationManager.instance.AddMaterial(coins);

            /* REGISTRATION */
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(cylinder, water);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(cylinder, coins);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(balance, coins);

            this.Publish();
        }

        private void BuildC()
        {
            this.Reset();

            var thermometer = new Thermometer();
            SimulationManager.instance.AddEquipmentItem(thermometer);

            SimulationManager.instance.AddEquipmentItem(cylinder);
            SimulationMixtureManager.instance.RegisterMixable(cylinder);

            SimulationManager.instance.AddEquipmentItem(balance);
            SimulationMixtureManager.instance.RegisterMixable(balance);

            var waterFifty = new Water();
            waterFifty.itemName = "50mL Water";
            waterFifty.Volume = 50;
            SimulationManager.instance.AddMaterial(waterFifty);

            var water = new Water();
            water.itemName = "100mL Water";
            water.Volume = 100;
            SimulationManager.instance.AddMaterial(water);

            /* REGISTRATION */
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(cylinder, water);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(cylinder, waterFifty);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(cylinder, thermometer);
            SimulationMixtureManager.instance.AddAllowableMixtureToMixable(balance, cylinder);

            this.Publish();
        }
    }
}

