#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

public partial class SYMBOL_CHECKER : AssetPostprocessor
{
	public static bool AUTO_REFRESH = false;

	static void OnPostprocessAllAssets(string[] importedAssets, string[] _, string[] __, string[] ___)
	{
		if (importedAssets.Any(path => path.EndsWith(".cs")) && AUTO_REFRESH)
		{
			CHECK_SYMBOL();
		}
		
		
	}

	public static void CHECK_SYMBOL()
	{
		UTILS_CHECK_SYMBOL();
		MEDIATION_SYMBOL_CHECK();
	}
	
	public static partial void UTILS_CHECK_SYMBOL();
	public static partial void MEDIATION_SYMBOL_CHECK();
	public static partial void MEDIATION_SYMBOL_CHECK()
	{
		
	}
	
}

#endif
