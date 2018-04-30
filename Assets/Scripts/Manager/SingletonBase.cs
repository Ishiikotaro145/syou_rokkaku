using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour 
{

	private static T instance;

	public static T GetInstance
	{
		get
		{ 
			if(instance == null)
			{
				instance = (T)FindObjectOfType(typeof(T));

				if(instance == null)
				{
					Debug.LogError (typeof(T) + "IsNothing");
				}
			}
			return instance;
		}
	}


}
