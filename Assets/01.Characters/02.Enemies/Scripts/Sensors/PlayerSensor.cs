using LubyAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabLuby.Sensors
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerSensor : MonoBehaviour
    {
        public delegate void PlayerEnterEvent(Transform player);
        public delegate void PlayerExitEvent(Vector3 lastKnownPosition);
        public event PlayerEnterEvent OnPlayerEnter;
        public event PlayerExitEvent OnPlayerExit;

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out UnitController player))
            {
                OnPlayerEnter?.Invoke(player.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            OnPlayerExit?.Invoke(other.transform.position);  
        }
    }

}
