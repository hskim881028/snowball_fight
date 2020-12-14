using System;
using System.Diagnostics;

namespace SF.Common.Util {
    public class LogicTimer {
        public const float FPS = 60.0f;
        public const float FixedDelta = 1.0f / FPS;

        private double _accumulator;
        private long _lastElapsedTicks;

        private Stopwatch _stopwatch;
        private readonly Action _action;

        public float LerpAlpha => (float) _accumulator / FixedDelta;

        public LogicTimer(Action action) {
            _stopwatch = new Stopwatch();
            _action = action;
        }

        public void Start() {
            _lastElapsedTicks = 0;
            _accumulator = 0.0;
            _stopwatch.Restart();
        }

        public void Stop() {
            _stopwatch.Stop();
        }

        public void Update() {
            long elapsedTicks = _stopwatch.ElapsedTicks; // 현재 인스턴스가 측정한 총 경과 시간(타이머 틱 수)을 가져옵니다.
            // (타이머 틱 수 - 마지막시간) / 초당 틱 수 
            _accumulator += (double) (elapsedTicks - _lastElapsedTicks) / Stopwatch.Frequency;
            _lastElapsedTicks = elapsedTicks;
            
            while (_accumulator >= FixedDelta)
            {
                _action();
                _accumulator -= FixedDelta;
            }
        }
        
    }
}
