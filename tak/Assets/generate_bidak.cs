using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_bidak : MonoBehaviour
{
    [SerializeField] GameObject objcloneputih;
    [SerializeField] GameObject objclonehitam;
    [SerializeField] GameObject parentputih;
    [SerializeField] GameObject parenthitam;
    [SerializeField] GameObject parentforchild;
    // Start is called before the first frame update
    void Start()
    {
        var posisiz = parentputih.transform.position.z;
        var counter = 0;
        for (int i = 0; i < 30; i++)
        {
            if (counter == 10)
            {
                posisiz += 4;
                counter = 0;
            }
            float penambahtinggi = 0.5f * counter;
            GameObject bidak = Instantiate(objcloneputih);
            bidak.name = "white_" + (i + 1);
            Vector3 posisibidak = parentputih.transform.position;
            posisibidak[1] += penambahtinggi;
            posisibidak[2] += posisiz;
            bidak.transform.position = posisibidak;
            bidak.AddComponent<MeshCollider>();
            bidak.transform.parent = parentforchild.transform;
            counter++;
        }
        posisiz = parenthitam.transform.position.z;
        counter = 0;
        for (int i = 0; i < 30; i++)
        {
            if (counter == 10)
            {
                posisiz += 4;
                counter = 0;
            }
            float penambahtinggi = 0.5f * counter;
            GameObject bidak = Instantiate(objclonehitam);
            bidak.name = "black_" + (i + 1);
            Vector3 posisibidak = parenthitam.transform.position;
            posisibidak[1] += penambahtinggi;
            posisibidak[2] += posisiz;
            bidak.transform.position = posisibidak;
            bidak.AddComponent<MeshCollider>();
            bidak.transform.parent = parentforchild.transform;
            counter++;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
