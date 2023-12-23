using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private void OnMouseUp()
    {
        var bidakpilih = datagame.selectedbidak;
        var children = parentbidak.transform;
        for(int i=0;i<children.childCount;i++)
        {
            if(children.GetChild(i).name.Equals(bidakpilih))
            {
                var hasiltaruh = datagame.taruhbidak();
                if (hasiltaruh != -1)
                {
                    var bidaknya = children.GetChild(i);
                    Vector3 papanposition = transform.position;
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
