using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public enum diff {
	easy,medium,hard
}

[System.Serializable]
public class QuestionLevel {
	public string enemyName;
	public string questionType;
	public diff difficulty;
	public int triviaDBNumber;
	public int quotaNumber;
}

public class GameProgression : MonoBehaviour {
	public QuestionLevel[] gameLevels;
	[Range (0, 9)]
	public int currLevel;
	TextMeshProUGUI LevelText, QuestionText;

	// Use this for initialization
	void Start () {
		currLevel = 0;
		LevelText = GameObject.Find ("Level").GetComponent<TextMeshProUGUI> ();
		QuestionText = GameObject.Find ("Question Text").GetComponent<TextMeshProUGUI> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene ().name == "VersusScreen") {
			LevelText.text = "Level " + (currLevel + 1);
			QuestionText.text = "Questions: " + gameLevels [currLevel].questionType;
			if (Input.GetButtonUp ("Submit")) {
				GetComponent<GameManger> ().questionDiff = gameLevels [currLevel].difficulty.ToString ();
				GetComponent<GameManger> ().categoryNum = gameLevels [currLevel].triviaDBNumber;
				SceneManager.LoadScene (1);
				//GetComponent<GameManger> ().FindQuestions (gameLevels [currLevel].triviaDBNumber, gameLevels [currLevel].difficulty.ToString ());
			}
		}
	}
}
