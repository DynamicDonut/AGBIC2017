using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LitJson;
using TMPro;

public class GameManger : MonoBehaviour {
    public JsonData questionData;
    public Transform myUI;
	public List<int> questionList;
	public int currQuestion, numofQuestions;
	public List<string> currAnswers;
    public bool foundQuestions, foundAnswers, questionAnswered;
    WWW myWWW;

    // Use this for initialization
    void Start() {
		myWWW = new WWW("https://opentdb.com/api.php?amount=" + numofQuestions + "&category=15&difficulty=medium&type=multiple&encode=url3986");
        foundQuestions = foundAnswers = false;
		currQuestion = Random.Range(0, numofQuestions);
		questionList.Add (currQuestion);
    }

    // Update is called once per frame
    void Update() {
        if (myWWW.isDone && !foundQuestions) {
            questionData = JsonMapper.ToObject(myWWW.text);
//            for (int i = 0; i < questionData["results"].Count; i++) {
//                Debug.Log(WWW.UnEscapeURL(questionData["results"][i]["question"].ToString()));
//            }
            foundQuestions = true;
			StartCoroutine (GenerateNewQuestion(0f));
        }

		if (questionAnswered){
			HighlightRightAnswer ();
			currQuestion = Random.Range (0, numofQuestions);
			for (int i = 0; i < questionList.Count; i++) {
				if (currQuestion == questionList [i]) {
					currQuestion = Random.Range (0, numofQuestions);
				}
			}
			StartCoroutine (GenerateNewQuestion(2.0f));
			questionAnswered = false;
		}

		//Debug.Log (questionData ["results"] [currQuestion] ["correct_answer"].ToString ());
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
