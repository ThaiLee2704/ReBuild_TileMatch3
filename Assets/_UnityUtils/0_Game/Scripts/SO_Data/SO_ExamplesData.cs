#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Example_Data", menuName = "Data/Example_Data")]
public class SO_ExamplesData : ScriptableObject
{
    [Serializable]
    public class DataExample
    {
#if UNITY_EDITOR
        public string name;
#endif


	   /* public void DataExample()
	    {
		    
	    }*/
        
        
    }

    public List<DataExample> dataLevels = new();
}

#endif