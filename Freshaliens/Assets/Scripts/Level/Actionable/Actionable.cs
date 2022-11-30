using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.Interaction
{
    public abstract class Actionable : MonoBehaviour
    {
        public virtual void OnAction() { }
    }
}