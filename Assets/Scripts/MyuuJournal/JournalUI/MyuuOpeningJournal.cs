using UnityEngine;
using System.Collections;

public enum MyuuJournalState {
	None,
	First,
	Other
}

public class MyuuOpeningJournal : MonoBehaviour {

	public MyuuModelBase myuuModelBase;
	public GameObject myuuButton;

	void Start () {
		ResetPosition ();
		//HACK
		GameDataManager.shared ().MyuuJournalState = MyuuJournalState.Other;
	}

	public void SetToLevPosition()
	{
		Vector3 levPos = MainObjectSingleton.shared (MainObjectType.Player).transform.position;
		Quaternion levRot = MainObjectSingleton.shared (MainObjectType.Player).transform.rotation;

		transform.position = levPos;
		transform.rotation = Quaternion.AngleAxis (80 + levRot.eulerAngles.y, Vector3.up);
	}

	public void ResetPosition()
	{
		transform.localPosition = Vector3.zero;
		transform.rotation = Quaternion.identity;
		gameObject.SetActive (false);
	}

	public void StartIntro()
	{
		if (GameDataManager.shared ().MyuuJournalState != MyuuJournalState.None) {
			SetToLevPosition ();
			gameObject.SetActive (true);
			myuuButton.SetActive (false);
			myuuModelBase.OnFinishIntro += FinishIntro;
			if (GameDataManager.shared ().MyuuJournalState == MyuuJournalState.First) {
				int unlockState = Animator.StringToHash("AUI_MyuuJournal_Unlock");
				myuuModelBase.myuuAnim.Play (unlockState);
				MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().playCustomAnimation("MAINMENU.AC_Lev_MyuuJournal_Unlock");
			}
		}
	}
	void FinishIntro()
	{
		myuuModelBase.OnFinishIntro -= FinishIntro;
		GameDataManager.shared ().MyuuJournalState = MyuuJournalState.Other;
		myuuButton.SetActive(true);
	}

	public void MyuuHighlight()
	{
		if (myuuModelBase.myuuAnim.GetCurrentAnimatorStateInfo (0).IsName ("AUI_MyuuJournal_Idle")) {
			int highlightState = Animator.StringToHash("AUI_MyuuJournal_Highlight");
			myuuModelBase.myuuAnim.Play (highlightState);
			MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().playCustomAnimation("MAINMENU.AC_Lev_MyuuJournal_Highlight");
		}
	}

	public void MyuuUnhighlight()
	{
		if ((myuuModelBase.myuuAnim.GetCurrentAnimatorStateInfo(0).IsName("AUI_MyuuJournal_Highlight")) || (myuuModelBase.myuuAnim.GetCurrentAnimatorStateInfo(0).IsName("AUI_MyuuJournal_Highlight_loop")))
			myuuModelBase.myuuAnim.SetTrigger ("Normal");
	}

	public void MyuuClicked(string sth)
	{
		myuuButton.SetActive (false);
		int pressedState = Animator.StringToHash(sth);
		myuuModelBase.myuuAnim.Play (pressedState);
		if (sth=="AUI_MyuuJournal_Select1")
			MainObjectSingleton.shared (MainObjectType.Player).GetComponent<LevController> ().playCustomAnimation("MAINMENU.AC_Lev_MyuuJournal_Select");
		myuuModelBase.OnFinishExit += MyuuJournalHide;
	}

	void MyuuJournalHide()
	{
		myuuModelBase.OnFinishExit -= MyuuJournalHide;
		ResetPosition ();
	}
}

