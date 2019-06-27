using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour {

	public UDPReceiver updReceiver;
	public Transform pedalTf;
	public Transform handleTf;
	public Quaternion pedalRot;
	public Quaternion handleRot;

	// Use this for initialization
	void Start () {
		UDPReceiver.PedalCallBack  += PedalAction;
		UDPReceiver.HandleCallBack  += HandleAction;
		updReceiver.UDPStart ();
	}

	public void PedalAction(int x){
		pedalRot =  Quaternion.Euler (x, 0, 0);
	}

	public void HandleAction(int x){
		handleRot =  Quaternion.Euler (x, 0, 0);
	}

	// Update is called once per frame
	void Update () {
		pedalTf.localRotation = pedalRot;
		handleTf.localRotation = handleRot;
	}
}
