using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cobaae : MonoBehaviour
{
    public data datagame;
    public ai aigame;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseDown()
    {
        Debug.Log(aigame.rootminimax.indexchildpilih);
        aigame.cetak(aigame.rootminimax.curr.papan_cek);
        aigame.cetak(aigame.rootminimax.children[aigame.rootminimax.indexchildpilih].curr.papan_cek);
        // foreach(var x in aigame.rootminimax.children[0].children)
        // {
        //     aigame.cetak(x.curr.papan_cek);
        // }
        // for (int i = 0; i < 36; i++)
        // {
        //     string namae = "";
        //     if (aigame.rootminimax.children[0].children[0].curr.papan_game[i] != null)
        //     {
        //         namae = aigame.rootminimax.children[0].children[0].curr.papan_game[i].namabidak;
        //     }
        //     Debug.Log(i + ":" + namae);
        // }
        // for (int i = 0; i < 36; i++)
        // {
        //     string namae = "";
        //     if (aigame.rootminimax.curr.papan_game[i] != null)
        //     {
        //         namae = aigame.rootminimax.curr.papan_game[i].namabidak;
        //     }
        //     Debug.Log(i + ":" + namae);
        // }
    }
}
