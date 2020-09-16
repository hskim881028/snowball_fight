using UnityEngine;
using UnityEngine.Serialization;

namespace hskim {
    public class JoystickView : MonoBehaviour {
        [FormerlySerializedAs("mInsideCircle")] [SerializeField]
        private RectTransform insideCircle;

        [FormerlySerializedAs("mOutsideCircle")] [SerializeField]
        private RectTransform outsideCircle;

        public Vector2 OnDrag(Vector2 position) {
            insideCircle.localPosition = InputToPosition(position, outsideCircle);
            return GetJoysticValue(insideCircle, outsideCircle);
        }

        public void OnPointerDown(Vector2 position) {
            outsideCircle.position = position;
        }

        public void OnPointerUp() {
            insideCircle.localPosition = Vector2.zero;
        }

        private Vector3 InputToPosition(Vector2 delta, RectTransform outsideCircle) {
            var position = outsideCircle.position;
            var radius = outsideCircle.rect.width * 0.5f;
            var vec = new Vector2(delta.x - position.x, delta.y - position.y);
            return Vector2.ClampMagnitude(vec, radius);
        }

        private Vector3 GetJoysticValue(RectTransform insideCircle, RectTransform outsideCircle) {
            var radius = outsideCircle.rect.width * 0.5f;
            var sqrMagnitude = (outsideCircle.position - insideCircle.position).sqrMagnitude / (radius * radius);
            return insideCircle.localPosition.normalized * sqrMagnitude;
        }
    }
}