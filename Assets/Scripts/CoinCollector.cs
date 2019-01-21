using System.Collections;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public int Value = 0;
    public Collider coinCollider;
    private bool collided = false;
    public AudioSource CoinCollectedSound;
    [Range(0f, 2f)]
    public float Test;
    public GameObject effect;

    public void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.tag == "Player" && !collided)
        {
            CoinCollectedSound.Play();

            obj.gameObject.GetComponent<PlayerController>().Score += Value;
            
            Instantiate(effect, transform.position, transform.rotation);

            Destroy(gameObject);
            FindObjectOfType<GameManager>().CoinsRemaining -= 1;
            collided = true;
        }
    }

    public enum OccilationFuntion { Sine, Cosine }
    public void Start()
    {
        StartCoroutine(Oscillate(OccilationFuntion.Sine, 0.1f, 2f));
    }

    private IEnumerator Oscillate(OccilationFuntion method, float scalar, float speed)
    {
        while (true)
        {
            if (method == OccilationFuntion.Sine)
            {
                transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * speed) * scalar + transform.localScale.y, transform.position.z);
            }
            else if (method == OccilationFuntion.Cosine)
            {
                transform.position = new Vector3(transform.position.x, Mathf.Cos(Time.time * speed) * scalar + transform.localScale.y, transform.position.z);
            }
            yield return null;
        }
    }
}
