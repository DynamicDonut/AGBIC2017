using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;
using TMPro;

public class GameManger : MonoBehaviour {
    public JsonData questionData;
    public Transform myUI;
	public List<int> questionList;
	public int numofQuestions, categoryNum, maxQuota;
	[Range(0, 10)] public int currQuota;
    WWW myWWW;
	TextMeshProUGUI quotaText;

	[System.NonSerialized] public bool foundQuestions, foundAnswers, questionAnswered;
	[System.NonSerialized] public int currQuestion;
	[System.NonSerialized] public string questionDiff;
	public List<string> currAnswers;

    void Start() {
		if (SceneManager.GetActiveScene ().name == "QuestionScreen") {
			myWWW = new WWW ("https://opentdb.com/api.php?amount=" + numofQuestions + "&category=" + categoryNum + "&difficulty=" + questionDiff + "&type=multiple&encode=url3986");
			quotaText = myUI.Find ("QUOTA").GetComponent<TextMeshProUGUI> ();
			foundQuestions = foundAnswers = false;
			currQuestion = Random.Range (0, numofQuestions);
			currQuota = 0;
		}
    }

    void Update() {
		if (SceneManager.GetActiveScene ().name == "QuestionScreen") {
			if (myWWW.isDone && !foundQuestions) {
				questionData = JsonMapper.ToObject (myWWW.text);
				foundQuestions = true;
				StartCoroutine (GenerateNewQuestion (0f));
			}

			if (questionAnswered) {
				HighlightRightAnswer ();
				questionList.Add (currQuestion);

				Start: 
				currQuestion = Random.Range (0, numofQuestions);
				for (int i = 0; i < questionList.Count; i++) {
					if (currQuestion == questionList [i])
						goto Start;
				}
				goto Outer;

				Outer: 
				StartCoroutine (GenerateNewQuestion (2.0f));
				questionAnswered = false;
			}

			quotaText.text = currQuota + "/" + maxQuota;
		}
    }

	public IEnumerator FindQuestions(int cNum, string qDiff){
		myWWW = new WWW("https://opentdb.com/api.php?amount=" + numofQuestions + "&category=" + cNum + "&difficulty=" + qDiff + "&type=multiple&encode=url3986");
		quotaText = myUI.Find("QUOTA").GetComponent<TextMeshProUGUI> ();
		foundQuestions = foundAnswers = false;
		currQuestion = Random.Range(0, numofQuestions);
		currQuota = 0;
		yield break;
	}

	IEnumerator GenerateNewQuestion(float waitTime){
		yield return new WaitForSeconds (waitTime);
		myUI.Find("Question").GetChild(0).GetComponent<TextMeshProUGUI> ().text = WWW.UnEscapeURL (questionData ["results"] [currQuestion] ["question"].ToString ());
		foundAnswers = false;
		RandomizeAnswers ();
		for (int i = 0; i < currAnswers.Count; i++) {
			myUI.Find ("Answer" + (i + 1)).GetChild(0).GetComponent<TextMeshProUGUI> ().text = currAnswers [i];
			myUI.Find ("Answer" + (i + 1)).GetComponent<AnswerCheck> ().amIRight = false;
			myUI.Find ("Answer" + (i + 1)).GetComponent<AnswerCheck> ().CheckValidAnswer ();
			myUI.Find ("Answer" + (i + 1)).GetComponent<Image> ().color = myUI.Find ("Answer" + (i + 1)).GetComponent<AnswerCheck> ().defaultColor;
		}
	}
		
	void RandomizeAnswers(){
		currAnswers.Clear ();
		if (!foundAnswers) {
			currAnswers.Add (WWW.UnEscapeURL (questionData ["results"] [currQuestion] ["correct_answer"].ToString ()));
			for (int i = 0; i < 3; i++) {
				currAnswers.Add (WWW.UnEscapeURL (questionData ["results"] [currQuestion] ["incorrect_answers"] [i].ToString ()));
			}

			System.Random newRand = new System.Random ();
			string temp = "temp";
			for (int i = 0; i < currAnswers.Count; i++) {
				int r = i + (int)(newRand.NextDouble() * (currAnswers.Count - i));
				temp = currAnswers[r];
				currAnswers[r] = currAnswers[i];
				currAnswers[i] = temp;
			}
			foundAnswers = true;
		}
	}

	void HighlightRightAnswer(){
		for (int i = 0; i < 4; i++) {
			if (myUI.GetChild (i + 1).GetComponent<AnswerCheck> ().amIRight == true) {
				myUI.GetChild (i + 1).GetComponent<Image> ().color = Color.green;
			}
		}
	}
}
