using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Level.Tools
{
    public class TagReplacerWindow : EditorWindow
    {
        private string m_tag;
        private GameObject m_prefab;

        /// <summary>Menu Item for initializing the window</summary>
        [MenuItem("Tools/Level/Tag Replacer")]
        static private void Init()
        {
            //Get existing window, or if none, make a new one
            TagReplacerWindow window = EditorWindow.GetWindow<TagReplacerWindow>(false, "Tag Replacer");
            window.Show();
        }

        private void OnGUI()
        {
            m_tag = GUILayout.TextField(m_tag);
            m_prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", m_prefab, typeof(GameObject));

            if(GUILayout.Button("Replace"))
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag(m_tag);

                foreach(GameObject o in objects)
                {
                    Instantiate(m_prefab, o.transform.position, m_prefab.transform.rotation);
                    DestroyImmediate(o);
                }

            }

        }

    }
}
