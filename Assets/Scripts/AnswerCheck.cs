using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerCheck : MonoBehaviour {
	public GameManger myGM;
	public PlayerControls myPC;
	public Color defaultColor;
	public bool amIRight, amIHighlighted;

	SpriteRenderer myHighlight;

	void Start(){
		myGM = GameObject.Find ("GameManager").GetComponent<GameManger>();
		myPC = GameObject.Find ("GameManager").GetComponent<PlayerControls> ();
		amIRight = false;
		defaultColor = GetComponent<Image> ().color;
		myHighlight = transform.Find ("Highlight").GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {
		if (name == myPC.currSelectedAnswer.name) {
			myHighlight.enabled = true;
		} else {
			myHighlight.enabled = false;
		}
	}

	public void CheckAnswer(){
		if (amIRight) {
			GetComponent<Image>().color = Color.green;
			myGM.currQuota++;
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
