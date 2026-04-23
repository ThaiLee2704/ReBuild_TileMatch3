#if PRIMETWEEN
using PrimeTween;
#elif LIT_MOTION
using LitMotion;
using LitMotion.Extensions;
#elif  DOTWEEN
using DG.Tweening;
#endif
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class Utils
{
    private static readonly int UILayer = LayerMask.NameToLayer("UI");
    public static readonly int[] RarityChances = { 40, 30, 15, 10, 5 };

    public static bool IsPointerOverUIElement()
    {
        var raycastResults = ListPool<RaycastResult>.Get();
        try
        {
            GetEventSystemRaycastResults(raycastResults);
            return raycastResults.Exists(result => result.gameObject.layer == UILayer);
        }
        finally
        {
            ListPool<RaycastResult>.Release(raycastResults);
        }
    }
    public static void RandomKFromN(int[] pool, int[] result, int k, int n)
    {
        // 1. Validation
        if (n > pool.Length)
        {
            Debug.LogError("n is larger than the pre-allocated pool!");
            return;
        }
        if (k > n || k > result.Length)
        {
            Debug.LogError("k is invalid for the provided range or result array! " + k + " " + n + " " + result.Length);

            return;
        }

        // 2. Ensure the pool segment is initialized (0 to n-1)
        // Note: You can skip this if you don't mind the pool keeping 
        // values from previous shuffles (which is still random).
        for (int i = 0; i < n; i++) pool[i] = i;

        // 3. Partial Fisher-Yates
        for (int i = 0; i < k; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, n);

            // Swap values in the pool
            int temp = pool[i];
            pool[i] = pool[randomIndex];
            pool[randomIndex] = temp;

            // Copy to the result array
            result[i] = pool[i];
        }
    }


    // Gets all event system raycast results of current mouse or touch position.
    private static void GetEventSystemRaycastResults(List<RaycastResult> raycastResults)
    {
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        EventSystem.current.RaycastAll(eventData, raycastResults);
    }
#if LIT_MOTION
	public static MotionHandle LColor(this SerializedPass target, string propertyName, Color startValue, Color endValue, float duration, Action onComplete = null, int loops = 0, LoopType loopType = LoopType.Yoyo)
	{
		return LMotion.Create(startValue, endValue, duration).WithLoops(loops, loopType).WithOnComplete(onComplete)
			.Bind((c) => target.SetColor(propertyName, c));
	}
	
	public static MotionHandle LitVirtual(float duration, Action callback, bool ignoreUpdate = false)
	{
		if (ignoreUpdate)
		{
			return LMotion.Create(0, 1, duration).WithScheduler(MotionScheduler.UpdateIgnoreTimeScale).WithOnComplete(callback).RunWithoutBinding();
		}
		else
		{
			return LMotion.Create(0, 1, duration).WithOnComplete(callback).RunWithoutBinding();
		}
	}
