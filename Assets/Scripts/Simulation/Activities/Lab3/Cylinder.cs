using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab3
{
    [System.Serializable]
    public class Cylinder : SimulationMixableBehavior
    {
        public Cylinder()
        {
            itemName = "50mL Graduated Cylinder";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab3/Equipments/Cylinder");
            AutoMix = true;
        }

        public Cylinder(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }

        public override bool DoMix(List<SimulationMixableBehavior> otherMixables, DropZoneObjectHandler dropZoneObject, DraggableObjectBehavior draggedObject = null, List<SimulationMixableBehavior> draggedMixables = null)
        {
            if (draggedObject != null)
            {
                if (draggedObject.MixtureItem.GetType() == typeof(CalciumChloride))
                {
                    if (otherMixables.Find(m => m.GetType() == typeof(SodiumCarbonate)) != null || otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) != null)
                    {
                        ModalPanel.Instance.ShowModalOK("Cylinder Full", "You cannot add more chemical into this cylinder.");
                    }
                    else
                    {
                        ImageAnimationManager.CreateAnimation(89, Parent.transform);
                        return true;
                    }
                }
                else if (draggedObject.MixtureItem.GetType() == typeof(SodiumCarbonate))
                {
                    if (otherMixables.Find(m => m.GetType() == typeof(CalciumChloride)) != null || otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) != null)
                    {
                        ModalPanel.Instance.ShowModalOK("Cylinder Full", "You cannot add more chemical into this cylinder.");
                    }
                    else
                    {
                        ImageAnimationManager.CreateAnimation(89, Parent.transform);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
