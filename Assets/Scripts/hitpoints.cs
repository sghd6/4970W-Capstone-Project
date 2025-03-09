using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;
    bool death = false;
    public float reviveTime = 2f;
    public int hitpoints = 3;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnCollisionEnter (Collision info)
    {
            if (info.collider.tag == "Obstacle")
            {
                hitpoints = hitpoints -1;

                if (hitpoints == 0)
                {
                    movement.enabled = false;
                    Invoke("Restart", reviveTime);
                }
            }
       // if (info.collider.tag == "Obstacle")
        //{
            //movement.enabled = false;
        //}
    }

    void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
