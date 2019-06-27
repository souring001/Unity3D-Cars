using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class UDPReceiver : MonoBehaviour {

	int LOCAL_PORT = 25252;
	static UdpClient udp;
	Thread thread;

	// public static Action<float, float, float> AccelCallBack;
	// public static Action<float, float, float, float> GyroCallBack;
	public static Action<int> PedalCallBack;
	public static Action<int> HandleCallBack;

	public void UDPStart(){
		udp = new UdpClient (LOCAL_PORT);
		thread = new Thread (new ThreadStart (ThreadMethod));
		thread.Start ();
	}

	private static void ThreadMethod()
	{
		while (true)
		{
			IPEndPoint remoteEp = null;
			byte[] data = udp.Receive (ref remoteEp);
			string text = Encoding.ASCII.GetString (data);

			Debug.Log(text);

			JsonNode jsonNode = JsonNode.Parse (text);
			string controller = jsonNode["controller"].Get<string> ();
			int pitch = (int) (jsonNode["pitch"].Get<long> ());

			if(controller == "pedal"){
				PedalCallBack (pitch);
			}else if(controller == "handle"){
				HandleCallBack (pitch);
			}



			//
			// double ax = jsonNode ["sensordata"] ["accel"] ["x"].Get<double> ();
			// double ay = jsonNode ["sensordata"] ["accel"] ["y"].Get<double> ();
			// double az = jsonNode ["sensordata"] ["accel"] ["z"].Get<double> ();
			//
			// double qutX = jsonNode ["sensordata"] ["quaternion"] ["x"].Get<double> ();
			// double qutY = jsonNode ["sensordata"] ["quaternion"] ["y"].Get<double> ();
			// double qutZ = jsonNode ["sensordata"] ["quaternion"] ["z"].Get<double> ();
			// double qutW = jsonNode ["sensordata"] ["quaternion"] ["w"].Get<double> ();
			//
			// AccelCallBack ((float)ax, (float)ay, (float)az);
			// GyroCallBack ((float)qutX, (float)qutY, (float)qutZ, (float)qutW);
		}
	}

	void OnApplicationQuit(){
		thread.Abort ();
	}
}
