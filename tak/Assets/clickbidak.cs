using System.Collections.Generic;
using UnityEngine;

public class clickbidak : MonoBehaviour
{
    public GameObject listbidak;
    public data datagame;
    public GameObject parentpapan;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //untuk mereset semua papannya supaya tidak tampil dulu (ini untuk mereset ketika papan sudah terhiglight sebelumnya)
        for (int i = 0; i < parentpapan.transform.childCount; i++)
        {
            var objpapan = parentpapan.transform.GetChild(i);
            (objpapan.transform.GetComponent<MeshRenderer>()).enabled = false;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            //untuk memberikan highlight pada papan ketika sudah memilih bidak
            if (datagame.selectedbidak != "")
            {
                if(datagame.childrenselectedbidak == null && datagame.get_namapapan_dari_bidak(datagame.selectedbidak) == "")
                {
                    //mencari papan yang bisa ditaruh (yang belum ada bidaknya)
                    List<string> possiblepapan = new List<string>();
                    for(int y = 1; y <= 6; y++)
                    {
                        for (int x = 1; x <= 6; x++)
                        {
                            int jos = 1;
                            var namapapan = "papan"+y+"_"+x;
                            foreach(var cek in datagame.bidakplayer)
                            {
                                if(cek.lokasipapan == namapapan)
                                {
                                    jos = 0;
                                    break;
                                }
                            }
                            if (jos == 1)
                            {
                                possiblepapan.Add(namapapan);
                            }
                        }
                    }
                    datagame.possiblepapan = possiblepapan;
                    int bisa = 0;
                    string papankenahover = "";
                    for (int i = 0; i < possiblepapan.Count; i++)
                    {
                        if (hit.transform.name.Contains("papan"))
                        {
                            //jika yang dihover langsung papan maka papan yang dihover langsung diambil
                            papankenahover = hit.transform.name;
                        }
                        if (hit.transform.name.Contains("white_") || hit.transform.name.Contains("black_"))
                        {
                            //jika yang dihover bidak maka akan manggil function untuk dapat nama papannya
                            papankenahover = datagame.get_namapapan_dari_bidak(hit.transform.name);
                        }
                        //untuk pengecekan apakah papan bisa ditaruh bidak ada di data.cs disini hanya melakukan cek highlight papan
                        //jadi ketika ke hover papan akan tercatat selectedpapan tapi ditak terhighlight jika tidak bisa diletak i
                        datagame.selectedpapan = papankenahover;
                        if (possiblepapan[i] == papankenahover)
                        {
                            bisa = 1;
                            break;
                        }
                    }
                    if(bisa == 1)
                    {
                        for (int i = 0; i < parentpapan.transform.childCount; i++)
                        {
                            var objpapan = parentpapan.transform.GetChild(i);
                            if (objpapan.name == papankenahover)
                            {
                                (objpapan.transform.GetComponent<MeshRenderer>()).enabled = true;
                            }
                            else
                            {
                                (objpapan.transform.GetComponent<MeshRenderer>()).enabled = false;
                            }
                        }
                    }
                }
                else
                {
                    //jika yang dipilih yang di papan
                    List<string> possiblepapan = new List<string>();
                    if(datagame.pilih_jalan_geser_bidak == "")
                    {
                        //jika awal geser maka yang bisa adalah kanan kiri atas bawah
                        var papansekarang = datagame.get_namapapan_dari_bidak(datagame.selectedbidak);
                        //membuat possible papan secara horizontal
                        int kolompapan = int.Parse(papansekarang.Split("_")[1]);
                        possiblepapan.Add(papansekarang);
                        if (kolompapan + 1 <= 6)
                        {
                            //pengecekan apakah di papna itu ada capstone/wall
                            var namapapantmp = papansekarang.Split("_")[0] + "_" + (kolompapan + 1);
                            if (datagame.possibe_bidak_aman(namapapantmp))
                            {
                                possiblepapan.Add(namapapantmp);
                            }
                        }
                        if (kolompapan - 1 >= 1)
                        {
                            var namapapantmp = papansekarang.Split("_")[0] + "_" + (kolompapan - 1);
                            if (datagame.possibe_bidak_aman(namapapantmp))
                            {
                                possiblepapan.Add(namapapantmp);
                            }
                        }
                        int barispapan = int.Parse(papansekarang.Split("_")[0].Replace("papan", ""));
                        if (barispapan + 1 <= 6)
                        {
                            var namapapantmp = "papan" + (barispapan + 1) + "_" + papansekarang.Split("_")[1];
                            if (datagame.possibe_bidak_aman(namapapantmp))
                            {
                                possiblepapan.Add(namapapantmp);
                            }
                        }
                        if (barispapan - 1 >= 1)
                        {
                            var namapapantmp = "papan" + (barispapan - 1) + "_" + papansekarang.Split("_")[1];
                            if (datagame.possibe_bidak_aman(namapapantmp))
                            {
                                possiblepapan.Add(namapapantmp);
                            }
                        }
                    }
                    else
                    {
                        var papansekarang = datagame.get_namapapan_dari_bidak(datagame.selectedbidak);
                        possiblepapan.Add(papansekarang);
                        //jika bukan awal geser maka mengikuti arah geser yang dipilih kanan /kiri/atas/bawah
                        if (datagame.pilih_jalan_geser_bidak == "kanan")
                        {
                            int kolompapan = int.Parse(papansekarang.Split("_")[1]);
                            if (kolompapan + 1 <= 6)
                            {
                                var namapapantmp = papansekarang.Split("_")[0] + "_" + (kolompapan + 1);
                                if (datagame.possibe_bidak_aman(namapapantmp))
                                {
                                    possiblepapan.Add(namapapantmp);
                                }
                            }
                        } else if (datagame.pilih_jalan_geser_bidak == "kiri")
                        {
                            int kolompapan = int.Parse(papansekarang.Split("_")[1]);
                            if (kolompapan - 1 >= 1)
                            {
                                var namapapantmp = papansekarang.Split("_")[0] + "_" + (kolompapan - 1);
                                if (datagame.possibe_bidak_aman(namapapantmp))
                                {
                                    possiblepapan.Add(namapapantmp);
                                }
                            }
                        }
                        else if (datagame.pilih_jalan_geser_bidak == "atas")
                        {
                            int barispapan = int.Parse(papansekarang.Split("_")[0].Replace("papan", ""));
                            if (barispapan + 1 <= 6)
                            {
                                var namapapantmp = "papan" + (barispapan - 1) + "_" + papansekarang.Split("_")[1];
                                if (datagame.possibe_bidak_aman(namapapantmp))
                                {
                                    possiblepapan.Add(namapapantmp);
                                }
                            }
                        }
                        else if (datagame.pilih_jalan_geser_bidak == "bawah")
                        {
                            int barispapan = int.Parse(papansekarang.Split("_")[0].Replace("papan", ""));
                            if (barispapan - 1 >= 1)
                            {
                                var namapapantmp = "papan" + (barispapan + 1) + "_" + papansekarang.Split("_")[1];
                                if (datagame.possibe_bidak_aman(namapapantmp))
                                {
                                    possiblepapan.Add(namapapantmp);
                                }
                            }
                        }
                    }
                    
                    datagame.possiblepapan = possiblepapan;
                    int bisa = 0;
                    string papankenahover = "";
                    for (int i = 0; i < possiblepapan.Count; i++)
                    {
                        if (hit.transform.name.Contains("papan"))
                        {
                            //jika yang dihover langsung papan maka papan yang dihover langsung diambil
                            papankenahover = hit.transform.name;
                        }
                        if (hit.transform.name.Contains("white_") || hit.transform.name.Contains("black_"))
                        {
                            //jika yang dihover bidak maka akan manggil function untuk dapat nama papannya
                            papankenahover = datagame.get_namapapan_dari_bidak(hit.transform.name);
                        }
                        //untuk pengecekan apakah papan bisa ditaruh bidak ada di data.cs disini hanya melakukan cek highlight papan
                        //jadi ketika ke hover papan akan tercatat selectedpapan tapi ditak terhighlight jika tidak bisa diletak i
                        datagame.selectedpapan = papankenahover;
                        if (possiblepapan[i] == papankenahover)
                        {
                            bisa = 1;
                            break;
                        }
                    }
                    if(bisa == 1)
                    {
                        for (int i = 0; i < parentpapan.transform.childCount; i++)
                        {
                            var objpapan = parentpapan.transform.GetChild(i);
                            if (objpapan.name == papankenahover)
                            {
                                (objpapan.transform.GetComponent<MeshRenderer>()).enabled = true;
                            }
                            else
                            {
                                (objpapan.transform.GetComponent<MeshRenderer>()).enabled = false;
                            }
                        }
                    }

                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if(datagame.countergame == 1)
                {
                    //untuk hitam jalan dulu, tidak boleh rotate
                    if (hit.transform.name.Contains("black_") && hit.transform.name != "black_31")
                    {
                        if (datagame.selectedbidak == "")
                        {
                            datagame.selectedbidak = hit.transform.name;
                            var posisibidak = hit.transform.position;
                            posisibidak[1] += 1;
                            hit.transform.position = posisibidak;
                            datagame.updatepenomoranbidak = 1;
                        }
                        else
                        {
                            if(hit.transform.name != datagame.selectedbidak)
                            {
                                if (datagame.get_namapapan_dari_bidak(hit.transform.name) == "")
                                {
                                    //jika bibdak yang di klik beda dan tidak berada di papan jadi unselect
                                    var children = listbidak.transform;
                                    for (int i = 0; i < children.childCount; i++)
                                    {
                                        if (children.GetChild(i).name.Equals(datagame.selectedbidak))
                                        {
                                            if (datagame.updatepenomoranbidak == 2)
                                            {
                                                children.GetChild(i).transform.eulerAngles = new Vector3(-90, 90, -90);
                                            }
                                            var posisibidak = children.GetChild(i).transform.position;
                                            posisibidak[1] -= 1;
                                            children.GetChild(i).transform.position = posisibidak;
                                            datagame.selectedbidak = "";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //tidak diberikan deteksi klik di bidak untuk menaruh karena pasti taruh di papan yang kosong (belum ada bidaknya)
                }else if(datagame.countergame == 2)
                {
                    //untuk hitam jalan dulu, tidak boleh rotate
                    if (hit.transform.name.Contains("white_") && hit.transform.name != "white_31")
                    {
                        if (datagame.selectedbidak == "")
                        {
                            datagame.selectedbidak = hit.transform.name;
                            var posisibidak = hit.transform.position;
                            posisibidak[1] += 1;
                            hit.transform.position = posisibidak;
                            datagame.updatepenomoranbidak = 1;
                        }
                        else
                        {
                            if (hit.transform.name != datagame.selectedbidak)
                            {
                                if (datagame.get_namapapan_dari_bidak(hit.transform.name) == "")
                                {
                                    //jika bibdak yang di klik beda dan tidak berada di papan jadi unselect
                                    var children = listbidak.transform;
                                    for (int i = 0; i < children.childCount; i++)
                                    {
                                        if (children.GetChild(i).name.Equals(datagame.selectedbidak))
                                        {
                                            if (datagame.updatepenomoranbidak == 2)
                                            {
                                                children.GetChild(i).transform.eulerAngles = new Vector3(-90, 90, -90);
                                            }
                                            var posisibidak = children.GetChild(i).transform.position;
                                            posisibidak[1] -= 1;
                                            children.GetChild(i).transform.position = posisibidak;
                                            datagame.selectedbidak = "";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //tidak diberikan deteksi klik di bidak untuk menaruh karena pasti taruh di papan yang kosong (belum ada bidaknya)
                }
                else
                {
                    //untuk click bidaknya
                    if (hit.transform.name.Contains("white_") || hit.transform.name.Contains("black"))
                    {
                        if (datagame.selectedbidak == "")
                        {
                            datagame.selectedbidak = hit.transform.name;
                            if (datagame.get_namapapan_dari_bidak(datagame.selectedbidak) == "")
                            {
                                int jalan = 0;
                                //pengecekan giliran
                                if(datagame.currentplayer == 1 && hit.transform.name.Contains("white"))
                                {
                                    jalan = 1;
                                }
                                if (datagame.currentplayer == -1 && hit.transform.name.Contains("black"))
                                {
                                    jalan = 1;
                                }
                                if (jalan == 1)
                                {
                                    //berarti click bidak baru (tidak dipapan)
                                    var posisibidak = hit.transform.position;
                                    posisibidak[1] += 1;
                                    hit.transform.position = posisibidak;
                                    datagame.updatepenomoranbidak = 1;
                                }
                                else
                                {
                                    datagame.selectedbidak = "";
                                }
                            }
                            else
                            {
                                //get bidak paling atas dari papan tersebut
                                var getpalingatas = datagame.get_bidak_atas(hit.transform.name);
                                int jalan = 0;
                                //pengecekan giliran
                                if (datagame.currentplayer == 1 && getpalingatas.Contains("white"))
                                {
                                    jalan = 1;
                                }
                                if (datagame.currentplayer == -1 && getpalingatas.Contains("black"))
                                {
                                    jalan = 1;
                                }
                                if (jalan == 1)
                                {
                                    datagame.selectedbidak = getpalingatas;
                                    //di get childrennya
                                    datagame.setchildselectbidak();
                                    //berarti click bidak yang ada di papan
                                    datagame.posisi_papan_awal = datagame.get_namapapan_dari_bidak(datagame.selectedbidak);
                                    datagame.posisi_paling_awal_papan = datagame.get_namapapan_dari_bidak(datagame.selectedbidak);
                                    //untuk menaikkan (select) bidakpalingatas
                                    var posisibidak = hit.transform.position;
                                    posisibidak[1] += 1;
                                    hit.transform.position = posisibidak;
                                    if (datagame.childrenselectedbidak != null)
                                    {
                                        //untuk menaikkan (select) bidak childrennya
                                        var currroot = datagame.childrenselectedbidak;
                                        do
                                        {
                                            for (int i = 0; i < listbidak.transform.childCount; i++)
                                            {
                                                if (listbidak.transform.GetChild(i).name == currroot.namabidak)
                                                {
                                                    posisibidak = listbidak.transform.GetChild(i).position;
                                                    posisibidak[1] += 1;
                                                    listbidak.transform.GetChild(i).position = posisibidak;
                                                    break;
                                                }
                                            }
                                            currroot = currroot.children;
                                        } while (currroot != null);
                                    }
                                }
                                else
                                {
                                    datagame.selectedbidak = "";
                                }
                            }
                        }
                        else
                        {
                            if (datagame.selectedbidak.Equals(hit.transform.name))
                            {
                                if (datagame.childrenselectedbidak == null && datagame.get_namapapan_dari_bidak(datagame.selectedbidak) == "")
                                {
                                    if (!(datagame.selectedbidak.Equals("black_31")) && !(datagame.selectedbidak.Equals("white_31")))
                                    {
                                        //jika yang diklik bidaknya sama dengan yang sedang aktif dan tidak berada di papan maka rotate, menjadi wall/kembali
                                        if (datagame.updatepenomoranbidak == 1)
                                        {
                                            //jika masih biasa (bukan wall)
                                            datagame.updatepenomoranbidak = 2;
                                            hit.transform.eulerAngles = new Vector3(0, 90, -90);
                                        }
                                        else
                                        {
                                            //jika sudah jadi wall
                                            datagame.updatepenomoranbidak = 1;
                                            hit.transform.eulerAngles = new Vector3(-90, 90, -90);
                                        }
                                    }
                                }
                                else
                                {
                                    //jika yang diklik bidak yang sama tetapi sudah ada di papan maka akan melakukan clickpapan
                                    //clickpapan berlaku hanya ketika menggeser bidak, karena kalau menaruh bida baru ke papan itu harus di papan yang kosong
                                    for (int i = 0; i < parentpapan.transform.childCount; i++)
                                    {
                                        if (parentpapan.transform.GetChild(i).name == datagame.selectedpapan)
                                        {
                                            parentpapan.transform.GetChild(i).gameObject.GetComponent<milihpapan>().klikpapan(parentpapan.transform.GetChild(i).gameObject);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (datagame.get_namapapan_dari_bidak(hit.transform.name) == "")
                                {
                                    //jika bibdak yang di klik beda dan tidak berada di papan jadi unselect
                                    var children = listbidak.transform;
                                    for (int i = 0; i < children.childCount; i++)
                                    {
                                        if (children.GetChild(i).name.Equals(datagame.selectedbidak))
                                        {
                                            if (datagame.updatepenomoranbidak == 2)
                                            {
                                                children.GetChild(i).transform.eulerAngles = new Vector3(-90, 90, -90);
                                            }
                                            var posisibidak = children.GetChild(i).transform.position;
                                            posisibidak[1] -= 1;
                                            children.GetChild(i).transform.position = posisibidak;
                                            datagame.selectedbidak = "";
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    //(script ditauh di 2 tempat yang berbeda yang satu di papan yang satu di click bidak 
                                    //karena kalau script hanya ditaruh di papan,
                                    //ketika ngeclick nya di bidak yang sudah ditaruh tidak bisa terdetek)
                                    //clickpapan berlaku hanya ketika menggeser bidak, karena kalau menaruh bida baru ke papan itu harus di papan yang kosong
                                    for (int i = 0; i < parentpapan.transform.childCount; i++)
                                    {
                                        if (parentpapan.transform.GetChild(i).name == datagame.selectedpapan)
                                        {
                                            parentpapan.transform.GetChild(i).gameObject.GetComponent<milihpapan>().klikpapan(parentpapan.transform.GetChild(i).gameObject);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
