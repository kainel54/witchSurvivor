using UnityEngine;

public class Reposition : MonoBehaviour
{
    // MAP_SIZE : 맵 한칸당 길이/너비
    private const float MAP_SIZE = 40f;
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 충돌체가 Area(유저에게 눈에 보이는 범위)가 아닌 경우 (플레이어에 붙어있음)
        if (collision.tag != "Area")
        {            
            // 체크하지 않는다
            return;
        }

        // 게임매니저가 없어질 때는 그냥 리턴
        if(GameManager.Instance == null){
            return;
        }

        if(GameManager.Instance.player == null)
        {
            return;
        }

        // playerPosition : 플레이어의 위치, GameManager의 player를 가져와서 받아옴
        // myPosition : Reposition이 붙은 게임오브젝트의 위치
        Vector3 playerPosition = GameManager.Instance.player.transform.position;
        Vector3 myPosition = transform.position;

        // diffX : 플레이어와 게임오브젝트 x 거리
        // diffY : 플레이어와 게임오브젝트 y 거리
        float diffX = Mathf.Abs(playerPosition.x - myPosition.x);
        float diffY = Mathf.Abs(playerPosition.y - myPosition.y);

        // playerDirection : 플레이어에게 받아지는 움직임 방향
        // dirX : 플레이어의 X방향
        // dirY : 플레이어의 Y방향
        Vector3 playerDirection = GameManager.Instance.player.moveDirection;
        // 1 : 오른쪽, -1 : 왼쪽
        float dirX = playerDirection.x < 0 ? -1 : 1;
        // 1 : 위, -1 : 아래
        float dirY = playerDirection.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            // Reposition 오브젝트가 바닥일 때
            case "Ground":
                // diffX가 클 경우 X방향으로 이동
                // 80 : 맵 크기(40)만큼 두 번 이동
                if (diffX > diffY)
                {                    
                    transform.Translate(Vector3.right * dirX * MAP_SIZE * 2f);
                }
                // 작을 경우 Y방향으로 이동
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * MAP_SIZE * 2f);
                }
                break;
            case "Enemy":
                // Enemy는 playerDirection으로 MAP_SIZE만큼, 그리고 추가로 -20에서 20 사이의 랜덤 X, Y 좌표로 이동
                transform.Translate(playerDirection * MAP_SIZE + new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0f));
                break;
        }

    }
}
