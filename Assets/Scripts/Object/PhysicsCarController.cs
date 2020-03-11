using UnityEngine;
using System.Collections;

/*
 ���ϸ� : ScriptPhysicsCarController

���� ������ �ٷ�� ��ũ��Ʈ�Դϴ�. ���� �̵��� ���� �Լ��� ���� ��ƹ��̳� �������� �浹�� ���� �Լ����� ��� �ֽ��ϴ�. 
�̵��� ������ �κ��� ������ �������ֽñ� �ٶ��ϴ�.
*/

public class PhysicsCarController : KeyHoleCheck
{
    /* public���� ���� */
    public float speedF;                //�չ��� �ӵ� float����
    public float speedB;                //�޹��� �ӵ� float����

    public float torqueF;               //�չ��� ������ float����
    public float torqueB;               //�޹��� ������ float ����

    public bool TractionFront = true;   //�չ��� ���������� bool����
    public bool TractionBack = true;    //�޹��� ���������� bool����

    public float carRotationSpeed;      //���� ȸ�� �ӵ� float����

    public WheelJoint2D frontwheel;     //�չ��� WheelJoint2D ������Ʈ ����
    public WheelJoint2D backwheel;      //�޹��� WheelJoint2D ������Ʈ ����

    public LayerMask whatIsGround;      // �� (�ڵ����� �浹�ϸ� ���ߴ�) ���̾�

    public bool useCar = false;
    public bool carDirection;

    /* private���� ���� */
    JointMotor2D motorFront;            //�չ��� JointMotor2D ����
    JointMotor2D motorBack;             //�޹��� JointMotor2D ����
    Rigidbody2D rigid;                  //Rigidbody2D ������Ʈ ����

    /* Awake() �Լ� ���� */
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    /*��ƿ��Ƽ �Լ� ����*/

    //���� ��ֹ� �浹 �Լ�
    void OnCollisionEnter2D(Collision2D arg_collision)
    {
        //�ӵ��� �ִ� ��뿡�� ��ֹ��� �浹�ϸ� �ı�
        if (arg_collision.gameObject.tag == "Breakable" && speedF > 500)
        {
            if (gameObject.transform.position.x < arg_collision.transform.position.x)
            {
                attack(-1);
            }
            else if (gameObject.transform.position.x > arg_collision.transform.position.x)
            {
                attack(1);
            }

            arg_collision.gameObject.GetComponent<BreakableBlock>().ExplodeThisGameObject();
        }
    }

    //��ֹ��� �浹ó�� �Լ�
    void attack(int dir)
    {
        rigid.AddForce(new Vector2(dir, 0) * 20, ForceMode2D.Impulse);
        speedF = 50;
        torqueF = 100;
    }

    //���� �Լ�
    void Acceleration()
    {
        speedF += 25;
        torqueF += 160;
    }

    /*���� �Լ� ����*/

    //���� �̵� �Լ�
    override public void WhenActive()
    {
        if (!isActive)
            isActive = true;

        // rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (Mathf.Abs(transform.rotation.z) * Mathf.Rad2Deg <= 35 )
        {
            freezeX(false);
            rigid.bodyType = RigidbodyType2D.Dynamic;
            useCar = !useCar;
        }
    }

    //���� �̵� �Լ�
    public void Move(bool direction)
    {
        //���Ͱ� ������� �ƴ϶�� �۵�
        if (!frontwheel.useMotor)
        {
            frontwheel.useMotor = true;
            speedF = 15;
            torqueF = 100;
        }

        if (!direction)
        {
            if (TractionFront)
            {
                motorFront.motorSpeed = speedF * 1;
                motorFront.maxMotorTorque = torqueF;
                frontwheel.motor = motorFront;
            }

            if (TractionBack)
            {
                motorBack.motorSpeed = speedF * 1;
                motorBack.maxMotorTorque = torqueF;
                backwheel.motor = motorBack;

            }
        }
        else
        {
            if (TractionFront)
            {
                motorFront.motorSpeed = speedF * -1;
                motorFront.maxMotorTorque = torqueF;
                frontwheel.motor = motorFront;
            }

            if (TractionBack)
            {
                motorBack.motorSpeed = speedF * -1;
                motorBack.maxMotorTorque = torqueF;
                backwheel.motor = motorBack;

            }
        }

        //�ְ� �ӵ�����
        if (speedF < 1000 && torqueF < 6666)
        {
            if (carDirection && direction || !carDirection && !direction)
                Acceleration();
            else
            {
                speedF = 1500;
                torqueF = 1998;
            }
        }
    }

    //���� ������Ű�� �Լ�
    public void stop()
    {
        frontwheel.useMotor = false;
        speedF = 0;
        torqueF = 0;
    }

    public void freezeX(bool freeze)
    {
        // freezeX ���� �� ���� �������� ���°�?
        if (freeze)
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
        else if (!freeze)
            rigid.constraints = RigidbodyConstraints2D.None;
    }
}