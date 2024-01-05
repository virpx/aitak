using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class aigame : MonoBehaviour
{
    public data datagame;
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

        public Bidak(bool iscapstone, string nama, int penomoran)
        {
            this.iscapstone = iscapstone;
            this.namabidak = nama;
            this.penomoran = penomoran;
        }
    }
    public class papanminimax
    {
        public List<Bidak> papan_game;
        public int[,] papan_cek = null;
        public papanminimax(List<Bidak> papan_game, int[,] papan_cek)
        {
            this.papan_game = papan_game;
            this.papan_cek = papan_cek;
        }
    }
    public class minimaxnode
    {
        public int value;
        public List<minimaxnode> children;
        public minimaxnode parent;
        public string state;
        public papanminimax curr;
        public int indexchildpilih = -1;
        public minimaxnode()
        {

        }
    }
    List<papanminimax> global_children_minimaxnode = new List<papanminimax>();
    // Start is called before the first frame update
    void Start()
    {
        // List<Bidak> papan = new List<Bidak>();
        // //bidak banyan berguna suupaya semua bidak di papan itu bisa tertaruh dengan kode while dibawah
        // // jadi setiap select nanti dibuatkan bidakbanyan dan setting parent dan children
        // var bidakbayagan = new Bidak(false, "bidak_bayangan", 1);
        // var bidak1 = new Bidak(false, "white1", 1);
        // var bidak2 = new Bidak(false, "white2", 1);
        // var bidak3 = new Bidak(false, "white3", 1);
        // bidak3.parent = bidak2;
        // bidak2.parent = bidak1;
        // bidak2.children = bidak3;
        // bidak1.children = bidak2;
        // bidak1.parent = bidakbayagan;
        // bidakbayagan.children = bidak1;
        // papan.Add(null);
        // papan.Add(null);
        // papan.Add(null);
        // papan.Add(null);
        // papan.Add(null);
        // papan.Add(null);
        // papan[0] = bidakbayagan;
        // int[,] papancek_e =
        // {
        //     {-1,-1,-1,-1,-1,-1},
        //     {-1,-1,-1,-1,-1,-1},
        //     {-1,-1,-1,-1,-1,-1},
        //     {-1,-1,-1,-1,-1,-1},
        //     {-1,-1,-1,-1,-1,-1},
        //     {-1,-1,-1,-1,-1,-1},
        // };
        // Debug.Log(history_cek_menang.Count);
        /*makepossible(papan);*/
        /*var catat = "";
        if (papan[0] == null)
        {
            catat += "null, ";
        }
        else
        {
            catat += papan[0].namabidak + ":" + papan[0].children.namabidak;
        }
        if (papan[1] == null)
        {
            catat += "null, ";
        }
        else
        {
            catat += papan[1].namabidak + ":" + papan[1].children.namabidak;
        }
        if (papan[2] == null)
        {
            catat += "null, ";
        }
        else
        {
            catat += papan[2].namabidak + ":" + papan[2].children.namabidak;
        }
        if (papan[3] == null)
        {
            catat += "null, ";
        }
        else
        {
            catat += papan[3].namabidak + ":" + papan[3].children.namabidak;
        }
        Debug.Log(catat);
        generate
        kombinasigerakkiri(bidakbayagan, papan, 4);*/
    }
    // Update is called once per frame
    void Update()
    {
        if (datagame.currentplayer == -1)
        {
            if (datagame.aiberpikir == 0)
            {
                if (datagame.countergame > 2)
                {
                    datagame.aiberpikir = 1;
                    //dibuat supaya data papan_game menjadi class Bidak yang internal
                    List<Bidak> papanai = new List<Bidak>();
                    foreach (var x in datagame.papan_game)
                    {
                        if (x == null)
                        {
                            papanai.Add(null);
                        }
                        else
                        {
                            papanai.Add(clonebidak_datagame(x));
                        }
                    }
                    //ai warna hitam
                    //ply1
                    minimaxnode rootminimax = new minimaxnode();
                    var papancurr = clonepapan(papanai);
                    rootminimax.curr = new papanminimax(papancurr, datagame.papan_cek);
                    rootminimax.state = "max";
                    rootminimax.parent = null;
                    global_children_minimaxnode.Clear();
                    makepossible(papancurr, "black_");
                    //setting children
                    List<minimaxnode> childrennya = new List<minimaxnode>();
                    foreach (var xx in global_children_minimaxnode)
                    {
                        minimaxnode baru = new minimaxnode();
                        baru.state = "min";
                        baru.curr = new papanminimax(xx.papan_game, xx.papan_cek);
                        baru.parent = rootminimax;
                        childrennya.Add(baru);
                    }
                    rootminimax.children = childrennya;
                    for (int i = 0; i < rootminimax.children.Count; i++)
                    {
                        var papan_curr_children = clonepapan(rootminimax.children[i].curr.papan_game);
                        global_children_minimaxnode.Clear();
                        makepossible(papan_curr_children, "white_");
                        List<minimaxnode> childrennya_children = new List<minimaxnode>();
                        foreach (var yy in global_children_minimaxnode)
                        {
                            minimaxnode node_baru = new minimaxnode();
                            node_baru.state = "terminal";
                            node_baru.curr = new papanminimax(yy.papan_game, yy.papan_cek);
                            node_baru.parent = rootminimax.children[i];
                            childrennya_children.Add(node_baru);
                        }
                        rootminimax.children[i].children = childrennya_children;
                    }
                    var papangame = rootminimax.children[0].children[0].curr.papan_game;
                    var papancek_e = clonepapancek(rootminimax.children[0].children[0].curr.papan_cek);
                    // int[,] papancek_e =
                    // {
                    //     {0,0,0,0,0,-1},
                    //     {3,3,3,3,3,3},
                    //     {3,3,3,3,3,3},
                    //     {3,3,3,3,3,3},
                    //     {3,3,3,3,3,3},
                    //     {3,3,3,3,3,3},
                    // };
                    var cari = "white";
                    if (cari == "white")
                    {
                        for (int y = 0; y < 6; y++)
                        {
                            for (int x = 0; x < 6; x++)
                            {
                                if (papancek_e[y, x] == -1)
                                {
                                    papancek_e[y, x] = 0;
                                }
                            }
                        }
                    }
                    else if (cari == "black")
                    {
                        for (int y = 0; y < 6; y++)
                        {
                            for (int x = 0; x < 6; x++)
                            {
                                if (papancek_e[y, x] == -1)
                                {
                                    papancek_e[y, x] = 3;
                                }
                            }
                        }
                    }
                    var papancek_bck = clonepapancek(papancek_e);
                    for (int y = 0; y < 6; y++)
                    {
                        papancek_e = clonepapancek(papancek_bck);
                        //kiri ke kanan
                        var jalan = true;
                        var x = 0;
                        while (jalan)
                        {
                            if (x > 5)
                            {
                                jalan = false;
                            }
                            else
                            {
                                var jumlahcarry = 0;
                                var bidaktmp = papangame[((6 * y) + x)];
                                if (bidaktmp != null)
                                {
                                    while (bidaktmp.children != null)
                                    {
                                        bidaktmp = bidaktmp.children;
                                    }
                                    var whilecari = true;
                                    while (whilecari)
                                    {
                                        if (!bidaktmp.namabidak.Contains(cari))
                                        {
                                            if (bidaktmp.parent == null)
                                            {
                                                whilecari = false;
                                            }
                                            else
                                            {
                                                //membuat bidak tmp berada di bidak yang sedang dicari misal putih, maka dari stack akan dicari yang putih dari bawah 
                                                //lalu seperti di tinggal kan satu, terus diitung keatas ada berapa bidak yang warnanya sama(nuat proses nggeser hitung prob)
                                                bidaktmp = bidaktmp.parent;
                                            }
                                        }
                                        else
                                        {
                                            whilecari = false;
                                        }
                                    }
                                    if (bidaktmp.parent != null)
                                    {
                                        //ini proses ninggal bidaknya
                                        bidaktmp = bidaktmp.parent;
                                        while (bidaktmp != null)
                                        {
                                            //dibuat sampe null supaya yang paling atas juga keproses
                                            if (bidaktmp.namabidak.Contains(cari))
                                            {
                                                jumlahcarry++;
                                            }
                                            bidaktmp = bidaktmp.parent;
                                        }
                                    }
                                }
                                if (cari == "white" && (papancek_e[y, x] == 0 || papancek_e[y, x] == 2))
                                {
                                    recurpossiblemenang(papancek_e, jumlahcarry, x, y, cari, "kiri");
                                    papancek_e[y, x] = -100;
                                    x++;
                                }
                                else if (cari == "black" && (papancek_e[y, x] == 3 || papancek_e[y, x] == 5))
                                {
                                    recurpossiblemenang(papancek_e, jumlahcarry, x, y, cari, "kiri");
                                    papancek_e[y, x] = -100;
                                    x++;
                                }
                                else
                                {
                                    jalan = false;
                                }
                            }
                        }
                        //kanan ke kiri
                        // jalan = true;
                        // x = 5;
                        // while (jalan)
                        // {
                        //     if (x < 0)
                        //     {
                        //         jalan = false;
                        //     }
                        //     else
                        //     {
                        //         var jumlahcarry = 0;
                        //         var bidaktmp = papangame[((6 * y) + x)];
                        //         if (bidaktmp != null)
                        //         {
                        //             while (bidaktmp.children != null)
                        //             {
                        //                 bidaktmp = bidaktmp.children;
                        //             }
                        //             var whilecari = true;
                        //             while (whilecari)
                        //             {
                        //                 if (!bidaktmp.namabidak.Contains(cari))
                        //                 {
                        //                     if (bidaktmp.parent == null)
                        //                     {
                        //                         whilecari = false;
                        //                     }
                        //                     else
                        //                     {
                        //                         //membuat bidak tmp berada di bidak yang sedang dicari misal putih, maka dari stack akan dicari yang putih dari bawah 
                        //                         //lalu seperti di tinggal kan satu, terus diitung keatas ada berapa bidak yang warnanya sama(nuat proses nggeser hitung prob)
                        //                         bidaktmp = bidaktmp.parent;
                        //                     }
                        //                 }
                        //                 else
                        //                 {
                        //                     whilecari = false;
                        //                 }
                        //             }
                        //             if (bidaktmp.parent != null)
                        //             {
                        //                 //ini proses ninggal bidaknya
                        //                 bidaktmp = bidaktmp.parent;
                        //                 while (bidaktmp != null)
                        //                 {
                        //                     //dibuat sampe null supaya yang paling atas juga keproses
                        //                     if (bidaktmp.namabidak.Contains(cari))
                        //                     {
                        //                         jumlahcarry++;
                        //                     }
                        //                     bidaktmp = bidaktmp.parent;
                        //                 }
                        //             }
                        //         }
                        //         if (cari == "white" && (papancek_e[y, x] == 0 || papancek_e[y, x] == 2))
                        //         {
                        //             recurpossiblemenang(papancek_e, jumlahcarry, x, y, cari, "kiri");
                        //             papancek_e[y, x] = -100;
                        //             x--;
                        //         }
                        //         else if (cari == "black" && (papancek_e[y, x] == 3 || papancek_e[y, x] == 5))
                        //         {
                        //             recurpossiblemenang(papancek_e, jumlahcarry, x, y, cari, "kiri");
                        //             papancek_e[y, x] = -100;
                        //             x--;
                        //         }
                        //         else
                        //         {
                        //             jalan = false;
                        //         }
                        //     }
                        // }
                    }
                    // for (int x = 0; x < 6; x++)
                    // {
                    //     //atas ke bawah
                    //     var jalan = true;
                    //     var y = 0;
                    //     while (jalan)
                    //     {
                    //         if (y > 5)
                    //         {
                    //             jalan = false;
                    //         }
                    //         else
                    //         {
                    //             var jumlahcarry = 0;
                    //             var bidaktmp = papangame[((6 * y) + x)];
                    //             if (bidaktmp != null)
                    //             {
                    //                 while (bidaktmp.children != null)
                    //                 {
                    //                     bidaktmp = bidaktmp.children;
                    //                 }
                    //                 var whilecari = true;
                    //                 while (whilecari)
                    //                 {
                    //                     if (!bidaktmp.namabidak.Contains(cari))
                    //                     {
                    //                         if (bidaktmp.parent == null)
                    //                         {
                    //                             whilecari = false;
                    //                         }
                    //                         else
                    //                         {
                    //                             //membuat bidak tmp berada di bidak yang sedang dicari misal putih, maka dari stack akan dicari yang putih dari bawah 
                    //                             //lalu seperti di tinggal kan satu, terus diitung keatas ada berapa bidak yang warnanya sama(nuat proses nggeser hitung prob)
                    //                             bidaktmp = bidaktmp.parent;
                    //                         }
                    //                     }
                    //                     else
                    //                     {
                    //                         whilecari = false;
                    //                     }
                    //                 }
                    //                 if (bidaktmp.parent != null)
                    //                 {
                    //                     //ini proses ninggal bidaknya
                    //                     bidaktmp = bidaktmp.parent;
                    //                     while (bidaktmp != null)
                    //                     {
                    //                         //dibuat sampe null supaya yang paling atas juga keproses
                    //                         if (bidaktmp.namabidak.Contains(cari))
                    //                         {
                    //                             jumlahcarry++;
                    //                         }
                    //                         bidaktmp = bidaktmp.parent;
                    //                     }
                    //                 }
                    //             }
                    //             if (cari == "white" && (papancek_e[y, x] == 0 || papancek_e[y, x] == 2))
                    //             {
                    //                 recurpossiblemenang(papancek_e, jumlahcarry, x, y, cari, "kiri");
                    //                 papancek_e[y, x] = -100;
                    //                 y++;
                    //             }
                    //             else if (cari == "black" && (papancek_e[y, x] == 3 || papancek_e[y, x] == 5))
                    //             {
                    //                 recurpossiblemenang(papancek_e, jumlahcarry, x, y, cari, "kiri");
                    //                 papancek_e[y, x] = -100;
                    //                 y++;
                    //             }
                    //             else
                    //             {
                    //                 jalan = false;
                    //             }
                    //         }
                    //     }
                    //     //kanan ke kiri
                    //     jalan = true;
                    //     y = 5;
                    //     while (jalan)
                    //     {
                    //         if (y < 0)
                    //         {
                    //             jalan = false;
                    //         }
                    //         else
                    //         {
                    //             var jumlahcarry = 0;
                    //             var bidaktmp = papangame[((6 * y) + x)];
                    //             if (bidaktmp != null)
                    //             {
                    //                 while (bidaktmp.children != null)
                    //                 {
                    //                     bidaktmp = bidaktmp.children;
                    //                 }
                    //                 var whilecari = true;
                    //                 while (whilecari)
                    //                 {
                    //                     if (!bidaktmp.namabidak.Contains(cari))
                    //                     {
                    //                         if (bidaktmp.parent == null)
                    //                         {
                    //                             whilecari = false;
                    //                         }
                    //                         else
                    //                         {
                    //                             //membuat bidak tmp berada di bidak yang sedang dicari misal putih, maka dari stack akan dicari yang putih dari bawah 
                    //                             //lalu seperti di tinggal kan satu, terus diitung keatas ada berapa bidak yang warnanya sama(nuat proses nggeser hitung prob)
                    //                             bidaktmp = bidaktmp.parent;
                    //                         }
                    //                     }
                    //                     else
                    //                     {
                    //                         whilecari = false;
                    //                     }
                    //                 }
                    //                 if (bidaktmp.parent != null)
                    //                 {
                    //                     //ini proses ninggal bidaknya
                    //                     bidaktmp = bidaktmp.parent;
                    //                     while (bidaktmp != null)
                    //                     {
                    //                         //dibuat sampe null supaya yang paling atas juga keproses
                    //                         if (bidaktmp.namabidak.Contains(cari))
                    //                         {
                    //                             jumlahcarry++;
                    //                         }
                    //                         bidaktmp = bidaktmp.parent;
                    //                     }
                    //                 }
                    //             }
                    //             if (cari == "white" && (papancek_e[y, x] == 0 || papancek_e[y, x] == 2))
                    //             {
                    //                 recurpossiblemenang(papancek_e, jumlahcarry, x, y, cari, "kiri");
                    //                 papancek_e[y, x] = -100;
                    //                 y--;
                    //             }
                    //             else if (cari == "black" && (papancek_e[y, x] == 3 || papancek_e[y, x] == 5))
                    //             {
                    //                 recurpossiblemenang(papancek_e, jumlahcarry, x, y, cari, "kiri");
                    //                 papancek_e[y, x] = -100;
                    //                 y--;
                    //             }
                    //             else
                    //             {
                    //                 jalan = false;
                    //             }
                    //         }
                    //     }
                    // }
                    Debug.Log(history_cek_menang.Count);
                }
            }
        }
    }
    private void cetak(int[,] papan)
    {
        var oute = "";
        for (int yy = 0; yy < 6; yy++)
        {
            for (int xx = 0; xx < 6; xx++)
            {
                oute += papan[yy, xx] + " ";
            }
            oute += "\n";
        }
        Debug.Log(oute);
    }
    List<int[,]> history_cek_menang = new List<int[,]>();
    private void pushhistory(int[,] papancek)
    {
        int ada = 0;
        foreach (var x in history_cek_menang)
        {
            int sama = 1;
            for (int yy = 0; yy < 6; yy++)
            {
                for (int xx = 0; xx < 6; xx++)
                {
                    if (x[yy, xx] != papancek[yy, xx])
                    {
                        sama = 0;
                        break;
                    }
                }
                if (sama == 0)
                {
                    break;
                }
            }
            ada += sama;
        }
        if (ada == 0)
        {
            history_cek_menang.Add(papancek);
        }
    }
    private void recurpossiblemenang(int[,] papancek, int carry, int x, int y, string warna, string start)
    {
        int[,] papanbaru =
        {
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
        };
        for (int yy = 0; yy < 6; yy++)
        {
            for (int xx = 0; xx < 6; xx++)
            {
                papanbaru[yy, xx] = papancek[yy, xx];
            }
        }
        //-100 sebagai penanda yang sudah dilewati
        if (x > 5 || y > 5 || x < 0 || y < 0)
        {

        }
        else
        {
            if (papanbaru[y, x] != -100)
            {
                if (papanbaru[y, x] != 1 && papanbaru[y, x] != 2 && papanbaru[y, x] != 4 && papanbaru[y, x] != 5)
                {
                    if (carry > 0)
                    {
                        if (warna == "black")
                        {
                            papanbaru[y, x] = 3;
                        }
                        if (warna == "white")
                        {
                            papanbaru[y, x] = 0;
                        }
                    }
                }
                int lanjut = 0;
                if (warna == "white" && (papanbaru[y, x] == 0 || papanbaru[y, x] == 2))
                {
                    papanbaru[y, x] = -100;
                    if (start == "kiri" && x == 5)
                    {
                        pushhistory(papanbaru);
                    }
                    else if (start == "atas" && y == 5)
                    {
                        pushhistory(papanbaru);
                    }
                    else if (start == "kanan" && x == 0)
                    {
                        pushhistory(papanbaru);
                    }
                    else if (start == "bawah" && y == 0)
                    {
                        pushhistory(papanbaru);
                    }
                    else
                    {
                        lanjut = 1;
                    }
                }
                else if (warna == "black" && (papanbaru[y, x] == 3 || papanbaru[y, x] == 5))
                {
                    papanbaru[y, x] = -100;
                    if (start == "kiri" && x == 5)
                    {
                        pushhistory(papanbaru);
                    }
                    else if (start == "atas" && y == 5)
                    {
                        pushhistory(papanbaru);
                    }
                    else if (start == "kanan" && x == 0)
                    {
                        pushhistory(papanbaru);
                    }
                    else if (start == "bawah" && y == 0)
                    {
                        pushhistory(papanbaru);
                    }
                    else
                    {
                        lanjut = 1;
                    }
                }
                if (lanjut == 1)
                {
                    if (start == "kiri")
                    {
                        //ke kanan
                        recurpossiblemenang(papanbaru, carry - 1, x + 1, y, warna, start);
                        //ke kiri
                        recurpossiblemenang(papanbaru, 0, x - 1, y, warna, start);
                        //ke atas
                        recurpossiblemenang(papanbaru, 0, x, y - 1, warna, start);
                        //ke bawah
                        recurpossiblemenang(papanbaru, 0, x, y + 1, warna, start);
                    }
                    if (start == "atas")
                    {
                        //ke kanan
                        recurpossiblemenang(papanbaru, 0, x + 1, y, warna, start);
                        //ke kiri
                        recurpossiblemenang(papanbaru, 0, x - 1, y, warna, start);
                        //ke atas
                        recurpossiblemenang(papanbaru, 0, x, y - 1, warna, start);
                        //ke bawah
                        recurpossiblemenang(papanbaru, carry - 1, x, y + 1, warna, start);
                    }
                    if (start == "kanan")
                    {
                        //ke kanan
                        recurpossiblemenang(papanbaru, 0, x + 1, y, warna, start);
                        //ke kiri
                        recurpossiblemenang(papanbaru, carry - 1, x - 1, y, warna, start);
                        //ke atas
                        recurpossiblemenang(papanbaru, 0, x, y - 1, warna, start);
                        //ke bawah
                        recurpossiblemenang(papanbaru, 0, x, y + 1, warna, start);
                    }
                    if (start == "bawah")
                    {
                        //ke kanan
                        recurpossiblemenang(papanbaru, 0, x + 1, y, warna, start);
                        //ke kiri
                        recurpossiblemenang(papanbaru, 0, x - 1, y, warna, start);
                        //ke atas
                        recurpossiblemenang(papanbaru, carry - 1, x, y - 1, warna, start);
                        //ke bawah
                        recurpossiblemenang(papanbaru, 0, x, y + 1, warna, start);
                    }
                }
            }
        }
    }
    private int[,] clonepapancek(int[,] papan)
    {
        int[,] papan_tmp =
        {
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
        };
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                papan_tmp[y, x] = papan[y, x];
            }
        }
        return papan_tmp;
    }
    private List<Bidak> clonepapan(List<Bidak> papan)
    {
        var hasil = new List<Bidak>();
        foreach (var papanambil in papan)
        {
            if (papanambil == null)
            {
                hasil.Add(null);
            }
            else
            {
                var tmp = clonebidak(papanambil);
                hasil.Add(tmp);
            }
        }
        return hasil;
    }
    private Bidak clonebidak_datagame(data.Bidak curr)
    {
        var sebelumnya = new Bidak(false, "", 1);
        sebelumnya.iscapstone = curr.iscapstone;
        sebelumnya.lokasipapan = curr.lokasipapan;
        sebelumnya.penomoran = curr.penomoran;
        sebelumnya.namabidak = curr.namabidak;
        Bidak hasil = sebelumnya;
        curr = curr.children;
        while (curr != null)
        {
            var childe = new Bidak(false, "", 1);
            childe.iscapstone = curr.iscapstone;
            childe.lokasipapan = curr.lokasipapan;
            childe.penomoran = curr.penomoran;
            childe.namabidak = curr.namabidak;
            childe.parent = sebelumnya;
            sebelumnya.children = childe;
            sebelumnya = childe;
            curr = curr.children;
        }
        return sebelumnya;
    }
    private Bidak clonebidak(Bidak curr)
    {
        var sebelumnya = new Bidak(false, "", 1);
        sebelumnya.iscapstone = curr.iscapstone;
        sebelumnya.lokasipapan = curr.lokasipapan;
        sebelumnya.penomoran = curr.penomoran;
        sebelumnya.namabidak = curr.namabidak;
        Bidak hasil = sebelumnya;
        curr = curr.children;
        while (curr != null)
        {
            var childe = new Bidak(false, "", 1);
            childe.iscapstone = curr.iscapstone;
            childe.lokasipapan = curr.lokasipapan;
            childe.penomoran = curr.penomoran;
            childe.namabidak = curr.namabidak;
            childe.parent = sebelumnya;
            sebelumnya.children = childe;
            sebelumnya = childe;
            curr = curr.children;
        }
        return hasil;
    }
    private void makepossible(List<Bidak> papane, string warnabidak)
    {
        int adacapstone = 0;
        string namabidaktaruh = "";
        foreach (var x in datagame.bidakplayer)
        {
            if (x.iscapstone && x.namabidak.Contains(warnabidak))
            {
                if (x.lokasipapan == "")
                {
                    adacapstone = 1;
                }
                break;
            }
        }
        foreach (var x in datagame.bidakplayer)
        {
            if (x.iscapstone == false && x.namabidak.Contains(warnabidak) && x.lokasipapan == "")
            {
                namabidaktaruh = x.namabidak;
                break;
            }
        }
        for (int i = 0; i < papane.Count; i++)
        {
            var papanclone = clonepapan(papane);
            if (papanclone[i] == null)
            {
                //untuk taruh bidak;
                if (namabidaktaruh != "")
                {
                    //untuk pengecekan masih ada bidak yang bisa ditaruh

                    //modebiasa
                    Bidak bidakbaru = new Bidak(false, warnabidak + "taruh", 1);
                    papanclone[i] = bidakbaru;
                    catatprobpapan(papanclone);
                    //modewall
                    bidakbaru = new Bidak(false, warnabidak + "taruh", 2);
                    papanclone[i] = bidakbaru;
                    catatprobpapan(papanclone);
                }
                if (adacapstone == 1)
                {
                    //jika masih ada captstone
                    Bidak bidakbaru = new Bidak(true, warnabidak + "31", 1);
                    papanclone[i] = bidakbaru;
                    catatprobpapan(papanclone);
                }
            }
            else if (papanclone[i].namabidak.Contains(warnabidak))
            {
                var bidakgeser = clonebidak(papanclone[i]);
                papanclone[i] = null;
                Bidak bidakbayangan = new Bidak(false, "bidak_bayangan", 1);
                bidakbayangan.children = bidakgeser;
                bidakgeser.parent = bidakbayangan;
                generatekombinasigerakkanan(bidakbayangan, papanclone, i);
                global_children_minimaxnode.RemoveAt(global_children_minimaxnode.Count - 1);
                generatekombinasigerakkiri(bidakbayangan, papanclone, i);
                global_children_minimaxnode.RemoveAt(global_children_minimaxnode.Count - 1);
                generatekombinasigerakatas(bidakbayangan, papanclone, i);
                global_children_minimaxnode.RemoveAt(global_children_minimaxnode.Count - 1);
                generatekombinasigerakbawah(bidakbayangan, papanclone, i);
                global_children_minimaxnode.RemoveAt(global_children_minimaxnode.Count - 1);
            }
        }
    }
    private void catatprobpapan(List<Bidak> papan)
    {
        int[,] papan_tmp =
        {
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1,-1},
        };
        int counter = 0;
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                var isi = -1;
                if (papan[counter] != null)
                {
                    if (papan[counter].namabidak.Contains("white"))
                    {
                        isi = 0;
                        if (papan[counter].penomoran == 2)
                        {
                            isi = 1;
                        }
                        if (papan[counter].iscapstone)
                        {
                            isi = 3;
                        }
                    }
                    if (papan[counter].namabidak.Contains("black"))
                    {
                        isi = 3;
                        if (papan[counter].penomoran == 2)
                        {
                            isi = 4;
                        }
                        if (papan[counter].iscapstone)
                        {
                            isi = 5;
                        }
                    }
                }
                papan_tmp[y, x] = isi;
                counter++;
            }
        }
        papanminimax baru = new papanminimax(clonepapan(papan), papan_tmp);
        global_children_minimaxnode.Add(baru);
    }
    public void generatekombinasigerakkanan(Bidak curr, List<Bidak> papan, int lokasi)
    {
        if (papan[lokasi] != null && (papan[lokasi].iscapstone || papan[lokasi].penomoran == 2))
        {

        }
        else
        {
            if (curr.namabidak == "bidak_bayangan" && curr.children == null)
            {
                catatprobpapan(papan);
            }
            else
            {

                //geser semua bidak lalu turunin satu statu 
                if ((lokasi + 1) % 6 == 0)
                {
                    if (papan[lokasi] != null)
                    {
                        //lepas semua bidak, karena sudah diujung
                        var currjalan = clonebidak(curr);
                        var papanjalan = clonepapan(papan);
                        var telusuri = currjalan;
                        while (telusuri.children != null)
                        {
                            //mencari paling bawah
                            telusuri = telusuri.children;
                        }
                        //tidak dicek apakah null, karena sudah pasti ada isinya dan tidak mungkin juga untuk menaruh di tempat ujung jika stacknya diujung
                        papanjalan[lokasi].parent = telusuri;
                        telusuri.children = papanjalan[lokasi];
                        //curr.children karena curr pasti yang bayangan
                        papanjalan[lokasi] = currjalan.children;
                        currjalan.children = null;
                        generatekombinasigerakkanan(currjalan, papanjalan, (lokasi + 1));
                    }
                }
                else
                {
                    var currjalan = clonebidak(curr);
                    var papanjalan = clonepapan(papan);
                    //geser semua bidak
                    //setting bidak paling bawah ke papan selanjutnya, bidak yang lagi di select (curr) tidak perlu dipindah papan nya
                    var telusuri = currjalan;
                    while (telusuri.children != null)
                    {
                        telusuri = telusuri.children;
                    }
                    telusuri.parent.children = null;
                    telusuri.parent = null;
                    if (papanjalan[lokasi + 1] != null)
                    {
                        if (!(papanjalan[lokasi + 1].iscapstone || papanjalan[lokasi + 1].penomoran == 2))
                        {
                            papanjalan[lokasi + 1].parent = telusuri;
                            telusuri.children = papanjalan[lokasi + 1];
                            papanjalan[lokasi + 1] = telusuri;
                        }
                    }
                    else
                    {
                        //kita tidak perlu mensetting lokasi papan karena itu untuk mempermudah pembuatan di unity saja
                        papanjalan[lokasi + 1] = telusuri;
                    }
                    generatekombinasigerakkanan(currjalan, papanjalan, (lokasi + 1));
                    var currjalan2 = clonebidak(curr);
                    var papanjalan2 = clonepapan(papan);
                    var jalan = true;
                    while (jalan)
                    {
                        if (currjalan2.children == null)
                        {
                            jalan = false;
                        }
                        else
                        {
                            telusuri = currjalan2;
                            while (telusuri.children != null)
                            {
                                telusuri = telusuri.children;
                            }
                            telusuri.parent.children = null;
                            telusuri.parent = null;
                            //melepas satu bidak di tempat itu
                            if (papanjalan2[lokasi] != null)
                            {
                                telusuri.children = papanjalan2[lokasi];
                                papanjalan2[lokasi].parent = telusuri;

                            }
                            papanjalan2[lokasi] = telusuri;
                            var bidakbck = clonebidak(currjalan2);
                            var papanbck = clonepapan(papanjalan2);
                            if (currjalan2.children != null)
                            {
                                telusuri = currjalan2;
                                while (telusuri.children != null)
                                {
                                    telusuri = telusuri.children;
                                }
                                telusuri.parent.children = null;
                                telusuri.parent = null;
                                //memindahkan bidak terakhir dari yang masih di select ke papan selanjutnya dan ditaruh
                                if (papanjalan2[lokasi + 1] != null)
                                {
                                    if (!(papanjalan2[lokasi + 1].iscapstone || papanjalan2[lokasi + 1].penomoran == 2))
                                    {
                                        papanjalan2[lokasi + 1].parent = telusuri;
                                        telusuri.children = papanjalan2[lokasi + 1];
                                    }
                                }
                                else
                                {
                                    papanjalan2[lokasi + 1] = telusuri;
                                }
                            }
                            generatekombinasigerakkanan(currjalan2, papanjalan2, (lokasi + 1));
                            papanjalan2 = clonepapan(papanbck);
                            currjalan2 = clonebidak(bidakbck);
                        }
                    }
                }
            }
        }
    }
    public void generatekombinasigerakkiri(Bidak curr, List<Bidak> papan, int lokasi)
    {
        if (papan[lokasi] != null && (papan[lokasi].iscapstone || papan[lokasi].penomoran == 2))
        {

        }
        else
        {
            if (curr.namabidak == "bidak_bayangan" && curr.children == null)
            {
                catatprobpapan(papan);
            }
            else
            {
                //geser semua bidak lalu turunin satu statu 
                if (lokasi % 6 == 0)
                {
                    if (papan[lokasi] != null)
                    {
                        //lepas semua bidak, karena sudah diujung
                        var currjalan = clonebidak(curr);
                        var papanjalan = clonepapan(papan);
                        var telusuri = currjalan;
                        while (telusuri.children != null)
                        {
                            //mencari paling bawah
                            telusuri = telusuri.children;
                        }
                        //tidak dicek apakah null, karena sudah pasti ada isinya dan tidak mungkin juga untuk menaruh di tempat ujung jika stacknya diujung
                        papanjalan[lokasi].parent = telusuri;
                        telusuri.children = papanjalan[lokasi];
                        //curr.children karena curr pasti yang bayangan
                        papanjalan[lokasi] = currjalan.children;
                        currjalan.children = null;
                        generatekombinasigerakkiri(currjalan, papanjalan, (lokasi - 1));
                    }
                }
                else
                {
                    var currjalan = clonebidak(curr);
                    var papanjalan = clonepapan(papan);
                    //geser semua bidak
                    //setting bidak paling bawah ke papan selanjutnya, bidak yang lagi di select (curr) tidak perlu dipindah papan nya
                    var telusuri = currjalan;
                    while (telusuri.children != null)
                    {
                        telusuri = telusuri.children;
                    }
                    telusuri.parent.children = null;
                    telusuri.parent = null;
                    if (papanjalan[lokasi - 1] != null)
                    {
                        if (!(papanjalan[lokasi - 1].iscapstone || papanjalan[lokasi - 1].penomoran == 2))
                        {
                            papanjalan[lokasi - 1].parent = telusuri;
                            telusuri.children = papanjalan[lokasi - 1];
                            papanjalan[lokasi - 1] = telusuri;
                        }
                    }
                    else
                    {
                        //kita tidak perlu mensetting lokasi papan karena itu untuk mempermudah pembuatan di unity saja
                        papanjalan[lokasi - 1] = telusuri;
                    }
                    generatekombinasigerakkiri(currjalan, papanjalan, (lokasi - 1));
                    var currjalan2 = clonebidak(curr);
                    var papanjalan2 = clonepapan(papan);
                    var jalan = true;
                    while (jalan)
                    {
                        if (currjalan2.children == null)
                        {
                            jalan = false;
                        }
                        else
                        {
                            telusuri = currjalan2;
                            while (telusuri.children != null)
                            {
                                telusuri = telusuri.children;
                            }
                            telusuri.parent.children = null;
                            telusuri.parent = null;
                            //melepas satu bidak di tempat itu
                            if (papanjalan2[lokasi] != null)
                            {
                                telusuri.children = papanjalan2[lokasi];
                                papanjalan2[lokasi].parent = telusuri;

                            }
                            papanjalan2[lokasi] = telusuri;
                            var bidakbck = clonebidak(currjalan2);
                            var papanbck = clonepapan(papanjalan2);
                            if (currjalan2.children != null)
                            {
                                telusuri = currjalan2;
                                while (telusuri.children != null)
                                {
                                    telusuri = telusuri.children;
                                }
                                telusuri.parent.children = null;
                                telusuri.parent = null;
                                //memindahkan bidak terakhir dari yang masih di select ke papan selanjutnya dan ditaruh
                                if (papanjalan2[lokasi - 1] != null)
                                {
                                    if (!(papanjalan2[lokasi - 1].iscapstone || papanjalan2[lokasi - 1].penomoran == 2))
                                    {
                                        papanjalan2[lokasi - 1].parent = telusuri;
                                        telusuri.children = papanjalan2[lokasi - 1];
                                    }
                                }
                                else
                                {
                                    papanjalan2[lokasi - 1] = telusuri;
                                }
                            }
                            generatekombinasigerakkiri(currjalan2, papanjalan2, (lokasi - 1));
                            papanjalan2 = clonepapan(papanbck);
                            currjalan2 = clonebidak(bidakbck);
                        }
                    }
                }
            }
        }
    }
    public void generatekombinasigerakatas(Bidak curr, List<Bidak> papan, int lokasi)
    {
        if (papan[lokasi] != null && (papan[lokasi].iscapstone || papan[lokasi].penomoran == 2))
        {

        }
        else
        {
            if (curr.namabidak == "bidak_bayangan" && curr.children == null)
            {
                catatprobpapan(papan);
            }
            else
            {
                //geser semua bidak lalu turunin satu statu 
                if (Math.Floor(lokasi / 6f) == 0)
                {
                    if (papan[lokasi] != null)
                    {
                        //karena kalau null itu pasti naruh di papan sendiri

                        //lepas semua bidak, karena sudah diujung
                        var currjalan = clonebidak(curr);
                        var papanjalan = clonepapan(papan);
                        var telusuri = currjalan;
                        while (telusuri.children != null)
                        {
                            //mencari paling bawah
                            telusuri = telusuri.children;
                        }
                        //tidak dicek apakah null, karena sudah pasti ada isinya dan tidak mungkin juga untuk menaruh di tempat ujung jika stacknya diujung
                        papanjalan[lokasi].parent = telusuri;
                        telusuri.children = papanjalan[lokasi];
                        //curr.children karena curr pasti yang bayangan
                        papanjalan[lokasi] = currjalan.children;
                        currjalan.children = null;
                        generatekombinasigerakatas(currjalan, papanjalan, (lokasi - 5));
                    }
                }
                else
                {
                    var currjalan = clonebidak(curr);
                    var papanjalan = clonepapan(papan);
                    //geser semua bidak
                    //setting bidak paling bawah ke papan selanjutnya, bidak yang lagi di select (curr) tidak perlu dipindah papan nya
                    var telusuri = currjalan;
                    while (telusuri.children != null)
                    {
                        telusuri = telusuri.children;
                    }
                    telusuri.parent.children = null;
                    telusuri.parent = null;
                    if (papanjalan[lokasi - 5] != null)
                    {
                        if (!(papanjalan[lokasi - 5].iscapstone || papanjalan[lokasi - 5].penomoran == 2))
                        {
                            papanjalan[lokasi - 5].parent = telusuri;
                            telusuri.children = papanjalan[lokasi - 5];
                            papanjalan[lokasi - 5] = telusuri;
                        }
                    }
                    else
                    {
                        //kita tidak perlu mensetting lokasi papan karena itu untuk mempermudah pembuatan di unity saja
                        papanjalan[lokasi - 5] = telusuri;
                    }
                    generatekombinasigerakatas(currjalan, papanjalan, (lokasi - 5));
                    var currjalan2 = clonebidak(curr);
                    var papanjalan2 = clonepapan(papan);
                    var jalan = true;
                    while (jalan)
                    {
                        if (currjalan2.children == null)
                        {
                            jalan = false;
                        }
                        else
                        {
                            telusuri = currjalan2;
                            while (telusuri.children != null)
                            {
                                telusuri = telusuri.children;
                            }
                            telusuri.parent.children = null;
                            telusuri.parent = null;
                            //melepas satu bidak di tempat itu
                            if (papanjalan2[lokasi] != null)
                            {
                                telusuri.children = papanjalan2[lokasi];
                                papanjalan2[lokasi].parent = telusuri;

                            }
                            papanjalan2[lokasi] = telusuri;
                            var bidakbck = clonebidak(currjalan2);
                            var papanbck = clonepapan(papanjalan2);
                            if (currjalan2.children != null)
                            {
                                telusuri = currjalan2;
                                while (telusuri.children != null)
                                {
                                    telusuri = telusuri.children;
                                }
                                telusuri.parent.children = null;
                                telusuri.parent = null;
                                //memindahkan bidak terakhir dari yang masih di select ke papan selanjutnya dan ditaruh
                                if (papanjalan2[lokasi - 5] != null)
                                {
                                    if (!(papanjalan2[lokasi - 5].iscapstone || papanjalan2[lokasi - 5].penomoran == 2))
                                    {
                                        papanjalan2[lokasi - 5].parent = telusuri;
                                        telusuri.children = papanjalan2[lokasi - 5];
                                    }
                                }
                                else
                                {
                                    papanjalan2[lokasi - 5] = telusuri;
                                }
                            }
                            generatekombinasigerakatas(currjalan2, papanjalan2, (lokasi - 5));
                            papanjalan2 = clonepapan(papanbck);
                            currjalan2 = clonebidak(bidakbck);
                        }
                    }
                }
            }
        }
    }
    public void generatekombinasigerakbawah(Bidak curr, List<Bidak> papan, int lokasi)
    {
        if (papan[lokasi] != null && (papan[lokasi].iscapstone || papan[lokasi].penomoran == 2))
        {

        }
        else
        {
            if (curr.namabidak == "bidak_bayangan" && curr.children == null)
            {
                catatprobpapan(papan);
            }
            else
            {
                //geser semua bidak lalu turunin satu statu 
                if (Math.Floor(lokasi / 6f) == 5)
                {
                    if (papan[lokasi] != null)
                    {
                        //lepas semua bidak, karena sudah diujung
                        var currjalan = clonebidak(curr);
                        var papanjalan = clonepapan(papan);
                        var telusuri = currjalan;
                        while (telusuri.children != null)
                        {
                            //mencari paling bawah
                            telusuri = telusuri.children;
                        }
                        //tidak dicek apakah null, karena sudah pasti ada isinya dan tidak mungkin juga untuk menaruh di tempat ujung jika stacknya diujung
                        papanjalan[lokasi].parent = telusuri;
                        telusuri.children = papanjalan[lokasi];
                        //curr.children karena curr pasti yang bayangan
                        papanjalan[lokasi] = currjalan.children;
                        currjalan.children = null;
                        generatekombinasigerakbawah(currjalan, papanjalan, (lokasi + 5));
                    }
                }
                else
                {
                    var currjalan = clonebidak(curr);
                    var papanjalan = clonepapan(papan);
                    //geser semua bidak
                    //setting bidak paling bawah ke papan selanjutnya, bidak yang lagi di select (curr) tidak perlu dipindah papan nya
                    var telusuri = currjalan;
                    while (telusuri.children != null)
                    {
                        telusuri = telusuri.children;
                    }
                    telusuri.parent.children = null;
                    telusuri.parent = null;
                    if (papanjalan[lokasi + 5] != null)
                    {
                        if (!(papanjalan[lokasi + 5].iscapstone || papanjalan[lokasi + 5].penomoran == 2))
                        {
                            papanjalan[lokasi + 5].parent = telusuri;
                            telusuri.children = papanjalan[lokasi + 5];
                            papanjalan[lokasi + 5] = telusuri;
                        }
                    }
                    else
                    {
                        //kita tidak perlu mensetting lokasi papan karena itu untuk mempermudah pembuatan di unity saja
                        papanjalan[lokasi + 5] = telusuri;
                    }
                    generatekombinasigerakbawah(currjalan, papanjalan, (lokasi + 5));
                    var currjalan2 = clonebidak(curr);
                    var papanjalan2 = clonepapan(papan);
                    var jalan = true;
                    while (jalan)
                    {
                        if (currjalan2.children == null)
                        {
                            jalan = false;
                        }
                        else
                        {
                            telusuri = currjalan2;
                            while (telusuri.children != null)
                            {
                                telusuri = telusuri.children;
                            }
                            telusuri.parent.children = null;
                            telusuri.parent = null;
                            //melepas satu bidak di tempat itu
                            if (papanjalan2[lokasi] != null)
                            {
                                telusuri.children = papanjalan2[lokasi];
                                papanjalan2[lokasi].parent = telusuri;

                            }
                            papanjalan2[lokasi] = telusuri;
                            var bidakbck = clonebidak(currjalan2);
                            var papanbck = clonepapan(papanjalan2);
                            if (currjalan2.children != null)
                            {
                                telusuri = currjalan2;
                                while (telusuri.children != null)
                                {
                                    telusuri = telusuri.children;
                                }
                                telusuri.parent.children = null;
                                telusuri.parent = null;
                                //memindahkan bidak terakhir dari yang masih di select ke papan selanjutnya dan ditaruh
                                if (papanjalan2[lokasi + 5] != null)
                                {
                                    if (!(papanjalan2[lokasi + 5].iscapstone || papanjalan2[lokasi + 5].penomoran == 2))
                                    {
                                        papanjalan2[lokasi + 5].parent = telusuri;
                                        telusuri.children = papanjalan2[lokasi + 5];
                                    }
                                }
                                else
                                {
                                    papanjalan2[lokasi + 5] = telusuri;
                                }
                            }
                            generatekombinasigerakbawah(currjalan2, papanjalan2, (lokasi + 5));
                            papanjalan2 = clonepapan(papanbck);
                            currjalan2 = clonebidak(bidakbck);
                        }
                    }
                }
            }
        }
    }
}
