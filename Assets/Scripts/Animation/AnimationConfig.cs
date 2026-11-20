using UnityEngine;

namespace Assets.Scripts.Animation
{
    [CreateAssetMenu(fileName = "AnimationConfig", menuName = "Config")]
    public class AnimationConfig : ScriptableObject
    {
        [SerializeField] private AnimationClip _clip;
        [SerializeField] private string _speedMultiplier;
        [SerializeField] private float _keyPoint;
        [SerializeField] private bool _isKey;

        public int SpeedMultiplierHash { get; private set; }
        public float Length => _clip.length;
        public float KeyPoint => _keyPoint;
        public bool IsKey => _isKey;

        public void Initialize()
        {
            SpeedMultiplierHash = Animator.StringToHash(_speedMultiplier);
        }
    }
}
