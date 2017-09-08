using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerCheck : MonoBehaviour {
	public GameManger myGM;
	public Color defaultColor;
	public bool amIRight;

	void Start(){
		myGM = GameObject.Find ("GameManager").GetComponent<GameManger>();
		amIRight = false;
		defaultColor = GetComponent<Image> ().color;
	}

	// Update is called once per frame
	void Update () {
	}

	public void CheckAnswer(){
		if (amIRight) {
			GetComponent<Image>().color = Color.green;
		} else {
			GetComponent<Image>().color = Color.red;
		}
		myGM.questionAnswered = true;
	}

	public void CheckValidAnswer(){
		if (transform.GetChild (0).GetComponent<TextMeshProUGUI> ().text == WWW.UnEscapeURL (myGM.questionData ["results"] [myGM.currQuestion] ["correct_answer"].ToString ())) {
			amIRight = true;
		} 
	}
}
