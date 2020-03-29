using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [Range(0.5f, 100f)]
    public float Speed = 5f;

    [Range(0.5f, 10000f)]
    public float ShootVelocity = 5f;

    public GameObject CannonballPrefab;

    public Transform[] ShootTransforms;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private Vector2 GetInput()
    {
        //Vector2 inp = new Vector2(
        //    Input.GetAxis("Horizontal")
        //    ) ;

        return  Vector2.zero;
    }

    public void Move(Vector2 dir)
    {
        var offset = (dir) * Speed * Time.deltaTime;
        transform.Translate(offset);
    }

    bool bound = false;
    bool shouldMove = false;

    private Vector2 direction;
    public void BindMove(InputAction.CallbackContext context)
    {
        PlayerInput input;
        if (!bound)
        {
            bound = true;
            context.action.canceled += (f) => { shouldMove = false; };
            context.action.performed += (f) =>
            {
                shouldMove = true;
                direction = f.ReadValue<Vector2>();
            };
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed)
        {
            var tf = ShootTransforms[shootCounter];
            var newProjectile = Instantiate(CannonballPrefab, tf.position, Quaternion.identity);
            newProjectile.transform.position = tf.position;
            newProjectile.GetComponent<Rigidbody>().AddForce(tf.up * ShootVelocity, ForceMode.Impulse);
            shootCounter++;
            shootCounter %= (ShootTransforms.Length);
        }
    }


    // Update is called once per frame
    void Update()
    {
        ExecMove();
    }

    void ExecMove() 
    { 
        if (shouldMove)
        {
            //var offset = new Vector3(direction.x, 0, direction.y) * Speed * Time.deltaTime;
            var targetRotation = Quaternion.Euler(0, Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg, 0);
            targetRotation *= Quaternion.Euler(0, -90, 0);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 180 * Time.deltaTime);
            transform.position += Speed * Time.deltaTime * transform.forward;
        }
    }

    int shootCounter = 0;
}
