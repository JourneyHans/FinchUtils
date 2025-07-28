using System;
using System.Collections;
using System.Collections.Generic;

namespace FinchUtils.Collections
{
    public class PriorityQueue<T> : ICollection<T> where T : IComparable<T> {

        private readonly LinkedList<T> _list = new();
        public T FirstValue => _list.First.Value;

        public void Enqueue(T item) {
            if (item == null) {
                throw new ArgumentNullException(nameof(item));
            }

            var current = _list.First;
            while (current != null) {
                if (item.CompareTo(current.Value) > 0) {
                    _list.AddBefore(current, item);
                    return;
                }

                current = current.Next;
            }

            _list.AddLast(item);
        }

        public T Dequeue() {
            if (IsEmpty) {
                return default;
            }

            var current = FirstValue;
            _list.RemoveFirst();
            return current;
        }

        public bool IsEmpty => Count == 0;

        #region ICollection<T>

        public bool IsReadOnly => false;

        public int Count => _list.Count;

        public void Add(T item) {
            Enqueue(item);
        }

        public bool Remove(T item) {
            return _list.Remove(item);
        }

        public void Clear() {
            _list.Clear();
        }

        public bool Contains(T item) {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator() {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _list.GetEnumerator();
        }

        #endregion
    }
}