using System.Collections;
using UnityEngine;
using System.Globalization;
using Unity.VisualScripting;

public class Utils_UIController : MonoBehaviour
{
	public static Utils_UIController Instance;

	
	private bool on_off_no_internet = true;
	public static bool on_off_reward_button = true;
	
	private void Awake()
	{
		Instance = this;
	}

	/*private void Start()
	{
		StartCoroutine(Init());
	}
	
	public IEnumerator Init()
	{
		on_off_no_internet = PrefData.on_off_no_internet;
		//on_off_reward_button = PrefData.on_off_reward_button;
		
		//yield return new WaitUntil(() => InitGame.LoadCompleted);
		
		//on_off_reward_button = PrefData.on_off_reward_button;
		on_off_no_internet = PrefData.on_off_no_internet;

		if (PrefData.app_update_on_off)
		{
			float currentVersion;
			float minVersion;
			bool parsedCurrent = float.TryParse(Application.version, NumberStyles.Float, CultureInfo.InvariantCulture, out currentVersion);
			bool parsedMin = float.TryParse(PrefData.app_update_min_version, NumberStyles.Float, CultureInfo.InvariantCulture, out minVersion);

			if (parsedCurrent && parsedMin)
			{
				if (currentVersion < minVersion)
				{
					forceUpdatePopup.Show();
				}
			}
		}
	}*/
	
	private void Update()
	{
#if NO_ADS
		return;
#endif
		//if (!InitGame.LoadCompleted || !on_off_no_internet) 
		//{
		//	return;
		//}
		//if (noInternetPopup.isShow) return;

		//if (Application.internetReachability == NetworkReachability.NotReachable)
		//{
		//	noInternetPopup.Show();
		//}
	}
}
