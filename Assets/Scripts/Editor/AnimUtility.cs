using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
 
public class AnimUtility : EditorWindow
{
    private static AnimUtility s_window = null;
 
    private AnimationClip[] m_clips = null;
//    private Animation m_animation = null;
 
    private float m_time = 0.0f;
   
    private bool m_isPlaying = false;
    private bool m_isLooping = false;
    private List<AnimationClip> m_playingAnimSequence = new List<AnimationClip>();
    private Dictionary<AnimationClip, bool> m_useAnim = new Dictionary<AnimationClip, bool>();
 
   
    [MenuItem("Custom/Animation Utility")]
    static void Init()
    {
        s_window = EditorWindow.GetWindow<AnimUtility>(true, "Anim Util", true);
        s_window.Show();
        s_window.Populate();
    }
 
    void Populate()
    {
        m_time = 0.0f;
//        m_animation = null;
        m_clips = null;
       
        if (m_playingAnimSequence != null && m_playingAnimSequence.Count > 0) {
            //for (int i=0; i<m_playingAnimSequence.Count;i++)
            m_playingAnimSequence.RemoveRange(0, m_playingAnimSequence.Count - 1);
        }
       
        m_playingAnimSequence = new List<AnimationClip>();
        m_useAnim = new Dictionary<AnimationClip, bool>(); //TODO Remove old ones?
       
        GameObject activeObject = Selection.activeGameObject;
        if (activeObject && activeObject.GetComponent<Animation>())
        {
//            m_animation = activeObject.GetComponent<Animation>();
			m_clips = AnimationUtility.GetAnimationClips(activeObject);
        }
       
        if (m_clips != null && m_clips.Length > 0) {
            foreach (AnimationClip clip in m_clips) {
                if (!m_useAnim.ContainsKey(clip))
                    m_useAnim.Add(clip, false);
            }
        }
    }
 
    void OnSelectionChange()
    {
        Populate();
        Repaint();
    }
 
    void OnEnable()
    {
        Populate();
    }
 
    void OnGUI()
    {
        if (m_clips != null)
        {
            GUILayout.BeginVertical();
            {
                foreach (AnimationClip anim in m_clips)
                {
                    bool prev = m_useAnim[anim];
                    m_useAnim[anim] = GUILayout.Toggle(m_useAnim[anim], anim.name + " " + anim.length + "s");
                    if (m_useAnim[anim] != prev) {
                        if (m_useAnim[anim]) {
                            m_playingAnimSequence.Add(anim);
                        } else {
                            m_playingAnimSequence.Remove(anim);
                        }
                    }
                }
                GUILayout.BeginHorizontal();
                m_isLooping =  GUILayout.Toggle(m_isLooping, "Loop ", GUILayout.ExpandWidth(true));
                m_isPlaying = GUILayout.Toggle(m_isPlaying, "Play ", GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
 
    void Update()
    {
        if (!m_isPlaying) return;
        if (m_playingAnimSequence.Count > 0) {
            //Debug.Log("playing Sequence of " + m_playingAnimSequence.Count + " clips");
           
            m_playingAnimSequence[0].SampleAnimation(Selection.activeGameObject, m_time);
            m_time += 0.01f;    //Update() is reportedly called 100 times per second
 
            if (m_time > m_playingAnimSequence[0].length)
            {
                Debug.Log("Done playing " + m_playingAnimSequence[0].name);
                m_time = 0.0f;
                if (m_isLooping) {
                    //remove it and add it back to the end
                    AnimationClip ac = m_playingAnimSequence[0];
                    m_playingAnimSequence.Remove(m_playingAnimSequence[0]);
                    m_playingAnimSequence.Add(ac);
                } else {
                    //uncheck it and then remove it
                    m_useAnim[m_playingAnimSequence[0]] = false;
                    m_playingAnimSequence.Remove(m_playingAnimSequence[0]);
                    Repaint();
                }
            }
        }
    }
}
