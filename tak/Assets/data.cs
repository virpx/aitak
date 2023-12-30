using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static data;

public class data : MonoBehaviour
{
    public class Bidak
    {
        public bool iscapstone;
        public Bidak parent = null;
        public Bidak children = null;
        //lokasi papan diisi nama papan;
        public string lokasipapan = "";
        //1 = BIDAK BIASA
        //2 = WALL/BIDAK BERDIRI
        public int penomoran;
        public string namabidak;
        public Bidak(bool iscapstone, string nama,int penomoran)
        {
            this.iscapstone = iscapstone;
            this.namabidak = nama;
            this.penomoran = penomoran; 
        }

    }
    //buat ngecek kemenangan pake papan ini
    public int[,] papan_cek =
    {
        {0,0,0,0,0,0},
        {0,0,0,0,0,0},
        {0,0,0,0,0,0},
        {0,0,0,0,0,0},
        {0,0,0,0,0,0},
        {0,0,0,0,0,0},
    };
    //buat papan permaianannya, dibuat menjadi 1 deret jadi tidak 2 dimensi, misal y=1 x=0 jadi index ke 6
    public List<Bidak> papan_game = new List<Bidak>();
    public List<Bidak> bidakplayer = new List<Bidak>();
    public string selectedbidak;

    /*variabel buat geser bidak di papan*/
    public string statejalan = "";
    public Bidak childrenselectedbidak = null;
    //untuk mencatat memilih menjalankan bidak ke mana : kanan/kiri/atas/bawah
    public string pilih_jalan_geser_bidak = "";
    /*ini buat mencatat bidak yang dilepas ketika menggeser bidak, berguna untuk mensetting parent nya nanti*/
    public Bidak bidakyangdilepas = null;
    public int childcount = 0;
    public string posisi_papan_awal = "";
    //untuk menlihat apakah return -101 tidak merubah posisi bidak
    public string posisi_paling_awal_papan = "";
    /*diisi bersamaan dengan pengecekan highlight papan di milihpapan.cs*/
    public List<string> possiblepapan = null;
    /*=========================================*/

