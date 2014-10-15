using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	public GameObject cylinderPrefab;
	public GUIText movesLeftText; 

	private NodeScript focusNode = null;
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
				SelectedNode(nodeScript);
				return;
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
				Debug.Log("THE GAME IS OVER");
				Invoke("lose", 0.25f); // send the win function after half a second
			}
		
		}
	}

	void lose() {
		gameRunning = false;
		movesLeftText.guiText.text = "YOU LOSE";
	}

	void win() {
		gameRunning = false;
		movesLeftText.guiText.text = "YOU WIN";
	}

	public void CollectedCoin(CoinScript coin) {
		totalCoins--;
		Destroy (coin.gameObject);

		if (totalCoins <= 0) {
			CancelInvoke("lose"); // you can't win if you're going to lose
			win ();
		}
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
