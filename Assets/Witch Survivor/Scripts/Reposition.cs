using UnityEngine;

public class Reposition : MonoBehaviour
{
    // MAP_SIZE : �� ��ĭ�� ����/�ʺ�
    private const float MAP_SIZE = 40f;
    private void OnTriggerExit2D(Collider2D collision)
    {
        // �浹ü�� Area(�������� ���� ���̴� ����)�� �ƴ� ��� (�÷��̾ �پ�����)
        if (collision.tag != "Area")
        {            
            // üũ���� �ʴ´�
            return;
        }

        // ���ӸŴ����� ������ ���� �׳� ����
        if(GameManager.Instance == null){
            return;
        }

        if(GameManager.Instance.player == null)
        {
            return;
        }

        // playerPosition : �÷��̾��� ��ġ, GameManager�� player�� �����ͼ� �޾ƿ�
        // myPosition : Reposition�� ���� ���ӿ�����Ʈ�� ��ġ
        Vector3 playerPosition = GameManager.Instance.player.transform.position;
        Vector3 myPosition = transform.position;

        // diffX : �÷��̾�� ���ӿ�����Ʈ x �Ÿ�
        // diffY : �÷��̾�� ���ӿ�����Ʈ y �Ÿ�
        float diffX = Mathf.Abs(playerPosition.x - myPosition.x);
        float diffY = Mathf.Abs(playerPosition.y - myPosition.y);

        // playerDirection : �÷��̾�� �޾����� ������ ����
        // dirX : �÷��̾��� X����
        // dirY : �÷��̾��� Y����
        Vector3 playerDirection = GameManager.Instance.player.moveDirection;
        // 1 : ������, -1 : ����
        float dirX = playerDirection.x < 0 ? -1 : 1;
        // 1 : ��, -1 : �Ʒ�
        float dirY = playerDirection.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            // Reposition ������Ʈ�� �ٴ��� ��
            case "Ground":
                // diffX�� Ŭ ��� X�������� �̵�
                // 80 : �� ũ��(40)��ŭ �� �� �̵�
                if (diffX > diffY)
                {                    
                    transform.Translate(Vector3.right * dirX * MAP_SIZE * 2f);
                }
                // ���� ��� Y�������� �̵�
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * MAP_SIZE * 2f);
                }
                break;
            case "Enemy":
                // Enemy�� playerDirection���� MAP_SIZE��ŭ, �׸��� �߰��� -20���� 20 ������ ���� X, Y ��ǥ�� �̵�
                transform.Translate(playerDirection * MAP_SIZE + new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0f));
                break;
        }

    }
}
