using System;
using UnityEngine;

namespace Assets.Scripts.Animation
{
    public class AnimationEventListener : MonoBehaviour
    {
        public event Action EventPerformed;

        public void OnEventPerformed()
        {
            EventPerformed?.Invoke();
        }
    }
}
