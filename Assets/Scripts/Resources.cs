using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Resources
{

    Dictionary<string, int> _resources = new Dictionary<string, int>();
    public string[] orderEmeralds = new string[7];
	private static Resources ResourceList;

	private Resources() 
	{
	   _resources.Add("metal", 0);
       _resources.Add("electricity", 0);
       _resources.Add("gamepads", 0);
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
			if(orderEmeralds[i] == "")
			{
				orderEmeralds[i] = emeraldName;
				return;
			}
		}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}