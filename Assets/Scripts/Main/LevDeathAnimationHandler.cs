using UnityEngine;
using System.Collections;


public class LevDeathAnimationHandler : MonoBehaviour {

	LevController levController;
	DieCausesType causes;

	void Awake()
	{
		levController = this.GetComponent<LevController>();
	}

	public void Die(DieCausesType causes)
	{
		this.causes = causes;
		levController.setInputEnable(false);
		levController.stopMoving();

		PlayCustomAnimation(causes);
	}


	private void PlayCustomAnimation(DieCausesType type)
	{
		string animationName = "DEATH.";
		float delay = 0.0f;
		switch (type) 
		{

    		case DieCausesType.Drown : 
    			animationName += "AC_Lev_Forest_Dead_Drown"; 
    			break;

    		case DieCausesType.Fall			: 
                    
    		case DieCausesType.DeepShadow	: 
    			animationName += "AC_Lev_Forest_Dead_Drop_Fall"; 
    			break;

    		case DieCausesType.EatenByAtatar : 
    			animationName += "AC_Lev_NoGauntlet_Atatar_Dead1_Facing"; 
    			animationName += levController.facingDirection.ToString();
    			delay = 0.2f;
    			break;
    		
    		case DieCausesType.Rock : 
    			animationName += "AC_Lev_Dead_Generic"; 
    			break;

            case DieCausesType.Moluvusha :
                animationName += "AC_Lev_Forest_Moluvusha_Dead";
                break;
		}

		levController.playCustomAnimation(animationName,delay);


	}

}
