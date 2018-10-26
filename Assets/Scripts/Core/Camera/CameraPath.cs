using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Artoncode.Core;

namespace Artoncode.Core.CameraPlatformer {
	public class CameraPath : SingletonMonoBehaviour<CameraPath> {
		
		[SerializeField] private CameraPathNode[] nodes = new CameraPathNode[0];
		[SerializeField] private CameraPathEdge[] edges = new CameraPathEdge[0];

		[HideInInspector] public CameraPathNode lastNearestNode = null;
		[HideInInspector] public CameraPathEdge lastNearestEdge = null;

		public CameraPathNode this[int index] {
			get { return nodes[index]; } 
		}
		public int nodeCount { 
			get { return nodes.Length; } 
		}

		public CameraPathEdge getEdge (int index) {
			return edges[index];
		}
		public int edgeCount { 
			get { return edges.Length; } 
		}

		public virtual void Awake () {
			base.Awake ();

			CameraPath mainCameraPath = shared ();
			if (mainCameraPath != this) {
				for (int i=0; i<nodes.Length; i++) {
					mainCameraPath.addNode (nodes[i]);
					nodes[i].transform.SetParent (mainCameraPath.transform, true);
				}
				for (int i=0; i<edges.Length; i++) {
					mainCameraPath.addEdge (edges[i]);
					edges[i].transform.SetParent (mainCameraPath.transform, true);
				}
				Destroy (gameObject);
			}
		}

		void Start () {
			nodes = nodes.OrderByDescending (x => x.priority).ToArray ();
			lastNearestNode = null;
			lastNearestEdge = null;
		}

		public CameraPathNode[] getNodes () {
			return nodes;
		}

		public int findNodeIndex (CameraPathNode node) {
			int result = -1;
			for(int i=0; i<nodes.Length; i++) {
				if(nodes[i] == node) {
					result = i;
					break;
				}
			}

			return result;
		}

		public void addNode (CameraPathNode node) {
			List<CameraPathNode> tempArray = new List<CameraPathNode> (nodes);
			tempArray.Add (node);
			nodes = tempArray.ToArray ();
		}

		public void addEdge (CameraPathEdge edge) {
			List<CameraPathEdge> tempArray = new List<CameraPathEdge> (edges);
			tempArray.Add (edge);
			edges = tempArray.ToArray ();
		}

		public CameraSetting getNearestCameraSetting (Vector3 position) {
			lastNearestNode = null;
			lastNearestEdge = null;

			// Find Volume Node first
			for (int i=0; i<nodes.Length; i++) {
				CameraPathNode node = nodes[i];
				if (node.enabled && node.gameObject.activeInHierarchy && node.type == CameraPathNode.CameraPathNodeType.Volume) {
					if (node.isInside (position)) {
						lastNearestNode = node;
						return new CameraSetting (node.cameraSetting);
					}
				}
			}

			float minDist = float.MaxValue;
			float ratio = 0;
			for (int i=0; i<edges.Length; i++) {
				CameraPathEdge edge = edges[i];
				if (edge.n1 != null && edge.n2 != null) {
					float dist;
					float r;
					if (edge.project (position, out r, out dist)) {
						if (dist < minDist) {
							minDist = dist;
							ratio = r;
							lastNearestEdge = edge;
						}
					}
				}
			}
			if (lastNearestEdge) {
				return CameraSetting.Lerp (lastNearestEdge.n1.cameraSetting, lastNearestEdge.n2.cameraSetting, lastNearestEdge.weight.Evaluate (ratio));
			}

			minDist = float.MaxValue;
			for (int i=0; i<nodes.Length; i++) {
				CameraPathNode node = nodes[i];
				if (node.enabled && node.gameObject.activeInHierarchy && node.type == CameraPathNode.CameraPathNodeType.Point) {
					float dist = Vector3.Distance (position, node.transform.position);
					if (dist < minDist) {
						minDist = dist;
						lastNearestNode = node;
					}
				}
			}
			if (lastNearestNode) {
				return new CameraSetting (lastNearestNode.cameraSetting);
			}

			return null;
		}
	}
}