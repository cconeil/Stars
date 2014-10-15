using UnityEngine;
using System.Collections;

public enum NodeType { StartStop = 1, Single, Double };

public class NodeScript : MonoBehaviour {
	
	public NodeType nodeType = NodeType.Single;
	public GameObject selectionPrefab;
	
	protected int selectionCount = 0;
	private GameObject selectionQuad = null;

	void Start () {
		UpdateMaterial ();
		UpdateSelectionCount();
	}

	void Update () {
	}

	void OnMouseDown() {
		GameScript gameScript = Camera.main.GetComponent (typeof(GameScript)) as GameScript;
		gameScript.SelectedNode (this);
	}

	// Selects the appropriate color based on the nodeType that this node is.
	void UpdateMaterial() {
		switch (nodeType) {
		case NodeType.StartStop: {
			Material material = Resources.Load("Mat_Red", typeof(Material)) as Material;
			this.renderer.material = material;
			
			break;
		}
		case NodeType.Single: {
			Material material = Resources.Load("Mat_Yellow", typeof(Material)) as Material;
			this.renderer.material = material;
			break;
		}
			
		case NodeType.Double: {
			Material material = Resources.Load("Mat_Blue", typeof(Material)) as Material;
			this.renderer.material = material;
			break;
		}
		}
	}

	void UpdateSelectionCount() {
		switch (nodeType) {
		case NodeType.StartStop:
			selectionCount = 2;
			break;
		case NodeType.Single:
			selectionCount = 1;
			break;
		case NodeType.Double:
			selectionCount = 2;
			break;
		}
	}
		
	public bool IsSelectable() {
		return selectionCount > 0;
	}
	
	public void Select() {
		selectionCount--;

		Vector3 selectionPosition = transform.position;
		selectionPosition.z = selectionPosition.z + 1f;
		selectionQuad = Instantiate (selectionPrefab, selectionPosition, Quaternion.identity) as GameObject;
	}

	public void Deselect() {
		Destroy (selectionQuad);
		selectionQuad = null;
	}

	public int MovesLeft() {
		return selectionCount;
	}
	
}
