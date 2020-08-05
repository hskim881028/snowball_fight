using UnityEngine;

namespace hskim {
    public class WaitForSec : CustomYieldInstruction
    {
        readonly float mTime;
        
        public override bool keepWaiting => mTime > Time.time;

        public WaitForSec(float second) {
            mTime = Time.time + second;
        }
    }
}