    //jika pada game merubah ke wall maka dimasukkan ke sini, inisialisasi awal semuanya capstone biasa (bukan wall)
    public int updatepenomoranbidak;
    public string selectedpapan;
    //currentplayer : 1 = white , -1 = black
    public int currentplayer = 1;
    //untuk game paling awal, kan 1 hitam sama 1 putih ditaruh dulu
    public int countergame = 1;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 36; i++)
        {
            papan_game.Add(null);
        }
        //generate player putih
        for(int i = 0; i < 30; i++)
        {
            bidakplayer.Add(new Bidak(false, "white_" + (i+1),1));
        }
        bidakplayer.Add(new Bidak(true, "white_31", 1));
        //generate player hitam
        for (int i = 0; i < 30; i++)
        {
            bidakplayer.Add(new Bidak(false, "black_" + (i + 1),1));
        }
        bidakplayer.Add(new Bidak(true, "black_31",1));
    }
    public void updatepapan()
    {
        int counter = 0;
        for(int y= 0; y <6;y++)
        {
            for(int x= 0; x < 6; x++)
            {
                Bidak cek = null; 
                foreach(var xs in bidakplayer)
                {
                    if(xs.lokasipapan == "papan" + (y+1) + "_" + (x+1))
                    {
                        cek = xs;
                        break;
                    }
                }
                if(cek != null)
                {
                    while(cek.parent != null)
                    {
                        cek = cek.parent;
                    }
                }
                int datapapancek = -1;
                if(cek != null )
                {
                    if (cek.namabidak.Contains("white"))
                    {
                        datapapancek = 0;
                        if(cek.penomoran == 2)
                        {
                            datapapancek = 1;
                        }
                        if(cek.iscapstone == true)
                        {
                            datapapancek = 2;
                        }
                    }
                    else
                    {
                        datapapancek = 3;
                        if (cek.penomoran == 2)
                        {
                            datapapancek = 4;
                        }
                        if (cek.iscapstone == true)
                        {
                            datapapancek = 5;
                        }
                    }
                }
                papan_cek[y,x] = datapapancek;
                papan_game[counter] = cek;
                counter++;
            }
        }
        int rows = papan_cek.GetLength(0);
        int columns = papan_cek.GetLength(1);

        Debug.Log("Logging 2D Array:");

        for (int i = 0; i < rows; i++)
        {
            string rowString = "";
            for (int j = 0; j < columns; j++)
            {
                rowString += papan_cek[i, j] + " ";
            }
            Debug.Log(rowString);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public string get_bidak_atas(string bidakcari)
    {
        var nama = "";
        Bidak currbidak = null;
        foreach(var bidak in bidakplayer)
        {
            if(bidak.namabidak == bidakcari)
            {
                currbidak = bidak;
                break;
            }
        }
        while(currbidak.parent != null)
        {
            currbidak = currbidak.parent;
        }
        nama = currbidak.namabidak;
        return nama;
    }
    public void resetdata()
    {
        statejalan = "";
        childrenselectedbidak = null;
        pilih_jalan_geser_bidak = "";
        bidakyangdilepas = null;
        childcount = 0;
        posisi_papan_awal = "";
        possiblepapan = null;
        selectedbidak = "";
        selectedpapan = "";
    }
    public List<Bidak> get_list_bidak_papan(string req = "")
    {
        List<Bidak> list = new List<Bidak>();
        if(req == "spesial")
        {
            List<string> lbidakskip = new List<string>();
            if(childcount != 0)
            {
                //jika childcount nya sudah 0 maka yang di childrenselectedbidak tidak berlaku maka tidak perlu di masukkan ke skip
                var currroot = childrenselectedbidak;
                while (currroot != null)
                {
                    lbidakskip.Add(currroot.namabidak);
                    currroot = currroot.children;
                }
            }
            lbidakskip.Add(selectedbidak);
            //supaya yang selectedbidak dan children yang belum fix di papan itu tidak masuk
            //atau kata lain membenarkan posisi bidak paling akhir dengan bidak yang sudah ada di papan itu (jika ada)
            foreach (var x in bidakplayer)
            {
                if (x.lokasipapan == selectedpapan)
                {
                    int masuk = 1;
                    foreach(var cek in lbidakskip)
                    {
                        if(x.namabidak == cek)
                        {
                            masuk = 0;
                            break;
                        }
                    }
                    if(masuk == 1)
                    {
                        list.Add(x);
                    }
                }
            }
        }
        else
        {
            foreach (var x in bidakplayer)
            {
                if (x.lokasipapan == selectedpapan)
                {
                    list.Add(x);
                }
            }
        }
        return list;
    }
    public float setting_ulang_posisi_bidak(string namabidake)
    {
        float hasil = 0f;
        foreach (var x in bidakplayer)
        {
            if(x.namabidak == namabidake)
            {
                var childrene = x.children;
                if(childrene == null)
                {
                    //children paling bawah seperti menaruh di papan tanpa tumpuk
                    hasil = 1.5f;
                }
                else
                {
                    int jumlahchilde = 0;
                    while (childrene != null)
                    {
                        jumlahchilde++;
                        childrene = childrene.children; 
                    }
                    hasil = (jumlahchilde * 0.5f) + 1.5f;
                }
                break;
            }
        }
        return hasil;
    }
    public void setting_parent_bidak_di_papan_pilih_untuk_selected_bidak()
    {
        //function ini berguna untuk mensetting parent dari bidak yang ada di papan yang dipilih, hanya umtuk bidak yang sedang dipilih
        //(karena untuk setiap childrennya sudah di setting di lepas_children_akhir_bidak tidak dibuat satu karena bentrok dengan kode di milihpapan)
        Bidak bidakselected = null;
        foreach(var x in bidakplayer)
        {
            if(x.namabidak == selectedbidak)
            {
                bidakselected = x;
                break;
            }
        }
        foreach (var ambil in bidakplayer)
        {
            //diber if ambil.namabidak != selectedbidak karena selected bidak sudah kegeser jadi kalo tidak diberi ini akan keselect yang selected bidak
            if (ambil.lokasipapan == selectedpapan && ambil.parent == null && ambil.namabidak != selectedbidak)
            {
                bidakselected.children = ambil;
                //ambil paling atas terus parentnya diganti dengan bidak yang ingin dilepas
                ambil.parent = bidakselected;
            }
        }
    }
    public void lepas_children_akhir_bidak(string from)
    {
        //mencari bidak yang sudah ada di papan 
        var currroot = childrenselectedbidak;
        while (currroot.children != null)
        {
            /*untuk mengambil children paling akhir*/
            currroot = currroot.children;
        };
        if(from == "sama")
        {
            //jika function ini dipanggil dari pelepasan bidak di tempat yang sama 

            //jika ditumpuk lagi kita set parent dari bidak sebelumnya yang dilepas ke bidak yang akan dilepas
            if (bidakyangdilepas != null)
            {
                bidakyangdilepas.parent = currroot;
                currroot.children = bidakyangdilepas;
            }
        }
        else
        {
            //jika function ini dipanggil dari pelepasan bidak dengan menggeser
            foreach(var ambil in bidakplayer)
            {
                //diber if ambil.namabidak != selectedbidak karena selected bidak sudah kegeser jadi kalo tidak diberi ini akan keselect yang selected bidak
                if (ambil.lokasipapan == selectedpapan && ambil.parent == null && ambil.namabidak != selectedbidak)
                {
                    currroot.children = ambil;
                    //ambil paling atas terus parentnya diganti dengan bidak yang ingin dilepas
                    ambil.parent = currroot;
                    break;
                }
            }
        }
        childcount--;
        bidakyangdilepas = currroot;
        //melepas parent dari children yang dilepas
        bidakyangdilepas.parent.children = null;
        bidakyangdilepas.parent = null;
    }
    public float taruhbidak()
    {
        if (get_namapapan_dari_bidak(selectedbidak) == "")
        {
            //untuk menaruh bidak baru ke papan

            //cek apakah tempat yang ingin ditaruh valid
            int bisa = 0;
            for (int i = 0; i < possiblepapan.Count; i++)
            {
                if (possiblepapan[i] == selectedpapan)
                {
                    bisa = 1;
                    break;
                }
            }
            if (bisa == 1)
            {
                Bidak bidakchildren = null;
                //mencari dulu bidak yang sudah ada di papan tersebut (dicari yang paling atas/ variabel parent pada bidak null)
                for (int i = 0; i < bidakplayer.Count; i++)
                {
                    if (bidakplayer[i].lokasipapan.Equals(selectedpapan))
                    {
                        if (bidakplayer[i].parent == null)
                        {
                            bidakchildren = bidakplayer[i];
                            break;
                        }
                    }
                }
                for (int i = 0; i < bidakplayer.Count; i++)
                {
                    if (bidakplayer[i].namabidak.Contains(selectedbidak))
                    {
                        //pengecekan bisa ditaruh pake possiblepapan
                        if (bidakchildren != null)
                        {
                            //untuk proses menumpuk bidak
                            bidakplayer[i].children = bidakchildren;
                            bidakplayer[i].penomoran = updatepenomoranbidak;
                            bidakplayer[i].lokasipapan = selectedpapan;
                            bidakchildren.parent = bidakplayer[i];
                            int counterchild = 0;
                            var bidaksekarang = bidakplayer[i];
                            float posisi = 0;
                            while (bidaksekarang.children != null)
                            {
                                counterchild++;
                                bidaksekarang = bidaksekarang.children;
                            }

                            posisi = (counterchild * 0.5f) + 1.5f;
                            return posisi;
                        }
                        else if (bidakchildren == null)
                        {
                            //untuk proses menaruh bidak tanpa menumpuk
                            bidakplayer[i].penomoran = updatepenomoranbidak;
                            bidakplayer[i].lokasipapan = selectedpapan;
                            return 1.5f;
                        }
                        break;
                    }
                }
            }
            /*jika -1 gagal taruh bidak*/
            return -1;
        }
        else
        {
            Bidak objselectedbidak = null;
            foreach (var bidak in bidakplayer)
            {
                if (bidak.namabidak == selectedbidak)
                {
                    objselectedbidak = bidak;
                    break;
                }
            }
            if (selectedpapan == objselectedbidak.lokasipapan)
            {
                if (childcount == 0)
                {
                    //untuk menaruh bidak yang paling atas
                    for (int i = 0; i < bidakplayer.Count; i++)
                    {
                        if (bidakplayer[i].namabidak == selectedbidak)
                        {
                            //mengambalikan children sebelumnya
                            bidakplayer[i].children = childrenselectedbidak;
                            if (bidakplayer[i].children != null)
                            {
                                bidakplayer[i].children.parent = bidakplayer[i];
                            }
                            break;
                        }
                    }
                    return -101;
                }
                else
                {
                    lepas_children_akhir_bidak("sama");
                    return -100;
                }
            }
            else
            {
                //cek apakah tempat yang ingin ditaruh valid
                int bisa = 0;
                for (int i = 0; i < possiblepapan.Count; i++)
                {
                    if (possiblepapan[i] == selectedpapan)
                    {
                        bisa = 1;
                        break;
                    }
                }
                if (bisa == 1)
                {
                    if (pilih_jalan_geser_bidak == "")
                    {
                        //awal geser bidak, jadi belum menentukan posisi yang diambil (kanan/kiri/atas/bawah)
                        int posisitujuan = int.Parse((selectedpapan.Split("_"))[1]);
                        int posisiawal = int.Parse((posisi_papan_awal.Split("_"))[1]);
                        if (posisitujuan > posisiawal)
                        {
                            pilih_jalan_geser_bidak = "kanan";
                        }
                        else if (posisitujuan < posisiawal)
                        {
                            pilih_jalan_geser_bidak = "kiri";
                        }
                        else if (posisiawal == posisitujuan)
                        {
                            posisitujuan = int.Parse(((selectedpapan.Split("_"))[0]).Replace("papan", ""));
                            posisiawal = int.Parse(((posisi_papan_awal.Split("_"))[0]).Replace("papan", ""));
                            if (posisitujuan > posisiawal)
                            {
                                pilih_jalan_geser_bidak = "bawah";
                            }
                            else if (posisitujuan < posisiawal)
                            {
                                pilih_jalan_geser_bidak = "atas";
                            }
                        }
                    }
                    foreach (var x in bidakplayer)
                    {
                        if (x.namabidak == selectedbidak)
                        {
                            //mengganti posisi papan dari bidak yang digeser
                            x.lokasipapan = selectedpapan;
                            break;
                        }
                    }
                    if(childcount != 0)
                    {
                        var currbidak = childrenselectedbidak;
                        while (currbidak != null)
                        {
                            currbidak.lokasipapan = selectedpapan;
                            currbidak = currbidak.children;
                        }
                    }
                    posisi_papan_awal = selectedpapan;
                    return -200;
                }
                return -1;
            }
        }
    }
    public bool possibe_bidak_aman(string namapapan)
    {
        bool hasil = true;
        // dicek apakah di tempat itu ada capstone / wall
        foreach (var x in bidakplayer)
        {
            if (x.lokasipapan == selectedpapan && x.parent == null)
            {
                if (x.iscapstone == true || x.penomoran == 2)
                {
                    hasil = false;
                }
                break;
            }
        }
        return hasil;
    }
    public string get_namapapan_dari_bidak(string namabidak)
    {
        string hasil = null;
        for (int i = 0; i < bidakplayer.Count; i++)
        {
            if (bidakplayer[i].namabidak.Contains(namabidak))
            {
                hasil = bidakplayer[i].lokasipapan;
            }
        }
        return hasil;
    }
    public void setchildselectbidak()
    {
        childrenselectedbidak = null;
        for(int i = 0;i < bidakplayer.Count; i++)
        {
            if (bidakplayer[i].namabidak == selectedbidak)
            {
                childrenselectedbidak = bidakplayer[i].children;
                break;
            }
        }
        if(childrenselectedbidak == null)
        {
            childcount = 0;
        }
        else
        {
            childcount = 0;
            var tmproot = childrenselectedbidak;
            while(tmproot != null)
            {
                childcount++;
                tmproot = tmproot.children;
            }
        }
    }
}
