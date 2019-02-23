using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab4
{
    public class Rack : SimulationMixableBehavior
    {
        bool hasTestTube = false;

        public Rack()
        {
            this.itemName = "Rack";
            this.icon = Resources.Load<Sprite>("Simulation/Lab4/Equipments/Rack");
            AutoMix = true;
        }

        public Rack(SimulationMixableBehavior other) : base(other)
        {

        }

        public override bool DoMix(List<SimulationMixableBehavior> otherMixables, DropZoneObjectHandler dropZoneObject, DraggableObjectBehavior draggedObject = null)
        {
            if (draggedObject != null)
            {
                if (draggedObject.MixtureItem.GetType() == typeof(TestTube))
                {
                    // mixing some test tubes
                    if (!hasTestTube)
                    {
                        hasTestTube = true;
                        this.icon = Resources.Load<Sprite>("Simulation/Lab4/Equipments/RackTube");
                        dropZoneObject.SetIcon(this.icon);
                        draggedObject.SetRemoveOnEnd();

                        return true;
                    }
                    else
                    {
                        ModalPanel.Instance.ShowModalOK("Has Test Tube", "This rack already have test tubes");
                    }
                }
                else if (hasTestTube)
                {
                    if (draggedObject.MixtureItem.GetType() == typeof(AmmoniumHydroxide) || draggedObject.MixtureItem.GetType() == typeof(HydrochloricAcid))
                    {
                        if (otherMixables.Find(m => m.GetType() == typeof(HydrochloricAcid) && m.GetType() == typeof(AmmoniumHydroxide)) == null)
                        {
                            draggedObject.SetRemoveOnEnd();
                            return true;
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Single Sample", "You can only put one sample for this test tube");
                        }
                    }
                }
                else
                {
                    ModalPanel.Instance.ShowModalOK("No Test Tube", "This item cannot be added. Try adding a test tube first");
                }
            }

            return false;
        }
    }
}
