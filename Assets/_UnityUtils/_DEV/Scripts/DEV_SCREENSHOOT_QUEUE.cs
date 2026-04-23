#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEV_SCREENSHOOT_QUEUE : MonoBehaviour
{
	public List<Transform> screenShoots;

	public Transform screenShoot;
	public DEV_RENDER_TEXTURE_EXPORTER renderTextureExporter;

	private int index = 0;


}

#endif
