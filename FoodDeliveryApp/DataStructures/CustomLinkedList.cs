using System;
using System.Collections;
using System.Collections.Generic;

namespace FoodDeliveryApp.DataStructures
{
    /// <summary>
    /// A generic singly-linked list data structure that supports standard list operations
    /// and implements <see cref="IEnumerable{T}"/> for foreach iteration.
    /// </summary>
    /// <typeparam name="T">The type of elements stored in the linked list.</typeparam>
    public class CustomLinkedList<T> : IEnumerable<T>
    {
        /// <summary>
        /// Represents a single node in the singly-linked list.
        /// </summary>
        /// <typeparam name="TNode">The type of data stored in the node.</typeparam>
        private class Node<TNode>
        {
            /// <summary>
            /// Gets or sets the data stored in this node.
            /// </summary>
            public TNode Data { get; set; }

            /// <summary>
            /// Gets or sets the reference to the next node in the list.
            /// </summary>
            public Node<TNode> Next { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Node{TNode}"/> class.
            /// </summary>
            /// <param name="data">The data to store in the node.</param>
            public Node(TNode data)
            {
                Data = data;
                Next = null;
            }
        }

        private Node<T> _head;

        /// <summary>
        /// Gets the head element of the linked list, or default if the list is empty.
        /// </summary>
        public T Head
        {
            get { return _head != null ? _head.Data : default(T); }
        }

        /// <summary>
        /// Gets the number of elements contained in the linked list.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Initializes a new, empty instance of the <see cref="CustomLinkedList{T}"/> class.
        /// </summary>
        public CustomLinkedList()
        {
            _head = null;
            Count = 0;
        }

        /// <summary>
        /// Appends an item to the end of the linked list.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddLast(T item)
        {
            var newNode = new Node<T>(item);

            if (_head == null)
            {
                _head = newNode;
            }
            else
            {
                var current = _head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }

            Count++;
        }

        /// <summary>
        /// Removes the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="index"/> is less than 0 or greater than or equal to <see cref="Count"/>.
        /// </exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

            if (index == 0)
            {
                _head = _head.Next;
            }
            else
            {
                var current = _head;
                for (int i = 0; i < index - 1; i++)
                {
                    current = current.Next;
                }
                current.Next = current.Next.Next;
            }

            Count--;
        }

        /// <summary>
        /// Removes the first occurrence of the specified item from the linked list.
        /// Uses <see cref="object.Equals(object)"/> for comparison.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns><c>true</c> if the item was found and removed; otherwise, <c>false</c>.</returns>
        public bool Remove(T item)
        {
            if (_head == null)
                return false;

            // Check if head is the item to remove
            if (_head.Data.Equals(item))
            {
                _head = _head.Next;
                Count--;
                return true;
            }

            var current = _head;
            while (current.Next != null)
            {
                if (current.Next.Data.Equals(item))
                {
                    current.Next = current.Next.Next;
                    Count--;
                    return true;
                }
                current = current.Next;
            }

            return false;
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="index"/> is less than 0 or greater than or equal to <see cref="Count"/>.
        /// </exception>
        public T Get(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

            var current = _head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }

            return current.Data;
        }

        /// <summary>
        /// Determines whether the linked list contains the specified item.
        /// Uses <see cref="object.Equals(object)"/> for comparison.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <returns><c>true</c> if the item is found; otherwise, <c>false</c>.</returns>
        public bool Contains(T item)
        {
            var current = _head;
            while (current != null)
            {
                if (current.Data.Equals(item))
                    return true;
                current = current.Next;
            }

            return false;
        }

        /// <summary>
        /// Removes all elements from the linked list.
        /// </summary>
        public void Clear()
        {
            _head = null;
            Count = 0;
        }

        /// <summary>
        /// Converts the linked list to a <see cref="List{T}"/>.
        /// </summary>
        /// <returns>A new <see cref="List{T}"/> containing all elements in order.</returns>
        public List<T> ToList()
        {
            var list = new List<T>();
            var current = _head;
            while (current != null)
            {
                list.Add(current.Data);
                current = current.Next;
            }

            return list;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the linked list.
        /// </summary>
        /// <returns>An enumerator for the linked list.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var current = _head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the linked list (non-generic).
        /// </summary>
        /// <returns>An enumerator for the linked list.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
