using System.Collections.Generic;

/// <summary>
/// Type-safe Event Manager. Holds pool of event listeners that are notified when the event is raised.
/// </summary>
public class Events
{
    private static Events _instance;
    public static Events instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Events();
            }

            return _instance;
        }
    }

    /// <summary>
    /// Delagate for game event data sent to listeners.
    /// </summary>
    /// <typeparam name="T">Game Event Sub Class</typeparam>
    public delegate void EventDelegate<T>(T e) where T : GameEvent;
    private delegate void InternalDelegate(GameEvent e);

    private Dictionary<System.Type, InternalDelegate> delegates = new Dictionary<System.Type, InternalDelegate>();
    private Dictionary<System.Delegate, InternalDelegate> delegateLookup = new Dictionary<System.Delegate, InternalDelegate>();

    /// <summary>
    /// Adds method to be invoked when event raised.
    /// </summary>
    /// <typeparam name="T">The event associated with the event delegate.</typeparam>
    /// <param name="del">The method to be stored and invoked if the event is raised.</param>
    public void AddListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        // Create non-generic delegate.
        InternalDelegate internalDelegate = (e) => del((T)e);

        // If event method already stored, return.
        if (delegateLookup.ContainsKey(del) && delegateLookup[del] == internalDelegate)
        {
            return;
        }

        // Store in delegate lookup for future checks.
        delegateLookup[del] = internalDelegate;

        // Store delegate in method lookup (invoked when event raised).
        // If delegates already contains an event of type T then the event is added to that 
        // else the event is stored in a new position in the dictionary.
        InternalDelegate tempDel;
        if (delegates.TryGetValue(typeof(T), out tempDel))
        {
            delegates[typeof(T)] = tempDel += internalDelegate;
        }
        else
        {
            delegates[typeof(T)] = internalDelegate;
        }
    }

    /// <summary>
    /// Removes method to be invoked when event raised.
    /// </summary>
    /// <typeparam name="T">The event associated with the event delegate.</typeparam>
    /// <param name="del">The method to be removed.</param>
    public void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        InternalDelegate internalDelegate;

        // Attempts to find delegate in lookup table.
        if (delegateLookup.TryGetValue(del, out internalDelegate))
        {
            InternalDelegate tempDel;

            // Attempt to find in delegate dictionary.
            if (delegates.TryGetValue(typeof(T), out tempDel))
            {
                // Removes internal delagte.
                tempDel -= internalDelegate;

                // If internal delegate is now null (as all events have been removed from it).
                if (tempDel == null)
                {
                    // Remove delegate completely.
                    delegates.Remove(typeof(T));
                }
                else
                {
                    // Store delegate (minus removed method).
                    delegates[typeof(T)] = tempDel;
                }
            }

            // Remove from lookup table.
            delegateLookup.Remove(del);
        }
    }

    /// <summary>
    /// Raises an event. ALl methods associated with event are invoked.
    /// </summary>
    /// <param name="e">THe event to raise. This is passed to associated delegates.</param>
    public void Raise(GameEvent e)
    {
        InternalDelegate del;
        if (delegates.TryGetValue(e.GetType(), out del))
        {
            del.Invoke(e);
        }
    }


}
