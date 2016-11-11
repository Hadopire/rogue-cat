using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

	public GameObject UICanvasPrefab;
	public GameObject actionButton1Prefab;
	public GameObject actionButton1TextPrefab;
	public GameObject bubblePrefab;
	public GameObject bubbleTextPrefab;

	public bool showButtons;
	public bool showBubble;

	GameObject UICanvas;

	GameObject[] buttonArray = new GameObject[1];

	GameObject bubble;
	GameObject bubbleText;

	struct BuildButton
	{
		public GameObject button;
		public GameObject text;
		public BuildButton(GameObject b, GameObject t)
		{
			button = b;
			text = t;
		}
	};

	public void init()
	{
		GUI.enabled = true;

		// instantiate the Canvas for UI
		UICanvas = Instantiate(UICanvasPrefab) as GameObject;

		// instantiate the buttons
		BuildButton[] buttonPrefabArray = {
			new BuildButton(actionButton1Prefab, actionButton1TextPrefab)
		};

		for (uint i = 0; i < buttonPrefabArray.Length; i ++)
		{
			// instantiate the button
			GameObject button = Instantiate(buttonPrefabArray[i].button) as GameObject;
			button.transform.SetParent(UICanvas.transform, false);
			// instantiate the text
			GameObject buttonText = Instantiate(buttonPrefabArray[i].text) as GameObject;
			buttonText.transform.SetParent(button.transform, false);

			// onClick -> play Animation ( the name of the animation should be the name of text )
			button.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.player.playAnim(buttonText.GetComponent<Text>().text));

			// Save in
			buttonArray[i] = button;
		}

		// instantiate the bubble
		bubble = Instantiate(bubblePrefab) as GameObject;
		bubble.transform.SetParent(UICanvas.transform, false);
		// instantiate the text
		bubbleText = Instantiate(bubbleTextPrefab) as GameObject;
		bubbleText.transform.SetParent(bubble.transform, false);
	}

	public void setBubbleText(string s)
	{
		bubbleText.GetComponent<Text>().text = s;
		showBubble = true;
	}

	void OnGUI()
	{

	}
}
