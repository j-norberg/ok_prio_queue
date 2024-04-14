#define EXTRA_DEBUG

namespace okPriorityQueue
{
    // Faster Priority Queue
    // jnorberg 2024-05-01

    public sealed class PriorityQueue
    {
        private float[] _prios; // priority
        private int[] _ids; // ids
        private int _count;
        public int Count => _count;

        // ctor
        public PriorityQueue(int initialCapacity)
        {
#if DEBUG
            if (initialCapacity <= 0)
            {
                throw new InvalidOperationException("New queue capacity cannot be smaller than 1");
            }
#endif

            _prios = new float[initialCapacity + 1];
            _ids = new int[initialCapacity + 1];

            Clear();
        }

        public void Clear()
        {
            _count = 0;

#if EXTRA_DEBUG
			for (int i = 0; i < _ids.Length; ++i)
			{
				_prios[i] = -1;
                _ids[i] = -1;
			}
#endif

        }

#if EXTRA_DEBUG
		public bool IsValid()
		{
			for (int i = 1; i <= _count; ++i)
			{
				float p = _prios[i];

				int iL = i * 2;
				int iR = iL + 1;

				if (iL <= _count)
				{
					float pL = _prios[iL];
					if (pL < p)
						return false;
				}

				if (iR <= _count)
				{
					float pR = _prios[iR];
					if (pR < p)
						return false;
				}
			}
			return true;
		}
#endif

        public void Enqueue(int id, float priority)
        {
            //Grow if needed
            if (_count >= _ids.Length - 1)
            {
                // 50%
                Resize(1 + _count + (_count >> 1));
            }

            // make space at end
            _count++;

            // "CascadeUp" inlined below

            int childIndex = _count;
            int parentIndex = _count;
            for (;;)
            {
                parentIndex >>= 1;
                if (parentIndex < 1)
                {
                    break;
                }

                if (_prios[parentIndex] <= priority)
                {
                    break;
                }

                //Node has lower priority value, so move parent down the heap to make room
                _prios[childIndex] = _prios[parentIndex];
                _ids[childIndex] = _ids[parentIndex];
                childIndex = parentIndex;
            }

            // finally write the values in the right spot
            _prios[childIndex] = priority;
            _ids[childIndex] = id;

        }

        // would we ever want to return the prio?
        public int Dequeue()
        {
#if DEBUG
            if (_count <= 0)
            {
                throw new InvalidOperationException("Cannot call Dequeue() on an empty queue");
            }
#endif

            int returnId = _ids[1];

            // If the node is already the last node, we can remove it immediately
            if (_count == 1)
            {
                _count = 0;
                return returnId;
            }

            // Swap the node with the last node
            int formerLastIndex = _count;

            float originalPrio = _prios[formerLastIndex];
            int originalId = _ids[formerLastIndex];

#if EXTRA_DEBUG
			_prios[formerLastIndex] = -1;
			_ids[formerLastIndex] = -1;
#endif
            _count--;

            int parentIndex = 1;

            for (;;)
            {
                int childLeftIndex = 2 * parentIndex;

                // If leaf node, we're done
                if (childLeftIndex > _count)
                {
                    break;
                }

                float leftPrio = _prios[childLeftIndex];

                int childRightIndex = childLeftIndex + 1;
                if (childRightIndex > _count)
                {
                    // only need to check left node
                    if (leftPrio < originalPrio)
                    {
                        // need to move up
                        _prios[parentIndex] = leftPrio;
                        _ids[parentIndex] = _ids[childLeftIndex];
                        parentIndex = childLeftIndex;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    // need to check both children
                    float rightPrio = _prios[childRightIndex];

                    bool goLeft = leftPrio < rightPrio;
                    float bestChildPrio = goLeft ? leftPrio : rightPrio;
                    int bestChildIndex = goLeft ? childLeftIndex : childRightIndex;

                    if (bestChildPrio < originalPrio)
                    {
                        // need to move up
                        _prios[parentIndex] = bestChildPrio;
                        _ids[parentIndex] = _ids[bestChildIndex];
                        parentIndex = bestChildIndex;
                    }
                    else
                    {
                        // done
                        break;
                    }
                }
            }

            _prios[parentIndex] = originalPrio;
            _ids[parentIndex] = originalId;

            return returnId;
        }

        public void Resize(int maxNodes)
        {
            if (maxNodes <= 0)
            {
                throw new InvalidOperationException("Queue size cannot be smaller than 1");
            }

            if (maxNodes < _count)
            {
                throw new InvalidOperationException("Called Resize(" + maxNodes + "), but current queue contains " + _count + " nodes");
            }

            int highestIndexToCopy = Math.Min(maxNodes, _count);

            int capacity = maxNodes + 1;
            int[] newIdsArray = new int[capacity];
            float[] newPriosArray = new float[capacity];

            Array.Copy(_ids, newIdsArray, highestIndexToCopy + 1);
            Array.Copy(_prios, newPriosArray, highestIndexToCopy + 1);

            _ids = newIdsArray;
            _prios = newPriosArray;
        }

    }

}