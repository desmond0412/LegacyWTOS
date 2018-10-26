using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class BezierPoint : MonoBehaviour{
	public enum HandleStyle{
		Mirror,
		SemiMirror,
		Free,
		None,
	}

	[SerializeField]
	private BezierCurve _curve;
	public BezierCurve curve{
		get{return _curve;}
		set
		{
			if(_curve) _curve.RemovePoint(this);
			_curve = value;
			_curve.AddPoint(this);
		}
	}

	public HandleStyle handleStyle;

	public Vector3 position{
		get { return transform.position; }
		set { transform.position = value; }
	}

	public Vector3 localPosition{
		get { return transform.localPosition; }
		set { transform.localPosition = value; }
	}

	[SerializeField] 
	private Vector3 _handle1;
	public Vector3 handle1{
		get { return _handle1; }
		set { 
			if(_handle1 == value) return;
			_handle1 = value;
			if (handleStyle == HandleStyle.None)
				handleStyle = HandleStyle.Free;
			else if (handleStyle == HandleStyle.Mirror)
				_handle2 = -value;
			else if (handleStyle == HandleStyle.SemiMirror) {
				float tempLength = _handle2.magnitude;
				_handle2 = -value.normalized * tempLength;
			}
			_curve.SetCompleteCurve();
		}
	}

	public Vector3 globalHandle1{
		get{return 	transform.TransformPoint(handle1);}
		set{handle1 = transform.InverseTransformPoint(value);}
	}

	[SerializeField] 
	private Vector3 _handle2;
	public Vector3 handle2
	{
		get { return _handle2; }
		set { 
			if(_handle2 == value) return;
			_handle2 = value;
			if (handleStyle == HandleStyle.None)
				handleStyle = HandleStyle.Free;
			else if (handleStyle == HandleStyle.Mirror)
				_handle1 = -value;
			else if (handleStyle == HandleStyle.SemiMirror) {
				float tempLength = _handle1.magnitude;
				_handle1 = -value.normalized * tempLength;
			}
			_curve.SetCompleteCurve();
		}		
	}

	public Vector3 globalHandle2{
		get{return 	transform.TransformPoint(handle2);}
		set{handle2 = transform.InverseTransformPoint(value);}
	}

	private Vector3 lastPosition;

	void Update()
	{
		if(!_curve.completeCurve && transform.position != lastPosition)
		{
			_curve.SetCompleteCurve();
			lastPosition = transform.position;
		}
	}
}
