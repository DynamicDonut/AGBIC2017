using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour {
	public GameObject currSelectedAnswer;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		currSelectedAnswer = EventSystem.current.currentSelectedGameObject;
	}
}
