using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Utils_PrimeTween))]
public class Utils_PrimeTweenEditor : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		DrawProp("targets");
		DrawProp("tweenType");
		DrawProp("spaceType");
		DrawProp("playType");
		DrawProp("tweenStart");
		DrawProp("valueMode");

		var obj = (Utils_PrimeTween)target;

		if (obj != null)
		{
			if (obj.GetType()
				.GetField("tweenType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.GetValue(obj) is Utils_PrimeTween.TweenType tweenType)
			{
				if (tweenType.HasFlag(Utils_PrimeTween.TweenType.Position))
					DrawProp("position", "Position Tween");

				if (tweenType.HasFlag(Utils_PrimeTween.TweenType.Rotation))
					DrawProp("rotation", "Rotation Tween");

				if (tweenType.HasFlag(Utils_PrimeTween.TweenType.Scale))
					DrawProp("scale", "Scale Tween");
			}

			if (obj.GetType()
				.GetField("tweenStart", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				?.GetValue(obj) is Utils_PrimeTween.TweenStart tweenStart)
			{
				if (tweenStart == Utils_PrimeTween.TweenStart.All)
				{
					
				}else if (tweenStart == Utils_PrimeTween.TweenStart.Once)
				{
					DrawProp("delay", "Delay per Target");
					DrawProp("reverseOrder", "Reverse Order");
					DrawProp("loopYoYo", "Loop Yoyo");
				}
			}
		}

		GUILayout.Space(6);

		GUI.enabled = !Application.isPlaying;

		if (GUILayout.Button("▶ Preview Tween"))
			obj.Play();

		if (GUILayout.Button("⏹ Stop"))
			obj.Stop();

		if (GUILayout.Button("↺ Reset"))
			obj.ResetTransform();

		GUI.enabled = true;


		serializedObject.ApplyModifiedProperties();
	}

	private void DrawProp(string name, string label = null)
	{
		EditorGUILayout.PropertyField(
			serializedObject.FindProperty(name),
			label == null ? null : new GUIContent(label),
			true
		);
	}
}
