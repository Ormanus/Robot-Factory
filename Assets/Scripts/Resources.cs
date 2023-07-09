using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public sealed class Resources
{

    Dictionary<string, int> _resources = new Dictionary<string, int>();
    public string[] orderEmeralds = new string[7];
	private static Resources ResourceList;
	public static UnityEvent<string> OnEmeraldGained = new();

	private Resources() 
	{
	   _resources.Add("metal", 200);
       _resources.Add("electricity", 400);
       _resources.Add("gamepads", 150);
	}

	//Singleton implementation.
	public static Resources GetInstance()
	{
		if (ResourceList == null)
		{
			ResourceList = new Resources();
		}
		return ResourceList;
	}

    public int GetResource(string res)
    {
		if (_resources.ContainsKey(res))
			return _resources[res];
		else
		{
	   		Debug.Log($"Error reading resource {res} data.");
	   		return -1;
		}
    }

    public bool GainResouce(string res, int amount)
    {
		if (res.StartsWith("emerald_"))
		{
			AddOrderEmerald(res.Replace("emerald_", ""));
			return true;
		}

        if (_resources.ContainsKey(res))
 		{
   	   		_resources[res] += amount;
	   		return true;
  		}
  		else
  		{
    	    Debug.LogError("Unknown resource name");
		  	return false;
  		}
    }

    public bool ConsumeResouce(string res, int amount)
    {
        if (_resources.ContainsKey(res))
 		{
	   		if(_resources[res] >= amount)
	   		{
   	   			_resources[res] -= amount;
				return true;
	   		}
	   		else
	   		{
				//either play sound effect or show message that there's not enough resources
				return false;
	   		}
  		}	
  		else
  		{
    		Debug.LogError("Unknown resource name");
			return false;
  		}
    }

    //This method adds the name of an Order Emerald to the array at the first empty spot, then returns.
    public void AddOrderEmerald(string emeraldName)
    {
    	for (int i = 0; i < orderEmeralds.Length; i++)
		{
			if(string.IsNullOrEmpty(orderEmeralds[i]))
			{
				orderEmeralds[i] = emeraldName;
				OnEmeraldGained?.Invoke(emeraldName);
                return;
			}
		}

		if (GottemAll())
		{
			SceneManager.LoadScene("Victory");
		}
    }

	bool GottemAll()
	{
		for (int i = 0; i < orderEmeralds.Length; i++)
		{
			if (string.IsNullOrEmpty(orderEmeralds[i]))
				return false;
		}
		return true;
	}
}