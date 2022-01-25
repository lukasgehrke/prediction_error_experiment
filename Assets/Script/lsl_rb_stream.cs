using UnityEngine;
using System.Collections;
using LSL;
using Assets.LSL4Unity.Scripts;
using Assets.LSL4Unity.Scripts.Common;

public class lsl_rb_stream : MonoBehaviour {

	// 6DOF streaming
    GameObject head;

	private liblsl.StreamOutlet outlet;
	private liblsl.StreamInfo streamInfo;
	private liblsl.XMLElement objs, obj;
	private liblsl.XMLElement channels, chan;
	public liblsl.StreamInfo GetStreamInfo()
	{
		return streamInfo; 
	}

	private float[] currentSample;

	public double dataRate=90;

	public const string uid = "21da06a47ac14a1a84d56d629d0486e5";

	public double GetDataRate()
	{
		return dataRate;
	}

	public void SetDataRate(double rate )
	{
		dataRate=rate;
	}


	public bool HasConsumer()
	{
		if (outlet != null)
			return outlet.have_consumers();

		return false;
	}

	// set LSL parameters
	public const string StreamName = "Rigid_Head";

	public string StreamType = "rigidBody";
	
	// we use 7 DoF:
	// 3 Pos. (x,y,z) + 4 Rot (x,y,z,w)   
	public int ChannelCount = 7; 

	public MomentForSampling sampling;

	public Transform sampleSource;

	// Use this for initialization
	void Start () 
	{
		
		head = GameObject.Find("Camera (eye)");

		if (head != null)
			{

			// initialize the array once
			currentSample = new float[ChannelCount];

			//dataRate = LSLUtils.GetSamplingRateFor(sampling);

			streamInfo = new liblsl.StreamInfo(StreamName, StreamType, ChannelCount, dataRate, liblsl.channel_format_t.cf_float32, uid);

			//setup LSL stream metadata (code from vizard) 
			//streamInfo.desc().append_child("synchronization").append_child_value("can_drop_samples", "true");
			var setup = streamInfo.desc().append_child("setup");
			setup.append_child_value("name", StreamName);
			// channels with position and orientation in quaternions
			objs = setup.append_child("objects");
			obj = objs.append_child("object");
			obj.append_child_value("label", StreamName);
			obj.append_child_value("id", StreamName);
			obj.append_child_value("type", "Mocap");

			channels = streamInfo.desc().append_child("channels");
			chan = channels.append_child("channel");
			chan.append_child_value("label", StreamName + "_X");
			chan.append_child_value("object", StreamName);
			chan.append_child_value("type", "PositionX");
			chan.append_child_value("unit", "meters");

			chan = channels.append_child("channel");
			chan.append_child_value("label", StreamName + "_Y");
			chan.append_child_value("object", StreamName);
			chan.append_child_value("type", "PositionY");
			chan.append_child_value("unit", "meters");

			chan = channels.append_child("channel");
			chan.append_child_value("label", StreamName + "_Z");
			chan.append_child_value("object", StreamName);
			chan.append_child_value("type", "PositionZ");
			chan.append_child_value("unit", "meters");

			chan = channels.append_child("channel");
			chan.append_child_value("label", StreamName + "_quat_X");
			chan.append_child_value("object", StreamName);
			chan.append_child_value("type", "OrientationX");
			chan.append_child_value("unit", "quaternion");

			chan = channels.append_child("channel");
			chan.append_child_value("label", StreamName + "_quat_Y");
			chan.append_child_value("object", StreamName);
			chan.append_child_value("type", "OrientationY");
			chan.append_child_value("unit", "quaternion");

			chan = channels.append_child("channel");
			chan.append_child_value("label", StreamName + "_quat_Z");
			chan.append_child_value("object", StreamName);
			chan.append_child_value("type", "OrientationZ");
			chan.append_child_value("unit", "quaternion");

			chan = channels.append_child("channel");
			chan.append_child_value("label", StreamName + "_quat_W");
			chan.append_child_value("object", StreamName);
			chan.append_child_value("type", "OrientationW");
			chan.append_child_value("unit", "quaternion");

			outlet = new liblsl.StreamOutlet(streamInfo);
			
		}
	}

	private void pushSample()
	{
		if (outlet == null)
			return;
		else
		{
			/* if (Vector3.Magnitude(firstDevice.velocity) > 1)
					Debug.Log("Position:" +firstDevice.transform.pos);
				if (Vector3.Magnitude(firstDevice.angularVelocity) > 1)
					Debug.Log("Rotation"+firstDevice.transform.rot);*/
			// reuse the array for each sample to reduce allocation costs
			// currently only for right-hand device
			currentSample[0] = head.transform.position.x;
			currentSample[1] = head.transform.position.y;
			currentSample[2] = head.transform.position.z;
			currentSample[3] = head.transform.rotation.x;
			currentSample[4] = head.transform.rotation.y;
			currentSample[5] = head.transform.rotation.z;
			currentSample[6] = head.transform.rotation.w;

			outlet.push_sample(currentSample, liblsl.local_clock());

			//Debug.Log(currentSample[0]+","+currentSample[1]+","+currentSample[2]);
		}
	}

	void FixedUpdate()
	{
		if (sampling == MomentForSampling.FixedUpdate)
			pushSample();
	}

	void Update()
	{
		if (sampling == MomentForSampling.Update)
			pushSample();
	}

	void LateUpdate()
	{
		if (sampling == MomentForSampling.LateUpdate)
			pushSample();
	}
}