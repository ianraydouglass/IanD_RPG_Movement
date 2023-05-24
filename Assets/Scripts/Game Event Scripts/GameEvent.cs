using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameEvent", menuName = "GameEvent System/GameEvent")]
public class GameEvent : ScriptableObject
{
	//script taken from unity article on achitecting game code with scriptable objects
	//https://unity.com/how-to/architect-game-code-scriptable-objects

	private List<GameEventListener> listeners =
		new List<GameEventListener>();

	public void Raise()
	{

		//added by Ian D, writes a debug message when an event is called with the name of the event. 
		Debug.Log("Event Called: " + this.name);

		for (int i = listeners.Count - 1; i >= 0; i--)
			listeners[i].OnEventRaised();
	}

	public void RegisterListener(GameEventListener listener)
	{
		listeners.Add(listener);
	}

	public void UnregisterListener(GameEventListener listener)
	{
		listeners.Remove(listener);
	}
}
