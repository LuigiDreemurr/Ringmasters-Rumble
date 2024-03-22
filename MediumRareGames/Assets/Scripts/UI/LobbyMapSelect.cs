using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMapSelect : MonoBehaviour
{

    public Text lvlTxt;
    public Image bg;
    public Button[] lvlsOrder;

    [Space]

    public MapDetails[] maps = new MapDetails[3];

    [System.Serializable]
    public class MapDetails
    {
        public string name;
        public string sceneName;
        public Sprite preview; //we could make this a .mov file and have a rotating preview instead of a still, later if we'd like
    };

    public static MapDetails selectedMap { get; private set; }

    private int mapIndex = -1;

    // Use this for initialization
    void Start()
    {
        SelectMap(true);
    }

    public void SelectMap(bool next)
    {
        //select the relevant map
        if (next) { mapIndex++; } else { mapIndex--; }
        if (mapIndex > maps.Length - 1) { mapIndex = 0; }
        else if (mapIndex < 0) { mapIndex = maps.Length - 1; }

        selectedMap = maps[mapIndex];

        //update UI & BG
        lvlTxt.text = selectedMap.name;
        bg.sprite = selectedMap.preview;

        /*
         * 0 1 2 | 0
         * 2 0 1 | 1
         * 1 2 0 | 2
         */
        int index1 = mapIndex;
        int index2 = (index1 + 1 > 2) ? 0 : index1 + 1;
        int index3 = (index2 + 1 > 2) ? 0 : index2 + 1;

        //update image order
        lvlsOrder[0].GetComponent<Image>().sprite = maps[index1].preview;
        lvlsOrder[1].GetComponent<Image>().sprite = maps[index2].preview;
        lvlsOrder[2].GetComponent<Image>().sprite = maps[index3].preview;
        
    }
}
