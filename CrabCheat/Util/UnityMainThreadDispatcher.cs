using BepInEx.Unity.IL2CPP.Utils.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JNNJMods.CrabCheat.Util;

public class UnityMainThreadDispatcher : MonoBehaviour
{
	public UnityMainThreadDispatcher(IntPtr ptr) : base(ptr) { }

	private static readonly Queue<Action> _executionQueue = new();

	private static UnityMainThreadDispatcher _instance;

	private void Awake()
	{
		_instance = this;
	}

	private void OnDestroy()
	{
		_instance = null;
		lock (_executionQueue)
		{
			_executionQueue.Clear();
		}
	}

	public void Update()
	{
		lock (_executionQueue)
		{
			while (_executionQueue.Count > 0)
			{
				_executionQueue.Dequeue().Invoke();
			}
		}
	}

	/// <summary>
	/// Locks the queue and adds the IEnumerator to the queue
	/// </summary>
	/// <param name="action">IEnumerator function that will be executed from the main thread.</param>
	public static void Enqueue(IEnumerator action)
	{
		lock (_executionQueue)
		{
			_executionQueue.Enqueue(() =>
			{
				_instance.StartCoroutine(action.WrapToIl2Cpp());
			});
		}
	}

	/// <summary>
	/// Locks the queue and adds the Action to the queue
	/// </summary>
	/// <param name="action">function that will be executed from the main thread.</param>
	public static void Enqueue(Action action)
	{
		Enqueue(ActionWrapper(action));
	}

	public static IEnumerator ActionWrapper(Action a)
	{
		a();
		yield return null;
	}
}