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
            if(datagame.selectedbidak != "")
            {
                if(datagame.childrenselectedbidak == null)
                {
                    //jika yang dipilih ada bidak yang dipinggir papan (belum di papan), maka tidak ada aturan untuk memilih papan
                    if (hit.transform.name.Contains("papan"))
                    {
                        datagame.selectedpapan = hit.transform.name;
                        for (int i = 0; i < parentpapan.transform.childCount; i++)
                        {
                            var objpapan = parentpapan.transform.GetChild(i);
                            if (objpapan.name == hit.transform.name)
                            {
                                (objpapan.transform.GetComponent<MeshRenderer>()).enabled = true;
                            }
                            else
                            {
                                (objpapan.transform.GetComponent<MeshRenderer>()).enabled = false;
                            }
                        }
                    }
                    if (hit.transform.name.Contains("white_") || hit.transform.name.Contains("black_"))
                    {
                        var papanbidak = datagame.get_namapapan_dari_bidak(hit.transform.name);
                        if (papanbidak != null)
                        {
                            datagame.selectedpapan = papanbidak;
                            for (int i = 0; i < parentpapan.transform.childCount; i++)
                            {
                                var objpapan = parentpapan.transform.GetChild(i);
                                if (objpapan.name == papanbidak)
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
                else
                {
                    //jika yang dipilih yang di papan
                    List<string> possiblepapan = new List<string>();
                    var papansekarang = datagame.get_namapapan_dari_bidak(datagame.selectedbidak);
                    for(int i = 0;i < 7; i++)
                    {
                        //untuk horizontal
                        possiblepapan.Add("papan"+(i+1)+"_"+papansekarang.Substring(7,1));
                        //untuk vertikal
                        possiblepapan.Add(papansekarang.Substring(0, 7)+(i+1));
                    }
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
                if (hit.transform.name.Contains("white_") || hit.transform.name.Contains("black"))
                {
                    if (datagame.selectedbidak == "")
                    {
                        datagame.selectedbidak = hit.transform.name;
                        var posisibidak = hit.transform.position;
                        posisibidak[1] += 1;
                        hit.transform.position = posisibidak;
                        datagame.updatepenomoranbidak = 1;
                        datagame.setchildselectbidak();
                    }
                    else
                    {
                        if (datagame.selectedbidak.Equals(hit.transform.name))
                        {
                            //jika yang diklik bidaknya sama dengan yang sedang aktif maka rotate, menjadi wall/kembali
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
                        else
                        {
                            if (datagame.get_namapapan_dari_bidak(hit.transform.name) == "")
                            {
                                //jika klik beda beda jadi unselect, tapi jika bidak yang diclick tidak berada di papan
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
                                var bidakpilih = datagame.selectedbidak;
                                var children = listbidak.transform;
                                var namapapanpilih = datagame.selectedpapan;
                                GameObject papane = null;
                                for(int i = 0; i < parentpapan.transform.childCount; i++)
                                {
                                    if (parentpapan.transform.GetChild(i).name == namapapanpilih)
                                    {
                                        papane = parentpapan.transform.GetChild(i).gameObject;
                                    }
                                }
                                for (int i = 0; i < children.childCount; i++)
                                {
                                    if (children.GetChild(i).name.Equals(bidakpilih))
                                    {
                                        var hasiltaruh = datagame.taruhbidak();
                                        if (hasiltaruh != -1)
                                        {
                                            var bidaknya = children.GetChild(i);
                                            Vector3 papanposition = papane.transform.position;
                                            papanposition[1] += hasiltaruh;
                                            if (bidaknya.name.Contains("black_"))
                                            {
                                                papanposition[0] += 0.7f;
                                            }
                                            bidaknya.position = papanposition;
                                        }
                                        break;
                                    }
                                }
                                //mengembalikan posisi bidak supaya turun (unselect) (sebelumnya kena select jadi agak naik)
                                for (int i = 0; i < children.childCount; i++)
                                {
                                    if (children.GetChild(i).name.Equals(datagame.selectedbidak))
                                    {
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
            }
        }
    }
}
