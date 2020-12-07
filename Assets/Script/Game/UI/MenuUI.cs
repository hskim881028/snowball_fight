using LibChaiiLatte;
using UnityEngine;
using UnityEngine.UI;

namespace SF.UI {
    public class MenuUI : MonoBehaviour {
        [SerializeField] private Text _disconnectedInfo;
        [SerializeField] private InputField _ipInputField;

        public void Init(Client client) {
            
        }
        
        public void OnClickHost() {
        }

        public void OnClickJoin() {
            
        }

        private void OnDisconnected(DisconnectInfo info) {
            _disconnectedInfo.text = info.Reason.ToString();
        }
    }
}