#endif

    public static Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t, Vector3 outDirection)
    {
        Vector3 horizontal = Vector3.Lerp(start, end, t);
        float parabola = -4f * height * (t - 0.5f) * (t - 0.5f) + height;
        return horizontal + outDirection.normalized * parabola;
    }
    public static Vector3 GetClosestPointOnLine(Vector3 point, Vector3 line_start, Vector3 line_end)
    {
        Vector3 line_direction = line_end - line_start;
        float line_length = line_direction.magnitude;
        line_direction.Normalize();
        float project_length = Mathf.Clamp(Vector3.Dot(point - line_start, line_direction), 0f, line_length);
        return line_start + line_direction * project_length;
    }

    #region CHANGE NUMBER TO STRING


    public static bool HasOneAfterDecimal(double div)
    {
        return ((long)(div * 10)) != ((long)div * 10);
    }
    public static readonly string[] suffixes = { "", "K", "M", "B", "T", "aa", "bb", "cc", "dd", "ee" };
    public static string GetNumberAroundString(this double input, bool formatInt = true)
    {
        if (input < 1000)
        {
            //if (formatInt || !Utils.HasOneAfterDecimal(input)) return ((int)input).ToString();
            //return input.ToString(CultureInfo.InvariantCulture);
            if (formatInt) return ((int)input).ToString();
            if (HasOneAfterDecimal(input))
            {
                return $"{input:F1}";
            }
            return input.ToString("0.##");
        }
        double div = GetNumberAroundString(input, out int suffix);
        if (HasOneAfterDecimal(div))
        {
            return $"{div:F1}{suffixes[suffix]}";
        }
        return $"{div:F2}{suffixes[suffix]}";
    }
    public static double GetNumberAroundString(this double input, out int suffix)
    {
        if (input < 1000)
        {
            suffix = 0;
            return input;
        }
        int k = (int)(Math.Log10(input) / 3); // get number of digits and divide by 3
        var dividor = Math.Pow(10, k * 3);  // actual number we print

        suffix = k;
        return input / dividor;
    }


    public static string GetNumberAroundString(this float input, bool formatInt = false)
    {
        if (input < 1000)
        {
            if (formatInt) return ((int)input).ToString();
            if (HasOneAfterDecimal(input))
            {
                return $"{input:F1}";
            }
            return input.ToString("0.##");
        }
        double div = GetNumberAroundString(input, out int suffix);
        if (HasOneAfterDecimal(div))
        {
            return $"{div:F1}{suffixes[suffix]}";
        }
        return $"{div:F2}{suffixes[suffix]}";
    }


    public static int GetNumberAround(this int input)
    {
        if (input < 5000)
        {
            return input;
        }
        else if (input < 1_000_000)
        {
            return (input / 1000) * 1000;
        }
        else if (input < int.MaxValue)
        {
            return (input / 1_000_000) * 1_000_000;
        }
        return input;
    }
    public static long GetNumberAround(this long input)
    {
        if (input < 5000)
        {
            return input;
        }
        else if (input < 1_000_000)
        {
            return (input / 1000) * 1000;
        }
        else if (input < 1_000_000_000)
        {
            return (input / 1_000_000) * 1_000_000;
        }
        else if (input < 1_000_000_000_000)
        {
            return (input / 1_000_000_000) * 1_000_000_000;
        }

        return input;
    }

    public static string GetNumberFormat(this int input)
    {
        string formatted = string.Format(CultureInfo.InvariantCulture, "{0:N0}", input);
        return formatted;
    }

    #endregion

    public static void AnimateMoneyChange(Text moneyText, int preMoney, int curMoney)
    {
        //AudioManager.Instance.PlayCashRegister();
#if DOTWEEN
		DOTween.To(() => preMoney, x => preMoney = x, curMoney, 2f)
			.OnUpdate(() => { moneyText.text = preMoney.ToString(); })
			.OnComplete(() => { moneyText.text = curMoney.ToString(); });
#elif PRIMETWEEN
        Tween.Custom(preMoney, curMoney, 2f, value =>
        {
            preMoney = Mathf.RoundToInt(value);
            moneyText.text = preMoney.ToString();
        })
        .OnComplete(() =>
        {
            moneyText.text = curMoney.ToString();
        });

#endif

    }

    public static void SetNativeImage(this Image image, Vector2 parentSize, float scale = 1f)
    {
        //var parentSize = image.GetComponent<RectTransform>().sizeDelta;
        var imgSize = new Vector2(image.sprite.texture.width, image.sprite.texture.height);

        var parentRatio = parentSize.x / parentSize.y;
        var imgRatio = imgSize.x / imgSize.y;

        if (parentRatio > imgRatio)
        {
            var yMax = parentSize.y;
            var x = (parentSize.y / imgSize.y) * imgSize.x;
            image.rectTransform.sizeDelta = new Vector2(x, yMax) * scale;
        }
        else
        {
            var y = (parentSize.x / imgSize.x) * imgSize.y;
            var xMax = parentSize.x;
            image.rectTransform.sizeDelta = new Vector2(xMax, y) * scale;
        }
    }

    public static Vector3 RandomAround(this Vector3 center, float range = 2.5f)
    {
        float randX = UnityEngine.Random.Range(-range, range);
        float randZ = UnityEngine.Random.Range(-range, range);
        return new Vector3(center.x + randX, center.y, center.z + randZ);
    }

    public static Vector3 PosDistanceArray(this Transform[] listTransform, Transform target, bool isMax = true)
    {
        if (listTransform == null || listTransform.Length == 0 || target == null)
            return Vector3.zero;

        float bestDistance = isMax ? float.MinValue : float.MaxValue;
        Vector3 result = Vector3.zero;

        foreach (var t in listTransform)
        {
            if (t == null || t == target) continue;

            float dist = Vector3.SqrMagnitude(t.position - target.position);

            if ((isMax && dist > bestDistance) || (!isMax && dist < bestDistance))
            {
                bestDistance = dist;
                result = t.position;
            }
        }

        return result;
    }
    #region BRAINROT
    /*public static string FormatPriceText(this int input, TypeTextDisplayBrainRot typeTextDisplayBrainRot)
    {
        switch (typeTextDisplayBrainRot)
        {
            case TypeTextDisplayBrainRot.Price:
                return $"${input.GetNumberAroundString()}";
                break;
            case TypeTextDisplayBrainRot.Profit:
                return $"${input.GetNumberAroundString()}/s";
                break;
        }
        return input.ToString();
    }*/
    static string m_noneFormat = "{0}";
    static string m_oneFormat = "{0:F1}";
    static string m_suffixNoneFormat = "{0}{1}";
    static string m_suffixOneFormat = "{0:F1}{1}";

    //public static void UpdateMoney(long money, TMP_Text text, TypeTextDisplayBrainRot typeTextDisplayBrainRot, bool formatInt = true)
    //{
    //    using (var sb = ZString.CreateStringBuilder())
    //    {
    //        sb.Append("$");
    //        if (money < 1000)
    //        {
    //            if (formatInt || !HasOneAfterDecimal(money)) sb.AppendFormat(m_noneFormat, money);
    //            else sb.AppendFormat(m_oneFormat, money);
    //        }
    //        else
    //        {
    //            double div = GetNumberAroundString(money, out int suffix);
    //            if (HasOneAfterDecimal(div)) sb.AppendFormat(m_suffixOneFormat, div, suffixes[suffix]);
    //            else sb.AppendFormat(m_suffixNoneFormat, (int)div, suffixes[suffix]);
    //        }
    //        if (typeTextDisplayBrainRot == TypeTextDisplayBrainRot.Profit) sb.Append("/s");
    //        text.SetText(sb.ToString());
    //    }
    //}
    //public static long BrainRotPrice(this int price, Brainrot_Mutation mutation)
    //{
    //    if (mutation == Brainrot_Mutation.Diamond) return (long)(price * 1.5f);
    //    if (mutation == Brainrot_Mutation.Blood) return (price * 2);
    //    if (mutation == Brainrot_Mutation.RainBow) return (long)(price * 2.5f);
    //    return price;
    //}

    //public static int BrainRotProfit(this int profit, Brainrot_Mutation mutation)
    //{
    //    if (mutation == Brainrot_Mutation.Gold) return (int)(profit * 1.5f);
    //    if (mutation == Brainrot_Mutation.Diamond) return (int)(profit * 3);
    //    if (mutation == Brainrot_Mutation.Blood) return (int)(profit * 3.5f);
    //    if (mutation == Brainrot_Mutation.RainBow) return (int)(profit * 4);
    //    return profit;
    //}



    /* public static string FormatPriceText(this long input, TypeTextDisplayBrainRot typeTextDisplayBrainRot)
     {
         switch (typeTextDisplayBrainRot)
         {
             case TypeTextDisplayBrainRot.Price:
                 return $"${input.GetNumberAroundString()}";
                 break;
             case TypeTextDisplayBrainRot.Profit:
                 return $"${input.GetNumberAroundString()}/s";
                 break;
         }
         return input.ToString();
     }*/
    #endregion
