using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public abstract class AbstractMessageHandler : MonoBehaviour, IHandlesMessages
    {
        protected virtual int defaultMaxWidth { get { return 100; } }

        public abstract void HandleMessage(Message msg);
    }
}