using UnityEngine;
using System.Collections;

public enum GlobalAudioType
{
	Ambience,
	InGameMusic,
	MainMenuMusic,
	GameOverMusic
}


public class GlobalAudioSingleton : GroupObjectSingleton<GlobalAudioSingleton>
{
	public GlobalAudioType type;
	public string startEventName;
	public string stopEventName;

	private bool isPlaying;

	private bool forceSilent = false;
	void Awake()
	{
		isPlaying = false;
		OnConstruct(type.ToString());
	}

	void OnDestroy()
	{
		Stop();
		OnDestruct(type.ToString());
	}

	public static GlobalAudioSingleton shared(GlobalAudioType type)
	{
		return shared(type.ToString());
	}


	protected override void OnInstanceExist (string key)
	{	
		//disable the light, but keep the setting;		
		this.gameObject.SetActive(false); 

//		Destroy(this.gameObject);
	}



	public static void StopAll()
	{
		string [] enumsName = System.Enum.GetNames(typeof(GlobalAudioType));

		foreach (var item in enumsName) {
			if(GlobalAudioSingleton.shared(item) != null)
				GlobalAudioSingleton.shared(item).Stop();
		}
	}

	private void StopOtherSound(bool includingThis = false)
	{
		string [] enumsName = System.Enum.GetNames(typeof(GlobalAudioType));

		foreach (var item in enumsName) {
			if(item != type.ToString() || includingThis)
				if(GlobalAudioSingleton.shared(item) != null)
					GlobalAudioSingleton.shared(item).Stop();
			}
	}

	public void Stop()
	{
		if(forceSilent)return;

		if(stopEventName.Equals(string.Empty)) return;
		
//        Debug.Log("stop : " + this.ToString() + " " + stopEventName);
		if(!isPlaying) return;

		AkSoundEngine.PostEvent(stopEventName,this.gameObject);		

		isPlaying = false;
	}

	public void Play(bool isAdditive = false)
	{
		if(forceSilent)return;
		
		if(!isAdditive)
			StopOtherSound();

		if(startEventName.Equals(string.Empty)) return;

//        Debug.Log("play : " + this.ToString() + " " + startEventName);
		if(isPlaying) return;

		AkSoundEngine.PostEvent(startEventName,this.gameObject);		

		isPlaying = true;
	}


	public override string ToString ()
	{
        string str = "";
        str += this.gameObject + " - ";
        str += type + " : ";
        str += isPlaying;
        return str;
	}

//	void Update(){
//		print(this.ToString());
//	}
}
