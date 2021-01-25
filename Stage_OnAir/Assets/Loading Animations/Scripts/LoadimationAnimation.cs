using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
public class LoadimationAnimation : MonoBehaviour {
	public bool useCanvasImage;

	public Texture multiSpriteTexture;
	public Sprite[] sprites;
	
	[Range(0.0f,0.99f)]
	public float animationSpeed = .9f;
	public bool reverse;
	public bool pingpong;

	public bool rotate;
	public float rotation = 15;
	public float rotationTick = .1f;

	int animateVariationsCounter;
	SpriteRenderer cacheRenderer;
	UnityEngine.UI.Image cacheImage;
	int countAdd = 1;
	bool pong;

	void Start() {
		if (!multiSpriteTexture) return;
		if (useCanvasImage) cacheImage = GetComponent<UnityEngine.UI.Image>();
		else cacheRenderer = GetComponent<SpriteRenderer>();
		Invoke("Animate", 1f - animationSpeed);
		Invoke("RotateSprite", rotationTick);
	}

	void Animate() {
		if (sprites.Length == 0 || animationSpeed == 0) goto anim; 
		if (pingpong && (animateVariationsCounter == sprites.Length || animateVariationsCounter == 0)) {
			if (!pong) 	reverse = pong = true;
			else reverse = pong = false;		
		}
		if (reverse) {
			if (animateVariationsCounter == 0) animateVariationsCounter = sprites.Length;
			countAdd = -1;
		}  else countAdd = 1;
		Sprite s = sprites[animateVariationsCounter % sprites.Length];
		if (!useCanvasImage)
			cacheRenderer.sprite = s;
		else
			cacheImage.sprite = s;
		animateVariationsCounter += countAdd;
		anim: Invoke("Animate", 1f - animationSpeed);
	}

	void RotateSprite() {
		if (!rotate || rotation == 0) goto anim;
		transform.Rotate(Vector3.forward * rotation);
		anim: Invoke("RotateSprite", rotationTick);
	}

#if UNITY_EDITOR
	// Bug fix
	// Hides the SpriteRenderer, it's causing memory to build in Unity Editor.
	void Awake() {
		if (useCanvasImage) return;
		GetComponent<SpriteRenderer>().hideFlags = HideFlags.HideInInspector;
	}

	void OnApplicationQuit() {
		if (useCanvasImage) return;
		GetComponent<SpriteRenderer>().hideFlags = HideFlags.None;
	}
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(LoadimationAnimation))]
[CanEditMultipleObjects]
public class LoadimationAnimationEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		if (EditorApplication.isPlaying) GUI.enabled = false;
		if (GUILayout.Button("Update Sprite Array")) {
			UpdateSpriteArray();
		}
		GUILayout.Space(10);
	}

	public void UpdateSpriteArray() {
		LoadimationAnimation tar = (target as LoadimationAnimation);
		string spriteSheet = AssetDatabase.GetAssetPath( tar.multiSpriteTexture );
		Object[] objs = AssetDatabase.LoadAllAssetsAtPath(spriteSheet);
		
		ArrayList alist = new ArrayList();
		foreach (Object o in objs) {
			if (o as Sprite != null) alist.Add(o as Sprite);
		}
		tar.sprites = (Sprite[])alist.ToArray(typeof(Sprite));
		//	objs = null;

		if (!tar.useCanvasImage)
			tar.GetComponent<SpriteRenderer>().sprite = tar.sprites[0];
		else
			tar.GetComponent<UnityEngine.UI.Image>().sprite = tar.sprites[0];
		EditorUtility.SetDirty(target);
	}
}
#endif