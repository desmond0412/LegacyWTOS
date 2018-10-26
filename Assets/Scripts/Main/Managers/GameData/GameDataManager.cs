using UnityEngine;
using System.Collections;
using Artoncode.Core;
using Artoncode.Core.Data;


public class GameDataManager : Singleton<GameDataManager> {

	public enum GameDataType
	{
		None,
		MainData 	=  1 << 1,
		ConfigData	=  1 << 2,
		MyuuData	=  1 << 3,
		AllData 	=  MainData | ConfigData | MyuuData
	}


	private const string SAVE_GAME_EXIST_KEY = "SAVE_GAME_EXIST";
    private const string PLAYER_LOCATION_KEY = "PLAYER_LOCATION";
    private const string PLAYER_PUZZLE_STATE_KEY = "PLAYER_PUZZLE_STATES";


	//CONFIG
	private const string CONFIG_FULLSCREEN_KEY 	= "CONFIG_FULLSCREEN_KEY";
	private const string CONFIG_VOLUME_KEY 		= "CONFIG_VOLUME_KEY";
	//CONFIG

	//MYUU
	private const string MYUU_JOURNAL_STATE_KEY = "MYUU_JOURNAL_STATE";
	//MYUU


	//LOCAL MEMORY
	private const string LOCAL_PLAYER_LOAD_OPENING_KEY = "PLAYER_LOAD_OPENING";
	private const string LOCAL_NEXTSCENE_FROM_LOADSCREEN_KEY = "NEXTSCENE_FROM_LOADSCREEN";
	private const string LOCAL_SCENE_TO_UNLOAD_KEY = "SCENE_TO_UNLOAD";
	//LOCAL MEMORY

	private DataManager mainDataManager;
	private DataManager myuuDataManager;
	private DataManager configDataManager;
	private DataManager localDataManager;

	public GameDataManager(){
		localDataManager 	= DataManager.create();
		mainDataManager 	= DataManager.create();
		myuuDataManager		= DataManager.create();
		configDataManager	= DataManager.create();


		configDataManager.setDefaultFilename("cdata.dat");
		configDataManager.setSaveFolders(Application.persistentDataPath+"/");

		myuuDataManager.setDefaultFilename("mdata.dat");
		myuuDataManager.setSaveFolders(Application.persistentDataPath+"/");

		mainDataManager.setDefaultFilename("data.dat");
		mainDataManager.setSaveFolders(Application.persistentDataPath+"/");
		Load();


	}


	public void Save(GameDataType type = GameDataType.AllData)
	{
		if((type & GameDataType.MainData) == GameDataType.MainData)
			mainDataManager.save();
		
		if((type & GameDataType.MyuuData) == GameDataType.MyuuData)
			myuuDataManager.save();

		if((type & GameDataType.ConfigData) == GameDataType.ConfigData)
			configDataManager.save();
	}

	public void Load()
	{
		mainDataManager.load();
		myuuDataManager.load();
		configDataManager.load();
	}

	public void Reset()
	{
		mainDataManager.reset();
	}

	public bool SaveGameExist {
		get
		{
			return (mainDataManager.getBool(SAVE_GAME_EXIST_KEY) != null) ? (bool) mainDataManager.getBool(SAVE_GAME_EXIST_KEY) : false ;
		}set
		{
			mainDataManager.setBool(SAVE_GAME_EXIST_KEY,value);
			Log("Save","SAVE_GAME_EXIST_KEY");
		}
	}

	public LocationModel PlayerLocation
	{
		get {
			return (LocationModel)mainDataManager.getObject(PLAYER_LOCATION_KEY);
		}
		set{
			mainDataManager.setObject (PLAYER_LOCATION_KEY , value);
			Log("Save","PLAYER_LOCATION_KEY");
		}
	}

    public Hashtable PlayerPuzzleStates
    {
        get {
            return (Hashtable)mainDataManager.getObject(PLAYER_PUZZLE_STATE_KEY);
        }
        set{
            mainDataManager.setObject (PLAYER_PUZZLE_STATE_KEY , value);
            Log("Save","PLAYER_PUZZLE_STATE");
        }
    }


	#region CONFIG DATA
	public bool CONFIG_IsFullScreen {
		get
		{
			return (configDataManager.getBool(CONFIG_FULLSCREEN_KEY) != null) ? (bool) configDataManager.getBool(CONFIG_FULLSCREEN_KEY) : false ;
		}set
		{
			configDataManager.setBool(CONFIG_FULLSCREEN_KEY,value);
			Log("Save","CONFIG_FULLSCREEN_KEY");
		}
	}

	public float CONFIG_Volume {
		get
		{
			return (configDataManager.getFloat(CONFIG_VOLUME_KEY) != null) ? (float) configDataManager.getFloat(CONFIG_VOLUME_KEY) : 100.0f;
		}set
		{
			configDataManager.setFloat(CONFIG_VOLUME_KEY,value);
			Log("Save","CONFIG_VOLUME_KEY");
		}
	}
	#endregion


	#region MYUU DATA STATE
	public MyuuJournalState MyuuJournalState {
		get
		{
			return (myuuDataManager.getObject(MYUU_JOURNAL_STATE_KEY) != null) ? (MyuuJournalState)myuuDataManager.getObject(MYUU_JOURNAL_STATE_KEY) : MyuuJournalState.None ;
		}set
		{
			myuuDataManager.setObject(MYUU_JOURNAL_STATE_KEY,value);
			Log("Save","MYUU_JOURNAL_STATE_KEY");
		}
	}
	#endregion


	#region LOCAL MEMORY Data
	public bool LOCAL_IsLoadOpening {
		get
		{
			return (localDataManager.getBool(LOCAL_PLAYER_LOAD_OPENING_KEY) != null) ? (bool) localDataManager.getBool(LOCAL_PLAYER_LOAD_OPENING_KEY) : false ;
		}set
		{
			localDataManager.setBool(LOCAL_PLAYER_LOAD_OPENING_KEY,value);
			Log("Save","IsOpening");
		}
	}


	public string LOCAL_NextSceneFromLoadScreen {
		get
		{
			return localDataManager.getString(LOCAL_NEXTSCENE_FROM_LOADSCREEN_KEY);
		}set
		{
			localDataManager.setString(LOCAL_NEXTSCENE_FROM_LOADSCREEN_KEY,value);
			Log("Save","NextSceneFromLoadScreen");
		}
	}
	public string LOCAL_SceneToUnLoad {
		get
		{
			return localDataManager.getString(LOCAL_SCENE_TO_UNLOAD_KEY);
		}set
		{
			localDataManager.setString(LOCAL_SCENE_TO_UNLOAD_KEY,value);
			Log("Save","SceneToUnLoad");
		}
	}
	#endregion




	private void Log(string pEvent, string pLogName)
	{
//		Debug.Log("GAME_MANAGER : "+pEvent+" : "+pLogName);
	}





}
