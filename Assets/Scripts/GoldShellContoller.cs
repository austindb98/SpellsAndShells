using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldShellContoller : MonoBehaviour
{
    private BasePlayer player;
    public Image icon;
    private Sprite goldShell;
    public AudioClip pickupSound;
    // Start is called before the first frame update
    private float startMoving;
    private static float waitTime = .25f;
    private static float maxSpeed = 50f;
    private static float minSpeed = 10f;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        //GameObject hud = GameObject.FindWithTag("Hud");
        goldShell = transform.GetComponent<SpriteRenderer>().sprite;

        player = playerObject.GetComponent<BasePlayer>();
        startMoving = Time.time + waitTime;
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        speed = Mathf.Max(minSpeed, Mathf.Min(maxSpeed, distanceToPlayer));
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > startMoving)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer < .5)
            {
                SoundController.PlaySound(pickupSound);
                icon.sprite = goldShell;
                Destroy(gameObject);
            }
            float diffX = player.transform.position.x - transform.position.x;
            float diffY = player.transform.position.y - transform.position.y;
            Vector3 change = new Vector3(diffX, diffY, 0);
            change = change.normalized;
            Vector3 position = transform.position;
            position.x = position.x + ((speed * change.x) * Time.deltaTime);
            position.y = position.y + ((speed * change.y) * Time.deltaTime);
            transform.position = position;

        }
    }
}
