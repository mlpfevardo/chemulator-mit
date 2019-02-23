using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum ObjectGlowState
{
    Default,
    Valid,
    Invalid
}

public interface IGlowingObject
{
    ObjectGlowState CurrentGlowState { get; set; }
    void ChangeGlowColor(ObjectGlowState state = ObjectGlowState.Default);
}