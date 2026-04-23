using UnityEngine;

public partial class DataController : MonoBehaviour
{
    public static DataController Instance;

    private static double m_maxCoins;
    public static double MaxCoin
    {
        get => m_maxCoins;

        set
        {
            if (value < m_maxCoins) return;
            m_maxCoins = value;
        }
    }
    public static float Setting_Sensitivity
    {
        get => _settingSensitivity;
        set
        {
            _settingSensitivity = value;
            PrefData.SettingSensitivity = value;
        }
    }

    private static float _settingSensitivity = 1;

    #region GET SET DATA

    //USER
    public static int UserMoney
    {
        get => _userMoney;
        set
        {
            _userMoney = value;
            PrefData.UserMoney = value;
        }
    }

    public static int UserLevel
    {
        get => _userLevel;
        set
        {
            _userLevel = value;
            PrefData.UserLevel = value;
        }
    }

    #endregion

    #region SETTING

    public static bool Setting_Music
    {
        get => _settingMusic;
        set
        {
            _settingMusic = value;
            PrefData.SettingMusic = value;
        }
    }

    public static bool Setting_SFX
    {
        get => _settingSFX;
        set
        {
            _settingSFX = value;
            PrefData.SettingSFX = value;
        }
    }

    //Slider
    /*
	public static float Setting_Music
	{
		get => _settingMusic;
		set
		{
			_settingMusic = value;
			UserData.SettingMusic = value;
		}
	} 
	
	public static float Setting_SFX
	{
		get => _settingSfx;
		set
		{
			_settingSfx = value;
			UserData.SettingSFX = value;
		}
	}
	*/


    public static bool Setting_Vibrate
    {
        get => _settingVibrate;
        set
        {
            _settingVibrate = value;
            PrefData.SettingVibrate = value;
        }
    }

    #endregion

    #region CACHE

    //USER
    private static int _userMoney;
    private static int _userLevel;
    private static int _userRound;
    private static int _userSkin;
    private static int _userSkinType;

    //SETTING
    private static bool _settingMusic;
    private static bool _settingSFX;
    private static bool _settingVibrate;
    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        _userMoney = PrefData.UserMoney;
        _userLevel = PrefData.UserLevel;

        _settingMusic = PrefData.SettingMusic;
        _settingSFX = PrefData.SettingSFX;
        _settingVibrate = PrefData.SettingVibrate;
        _settingSensitivity = PrefData.SettingSensitivity;



        /*for (int i = 0; i < m_minigames.miniGameList.Count; i++)
        {
	        if (m_minigames.miniGameList[i].adsBreak)
	        {
		        Mediation_Manager.MiniGameAdsBreak.Add(i);
	        }else if (m_minigames.miniGameList[i].adsAFK)
	        {
		        Mediation_Manager.MiniGameAdsAFK.Add(i);
	        }
        }*/
    }

#if UNITY_IOS
    public void ATT()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer && PlayerPrefs.GetInt("ATTShowed", 0) == 0 && UnityATTPlugin.Instance.IsIOS14AndAbove())
        {
            TrackingEvent.LogFirebase("ATTShow", new[] {new Parameter("ATTShow", "ATTShow")});
            UnityATTPlugin.Instance.ShowATTRequest((action) =>
            {
                if (action == ATTStatus.Authorized)
                {
                    AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(true);
                    AdsAdapter.LogFirebase("ATTSuccess", new[] {new Parameter("ATTSuccess", "ATTSuccess")});
                }
                else{
                    AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(false);
                }
            });
            PlayerPrefs.SetInt("ATTShowed", 1);
        }

    }
#endif
}
