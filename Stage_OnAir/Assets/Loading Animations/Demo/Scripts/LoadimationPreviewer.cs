using UnityEngine;

public class LoadimationPreviewer : MonoBehaviour {

	public GameObject loadimation;
	public GameObject loadimation2;
	public GameObject loadimation3;
	public UnityEngine.UI.Text text;
	
	public GameObject[] loadimations;
	int current;

	void Start() {
		text.text = "1 / " + loadimations.Length;

	}

	public void Next () {
		current = (current+1)%loadimations.Length;
		Change();
	}
	
	public void Previous() {
		current = current - 1;
		if (current < 0) current = loadimations.Length -1;
		Change();
	}

	void Change () {
		Destroy(loadimation);
		Vector3 pos = loadimation.transform.position;
		Quaternion rot = Quaternion.identity;
		loadimation = (GameObject)Instantiate(loadimations[current], pos, rot);
		Destroy(loadimation2);
		pos = loadimation2.transform.position;
		loadimation2 = (GameObject)Instantiate(loadimations[current], pos, rot);
		loadimation2.transform.localScale = Vector3.one * .75f;
		Destroy(loadimation3);
		pos = loadimation3.transform.position;
		loadimation3 = (GameObject)Instantiate(loadimations[current], pos, rot);
		loadimation3.transform.localScale = Vector3.one * .5f;
		text.text = (current + 1) + " / " + loadimations.Length;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.RightArrow)) 
			Next();
		if (Input.GetKeyDown(KeyCode.LeftArrow)) 
			Previous();
	}
}
