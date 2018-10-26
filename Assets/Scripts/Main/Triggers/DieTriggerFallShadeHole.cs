using UnityEngine;
using System.Collections;

public class DieTriggerFallShadeHole : DieTriggerFall
{
	
	public override void Start ()
	{
		base.Start ();
		causes = DieCausesType.DeepShadow;
	}

	protected override void StartDieFX ()
	{
		StartCoroutine(DelayedStartDieFX(1.0f));
	}

	protected IEnumerator DelayedStartDieFX(float delay)
	{
		yield return new WaitForSeconds(delay);
		base.StartDieFX();		
	}

}
