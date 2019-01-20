using UnityEngine;

public class TilesColider : MonoBehaviour
{
    public BoxCollider myCollide;
    public BoxCollider myCollideTrigger;
    public Color color;
    private bool beBlue;

    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        myCollideTrigger = GetComponents<BoxCollider>()[0];
        myCollide = GetComponents<BoxCollider>()[1];
        myCollide.enabled = false;
        setColor();
        setTexture();
    }

    private void setTexture()
    {
        var renderer = GetComponent<MeshRenderer>();
        renderer.material = material;
    }

    private void setColor()
    {
       // GetComponent<MeshRenderer>().material.SetColor("_Color", beBlue ? Color.blue : color);
    }


    private void OnTriggerEnter(Collider other)
    {
        beBlue = true;
        setColor();        
    }

    private void OnTriggerExit(Collider other)
    {
        beBlue = false;
        setColor();
    }
    void onCollisionEnter(GameObject other)
    {
        
    }
}
