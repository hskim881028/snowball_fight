using LibChaiiLatte;
using SF.Service;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SF.UI {
    public class MenuUI : MonoBehaviour {
        [SerializeField] private Text _disconnectedInfo;
        [SerializeField] private InputField _ipInputField;
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _joinButton;

        public void Init(ServerSerivce serverSerivce, ClientService clientService) {
            _hostButton.OnClickAsObservable().Subscribe(_ => {
                serverSerivce.StartServer();
                clientService.Connect("localhost", OnDisconnected);
                gameObject.SetActive(false);
            });
            _joinButton.OnClickAsObservable().Subscribe(_ => {
                clientService.Connect(_ipInputField.text, OnDisconnected);
                gameObject.SetActive(false);
            });

            _ipInputField.text = NetUtils.GetLocalIp(LocalAddrType.IPv4);
        }

        private void OnDisconnected(DisconnectInfo info) {
            _disconnectedInfo.text = info.Reason.ToString();
        }
    }
}