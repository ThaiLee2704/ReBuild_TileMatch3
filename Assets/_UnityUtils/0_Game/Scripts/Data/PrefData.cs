using UnityEngine;

public partial class PrefData : MonoBehaviour
{
#if UNITY_EDITOR
    #region EXAM

    public static bool ExamBool
    {
        get => PlayerPrefs.GetInt("exam_bool", 1) == 1;
        set => PlayerPrefs.SetInt("exam_bool", value ? 1 : 0);
    }

    public static int ExamInt
    {
        get => PlayerPrefs.GetInt("exam_int", 0);
        set => PlayerPrefs.SetInt("exam_int", value);
    }

    public static float ExamFloat
    {
        get => PlayerPrefs.GetFloat("exam_float", 0);
        set => PlayerPrefs.SetFloat("exam_float", value);
    }

    public static string ExamString
    {
        get => PlayerPrefs.GetString("exam_string");
        set => PlayerPrefs.SetString("exam_string", value);
    }


    //Use when need to load with index
    public static int GetExam(int index)
    {
        return PlayerPrefs.GetInt("exam_" + index, 0);
    }

    //Use when need to save with index
    public static void SetExam(int index, int value)
    {
        PlayerPrefs.SetInt("exam_" + index, value);
    }


    #endregion
#endif

    #region USER
    public static int CurrentMinigame = -1;

    public static int count_open_app
    {
        get => PlayerPrefs.GetInt("count_open_app", 0);
        set => PlayerPrefs.SetInt("count_open_app", value);
    }

    public static bool FirstTimeOpen
    {
        get => PlayerPrefs.GetInt("first_time_open", 1) == 1;
        set => PlayerPrefs.SetInt("first_time_open", value ? 1 : 0);
    }

    public static int UserMoney
    {
#if UNITY_EDITOR
        get => PlayerPrefs.GetInt("user_money", 50000);
#elif !UNITY_EDITOR
        get => PlayerPrefs.GetInt("coin", 0);
#endif
        set => PlayerPrefs.SetInt("user_money", value);
    }

    public static int UserLevel
    {
        get => PlayerPrefs.GetInt("user_level", 0);
        set => PlayerPrefs.SetInt("user_level", value);
    }

    public static bool User_RateShowed
    {
        get => PlayerPrefs.GetInt("user_rate_showed", 0) == 1;
        set => PlayerPrefs.SetInt("user_rate_showed", value ? 1 : 0);
    }
    public static bool AppRated
    {
        get => PlayerPrefs.GetInt("app_rated", 0) == 1;
        set => PlayerPrefs.SetInt("app_rated", value ? 1 : 0);
    }

    #endregion

    #region SETTINGS

    public static bool SettingMusic
    {
        get => PlayerPrefs.GetInt("setting_music", 1) == 1;
        set => PlayerPrefs.SetInt("setting_music", value ? 1 : 0);
    }

    public static bool SettingSFX
    {
        get => PlayerPrefs.GetInt("setting_sfx", 1) == 1;
        set => PlayerPrefs.SetInt("setting_sfx", value ? 1 : 0);
    }

    public static bool SettingVibrate
    {
        get => PlayerPrefs.GetInt("setting_vibrate", 1) == 1;
        set => PlayerPrefs.SetInt("setting_vibrate", value ? 1 : 0);
    }

    //Slider
    /*
	public static float SettingMusic
	{
		get => PlayerPrefs.GetFloat("setting_music", 1);
		set => PlayerPrefs.SetFloat("setting_music", value);
	}
    
	public static float SettingSFX
	{
		get => PlayerPrefs.GetFloat("setting_sfx", 1);
		set => PlayerPrefs.SetFloat("setting_sfx", value);
	}
	*/

    public static float SettingSensitivity
    {
        get => PlayerPrefs.GetFloat("setting_sensitivity", .0375f);
        set => PlayerPrefs.SetFloat("setting_sensitivity", value);
    }
    //Use when localization is available
    public static int SettingLanguageIndex
    {
        get => PlayerPrefs.GetInt("language", -1);
        set => PlayerPrefs.SetInt("language", value);
    }

