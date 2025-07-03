using Heavenage.Scripts.AbilitySystem.Runtime.Components;
using UnityEngine;

namespace Heavenage.Scripts.Characters
{
    public class PlayerCharacter : MonoBehaviour
    {
        private AbilitySystemComponent _asc;

        private void Start()
        {
            _asc = GetComponent<AbilitySystemComponent>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) _asc.TryActivateAbility(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) _asc.TryActivateAbility(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) _asc.TryActivateAbility(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) _asc.TryActivateAbility(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) _asc.TryActivateAbility(4);
        }
    }
}
