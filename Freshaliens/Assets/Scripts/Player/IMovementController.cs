using UnityEngine;

namespace Freshaliens.Player.Components
{   
    public interface IMovementController
    {
        public float KnockbackTime();
        public void Knockback(Vector3 ObstaclePosition){}
        
    }

}
