using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _transform;

        public void SetPosition(Vector3 position)
        {
            bool visible = position.z > 0;

            gameObject.SetActive(visible);

            if (visible)
                _transform.position = position;
        }

        public void SetImageFillAmount(float amount)
        {
            _image.fillAmount = amount;
        }
    }
}
