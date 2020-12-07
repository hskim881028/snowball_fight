using LibChaiiLatte;
using UnityEngine;
using UnityEngine.UI;

namespace SF.UI {
    public class MenuUI : MonoBehaviour {
        [SerializeField] private Text _disconnectedInfo;
        [SerializeField] private InputField _ipInputField;

        private Server _server;
        private Client _client;

        public void Init(Server server, Client client) {
            _server = server;
            _client = client;
        }
        
        public void OnClickHost() {
            _server.StartServer();
            _client.Connect("localhost", OnDisconnected);
            this.gameObject.SetActive(false);
        }

        public void OnClickJoin() {
            _client.Connect(_ipInputField.text, OnDisconnected);
            this.gameObject.SetActive(false);
        }

        private void OnDisconnected(DisconnectInfo info) {
            _disconnectedInfo.text = info.Reason.ToString();
        }
    }
}
