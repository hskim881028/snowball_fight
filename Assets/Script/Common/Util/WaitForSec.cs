using UnityEngine;

namespace SF.Common.Util {
    public class WaitForSec : CustomYieldInstruction {
        private readonly float _time;

        public WaitForSec(float second) {
            _time = Time.time + second;
        }

        public override bool keepWaiting => _time > Time.time;
    }
}