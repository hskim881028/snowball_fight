using System;
using System.Collections;
using System.Collections.Generic;

namespace SF.Common.Util {
    public class CircularBuffer<T> : IEnumerable<T> {
        private readonly T[] _elements;
        private readonly int _capacity;
        private int _start;
        private int _end;

        public int Count { get; private set; }
        public T this[int i] => _elements[(_start + i) % _capacity];
        public T First => _elements[_start];
        public T Last => _elements[(_start + Count - 1) % _capacity];
        public bool IsFull => _capacity == Count;

        public CircularBuffer(int capacity) {
            _elements = new T[capacity];
            _capacity = capacity;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator() {
            int counter = _start;
            while (counter != _end) {
                yield return _elements[counter];
                counter = (counter + 1) % _capacity;
            }
        }

        public void Add(T element) {
            if (_capacity == Count) {
                throw new ArgumentException();
            }

            _elements[_end] = element;
            _end = (_end + 1) % _capacity;
            Count++;
        }

        public void RemoveFromStart(int count) {
            if (count > _capacity || count > Count) {
                throw new ArgumentException();
            }

            _start = (_start + count) % _capacity;
            Count -= count;
        }

        public void Clear() {
            _start = 0;
            _end = 0;
            Count = 0;
        }
    }
}