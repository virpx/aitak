using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cekgame : MonoBehaviour
{
    public data datagame;
    // Start is called before the first frame update
    void Start()
    {
        // int[,] papan_tmp =
        //         {
        //     {-1,-1,-1,-1,-1,-1},
        //     {-1,-1,-1,-1,-1,-1},
        //     {-1,-1,-1,-1,-1,-1},
        //     {-1,-1,-1,-1,-1,-1},
        //     {-1,-1,-1,0,2,0},
        //     {0,0,0,0,4,0},
        // };
        // Debug.Log(cekmenang(papan_tmp));
    }

    // Update is called once per frame
    void Update()
    {
        var hasil = cekmenang(null);
        if(hasil == 1)
        {
            Debug.Log("White Win");
            datagame.currentplayer = -2;
        }else if(hasil == 2)
        {
            Debug.Log("Black Win");
            datagame.currentplayer = -2;
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
    public int recursamping(int[,] papan, string warna, int x, int y)
    {
        var papancek = clonepapancek(papan);
        if (x <= 5 && y <= 5 && x >= 0 && y >= 0)
        {
            if (papancek[y, x] != -100)
            {
                int angka_masuk = warna == "white" ? 0 : 3;
                int angka_masukk = warna == "white" ? 2 : 5;
                if (papancek[y, x] == angka_masuk || papancek[y, x] == angka_masukk)
                {
                    if (x == 5)
                    {
                        return 1;
                    }
                    else
                    {
                        papancek[y, x] = -100;
                        var hasil = 0;
                        hasil += recursamping(papancek, warna, (x + 1), y);
                        hasil += recursamping(papancek, warna, (x - 1), y);
                        hasil += recursamping(papancek, warna, x, (y + 1));
                        hasil += recursamping(papancek, warna, x, (y - 1));
                        return hasil;
                    }
                }
            }
        }
        return 0;
    }
    public int recurbawah(int[,] papan, string warna, int x, int y)
    {
        var papancek = clonepapancek(papan);
        if (x <= 5 && y <= 5 && x >= 0 && y >= 0)
        {
            if (papancek[y, x] != -100)
            {
                int angka_masuk = warna == "white" ? 0 : 3;
                int angka_masukk = warna == "white" ? 2 : 5;
                if (papancek[y, x] == angka_masuk || papancek[y, x] == angka_masukk)
                {
                    if (y == 5)
                    {
                        return 1;
                    }
                    else
                    {
                        papancek[y, x] = -100;
                        var hasil = 0;
                        hasil += recurbawah(papancek, warna, (x + 1), y);
                        hasil += recurbawah(papancek, warna, (x - 1), y);
                        hasil += recurbawah(papancek, warna, x, (y + 1));
                        hasil += recurbawah(papancek, warna, x, (y - 1));
                        return hasil;
                    }
                }
            }
        }
        return 0;
    }
    public int cekmenang(int[,] papan)
    {
        if (papan == null)
        {
            papan = datagame.papan_cek;
        }
        //cek kesamping
        for (int y = 0; y < 6; y++)
        {
            if (papan[y, 0] != -1)
            {
                //untuk putih
                if (papan[y, 0] >= 0 && papan[y, 0] <= 2)
                {
                    if (recursamping(papan, "white", 0, y) > 0)
                    {
                        return 1;
                    }
                }
                //untuk hitam
                if (papan[y, 0] >= 3 && papan[y, 0] <= 5)
                {
                    if (recursamping(papan, "black", 0, y) > 0)
                    {
                        return 2;
                    }
                }
            }
        }
        //untuk kebawah
        for (int x = 0; x < 6; x++)
        {
            if (papan[0, x] != -1)
            {
                //untuk putih
                if (papan[0, x] >= 0 && papan[0, x] <= 2)
                {
                    if (recurbawah(papan, "white", x, 0) > 0)
                    {
                        return 1;
                    }
                }
                //untuk hitam
                if (papan[0, x] >= 3 && papan[0, x] <= 5)
                {
                    if (recurbawah(papan, "black", x, 0) > 0)
                    {
                        return 2;
                    }
                }
            }
        }
        return -1;
    }
}