    #endregion
    #region REMOTE CONFIG

    public static bool on_off_ads
    {
#if UNITY_EDITOR
        get => PlayerPrefs.GetInt("on_off_ads", 1) == 1;
#else
        get => PlayerPrefs.GetInt("on_off_ads", 1) == 1;
#endif
        set => PlayerPrefs.SetInt("on_off_ads", value ? 1 : 0);
    }

    public static bool open_ad_on_off
    {
        get => PlayerPrefs.GetInt("open_ad_on_off", 1) == 1;
        set => PlayerPrefs.SetInt("open_ad_on_off", value ? 1 : 0);
    }

    public static bool show_open_ads_first_open
    {
        get => PlayerPrefs.GetInt("show_open_ads_first_open", 0) == 1;
        set => PlayerPrefs.SetInt("show_open_ads_first_open", value ? 1 : 0);
    }

    public static bool resume_ads
    {
        get => PlayerPrefs.GetInt("resume_ads", 1) == 1;
        set => PlayerPrefs.SetInt("resume_ads", value ? 1 : 0);
    }

    public static bool show_open_ads_resume
    {
        get => PlayerPrefs.GetInt("show_open_ads_resume", 0) == 1;
        set => PlayerPrefs.SetInt("show_open_ads_resume", value ? 1 : 0);
    }

    public static int open_ads_capping_time
    {
        get => PlayerPrefs.GetInt("open_ads_capping_time ", 25);
        set => PlayerPrefs.SetInt("open_ads_capping_time ", value);
    }

    public static bool banner_ad_on_off
    {
        get => PlayerPrefs.GetInt("banner_ad_on_off", 1) == 1;
        set => PlayerPrefs.SetInt("banner_ad_on_off", value ? 1 : 0);
    }

    public static int banner_size_option
    {
        get => PlayerPrefs.GetInt("banner_size_option", 1);
        set => PlayerPrefs.SetInt("banner_size_option", value);
    }

    public static bool inter_ad_on_off
    {
        get => PlayerPrefs.GetInt("inter_ad_on_off", 1) == 1;
        set => PlayerPrefs.SetInt("inter_ad_on_off", value ? 1 : 0);
    }

    public static int no_inter_duration_1st_open
    {
        get => PlayerPrefs.GetInt("no_inter_duration_1st_open", 30);
        set => PlayerPrefs.SetInt("no_inter_duration_1st_open", value);
    }
    public static int no_inter_duration_2nd_open
    {
        get => PlayerPrefs.GetInt("no_inter_duration_2nd_open", 20);
        set => PlayerPrefs.SetInt("no_inter_duration_2nd_open", value);
    }

    public static int inter_capping_time
    {
        get => PlayerPrefs.GetInt("inter_capping_time", 10);
        set => PlayerPrefs.SetInt("inter_capping_time", value);
    }


    public static bool user_highly_engagement_on_off
    {
        get => PlayerPrefs.GetInt("user_highly_engagement_on_off", 1) == 1;
        set => PlayerPrefs.SetInt("user_highly_engagement_on_off", value ? 1 : 0);
    }

    public static int user_highly_engagement_time
    {
        get => PlayerPrefs.GetInt("user_highly_engagement_time", 5); //tinh theo phut
        set => PlayerPrefs.SetInt("user_highly_engagement_time", value);
    }

    public static int user_highly_engagement_inter_adbreak_capping
    {
        get => PlayerPrefs.GetInt("user_highly_engagement_inter_adbreak_capping", 45); //tinh theo giay
        set => PlayerPrefs.SetInt("user_highly_engagement_inter_adbreak_capping", value);
    }

    public static int delay_call
    {
        get => PlayerPrefs.GetInt("delay_call", 700); //tinh theo miligiay
        set => PlayerPrefs.SetInt("delay_call", value);
    }

    public static bool mrec_ad_on_off
    {
        get => PlayerPrefs.GetInt("mrec_ad_on_off", 1) == 1;
        set => PlayerPrefs.SetInt("mrec_ad_on_off", value ? 1 : 0);
    }

