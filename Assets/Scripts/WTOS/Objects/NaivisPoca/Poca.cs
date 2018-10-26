using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Poca : UsableObject {

	public PocaPetal pocaPetal;

	public override void UseFunction ()
	{
		if(!isAccessible) return;
		base.UseFunction ();
		pocaPetal.Use ();
	}
}
