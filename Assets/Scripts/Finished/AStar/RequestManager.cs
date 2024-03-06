/********************************************************
 * NAME: Travis Wright									*
 * CLASS: 4373.003										*
 * ASSIGNMENT: Project 4								*
 * FILE: RequestManager.cs								*
 * DESCRIPTION: Used to manage pathfinding requests by	*
 * 		enemy AI and prevent clogging of pipeline.		*
 * ******************************************************/

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RequestManager : MonoBehaviour {

	private Queue<PathRequest> pathQueue = new Queue<PathRequest>();
	private PathRequest currRequest; //instance of pathrequest structure
	static RequestManager instance; //an instance of the requestmanager class
	private Pathfinding pathfind; //instance of pathfinding class
	private bool isProcessing; //prevent multiple requests from being processed at once

	//initialize class
	void Awake()
	{
		instance = this;
		pathfind = GetComponent<Pathfinding>();
	}

	//function that allows the requesting of a path
	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
	{
		PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback); //create ne request
		instance.pathQueue.Enqueue(newRequest); //queue the new path request
		instance.tryProcessNext(); //attempt the new process
	}

	//structure for path requests
	struct PathRequest
	{
		public Vector3 pathStart; //start position at creation
		public Vector3 pathEnd;	//path end at creation
		public Action<Vector3[], bool> callback; //and action containing the path of waypoints and a boolean for compleation

		//constructor
		public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> call)
		{
			pathStart = start;
			pathEnd = end;
			callback = call;
		}
	}

	//attempt the next queued pathfind
	void tryProcessNext()
	{ //attempt only if there is not a current request pathfinding and there is a request present
		if(!isProcessing && pathQueue.Count > 0)
		{
			currRequest = pathQueue.Dequeue(); //dequeue current request
			isProcessing = true; //flip boolean
			pathfind.startFindPath(currRequest.pathStart, currRequest.pathEnd); //start the new path find
		}
	}

	//called when a request is finished processing
	public void finishedPath(Vector3[] path, bool success)
	{
		currRequest.callback(path, success); //build action with path and boolean determining if end is reached
		isProcessing = false; //flip bool
		tryProcessNext(); //try next
	}
	
}
