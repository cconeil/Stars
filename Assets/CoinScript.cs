using UnityEngine;
using System.Collections;

public class CoinScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter(Collider col) {
		GameScript gameScript = Camera.main.GetComponent (typeof(GameScript)) as GameScript;
		gameScript.CollectedCoin (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
