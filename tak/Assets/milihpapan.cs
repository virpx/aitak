using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class milihpapan : MonoBehaviour
{
    [SerializeField] data datagame;
    [SerializeField] GameObject parentbidak;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if(datagame.selectedbidak != "")
        {
            //dibuat memanjang semua object papannya supaya bisa terpilih meskipun ada bidak yang sudah ditaruh
            var scaleobj = transform.localScale;
            scaleobj[1] += 1000;
            transform.localScale = scaleobj;
        }*/
    }
    public void klikpapan(GameObject papanpilih)
    {
        var bidakpilih = datagame.selectedbidak;
        var children = parentbidak.transform;
        if(datagame.childrenselectedbidak == null && datagame.get_namapapan_dari_bidak(datagame.selectedbidak) == "")
        {
            //taruh bidak baru ke papan 
            for (int i = 0; i < children.childCount; i++)
            {
                if (children.GetChild(i).name.Equals(bidakpilih))
                {
                    //pengecekan apakah bisa ditaruh
                    var hasiltaruh = datagame.taruhbidak();
                    if (hasiltaruh != -1)
                    {
                        var bidaknya = children.GetChild(i);
                        Vector3 papanposition = papanpilih.transform.position;
                        papanposition[1] += hasiltaruh;
                        if (bidaknya.name.Contains("black_") && !(bidaknya.name.Equals("black_31")))
                        {
                            papanposition[0] += 0.7f;
                        }
                        bidaknya.position = papanposition;
                        var posisibidak = bidaknya.transform.position;
                        posisibidak[1] -= 1;
                        bidaknya.transform.position = posisibidak;
                        datagame.updatepapan();
                        datagame.resetdata();
                        if(datagame.countergame < 3)
                        {
                            datagame.countergame++;
                        }
                        else
                        {
                            datagame.currentplayer *= -1;
                        }
                    }
                    break;
                }
            }
        }
        else
        {
            var hasiltaruh = datagame.taruhbidak();
            if (hasiltaruh != -1)
            {
                if (hasiltaruh == -100)
                {
                    //jika return -100 maka hanya taruh bidak (di tempat yang sama(untuk childrennya), jadi tidak perlu script banyak")
                    /*for (int i = 0; i < children.childCount; i++)
                    {
                        if (children.GetChild(i).name.Equals(datagame.bidakyangdilepas.namabidak))
                        {
                            var posisibidak = children.GetChild(i).transform.position;
                            posisibidak[1] -= 1;
                            children.GetChild(i).transform.position = posisibidak;
                            break;
                        }
                    }*/
                    var listbidak = datagame.get_list_bidak_papan("spesial");
                    foreach (var item in listbidak)
                    {
                        GameObject gameobjbidak = null;
                        foreach (Transform item2 in children)
                        {
                            if (item2.name == item.namabidak)
                            {
                                gameobjbidak = item2.gameObject;
                                break;
                            }
                        }
                        Vector3 posisibaru = papanpilih.transform.position;
                        posisibaru[1] += (datagame.setting_ulang_posisi_bidak(item.namabidak) - 1);
                        if (item.namabidak.Contains("black_") && !(item.namabidak.Equals("black_31")))
                        {
                            posisibaru[0] += 0.7f;
                        }
                        gameobjbidak.transform.position = posisibaru;
                    }
                }
                if (hasiltaruh == -101)
                {
                    //jika return -101 maka hanya taruh bidak (di tempat yang sama (untuk paling atas tapi bukan berarti dari awal di tempat itu terus bisa saja geser ke papan baru terus di taruh di papan baru itu ), jadi tidak perlu script banyak")
                    var listbidak = datagame.get_list_bidak_papan();
                    foreach (var item in listbidak)
                    {
                        GameObject gameobjbidak = null;
                        foreach (Transform item2 in children)
                        {
                            if (item2.name == item.namabidak)
                            {
                                gameobjbidak = item2.gameObject;
                                break;
                            }
                        }
                        Vector3 posisibaru = papanpilih.transform.position;
                        posisibaru[1] += (datagame.setting_ulang_posisi_bidak(item.namabidak) - 1);
                        if (item.namabidak.Contains("black_") && !(item.namabidak.Equals("black_31")))
                        {
                            posisibaru[0] += 0.7f;
                        }
                        gameobjbidak.transform.position = posisibaru;
                    }
                    datagame.updatepapan();
                    datagame.resetdata();
                    if(datagame.posisi_paling_awal_papan != datagame.selectedpapan)
                    {
                        datagame.currentplayer *= -1;
                    }
                }
                void geser_bidak_ke_papan_baru(string namabidak)
                {
                    GameObject currselect = null;
                    foreach (Transform x in children)
                    {
                        if (x.name == namabidak)
                        {
                            currselect = x.gameObject;
                            break;
                        }
                    }
                    Vector3 papanposition = papanpilih.transform.position;
                    papanposition[1] = currselect.transform.position.y;
                    if (currselect.name.Contains("black_") && !(currselect.name.Equals("black_31")))
                    {
                        papanposition[0] += 0.7f;
                    }
                    currselect.transform.position = papanposition;
                }
                if (hasiltaruh == -200)
                {
                    //jika return -200 maka bidak digeser ke tempat lain

                    //menggeser yang di select dulu
                    geser_bidak_ke_papan_baru(datagame.selectedbidak);
                    var currootchild = datagame.childrenselectedbidak;
                    if (datagame.childcount != 0)
                    {
                        //jika childcount != 0 maka benar" masih ada child nya, tapi jika 0 maka yang di childrenselectedbidak sebenarnya sudah terunselect
                        while (currootchild != null)
                        {
                            //menggeser childrennya
                            geser_bidak_ke_papan_baru(currootchild.namabidak);
                            currootchild = currootchild.children;
                        }
                        //meleepas bidak paling bawah(setelah geser maka bidak children paling bawah dilepas)
                        datagame.lepas_children_akhir_bidak("beda");
                        //setting ulang posisi y bidak

                        //get dulu semua bidak di papan yang diselect 
                        var listbidak = datagame.get_list_bidak_papan("spesial");
                        foreach (var item in listbidak)
                        {
                            GameObject gameobjbidak = null;
                            foreach (Transform item2 in children)
                            {
                                if (item2.name == item.namabidak)
                                {
                                    gameobjbidak = item2.gameObject;
                                    break;
                                }
                            }
                            Vector3 posisibaru = papanpilih.transform.position;
                            posisibaru[1] += (datagame.setting_ulang_posisi_bidak(item.namabidak) - 1);
                            if (item.namabidak.Contains("black_") && !(item.namabidak.Equals("black_31")))
                            {
                                posisibaru[0] += 0.7f;
                            }
                            gameobjbidak.transform.position = posisibaru;
                        }
                    }
                    else
                    {
                        datagame.setting_parent_bidak_di_papan_pilih_untuk_selected_bidak();
                        for (int i = 0; i < children.childCount; i++)
                        {
                            if (children.GetChild(i).name.Equals(datagame.selectedbidak))
                            {
                                var posisibidak = children.GetChild(i).transform.position;
                                posisibidak[1] -= 1.5f;
                                children.GetChild(i).transform.position = posisibidak;
                                break;
                            }
                        }
                        //setting ulang posisi y bidak

                        //get dulu semua bidak di papan yang diselect 
                        var listbidak = datagame.get_list_bidak_papan();
                        foreach (var item in listbidak)
                        {
                            GameObject gameobjbidak = null;
                            foreach (Transform item2 in children)
                            {
                                if (item2.name == item.namabidak)
                                {
                                    gameobjbidak = item2.gameObject;
                                    break;
                                }
                            }
                            Vector3 posisibaru = papanpilih.transform.position;
                            posisibaru[1] += (datagame.setting_ulang_posisi_bidak(item.namabidak) - 1);
                            if (item.namabidak.Contains("black_") && !(item.namabidak.Equals("black_31")))
                            {
                                posisibaru[0] += 0.7f;
                            }
                            gameobjbidak.transform.position = posisibaru;
                        }
                        datagame.updatepapan();
                        datagame.resetdata();
                        datagame.currentplayer *= -1;
                    }
                }
            }
        }
    }
    private void OnMouseUp()
    {
        klikpapan(this.gameObject);
    }
}
