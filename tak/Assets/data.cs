using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

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
    public Bidak childrenselectedbidak;
    //untuk mencatat memilih menjalankan bidak ke mana : kanan/kiri/atas/bawah
    public string pilih_jalan_geser_bidak = "";
    //jika pada game merubah ke wall maka dimasukkan ke sini, inisialisasi awal semuanya capstone biasa (bukan wall)
    public int updatepenomoranbidak;
    public string selectedpapan;
    //currentplayer : 1 = white , -1 = black
    public int currentplayer = 1;
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

    // Update is called once per frame
    void Update()
    {
        
    }
    public float taruhbidak()
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
                //pengecekan jika yang ingin ditumpuk itu bukan wall
                if (bidakchildren != null && bidakchildren.penomoran == 1)
                {
                    //untuk proses menumpuk bidak
                    bidakplayer[i].children = bidakchildren;
                    bidakplayer[i].penomoran = updatepenomoranbidak;
                    bidakplayer[i].lokasipapan = selectedpapan;
                    bidakchildren.parent = bidakplayer[i];
                    int childcount = 0;
                    var bidaksekarang = bidakplayer[i];
                    float posisi = 0;
                    while(bidaksekarang.children != null)
                    {
                        childcount++;
                        bidaksekarang = bidaksekarang.children;
                    }
                    if (childcount == 1)
                    {
                        //dibuat seperti ini karena ketika proses menumpuk bidak 1 aman tapi kalau bidak ke 2 ketinggian
                        posisi = 1.8f;
                    }
                    else
                    {
                        posisi = (childcount*1.3f);
                    }
                    currentplayer *= -1;
                    return posisi;
                }
                else if (bidakchildren == null)
                {
                    //untuk proses menaruh bidak tanpa menumpuk
                    bidakplayer[i].penomoran = updatepenomoranbidak;
                    bidakplayer[i].lokasipapan = selectedpapan;
                    currentplayer *= -1;
                    return 1.5f;
                }
                break;
            }
        }
        /*jika -1 gagal taruh bidak*/
        return -1;
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
        for(int i = 0;i < bidakplayer.Count; i++)
        {
            if (bidakplayer[i].namabidak == selectedbidak)
            {
                childrenselectedbidak = bidakplayer[i].children;
            }
        }
    }
}
