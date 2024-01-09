using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ai : MonoBehaviour
{
    public cekgame cekmenang;
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
        public int alpha = int.MinValue;
        public int beta = int.MaxValue;
        public int indexchildpilih = -1;
        public minimaxnode()
        {

        }
    }
    List<papanminimax> global_children_minimaxnode = new List<papanminimax>();
    public minimaxnode rootminimax;
    // Start is called before the first frame update
    void Start()
    {
        // Bidak bidakbayangan = new Bidak(false, "bidak_bayangan", 0);
        // Bidak bidak1 = new Bidak(false, "white_1", 2);
        // Bidak bidak2 = new Bidak(true, "bidak2", 0);
        // bidak1.parent = bidakbayangan;
        // bidakbayangan.children = bidak1;
        // List<Bidak> papan = new List<Bidak>();
        // for (int i = 0; i < 36; i++)
        // {
        //     papan.Add(null);
        // }
        // generatekombinasigerakkanan(bidakbayangan, papan, 0);
        // Debug.Log(global_children_minimaxnode.Count);
        // foreach (var x in global_children_minimaxnode)
        // {
        //     cetak(x.papan_cek);
        // }
        // int[,] papan_cek =
        // {
        //     {3,-1,-1,-1,-1,-1,},
        //     {3,-1,-1,-1,-1,-1,},
        //     {3,-1,-1,-1,-1,-1,},
        //     {3,-1,-1,-1,-1,-1,},
        //     {3,-1,-1,-1,-1,-1,},
        //     {-1,-1,-1,-1,-1,-1,},
        // };
        // Debug.Log(terminlarecurmenang2(papan_cek));
    }

    // Update is called once per frame
    void Update()
    {
        if (datagame.currentplayer == -1 && datagame.selectedpapan == "")
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
                    rootminimax = new minimaxnode();
                    var papancurr = clonepapan(papanai);
                    rootminimax.curr = new papanminimax(papancurr, clonepapancek(datagame.papan_cek));
                    rootminimax.state = "max";
                    rootminimax.parent = null;
                    rootminimax.value = int.MinValue;
                    rootminimax = minimaxnode_generate(rootminimax, 1, 2, "black_", "max");
                    int jumlahpilihan = 0;
                    List<int> indexprobsama = new List<int>();
                    // pengecekan jika value yang dimiliki root ada di beberapa children (banyak pilihan)
                    for (int i = 0; i < rootminimax.children.Count; i++)
                    {
                        if (rootminimax.children[i].value == rootminimax.value)
                        {
                            jumlahpilihan += 1;
                            indexprobsama.Add(i);
                        }
                    }
                    // jika ada chance menaruh yang bukan wall maka yang chance wall akan dihapus
                    if (jumlahpilihan > 1)
                    {
                        var indexterpilih = -1000;
                        var min = 100000000;
                        foreach (var yyx in indexprobsama)
                        {
                            //dibuat -1 karena biar kalau geser dari posisi semula masih 0 dan jika bukan yang dicari maka tidak masuk hitungan
                            var hasile = terminlarecurmenang2(rootminimax.children[yyx].curr.papan_cek);
                            if (hasile < min)
                            {
                                min = hasile;
                                indexterpilih = yyx;
                            }
                        }
                        // System.Random rnd = new System.Random();
                        // rootminimax.indexchildpilih = indexprobsama[rnd.Next(indexprobsama.Count)];
                        if (indexterpilih == -1000)
                        {
                            //random 
                            System.Random rnd = new System.Random();
                            rootminimax.indexchildpilih = indexprobsama[rnd.Next(indexprobsama.Count)];
                        }
                        else
                        {
                            rootminimax.indexchildpilih = indexterpilih;
                        }
                    }
                    datagame.dispatchai(clonepapan(rootminimax.children[rootminimax.indexchildpilih].curr.papan_game));
                    datagame.updatepapan();
                    datagame.aiberpikir = 0;
                    datagame.currentplayer *= -1;
                    // Debug.Log(rootminimax.children[0].value);
                    // cetak(rootminimax.children[0].curr.papan_cek);
                    //     int[,] papan_cek =
                    //    {{-1,-1,-1,-1,-1,-1,},
                    //     {-1,-1,3,-1,-1,3,},
                    //     {-1,-1,-1,-1,-1,-1,},
                    //     {3,-1,-1,-1,-1,-1,},
                    //     {0,0,0,0,-1,0,},
                    //     {3,-1,-1,3,-1,-1,},
                    //     };
                    //                     Debug.Log(cekmenang.cekmenang(papan_cek));
                    // Debug.Log(terminal_recur_menang(papan_cek, papancurr, "white"));
                    // Debug.Log(terminal_recur_menang(papan_cek, papancurr, "black"));
                }
            }
        }
    }
    private int recur_get_jumlah_berdekatan(int[,] papancek, int x, int y, int counter)
    {
        if (x >= 0 && y >= 0 && x <= 5 && y <= 5)
        {
            if (papancek[y, x] == 3 || papancek[y, x] == 5)
            {
                var papanclone = clonepapancek(papancek);
                papanclone[y, x] = -100;
                int hasil = 0;
                hasil += recur_get_jumlah_berdekatan(papanclone, (x + 1), y, (counter + 1));
                hasil += recur_get_jumlah_berdekatan(papanclone, (x - 1), y, (counter + 1));
                hasil += recur_get_jumlah_berdekatan(papanclone, x, (y + 1), (counter + 1));
                hasil += recur_get_jumlah_berdekatan(papanclone, x, (y - 1), (counter + 1));
                return hasil;
            }
            else
            {
                if (counter > 0 && papancek[y, x] != -100)
                {
                    //kalau di next" sudah pasti menuju yang tidak ada isinya tapi pembedanya ada di counter, ini bekas jalan atau tidak
                    //jika bekas jalan maka 1 jadi ada berdekatan
                    return 1;
                }
                else
                {
                    //jik tidak 0 karen awal sudah langsung 0
                    return 0;
                }
            }
        }
        else
        {
            if (counter > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
    public int terminlarecurmenang2(int[,] papan)
    {
        int hasil = int.MaxValue;
        var papane = clonepapancek(papan);
        //ganti papan yang null dengan yang dicari
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                if (papane[y, x] == -1)
                {
                    papane[y, x] = 3;
                }
            }
        }
        for (int y = 0; y < 6; y++)
        {
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
                    if (papan[y, x] == 3 || papan[y, x] == 5)
                    {
                        historycounter.Clear();
                        recurpossiblemenang(papane, x, y, "black", "kiri", 0);
                        if (historycounter.Count > 0)
                        {
                            if (hasil > historycounter.Min())
                            {
                                hasil = historycounter.Min();
                            }
                        }
                        x++;
                    }
                    else
                    {
                        jalan = false;
                    }
                }
            }
            jalan = true;
            x = 5;
            while (jalan)
            {
                if (x < 0)
                {
                    jalan = false;
                }
                else
                {
                    if (papan[y, x] == 3 || papan[y, x] == 5)
                    {
                        historycounter.Clear();
                        recurpossiblemenang(papane, x, y, "black", "kanan", 0);
                        if (historycounter.Count > 0)
                        {
                            if (hasil > historycounter.Min())
                            {
                                hasil = historycounter.Min();
                            }
                        }
                        x--;
                    }
                    else
                    {
                        jalan = false;
                    }
                }
            }
        }
        for (int x = 0; x < 6; x++)
        {
            var jalan = true;
            var y = 0;
            while (jalan)
            {
                if (y > 5)
                {
                    jalan = false;
                }
                else
                {
                    if (papan[y, x] == 3 || papan[y, x] == 5)
                    {
                        historycounter.Clear();
                        recurpossiblemenang(papane, x, y, "black", "atas", 0);
                        if (historycounter.Count > 0)
                        {
                            if (hasil > historycounter.Min())
                            {
                                hasil = historycounter.Min();
                            }
                        }
                        y++;
                    }
                    else
                    {
                        jalan = false;
                    }
                }
            }
            jalan = true;
            y = 5;
            while (jalan)
            {
                if (y < 0)
                {
                    jalan = false;
                }
                else
                {
                    if (papan[y, x] == 3 || papan[y, x] == 5)
                    {
                        historycounter.Clear();
                        recurpossiblemenang(papane, x, y, "black", "bawah", 0);
                        if (historycounter.Count > 0)
                        {
                            if (hasil > historycounter.Min())
                            {
                                hasil = historycounter.Min();
                            }
                        }
                        y--;
                    }
                    else
                    {
                        jalan = false;
                    }
                }
            }
        }
        return hasil;
    }
    private int terminal_recur_menang(int[,] papan, List<Bidak> papangame, string cari)
    {
        List<int[,]> list_papan_cek = new List<int[,]>();
        int hasil = int.MaxValue;
        void pushpapan(int[,] papancek)
        {
            int ada = 0;
            foreach (var x in list_papan_cek)
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
                list_papan_cek.Add(papancek);
            }
        }
        int getcarry(Bidak horas)
        {
            var jumlahcarry = 0;
            var bidaktmp = horas;
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
            return jumlahcarry;
        }
        // bongkar stack dan push ke list_papan_cek
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                var papantelusuri = clonepapancek(papan);
                var bisageser = getcarry(papangame[((6 * y) + x)]);
                if (bisageser > 0)
                {
                    for (int i = 1; i <= bisageser; i++)
                    {
                        int xkanan = x + i;
                        int ybawah = y + i;
                        int xkiri = x - i;
                        int yatas = y - i;
                        //jika yang dicari putih maka angka yang mungkin untuk diganti adalah 3 (black bidak biasa)
                        int angkapossible = cari == "white" ? 3 : 0;
                        int angkaganti = cari == "white" ? 0 : 3;
                        //pengecekan apakah masih bisa lepas bidak pada x dan y baru
                        if (xkanan <= 5 && (papantelusuri[y, xkanan] == angkapossible || papantelusuri[y, xkanan] == -1))
                        {
                            papantelusuri[y, xkanan] = angkaganti;
                        }
                        if (ybawah <= 5 && (papantelusuri[ybawah, x] == angkapossible || papantelusuri[ybawah, x] == -1))
                        {
                            papantelusuri[ybawah, x] = angkaganti;
                        }
                        if (xkiri >= 0 && (papantelusuri[y, xkiri] == angkapossible || papantelusuri[y, xkiri] == -1))
                        {
                            papantelusuri[y, xkiri] = angkaganti;
                        }
                        if (yatas >= 0 && (papantelusuri[yatas, x] == angkapossible || papantelusuri[yatas, x] == -1))
                        {
                            papantelusuri[yatas, x] = angkaganti;
                        }
                    }
                }
                pushpapan(papantelusuri);
            }
        }
        //untuk membuat pojok kiri dan atas menjadi sesuai yang dicari jika pada baris itu ada
        foreach (var bruh in list_papan_cek)
        {
            var bruhcopy = clonepapancek(bruh);
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    int angkcari = cari == "white" ? 0 : 3;
                    int angkcarii = cari == "white" ? 2 : 5;
                    int ganti = cari == "white" ? 0 : 3;
                    int tidakbisagantikalauangka = cari == "white" ? 3 : 0;
                    int tidakbisagantikalauangkaa = cari == "white" ? 4 : 1;
                    int tidakbisagantikalauangkaaa = cari == "white" ? 5 : 2;
                    if (bruh[y, x] == angkcari || bruh[y, x] == angkcarii)
                    {
                        //meskipun yang diganti capstone (misal yang ujung sudah ada dan itu capstone) tidak masalah kareana akan tetap warna itu
                        if (bruh[0, x] != tidakbisagantikalauangka && bruh[0, x] != tidakbisagantikalauangkaa && bruh[0, x] != tidakbisagantikalauangkaaa)
                        {
                            bruhcopy[0, x] = ganti;
                        }
                        if (bruh[y, 0] != tidakbisagantikalauangka && bruh[y, 0] != tidakbisagantikalauangkaa && bruh[y, 0] != tidakbisagantikalauangkaaa)
                        {
                            bruhcopy[y, 0] = ganti;
                        }
                        if (bruh[5, x] != tidakbisagantikalauangka && bruh[5, x] != tidakbisagantikalauangkaa && bruh[5, x] != tidakbisagantikalauangkaaa)
                        {
                            bruhcopy[5, x] = ganti;
                        }
                        if (bruh[y, 5] != tidakbisagantikalauangka && bruh[y, 5] != tidakbisagantikalauangkaa && bruh[y, 5] != tidakbisagantikalauangkaaa)
                        {
                            bruhcopy[y, 5] = ganti;
                        }
                    }
                }
            }
            var papane = clonepapancek(bruhcopy);
            //ganti papan yang null dengan yang dicari
            if (cari == "white")
            {
                for (int y = 0; y < 6; y++)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        if (papane[y, x] == -1)
                        {
                            papane[y, x] = 0;
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
                        if (papane[y, x] == -1)
                        {
                            papane[y, x] = 3;
                        }
                    }
                }
            }
            // pencarian dari kiri ke kanan
            for (int y = 0; y < 6; y++)
            {
                int angkcari = cari == "white" ? 0 : 3;
                int angkcarii = cari == "white" ? 2 : 5;
                //yang pucuk kiri bukan bidka yang dicari maka skip, misal cari putih jika ujung kiri baris itu bukan putih di skip
                if (bruh[y, 0] == angkcari || bruh[y, 0] == angkcarii)
                {
                    historycounter.Clear();
                    recurpossiblemenang(papane, 0, y, cari, "kiri", 0);
                    if (historycounter.Count > 0)
                    {
                        if (hasil > historycounter.Min())
                        {
                            hasil = historycounter.Min();
                        }
                    }
                }
            }
            if (hasil > 5)
            {
                // pencarian dari atas ke bawah
                for (int x = 0; x < 6; x++)
                {
                    int angkcari = cari == "white" ? 0 : 3;
                    int angkcarii = cari == "white" ? 2 : 5;
                    //yang pucuk kiri bukan bidka yang dicari maka skip, misal cari putih jika ujung kiri baris itu bukan putih di skip
                    if (bruh[0, x] == angkcari || bruh[0, x] == angkcarii)
                    {
                        historycounter.Clear();
                        recurpossiblemenang(papane, x, 0, cari, "atas", 0);
                        if (historycounter.Count > 0)
                        {
                            if (hasil > historycounter.Min())
                            {
                                hasil = historycounter.Min();
                            }
                        }
                    }
                }
            }
            if (hasil > 5)
            {
                // pencarian dari kanan ke kiri
                for (int y = 0; y < 6; y++)
                {
                    int angkcari = cari == "white" ? 0 : 3;
                    int angkcarii = cari == "white" ? 2 : 5;
                    //yang pucuk kiri bukan bidka yang dicari maka skip, misal cari putih jika ujung kiri baris itu bukan putih di skip
                    if (bruh[y, 5] == angkcari || bruh[y, 5] == angkcarii)
                    {
                        historycounter.Clear();
                        recurpossiblemenang(papane, 5, y, cari, "kanan", 0);
                        if (historycounter.Count > 0)
                        {
                            if (hasil > historycounter.Min())
                            {
                                hasil = historycounter.Min();
                            }
                        }
                    }
                }
            }
            if (hasil > 5)
            {
                // pencarian dari bawah ke atas
                for (int x = 0; x < 6; x++)
                {
                    int angkcari = cari == "white" ? 0 : 3;
                    int angkcarii = cari == "white" ? 2 : 5;
                    //yang pucuk kiri bukan bidka yang dicari maka skip, misal cari putih jika ujung kiri baris itu bukan putih di skip
                    if (bruh[5, x] == angkcari || bruh[5, x] == angkcarii)
                    {
                        historycounter.Clear();
                        recurpossiblemenang(papane, x, 5, cari, "bawah", 0);
                        if (historycounter.Count > 0)
                        {
                            if (hasil > historycounter.Min())
                            {
                                hasil = historycounter.Min();
                            }
                        }
                    }
                }
            }
        }
        return hasil;
    }
    private minimaxnode minimaxnode_generate(minimaxnode currminmaxnode, int counter, int max, string cari, string currsate)
    {
        if (counter <= max)
        {
            if (counter == max)
            {
                var papancurr = clonepapan(currminmaxnode.curr.papan_game);
                global_children_minimaxnode.Clear();
                makepossible(papancurr, cari);
                List<minimaxnode> childrennya = new List<minimaxnode>();
                foreach (var xx in global_children_minimaxnode)
                {
                    if (currminmaxnode.alpha >= currminmaxnode.beta)
                    {
                        break;
                    }
                    else
                    {
                        minimaxnode baru = new minimaxnode();
                        baru.state = "terminal_node";
                        baru.curr = new papanminimax(xx.papan_game, xx.papan_cek);
                        baru.parent = currminmaxnode;
                        var hasilmenang = cekmenang.cekmenang(xx.papan_cek);
                        if (hasilmenang == 1)
                        {
                            baru.value = -1000000;
                        }
                        else if (hasilmenang == 2)
                        {
                            baru.value = 1000000;
                        }
                        else
                        {
                            var black = terminal_recur_menang(xx.papan_cek, xx.papan_game, "black");
                            var white = terminal_recur_menang(xx.papan_cek, xx.papan_game, "white");
                            baru.value = white - black;
                        }
                        childrennya.Add(baru);
                        if (currminmaxnode.state == "max" && currminmaxnode.value < baru.value)
                        {
                            currminmaxnode.value = baru.value;
                            currminmaxnode.alpha = currminmaxnode.value;
                        }
                        if (currminmaxnode.state == "min" && currminmaxnode.value > baru.value)
                        {
                            currminmaxnode.value = baru.value;
                            currminmaxnode.beta = currminmaxnode.value;
                        }
                    }
                }
                currminmaxnode.children = childrennya;
            }
            else
            {
                var papancurr = clonepapan(currminmaxnode.curr.papan_game);
                global_children_minimaxnode.Clear();
                makepossible(papancurr, cari);
                List<minimaxnode> childrennya = new List<minimaxnode>();
                foreach (var xx in global_children_minimaxnode)
                {
                    minimaxnode baru = new minimaxnode();
                    baru.state = currsate == "max" ? "min" : "max";
                    baru.value = currsate == "max" ? int.MaxValue : int.MinValue;
                    baru.curr = new papanminimax(xx.papan_game, xx.papan_cek);
                    baru.parent = currminmaxnode;
                    childrennya.Add(baru);
                }
                currminmaxnode.children = childrennya;
                currminmaxnode.children[0] = minimaxnode_generate(currminmaxnode.children[0], (counter + 1), max, cari == "black_" ? "white_" : "black_", currsate == "max" ? "min" : "max");
                for (int i = 0; i < currminmaxnode.children.Count; i++)
                {
                    if (currminmaxnode.alpha >= currminmaxnode.beta)
                    {
                        break;
                    }
                    else
                    {
                        var hasilmenang = cekmenang.cekmenang(currminmaxnode.children[i].curr.papan_cek);
                        if (hasilmenang == 1)
                        {
                            currminmaxnode.children[i].value = -1000000;
                        }
                        else if (hasilmenang == 2)
                        {
                            currminmaxnode.children[i].value = +1000000;
                        }
                        else
                        {
                            currminmaxnode.children[i] = minimaxnode_generate(currminmaxnode.children[i], (counter + 1), max, cari == "black_" ? "white_" : "black_", currsate == "max" ? "min" : "max");
                        }
                        if (currminmaxnode.state == "max" && currminmaxnode.value < currminmaxnode.children[i].value)
                        {
                            currminmaxnode.value = currminmaxnode.children[i].value;
                            currminmaxnode.alpha = currminmaxnode.value;
                            currminmaxnode.indexchildpilih = i;
                        }
                        if (currminmaxnode.state == "min" && currminmaxnode.value > currminmaxnode.children[i].value)
                        {
                            currminmaxnode.value = currminmaxnode.children[i].value;
                            currminmaxnode.beta = currminmaxnode.value;
                            currminmaxnode.indexchildpilih = i;
                        }
                    }
                }
            }
            return currminmaxnode;
        }
        return null;
    }
    public void cetak(int[,] papan)
    {
        var oute = "";
        for (int yy = 0; yy < 6; yy++)
        {
            oute += "{";
            for (int xx = 0; xx < 6; xx++)
            {
                oute += papan[yy, xx] + ",";
            }
            oute += "},\n";
        }
        Debug.Log("{" + oute + "}");
    }
    List<int> historycounter = new List<int>();
    private void recurpossiblemenang(int[,] papancek, int x, int y, string warna, string start, int counter)
    {
        var papanbaru = clonepapancek(papancek);
        //-100 sebagai penanda yang sudah dilewati
        if (x > 5 || y > 5 || x < 0 || y < 0)
        {

        }
        else
        {
            if (papanbaru[y, x] != -100)
            {
                int lanjut = 0;
                if (warna == "white" && (papanbaru[y, x] == 0 || papanbaru[y, x] == 2))
                {
                    papanbaru[y, x] = -100;
                    if (start == "kiri" && x == 5)
                    {
                        historycounter.Add(counter);
                    }
                    else if (start == "atas" && y == 5)
                    {
                        historycounter.Add(counter);
                    }
                    else if (start == "kanan" && x == 0)
                    {
                        historycounter.Add(counter);
                    }
                    else if (start == "bawah" && y == 0)
                    {
                        historycounter.Add(counter);
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
                        historycounter.Add(counter);
                    }
                    else if (start == "atas" && y == 5)
                    {
                        historycounter.Add(counter);
                    }
                    else if (start == "kanan" && x == 0)
                    {
                        historycounter.Add(counter);
                    }
                    else if (start == "bawah" && y == 0)
                    {
                        historycounter.Add(counter);
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
                        recurpossiblemenang(papanbaru, x + 1, y, warna, start, (counter + 1));
                        if (historycounter.Count == 0)
                        {
                            //ke atas
                            recurpossiblemenang(papanbaru, x, y - 1, warna, start, (counter + 1));
                            //ke bawah
                            recurpossiblemenang(papanbaru, x, y + 1, warna, start, (counter + 1));
                        }
                    }
                    if (start == "atas")
                    {
                        //ke bawah
                        recurpossiblemenang(papanbaru, x, y + 1, warna, start, (counter + 1));
                        if (historycounter.Count == 0)
                        {
                            //ke kanan
                            recurpossiblemenang(papanbaru, x + 1, y, warna, start, (counter + 1));
                            //ke kiri
                            recurpossiblemenang(papanbaru, x - 1, y, warna, start, (counter + 1));
                        }
                    }
                    if (start == "kanan")
                    {
                        //ke kiri
                        recurpossiblemenang(papanbaru, x - 1, y, warna, start, (counter + 1));
                        if (historycounter.Count == 0)
                        {
                            //ke atas
                            recurpossiblemenang(papanbaru, x, y - 1, warna, start, (counter + 1));
                            //ke bawah
                            recurpossiblemenang(papanbaru, x, y + 1, warna, start, (counter + 1));
                        }
                    }
                    if (start == "bawah")
                    {
                        //ke atas
                        recurpossiblemenang(papanbaru, x, y - 1, warna, start, (counter + 1));
                        if (historycounter.Count == 0)
                        {
                            //ke kanan
                            recurpossiblemenang(papanbaru, x + 1, y, warna, start, (counter + 1));
                            //ke kiri
                            recurpossiblemenang(papanbaru, x - 1, y, warna, start, (counter + 1));
                        }
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
        var telusuri = sebelumnya;
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
            telusuri.children = childe;
            telusuri = childe;
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
                    Bidak bidakbaru = new Bidak(false, namabidaktaruh, 1);
                    papanclone[i] = bidakbaru;
                    catatprobpapan(papanclone);
                    //modewall
                    bidakbaru = new Bidak(false, namabidaktaruh, 2);
                    papanclone[i] = bidakbaru;
                    catatprobpapan(papanclone);

                }
                if (adacapstone == 1)
                {
                    // jika masih ada captstone
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
                int banyakchildsebelumnya;
                banyakchildsebelumnya = global_children_minimaxnode.Count;
                generatekombinasigerakkanan(bidakbayangan, papanclone, i);
                if (banyakchildsebelumnya != global_children_minimaxnode.Count)
                {
                    //karena kalau gak bisa gesert dia akan tidak melakukan push apa-apa & kalau push akan menghasilkan taruh di bidak sendiri(itu yang dihapus)
                    global_children_minimaxnode.RemoveAt(global_children_minimaxnode.Count - 1);
                }
                banyakchildsebelumnya = global_children_minimaxnode.Count;
                generatekombinasigerakkiri(bidakbayangan, papanclone, i);
                if (banyakchildsebelumnya != global_children_minimaxnode.Count)
                {
                    //karena kalau gak bisa gesert dia akan tidak melakukan push apa-apa & kalau push akan menghasilkan taruh di bidak sendiri(itu yang dihapus)
                    global_children_minimaxnode.RemoveAt(global_children_minimaxnode.Count - 1);
                }
                // banyakchildsebelumnya = global_children_minimaxnode.Count;
                // generatekombinasigerakatas(bidakbayangan, papanclone, i);
                // if (banyakchildsebelumnya != global_children_minimaxnode.Count)
                // {
                //     //karena kalau gak bisa gesert dia akan tidak melakukan push apa-apa & kalau push akan menghasilkan taruh di bidak sendiri(itu yang dihapus)
                //     global_children_minimaxnode.RemoveAt(global_children_minimaxnode.Count - 1);
                // }
                // banyakchildsebelumnya = global_children_minimaxnode.Count;
                // generatekombinasigerakbawah(bidakbayangan, papanclone, i);
                // if (banyakchildsebelumnya != global_children_minimaxnode.Count)
                // {
                //     //karena kalau gak bisa gesert dia akan tidak melakukan push apa-apa & kalau push akan menghasilkan taruh di bidak sendiri(itu yang dihapus)
                //     global_children_minimaxnode.RemoveAt(global_children_minimaxnode.Count - 1);
                // }
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
                            isi = 2;
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
        if (lokasi <= 35)
        {
            if (curr.namabidak == "bidak_bayangan" && curr.children == null)
            {
                catatprobpapan(papan);
            }
            else
            {

                //alur => geser semua bidak lalu turunin satu statu 

                if ((lokasi + 1) % 6 == 0)
                {
                    if (papan[lokasi] != null && !(papan[lokasi].iscapstone || papan[lokasi].penomoran == 2))
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
                                        papanjalan2[lokasi + 1] = telusuri;
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
        if (lokasi >= 0)
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
                    if (papan[lokasi] != null && (papan[lokasi].iscapstone || papan[lokasi].penomoran == 2))
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
                            Debug.Log("papan jalan 2  ke " + lokasi + " = " + papanjalan2[lokasi].namabidak + " sebelumnya " + (papanjalan2[lokasi - 1] != null ? papanjalan2[lokasi - 1].namabidak + "==" + papanjalan2[lokasi - 1].penomoran : "")); ;
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
                                        papanjalan2[lokasi - 1] = telusuri;
                                    }
                                }
                                else
                                {
                                    papanjalan2[lokasi - 1] = telusuri;
                                }
                            }
                            Debug.Log("papan jalan 2  ke " + lokasi + " = " + papanjalan2[lokasi].namabidak + " oke sebelumnya " + (papanjalan2[lokasi - 1] != null ? papanjalan2[lokasi - 1].namabidak + "==" + papanjalan2[lokasi - 1].penomoran : "")); ;
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
        if (lokasi >= 0)
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
                    if (papan[lokasi] != null && (papan[lokasi].iscapstone || papan[lokasi].penomoran == 2))
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
                        generatekombinasigerakatas(currjalan, papanjalan, (lokasi - 6));
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
                    if (papanjalan[lokasi - 6] != null)
                    {
                        if (!(papanjalan[lokasi - 6].iscapstone || papanjalan[lokasi - 6].penomoran == 2))
                        {
                            papanjalan[lokasi - 6].parent = telusuri;
                            telusuri.children = papanjalan[lokasi - 6];
                            papanjalan[lokasi - 6] = telusuri;
                        }
                    }
                    else
                    {
                        //kita tidak perlu mensetting lokasi papan karena itu untuk mempermudah pembuatan di unity saja
                        papanjalan[lokasi - 6] = telusuri;
                    }
                    generatekombinasigerakatas(currjalan, papanjalan, (lokasi - 6));
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
                                if (papanjalan2[lokasi - 6] != null)
                                {
                                    if (!(papanjalan2[lokasi - 6].iscapstone || papanjalan2[lokasi - 6].penomoran == 2))
                                    {
                                        papanjalan2[lokasi - 6].parent = telusuri;
                                        telusuri.children = papanjalan2[lokasi - 6];
                                        papanjalan2[lokasi - 6] = telusuri;
                                    }
                                }
                                else
                                {
                                    papanjalan2[lokasi - 6] = telusuri;
                                }
                            }
                            generatekombinasigerakatas(currjalan2, papanjalan2, (lokasi - 6));
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
        if (lokasi <= 35)
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
                    if (papan[lokasi] != null && (papan[lokasi].iscapstone || papan[lokasi].penomoran == 2))
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
                        generatekombinasigerakbawah(currjalan, papanjalan, (lokasi + 6));
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
                    if (papanjalan[lokasi + 6] != null)
                    {
                        if (!(papanjalan[lokasi + 6].iscapstone || papanjalan[lokasi + 6].penomoran == 2))
                        {
                            papanjalan[lokasi + 6].parent = telusuri;
                            telusuri.children = papanjalan[lokasi + 6];
                            papanjalan[lokasi + 6] = telusuri;
                        }
                    }
                    else
                    {
                        //kita tidak perlu mensetting lokasi papan karena itu untuk mempermudah pembuatan di unity saja
                        papanjalan[lokasi + 6] = telusuri;
                    }
                    generatekombinasigerakbawah(currjalan, papanjalan, (lokasi + 6));
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
                                if (papanjalan2[lokasi + 6] != null)
                                {
                                    if (!(papanjalan2[lokasi + 6].iscapstone || papanjalan2[lokasi + 6].penomoran == 2))
                                    {
                                        papanjalan2[lokasi + 6].parent = telusuri;
                                        telusuri.children = papanjalan2[lokasi + 6];
                                        papanjalan2[lokasi + 6] = telusuri;
                                    }
                                }
                                else
                                {
                                    papanjalan2[lokasi + 6] = telusuri;
                                }
                            }
                            generatekombinasigerakbawah(currjalan2, papanjalan2, (lokasi + 6));
                            papanjalan2 = clonepapan(papanbck);
                            currjalan2 = clonebidak(bidakbck);
                        }
                    }
                }
            }
        }
    }
}
