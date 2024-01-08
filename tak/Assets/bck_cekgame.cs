using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cekgamee : MonoBehaviour
{
    public data datagame;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // var hasil = cekmenang(null);
        // if(hasil == 1)
        // {
        //     Debug.Log("White Win");
        // }else if(hasil == 2)
        // {
        //     Debug.Log("Black Win");
        // }
    }
    private int recur_jalan_cek_menang_hitam_kesamping(int y, int x, List<int[]> history_kunjungan)
    {
        var papan = datagame.papan_cek;
        if (x == 5)
        {
            return 1;
        }
        else
        {
            int ybaru = 0;
            int xbaru = 0;
            //pengecekan kesebelah atas dari current
            if (y - 1 >= 0)
            {
                ybaru = y - 1;
                xbaru = x;
                if (papan[ybaru, xbaru] == 3 || papan[ybaru, xbaru] == 5)
                {
                    int sudahpernah = 0;
                    foreach (var z in history_kunjungan)
                    {
                        if (z[0] == xbaru && z[1] == ybaru)
                        {
                            sudahpernah = 1;
                            break;
                        }
                    }
                    if (sudahpernah == 0)
                    {
                        int[] databaru = { xbaru, ybaru };
                        history_kunjungan.Add(databaru);
                        var hasil = recur_jalan_cek_menang_hitam_kesamping(ybaru, xbaru, history_kunjungan);
                        if (hasil == 1)
                        {
                            return hasil;
                        }
                    }
                }
            }
            // pengecekan keseblah bawah dari current
            if (y + 1 <= 5)
            {
                ybaru = y + 1;
                xbaru = x;
                if (papan[ybaru, xbaru] == 3 || papan[ybaru, xbaru] == 5)
                {
                    int sudahpernah = 0;
                    foreach (var z in history_kunjungan)
                    {
                        if (z[0] == xbaru && z[1] == ybaru)
                        {
                            sudahpernah = 1;
                            break;
                        }
                    }
                    if (sudahpernah == 0)
                    {
                        int[] databaru = { xbaru, ybaru };
                        history_kunjungan.Add(databaru);
                        var hasil = recur_jalan_cek_menang_hitam_kesamping(ybaru, xbaru, history_kunjungan);
                        if (hasil == 1)
                        {
                            return hasil;
                        }
                    }
                }
            }
            /*pengecekan kesebelah kanan dari current*/
            xbaru = x + 1;
            ybaru = y;
            if (papan[ybaru, xbaru] == 3 || papan[ybaru, xbaru] == 5)
            {
                int sudahpernah = 0;
                foreach (var z in history_kunjungan)
                {
                    if (z[0] == xbaru && z[1] == ybaru)
                    {
                        sudahpernah = 1;
                        break;
                    }
                }
                if (sudahpernah == 0)
                {
                    int[] databaru = { xbaru, ybaru };
                    history_kunjungan.Add(databaru);
                    var hasil = recur_jalan_cek_menang_hitam_kesamping(ybaru, xbaru, history_kunjungan);
                    if (hasil == 1)
                    {
                        return hasil;
                    }
                }
            }
            return 0;
        }
    }
    private int recur_jalan_cek_menang_hitam_kebawah(int y, int x, List<int[]> history_kunjungan)
    {
        var papan = datagame.papan_cek;
        if (y == 5)
        {
            return 1;
        }
        else
        {
            int ybaru = 0;
            int xbaru = 0;
            //pengecekan ke atas dari current
            if (x - 1 >= 0)
            {
                ybaru = y;
                xbaru = x - 1;
                if (papan[ybaru, xbaru] == 3 || papan[ybaru, xbaru] == 5)
                {
                    int sudahpernah = 0;
                    foreach (var z in history_kunjungan)
                    {
                        if (z[0] == xbaru && z[1] == ybaru)
                        {
                            sudahpernah = 1;
                            break;
                        }
                    }
                    if (sudahpernah == 0)
                    {
                        int[] databaru = { xbaru, ybaru };
                        history_kunjungan.Add(databaru);
                        var hasil = recur_jalan_cek_menang_hitam_kebawah(ybaru, xbaru, history_kunjungan);
                        if (hasil == 1)
                        {
                            return hasil;
                        }
                    }
                }
            }
            /*pengecekan kesebelah kanan dari current*/
            if (x + 1 < 5)
            {
                ybaru = y;
                xbaru = x + 1;
                if (papan[ybaru, xbaru] == 3 || papan[ybaru, xbaru] == 5)
                {
                    int sudahpernah = 0;
                    foreach (var z in history_kunjungan)
                    {
                        if (z[0] == xbaru && z[1] == ybaru)
                        {
                            sudahpernah = 1;
                            break;
                        }
                    }
                    if (sudahpernah == 0)
                    {
                        int[] databaru = { xbaru, ybaru };
                        history_kunjungan.Add(databaru);
                        var hasil = recur_jalan_cek_menang_hitam_kebawah(ybaru, xbaru, history_kunjungan);
                        if (hasil == 1)
                        {
                            return hasil;
                        }
                    }
                }
            }
            xbaru = x;
            ybaru = y + 1;
            if (papan[ybaru, xbaru] == 3 || papan[ybaru, xbaru] == 5)
            {
                int sudahpernah = 0;
                foreach (var z in history_kunjungan)
                {
                    if (z[0] == xbaru && z[1] == ybaru)
                    {
                        sudahpernah = 1;
                        break;
                    }
                }
                if (sudahpernah == 0)
                {
                    int[] databaru = { xbaru, ybaru };
                    history_kunjungan.Add(databaru);
                    var hasil = recur_jalan_cek_menang_hitam_kebawah(ybaru, xbaru, history_kunjungan);
                    if (hasil == 1)
                    {
                        return hasil;
                    }
                }
            }
            return 0;
        }
    }
    private int recur_jalan_cek_menang_putih_kesamping(int y, int x, List<int[]> history_kunjungan)
    {
        var papan = datagame.papan_cek;
        if (x == 5)
        {
            return 1;
        }
        else
        {
            int ybaru = 0;
            int xbaru = 0;
            //pengecekan kesebelah atas dari current
            if (y - 1 >= 0)
            {
                ybaru = y - 1;
                xbaru = x;
                if (papan[ybaru, xbaru] == 0 || papan[ybaru, xbaru] == 2)
                {
                    int sudahpernah = 0;
                    foreach (var z in history_kunjungan)
                    {
                        if (z[0] == xbaru && z[1] == ybaru)
                        {
                            sudahpernah = 1;
                            break;
                        }
                    }
                    if (sudahpernah == 0)
                    {
                        int[] databaru = { xbaru, ybaru };
                        history_kunjungan.Add(databaru);
                        var hasil = recur_jalan_cek_menang_putih_kesamping(ybaru, xbaru, history_kunjungan);
                        if (hasil == 1)
                        {
                            return hasil;
                        }
                    }
                }
            }
            // pengecekan keseblah bawah dari current
            if (y + 1 <= 5)
            {
                ybaru = y + 1;
                xbaru = x;
                if (papan[ybaru, xbaru] == 0 || papan[ybaru, xbaru] == 2)
                {
                    int sudahpernah = 0;
                    foreach (var z in history_kunjungan)
                    {
                        if (z[0] == xbaru && z[1] == ybaru)
                        {
                            sudahpernah = 1;
                            break;
                        }
                    }
                    if (sudahpernah == 0)
                    {
                        int[] databaru = { xbaru, ybaru };
                        history_kunjungan.Add(databaru);
                        var hasil = recur_jalan_cek_menang_putih_kesamping(ybaru, xbaru, history_kunjungan);
                        if (hasil == 1)
                        {
                            return hasil;
                        }
                    }
                }
            }
            /*pengecekan kesebelah kanan dari current*/
            xbaru = x + 1;
            ybaru = y;
            if (papan[ybaru, xbaru] == 0 || papan[ybaru, xbaru] == 2)
            {
                int sudahpernah = 0;
                foreach (var z in history_kunjungan)
                {
                    if (z[0] == xbaru && z[1] == ybaru)
                    {
                        sudahpernah = 1;
                        break;
                    }
                }
                if (sudahpernah == 0)
                {
                    int[] databaru = { xbaru, ybaru };
                    history_kunjungan.Add(databaru);
                    var hasil = recur_jalan_cek_menang_putih_kesamping(ybaru, xbaru, history_kunjungan);
                    if (hasil == 1)
                    {
                        return hasil;
                    }
                }
            }
            return 0;
        }
    }
    private int recur_jalan_cek_menang_putih_kebawah(int y, int x, List<int[]> history_kunjungan)
    {
        var papan = datagame.papan_cek;
        if (y == 5)
        {
            return 1;
        }
        else
        {
            int ybaru = 0;
            int xbaru = 0;
            //pengecekan ke atas dari current
            if (x - 1 >= 0)
            {
                ybaru = y;
                xbaru = x - 1;
                if (papan[ybaru, xbaru] == 0 || papan[ybaru, xbaru] == 2)
                {
                    int sudahpernah = 0;
                    foreach (var z in history_kunjungan)
                    {
                        if (z[0] == xbaru && z[1] == ybaru)
                        {
                            sudahpernah = 1;
                            break;
                        }
                    }
                    if (sudahpernah == 0)
                    {
                        int[] databaru = { xbaru, ybaru };
                        history_kunjungan.Add(databaru);
                        var hasil = recur_jalan_cek_menang_putih_kebawah(ybaru, xbaru, history_kunjungan);
                        if (hasil == 1)
                        {
                            return hasil;
                        }
                    }
                }
            }
            /*pengecekan kesebelah kanan dari current*/
            if (x + 1 < 5)
            {
                ybaru = y;
                xbaru = x + 1;
                if (papan[ybaru, xbaru] == 0 || papan[ybaru, xbaru] == 2)
                {
                    int sudahpernah = 0;
                    foreach (var z in history_kunjungan)
                    {
                        if (z[0] == xbaru && z[1] == ybaru)
                        {
                            sudahpernah = 1;
                            break;
                        }
                    }
                    if (sudahpernah == 0)
                    {
                        int[] databaru = { xbaru, ybaru };
                        history_kunjungan.Add(databaru);
                        var hasil = recur_jalan_cek_menang_putih_kebawah(ybaru, xbaru, history_kunjungan);
                        if (hasil == 1)
                        {
                            return hasil;
                        }
                    }
                }
            }
            xbaru = x;
            ybaru = y + 1;
            if (papan[ybaru, xbaru] == 0 || papan[ybaru, xbaru] == 2)
            {
                int sudahpernah = 0;
                foreach (var z in history_kunjungan)
                {
                    if (z[0] == xbaru && z[1] == ybaru)
                    {
                        sudahpernah = 1;
                        break;
                    }
                }
                if (sudahpernah == 0)
                {
                    int[] databaru = { xbaru, ybaru };
                    history_kunjungan.Add(databaru);
                    var hasil = recur_jalan_cek_menang_putih_kebawah(ybaru, xbaru, history_kunjungan);
                    if (hasil == 1)
                    {
                        return hasil;
                    }
                }
            }
            return 0;
        }
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
                    if (recur_jalan_cek_menang_putih_kesamping(y, 0, new List<int[]>()) == 1)
                    {
                        return 1;
                    }
                }
                //untuk hitam
                if (papan[y, 0] >= 3 && papan[y, 0] <= 5)
                {
                    if (recur_jalan_cek_menang_hitam_kesamping(y, 0, new List<int[]>()) == 1)
                    {
                        return 2;
                    }
                }
            }
        }
        // //untuk kebawah
        // for (int x = 0; x < 6; x++)
        // {
        //     if (papan[0,x] != -1)
        //     {
        //         //untuk putih
        //         if (papan[0,x] >= 0 && papan[0, x] <= 2)
        //         {
        //             if (recur_jalan_cek_menang_putih_kebawah(0, x, new List<int[]>()) == 1)
        //             {
        //                 return 1;
        //             }
        //         }
        //         //untuk hitam
        //         if (papan[0, x] >= 3 && papan[0, x] <= 5)
        //         {
        //             if (recur_jalan_cek_menang_hitam_kebawah(0, x, new List<int[]>()) == 1)
        //             {
        //                 return 2;
        //             }
        //         }
        //     }
        // }
        return -1;
    }
}