#if UNITY_EDITOR


    public static GameObject FindInThisChildren(this GameObject parent, string childName)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == childName)
            {
                return child.gameObject;
            }
            GameObject result = FindInChildren(child.gameObject, childName);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }
    public static GameObject FindInChildren(GameObject parent, string childName)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == childName)
            {
                return child.gameObject;
            }
            GameObject result = FindInChildren(child.gameObject, childName);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }



    public static void SetDirty<T>(this T target) where T : UnityEngine.Object
    {
        UnityEditor.EditorUtility.SetDirty(target);
    }

    public static void SO_SetDirty<T>(this T target) where T : ScriptableObject
    {
        UnityEditor.EditorUtility.SetDirty(target);
    }

#endif
}


public static class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif
    public static void Vibrate(long milliseconds)
    {
        // if (UIController.instace.vibrate == 0)
        // {
        //     return;
        // }
        if (!DataController.Setting_Vibrate)
        {
            return;
        }
        if (isAndroid())
            vibrator.Call("vibrate", milliseconds);
        else
            Handheld.Vibrate();
    }

    public static bool HasVibrator()
    {
        return isAndroid();
    }

    public static void Cancel()
    {
        if (isAndroid())
            vibrator.Call("cancel");
    }

    private static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
        return false;
#endif
    }
}
