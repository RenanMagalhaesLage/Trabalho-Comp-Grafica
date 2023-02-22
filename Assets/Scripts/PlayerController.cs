using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   private CharacterController controller;
   private Vector3 direction;
   public float forwardSpeed; //velocidade do player
   public float maxSpeed;

    private int desiredLane = 1; //0->Esquerda  1->Meio   2->Direita
    public float laneDistance = 4; //distancia entre duas linhas (2.5f)

    public float jumpForce;
    public float Gravity = -30;

    public Animator animator;
    private bool isSliding = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerManager.isGamerStarted)
        {
            return;
        }

        /*Vai aumentar a velocidade somente se a velocidade atual for menor que a máxima permitida*/
        if(forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime;

        }

        animator.SetBool("isGameStarted",true);
        animator.SetBool("isGrounded",controller.isGrounded);

        direction.z = forwardSpeed;
        direction.y += Gravity * Time.deltaTime;
        /*Para movimentação no teclado*/
        if(controller.isGrounded)
        {
            //direction.y = 0;
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
        }
        /*Para movimentação no Android*/
         if(controller.isGrounded)
        {
            //direction.y = 0;
            if(SwipeManager.swipeUp)
            {
                Jump();
            }
        }
        /* DEFEITO
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }*/

        /*Para movimentação no Android*/
        if(SwipeManager.swipeDown && !isSliding)
        {
            StartCoroutine(Slide());
        }
        /*Para movimentação no teclado*/
        
        if(Input.GetKeyDown(KeyCode.DownArrow) && !isSliding)
        {
            StartCoroutine(Slide());
        }

        //Colocar as entradas em qual faixa devemos estar

        /*Para movimentação no Android*/
        if(SwipeManager.swipeRight)
        {
            desiredLane++;
            if(desiredLane == 3)
            {
                desiredLane = 2;
            }
        }
        /*Para movimentação no Android*/
        if(SwipeManager.swipeLeft)
        {
            desiredLane--;
            if(desiredLane == -1)
            {
                desiredLane = 0;
            }
        }
        /*Para movimentação no teclado*/
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if(desiredLane == 3)
            {
                desiredLane = 2;
            }
        }
        /*Para movimentação no teclado*/
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if(desiredLane == -1)
            {
                desiredLane = 0;
            }
        }

        //Calcular em qual posição devemos estar no futuro

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if(desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }else if(desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        //transform.position = Vector3.Lerp(transform.position, targetPosition,70 * Time.fixedDeltaTime);
        //Adicionando colisão ao cone
        //controller.center = controller.center;
        if(transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if(moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);

        
    }
    private void FixedUpdate()
    {
        if(!PlayerManager.isGamerStarted)
        {
            return;
        }
        controller.Move(direction * Time.fixedDeltaTime);

    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;

        yield return new WaitForSeconds(0.9f);
        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        animator.SetBool("isSliding", false);
        isSliding = false;
    }
}