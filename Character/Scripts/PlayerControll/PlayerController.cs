using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject pointShoot;
    [SerializeField] private float speedBullet;
    public GameObject bulletPrefab;
    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    private float gravityStart;
    private Movement movement;
    private CharacterController characterController;
    private Coroutine courutine;
    private Shoot shoot;
    private Animator anim;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        movement = new Movement(characterController,gameObject);
        shoot = new Shoot(pointShoot, bulletPrefab, speedBullet,gameObject);
        gravityStart = gravity;
        anim = GetComponent<Animator>();
       
    }
    private void FixedUpdate()
    {
        movement.KeyPressed(speed, gravity,anim);
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("Fired", true);
            shoot.ShootPlayer();
        }
        else
        {
            anim.SetBool("Fired", false);
        }
        
    }
    public void SwitcherCoroutine(bool work)
    {
        if (work)
        {
            StopCoroutine(courutine);
            gravity = gravityStart;
        }
        else
            courutine = StartCoroutine(ScaleGravity());
    }
    public  IEnumerator ScaleGravity()
    {   
        yield return new WaitForSeconds(1.0f);
        gravity += 10f;
    }
}
public class Movement : IMovement
{
    private CharacterController characterController;
    private GameObject currentObject;
    private float x,z;

    public Movement(CharacterController characterController, GameObject currentObject)
    {
        this.characterController = characterController;
        this.currentObject = currentObject;
        
    }
    public void KeyPressed(float speed,float gravity,Animator anim)
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        Move(x,z,speed,gravity);
        anim.SetFloat("X", x);
        anim.SetFloat("Z",z);
    }
    public void Move(float x, float z,float speed,float gravity)
    {
            float rotationAngle = currentObject.transform.eulerAngles.y;
            Vector3 forwardDirection = Quaternion.Euler(0, rotationAngle, 0) * new Vector3(x, 0, z);
            characterController.Move(forwardDirection * speed);
            Approaching(gravity);
    }
    public void Approaching(float gravity)
    {
     
        if (characterController.isGrounded)
        {
            characterController.Move(Vector3.down * characterController.height * 0.5f);
            currentObject.GetComponent<PlayerController>().SwitcherCoroutine(true);
        }
        else
        {
            ApplyGravity(gravity);
            currentObject.GetComponent<PlayerController>().SwitcherCoroutine(false);
        }
       
    }
    public void ApplyGravity(float gravity)
    {
        Vector3 gravityVector = Vector3.down * gravity * Time.deltaTime;
        characterController.Move(gravityVector);
    }
}

public class Shoot : IShoot
{
    private GameObject rotateObject;
    private GameObject pointShoot;
    private GameObject bullet;
    private float speed;
    public Shoot(GameObject pointShoot, GameObject bullet, float speed, GameObject rotateObject)
    {
        this.pointShoot = pointShoot;
        this.bullet = bullet;
        this.speed = speed;
        this.rotateObject = rotateObject;

    }
    public void ShootPlayer()
    {
            
            CreateBullet();
        
    }
    public void CreateBullet()
    {
        GameObject bullettemp = GameObject.Instantiate(bullet);
        bullettemp.GetComponent<Bullet>().speed = speed;
        bullettemp.transform.position = pointShoot.transform.position;
        bullettemp.GetComponent<Bullet>().setLookDirection(rotateObject.transform.eulerAngles);
    }
}