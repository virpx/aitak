using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cobaae : MonoBehaviour
{
    public string action;
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
        // aigame.cetak(aigame.rootminimax.children[0].curr.papan_cek);
        // Debug.Log(aigame.terminlarecurmenang2(aigame.rootminimax.children[0].curr.papan_cek));
        // if (action.Contains("awal"))
        // {
        //     for (int a = 0; a < 36; a++)
        //     {
        //         string namae = "";
        //         if (datagame.papan_game[a] != null)
        //         {
        //             namae = datagame.papan_game[a].namabidak + " mode : " + datagame.papan_game[a].penomoran;
        //         }
        //         Debug.Log(a + ":" + namae);
        //     }
        // }
        // else
        // {
        //     Debug.Log(aigame.rootminimax.indexchildpilih);
        //     aigame.cetak(aigame.rootminimax.curr.papan_cek);
        //     foreach (var x in aigame.rootminimax.children)
        //     {
        //         aigame.cetak(x.curr.papan_cek);
        //     }
        // }
        // aigame.cetak(aigame.rootminimax.children[aigame.rootminimax.indexchildpilih].curr.papan_cek);
        for (int i = 0; i < 36; i++)
        {
            string namae = "";
            string childe = "";
            if (datagame.papan_game[i] != null)
            {
                namae = datagame.papan_game[i].namabidak+" == "+datagame.papan_game[i].penomoran;
                var telusuri = datagame.papan_game[i];
                while (telusuri.children != null)
                {
                    childe += datagame.papan_game[i].children.namabidak+" == "+datagame.papan_game[i].children.penomoran + "," ;
                    telusuri = telusuri.children;
                }
            }
            Debug.Log(i + ":" + namae+" children e : "+childe);
        }
    }
}
