﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour {

	public UDPReceiver updReceiver;

	void Start () {
		UDPReceiver.PedalCallBack  += PedalAction;
		UDPReceiver.HandleCallBack  += HandleAction;
		updReceiver.UDPStart ();
	}

	public void PedalAction(int x){
		pedal = Mathf.Min(Mathf.Max(1.0f * (x - minPedal ) / (maxPedal-minPedal), 0.0f), 1.0f);
	}

	public void HandleAction(int x){
		handle = Mathf.Min(Mathf.Max(-1.0f * x / maxHandle, -1.0f), 1.0f);
	}

	public void GetInput()
	{
		// m_horizontalInput = Input.GetAxis("Horizontal");
		// m_verticalInput = Input.GetAxis("Vertical");
		m_horizontalInput = handle;
		m_verticalInput = pedal;
	}

	private void Steer()
	{
		m_steeringAngle = maxSteerAngle * m_horizontalInput;
		frontDriverW.steerAngle = m_steeringAngle;
		frontPassengerW.steerAngle = m_steeringAngle;
	}

	private void Accelerate()
	{
		frontDriverW.motorTorque = m_verticalInput * motorForce;
		frontPassengerW.motorTorque = m_verticalInput * motorForce;
	}

	private void UpdateWheelPoses()
	{
		UpdateWheelPose(frontDriverW, frontDriverT);
		UpdateWheelPose(frontPassengerW, frontPassengerT);
		UpdateWheelPose(rearDriverW, rearDriverT);
		UpdateWheelPose(rearPassengerW, rearPassengerT);
	}

	private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
	{
		Vector3 _pos = _transform.position;
		Quaternion _quat = _transform.rotation;

		_collider.GetWorldPose(out _pos, out _quat);

		_transform.position = _pos;
		_transform.rotation = _quat;
	}

	private void FixedUpdate()
	{
		GetInput();
		Steer();
		Accelerate();
		UpdateWheelPoses();
	}

	private float pedal;
	private float handle;

	private int maxPedal = -14;
	private int minPedal = -34;
	private int maxHandle = 70;

	private float m_horizontalInput;
	private float m_verticalInput;
	private float m_steeringAngle;

	public WheelCollider frontDriverW, frontPassengerW;
	public WheelCollider rearDriverW, rearPassengerW;
	public Transform frontDriverT, frontPassengerT;
	public Transform rearDriverT, rearPassengerT;
	public float maxSteerAngle = 30;
	public float motorForce = 50;
}
