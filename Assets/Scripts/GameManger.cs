using UnityEngine;
using System.Collections;
using LitJson;

public class GameManger : MonoBehaviour {
    private JsonData questionData;
    public Transform myUI;
    public int currQuestion;
    bool foundQuestions;
    WWW myWWW;

    // Use this for initialization
    void Start() {
        myWWW = new WWW("https://opentdb.com/api.php?amount=10&category=15&difficulty=medium&type=multiple&encode=url3986");
        foundQuestions = false;
        currQuestion = Random.Range(0, 9);
    }

    // Update is called once per frame
    void Update() {
        if (myWWW.isDone && !foundQuestions) {
            questionData = JsonMapper.ToObject(myWWW.text);
            for (int i = 0; i < questionData["results"].Count; i++) {
                Debug.Log(WWW.UnEscapeURL(questionData["results"][i]["question"].ToString()));
            }
            foundQuestions = true;
        }
    }
}
