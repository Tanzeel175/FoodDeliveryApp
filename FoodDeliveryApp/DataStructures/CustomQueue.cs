using System;
using System.Collections;
using System.Collections.Generic;

namespace FoodDeliveryApp.DataStructures
{
    /// <summary>
    /// A generic first-in, first-out (FIFO) queue data structure built on top of
    /// <see cref="CustomLinkedList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements stored in the queue.</typeparam>
    public class CustomQueue<T> : IEnumerable<T>
    {
        /// <summary>
        /// The internal linked list used to store queue elements.
        /// </summary>
        private CustomLinkedList<T> _list = new CustomLinkedList<T>();

        /// <summary>
        /// Gets the number of elements contained in the queue.
        /// </summary>
        public int Count => _list.Count;

        /// <summary>
        /// Gets a value indicating whether the queue is empty.
        /// </summary>
        public bool IsEmpty => _list.Count == 0;

        /// <summary>
        /// Adds an item to the back of the queue.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        public void Enqueue(T item)
        {
            _list.AddLast(item);
        }

        /// <summary>
        /// Removes and returns the item at the front of the queue.
        /// </summary>
        /// <returns>The item that was at the front of the queue.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the queue is empty.
        /// </exception>
        public T Dequeue()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Cannot dequeue from an empty queue.");

            T item = _list.Get(0);
            _list.RemoveAt(0);
            return item;
        }

        /// <summary>
        /// Returns the item at the front of the queue without removing it.
        /// </summary>
        /// <returns>The item at the front of the queue.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the queue is empty.
        /// </exception>
        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Cannot peek into an empty queue.");

            return _list.Get(0);
        }

        /// <summary>
        /// Removes all items from the queue.
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }

        /// <summary>
        /// Creates a snapshot of the queue as a <see cref="List{T}"/>,
        /// suitable for data binding in WPF.
        /// </summary>
        /// <returns>A new <see cref="List{T}"/> containing all queued elements in FIFO order.</returns>
        public List<T> ToList()
        {
            return _list.ToList();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the queue from front to back.
        /// </summary>
        /// <returns>An enumerator for the queue.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the queue (non-generic).
        /// </summary>
        /// <returns>An enumerator for the queue.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
