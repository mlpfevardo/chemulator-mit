using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab5
{
    public class Rack : SimulationMixableBehavior
    {
        private bool hasTestTube = false;
        public Rack()
        {
            this.itemName = "Test Tube Rack";
            this.icon = Resources.Load<Sprite>("Simulation/Lab5/Equipments/Rack");
            this.MixButtonTitle = "Stir";
            this.Scale = 40f;
            AutoMix = true;
        }

        public Rack(Rack otherRack) : base(otherRack)
        {
            //hasTestTube = otherRack.hasTestTube;
        }

        public override bool DoMix(List<SimulationMixableBehavior> otherMixables, DropZoneObjectHandler dropZoneObject, DraggableObjectBehavior draggedObject = null)
        {
            if (draggedObject != null)
            {
                if (draggedObject.MixtureItem.GetType() == typeof(MicroTestTube))
                {
                    // mixing some test tubes
                    if (!hasTestTube)
                    {
                        hasTestTube = true;
                        this.icon = Resources.Load<Sprite>("Simulation/Lab5/Equipments/Rack-Tube");
                        dropZoneObject.SetIcon(this.icon);
                        draggedObject.SetRemoveOnEnd();

                        return true;
                    }
                    else
                    {
                        ShowNoDuplicateError(draggedObject.MixtureItem);
                    }
                }
                else if (hasTestTube)
                {
                    // do performance stuffs
                    if (draggedObject.MixtureItem.GetType() == typeof(Water) || draggedObject.MixtureItem.GetType() == typeof(Kerosene))
                    {
                        if (otherMixables.Find(m => m.itemName == draggedObject.MixtureItem.itemName) == null)
                        {
                            // first time water
                            draggedObject.SetRemoveOnEnd();
                            return true;
                        }
                        else
                        {
                            ShowNoDuplicateError(draggedObject.MixtureItem);
                        }
                    }
                    else if (draggedObject.MixtureItem.GetType() == typeof(SodiumChloride) 
                        || draggedObject.MixtureItem.GetType() == typeof(Naphthalene) 
                        || draggedObject.MixtureItem.GetType() == typeof(SodiumChloride)
                        || draggedObject.MixtureItem.GetType() == typeof(EthylAlcohol)
                        || draggedObject.MixtureItem.GetType() == typeof(CoconutOil))
                    {
                        if (otherMixables.Count(m => m.itemName == draggedObject.MixtureItem.itemName) < 2)
                        {
                            // first time nacl
                            draggedObject.SetRemoveOnEnd();
                            return true;
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Max Amount", "Only a maximum of two of this material can be added");
                        }
                    }
                }
                else
                {
                    ModalPanel.Instance.ShowModalOK("Test Tube Required", "Need a test tube to put this material. Please add a test tube first.");
                }
            }
            else
            {
                if (LabFiveManager.instance.ActivePart == LabFiveManager.LabPart.PartA)
                {
                    if (otherMixables.Count(m => m.GetType() == typeof(SodiumChloride)) == 2 
                        && otherMixables.Count(m => m.GetType() == typeof(Naphthalene)) == 2 
                        && otherMixables.Find(m => m.GetType() == typeof(Water)) != null
                        && otherMixables.Find(m => m.GetType() == typeof(Kerosene)) != null)
                    {
                        ModalPanel.Instance.ShowModalOK("Result", "Sodium chloride dissolved in water but not in kerosene. Naphthalene dissolved in kerosene but not in water. Proceeding to Part B", () =>
                        {
                            LabFiveManager.instance.MoveToPartB();
                        });
                    }
                    else
                    {
                        ModalPanel.Instance.ShowModalOK("Incomplete Materials", "Not enough materials to stir. Please add all materials required.");
                    }
                }
                else
                {
                    if (otherMixables.Count(m => m.GetType() == typeof(EthylAlcohol)) == 2
                        && otherMixables.Count(m => m.GetType() == typeof(CoconutOil)) == 2
                        && otherMixables.Find(m => m.GetType() == typeof(Water)) != null
                        && otherMixables.Find(m => m.GetType() == typeof(Kerosene)) != null)
                    {
                        ModalPanel.Instance.ShowModalOK("Result", "Ethyl Alcohol dissolved in water but not in kerosene. Coconut Oil dissolved in kerosene but not in water.", () =>
                        {
                            LabFiveManager.instance.Finish();
                        });
                    }
                    else
                    {
                        ModalPanel.Instance.ShowModalOK("Incomplete Materials", "Not enough materials to stir. Please add all materials required.");
                    }
                }
            }

            return false;
        }

        private void ShowNoDuplicateError(SimulationMixableBehavior material)
        {
            ModalPanel.Instance.ShowModalOK("Add Material", material.itemName + " is already present. No need to add this again.");
        }
    }
}
