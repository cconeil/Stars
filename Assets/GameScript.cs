using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	public GameObject cylinderPrefab;
	public GUIText movesLeftText; 
	public int level;
	private int TOTAL_LEVELS = 2;

	private NodeScript focusNode = null;
	private NodeScript startStopNode = null;
	private int totalCoins = 0;
	private bool gameRunning = true;

	// Use this for initialization
	void Start () {
		totalCoins = GetCoinCount ();
		movesLeftText.guiText.text = "START";
		Invoke ("selectStartStopNode", 0.25f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void selectStartStopNode() {
		GameObject[] nodes = GameObject.FindGameObjectsWithTag ("Node");
		foreach (GameObject node in nodes) {

			NodeScript nodeScript = node.GetComponent(typeof(NodeScript)) as NodeScript;
			if (nodeScript.nodeType == NodeType.StartStop) {
				startStopNode = nodeScript;
				SelectedNode(nodeScript);
			}

		}
	}

	public void SelectedNode(NodeScript node) {
		if (node.IsSelectable () && gameRunning) {

			node.Select (); // select the node
			
			if (focusNode) {
				ConnectNodeToFocusNode(node);
				focusNode.Deselect();
			}
			
			focusNode = node;
			
			int movesLeft = TotalMovesLeft ();
			movesLeftText.guiText.text = "Moves Left: " + movesLeft;
			
			if (movesLeft <= 0) {

				// If there is a stop start node, you must finish your path on it.
				if (startStopNode && startStopNode != node) {
					Lose ();
					return;
				}

				// You must collect all of the coins
				if (totalCoins > 0) {
					Lose();
					return;
				}

				Win();
			}
		
		}
	}

	void Lose() {
		gameRunning = false;
		movesLeftText.guiText.text = "YOU LOSE";
		Invoke ("RestartLevel", 0.5f);
	}

	void Win() {
		gameRunning = false;
		movesLeftText.guiText.text = "YOU WIN";
		Invoke ("NextLevel", 0.5f);
	}

	void NextLevel() {
		if (level == TOTAL_LEVELS) {
			Debug.Log ("YOU WIN THE WHOLE GAME");
			Invoke("LoadFirstLevel", 0.25f);
		} else {
			Application.LoadLevel ("Level_" + (level + 1)); 
		}
	}

	void LoadFirstLevel() {
		Application.LoadLevel ("Level_1");
	}

	void RestartLevel() {
		Application.LoadLevel ("Level_" + level);
	}

	public void CollectedCoin(CoinScript coin) {
		totalCoins--;
		Destroy (coin.gameObject);
	}

	void ConnectNodeToFocusNode(NodeScript node) {
		Vector3 newNodePosition = node.transform.position;
		Vector3 focusNodePosition = focusNode.transform.position;
		
		CreateCylinderBetweenPoints (focusNodePosition, newNodePosition, 0.25f);
	}

	void CreateCylinderBetweenPoints(Vector3 start, Vector3 end, float width) {
		Vector3 offset = end - start;
		Vector3 scale = new Vector3 (width, offset.magnitude / 2.0f, width);
		Vector3 position = start + offset / 2.0f;

		GameObject cylinder = Instantiate (cylinderPrefab, position, Quaternion.identity) as GameObject;
		cylinder.transform.up = offset;
		cylinder.transform.localScale = scale;
	}

	int TotalMovesLeft() {
		GameObject[] nodes = GameObject.FindGameObjectsWithTag ("Node");

		int totalMovesLeft = 0;
		foreach (GameObject node in nodes) {
			NodeScript nodeScript = node.GetComponent(typeof(NodeScript)) as NodeScript;
			totalMovesLeft += nodeScript.MovesLeft();
		}

		return totalMovesLeft;
	}

	int GetCoinCount() {
		GameObject[] coins = GameObject.FindGameObjectsWithTag ("Coin");
		return coins.Length;
	}
}
