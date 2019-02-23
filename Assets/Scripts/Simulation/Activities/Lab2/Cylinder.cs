using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab2
{
    public class Cylinder : SimulationMixableBehavior
    {
        public Cylinder()
        {
            itemName = "100mL Graduated Cylinder";
            icon = Resources.Load<Sprite>("Simulation/Lab2/Equipments/Cylinder");
            MixButtonTitle = "Measure";
            MinAllowableMix = 0;
            AutoMix = true;
        }

        public Cylinder(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }

        public override bool DoMix(List<SimulationMixableBehavior> otherMixables, DropZoneObjectHandler dropZoneObject, DraggableObjectBehavior draggedObject = null)
        {
            if (draggedObject != null)
            {
                if (draggedObject.MixtureItem.GetType() == typeof(Coins))
                {
                    if (otherMixables.Find(m => m.GetType() == typeof(Water)) != null)
                    {
                        if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                        {
                            // first time water
                            ImageAnimationManager.Instance.ShowAnimation(83);
                            draggedObject.SetRemoveOnEnd();
                            return true;
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Duplicate Item", "You can only put one kind of this item inside the cylinder");
                        }
                    }
                    else
                    {
                        ModalPanel.Instance.ShowModalOK("No Water", "Add water first before putting the coins");
                    }
                }
                else if (draggedObject.MixtureItem.GetType() == typeof(Water))
                {
                    if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                    {
                        // first time water
                        draggedObject.SetRemoveOnEnd();
                        return true;
                    }
                    else
                    {
                        ModalPanel.Instance.ShowModalOK("Duplicate Item", "You can only put one kind of this item inside the cylinder");
                    }
                }
                else if (draggedObject.MixtureItem.GetType() == typeof(Thermometer))
                {
                    var w = otherMixables.Find(m => m.GetType() == typeof(Water));
                    if (w != null)
                    {
                        if ((w as Water).Volume == 50)
                        {
                            ImageAnimationManager.Instance.ShowAnimation(87);
                        }
                        else
                        {
                            ImageAnimationManager.Instance.ShowAnimation(88);
                        }
                        ModalPanel.Instance.ShowModalOK("Temperature", "The temperature is 25.0C");
                    }
                    else
                    {
                        ModalPanel.Instance.ShowModalOK("Temperature", "Nothing to measure here");
                    }
                }
            }
            else
            {
                if (otherMixables.Find(m => m.GetType() == typeof(Water)) == null)
                {
                    ModalPanel.Instance.ShowModalOK("Current Volume", "There is no water");
                }
                else if (otherMixables.Find(m => m.GetType() == typeof(Coins)) == null)
                {
                    ModalPanel.Instance.ShowModalOK("Current Volume", "The water is currently at 50 mL");
                }
                else
                {
                    ModalPanel.Instance.ShowModalOK("Current Volume", "The water is currently at 59 mL");
                }
            }

            return false;
        }
    }
}