    public static bool on_off_ads_break
    {
        get => PlayerPrefs.GetInt("on_off_ads_break", 1) == 1;
        set => PlayerPrefs.SetInt("on_off_ads_break", value ? 1 : 0);
    }

    public static int ads_break_time
    {
        get => PlayerPrefs.GetInt("ads_break_time ", 30);
        set => PlayerPrefs.SetInt("ads_break_time ", value);
    }

    public static bool on_off_no_internet
    {
        get => PlayerPrefs.GetInt("on_off_no_internet", 1) == 1;
        set => PlayerPrefs.SetInt("on_off_no_internet", value ? 1 : 0);
    }

    public static bool app_update_on_off
    {
        get => PlayerPrefs.GetInt("app_update_on_off", 1) == 1;
        set => PlayerPrefs.SetInt("app_update_on_off", value ? 1 : 0);
    }

    public static string app_update_min_version
    {
        get => PlayerPrefs.GetString("app_update_min_version", "1.0");
        set => PlayerPrefs.SetString("app_update_min_version", value);
    }

    public static string open_ad_id
    {
        get => PlayerPrefs.GetString("open_ad_id", "ca-app-pub-8058135677749863/7849798024");
        set => PlayerPrefs.SetString("open_ad_id", value);
    }

    public static string banner_ad_id
    {
        get => PlayerPrefs.GetString("banner_ad_id", "ca-app-pub-8058135677749863/9051199054");
        set => PlayerPrefs.SetString("banner_ad_id", value);
    }

    public static string mrec_ad_id
    {
        get => PlayerPrefs.GetString("mrec_ad_id", "ca-app-pub-8058135677749863/2182360769");
        set => PlayerPrefs.SetString("mrec_ad_id", value);
    }

    public static string inter_ad_id
    {
        get => PlayerPrefs.GetString("inter_ad_id", "ca-app-pub-8058135677749863/7285130747");
        set => PlayerPrefs.SetString("inter_ad_id", value);
    }

    public static string reward_ad_id
    {
        get => PlayerPrefs.GetString("reward_ad_id", "ca-app-pub-8058135677749863/7765859932");
        set => PlayerPrefs.SetString("reward_ad_id", value);
    }

    public static bool on_off_banner_in_loading
    {
        get => PlayerPrefs.GetInt("on_off_banner_in_loading", 1) == 1;
        set => PlayerPrefs.SetInt("on_off_banner_in_loading", value ? 1 : 0);
    }

    public static bool on_off_rate
    {
        get => PlayerPrefs.GetInt("on_off_rate", 0) == 1;
        set => PlayerPrefs.SetInt("on_off_rate", value ? 1 : 0);
    }

    public static bool on_off_bonus_inter
    {
        get => PlayerPrefs.GetInt("on_off_bonus_inter", 1) == 1;
        set => PlayerPrefs.SetInt("on_off_bonus_inter", value ? 1 : 0);
    }

    public static bool on_off_call_back_show_fail_reward
    {
        get => PlayerPrefs.GetInt("on_off_call_back_show_fail_reward", 1) == 1;
        set => PlayerPrefs.SetInt("on_off_call_back_show_fail_reward", value ? 1 : 0);
    }

    #endregion
    #region FLOOR

    public static void User_SetSpeed(int index, float value)
    {
        PlayerPrefs.SetFloat($"user_speed_game_{index}", value);
    }

    public static float User_GetSpeed(int index)
    {
        return PlayerPrefs.GetFloat($"user_speed_game_{index}", 0);
    }


    public static int User_FloorMap
    {
        get => PlayerPrefs.GetInt("user_floor_map", 0);
        set => PlayerPrefs.SetInt("user_floor_map", value);
    }

    public static int User_SpeedLevel
    {
        get => PlayerPrefs.GetInt("user_speed_level", 0);
        set => PlayerPrefs.SetInt("user_speed_level", value);
    }

    public static bool User_GetFloorMapUnlocked(int index)
    {
        return PlayerPrefs.GetInt($"user_floor_map_{index}", 0) == 1;
    }

    public static void User_SetFloorMapUnlocked(int index)
    {
        PlayerPrefs.SetInt($"user_floor_map_{index}", 1);
    }

    #endregion

}
