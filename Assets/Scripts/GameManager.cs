using UnityEngine;
using UnityEngine.SceneManagement;

/*
���ϸ�: gameManager
�⺻���� �ʰ� UI�� ������ �ٷ�� ��ũ��Ʈ�Դϴ�. �� �̵��� ���� �Լ��� ĳ������ �⺻���� ������ ���� �����մϴ�.
���� �⺻���� UI���� �����մϴ�.
*/

public class GameManager : MonoBehaviour
{
    /* pubic ���� ���� */
    public int stageIndex;                      // ���� �ִ� �������� ��

    public GameObject player;
    PlayerMovement pm;   // player�� �Լ��� �������� ����


    void Start()
    {
        pm = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (player.transform.position.y < -10)
        {
            GameOver();
        }
    }

    //�÷��̾ �ٽ� �������� �������� �Լ�
    void PlayerReposition()
    {
        pm.transform.position = Vector3.zero;
        pm.velocity = Vector3.zero;
    }

    /* ���� �Լ� */
    // ���� ���������� �Ѿ�� ���� �Լ�
    // �ӽ�, ������ ��!
    public void NextStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GameOver() // ���� ����, �ӽ÷� ���� ���̹Ƿ� ���߿� ������ ��
    {
        player.SetActive(false);
        Invoke("PlayerRestart", 1f);
    }

    public void PlayerRestart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}