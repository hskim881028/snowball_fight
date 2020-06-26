using UnityEngine;

namespace hskim {
    public class JoystickView : MonoBehaviour {
        [SerializeField] RectTransform mInsideCircle = null;
        [SerializeField] RectTransform mOutsideCircle = null;

        public Vector2 OnDrag(Vector2 position) {
            mInsideCircle.localPosition = InputToPosition(position, mOutsideCircle);
            return GetJoysticValue(mInsideCircle, mOutsideCircle);
        }
        
        public void OnPointerDown(Vector2 position) {
            mOutsideCircle.position = position;
        }
        
        public void OnPointerUp() {
            mInsideCircle.localPosition = Vector2.zero;
        }
        
        Vector3 InputToPosition(Vector2 delta, RectTransform outsideCircle) 
        {
            Vector3 position = outsideCircle.position;
            float radius = outsideCircle.rect.width * 0.5f;
            Vector2 vec = new Vector2(delta.x - position.x, delta.y - position.y);
            return Vector2.ClampMagnitude(vec, radius);
        }

        Vector3 GetJoysticValue(RectTransform insideCircle, RectTransform outsideCircle) {
            float radius = outsideCircle.rect.width * 0.5f;
            float sqrMagnitude = (outsideCircle.position - insideCircle.position).sqrMagnitude / (radius * radius);
            return insideCircle.localPosition.normalized * sqrMagnitude;
        }
    }
}