public class GameManager : MonoSingleton<GameManager>
{
    public PoolManager poolManager { get; private set; }
    public Player player { get; private set; }

    private void Awake()
    {
        poolManager = FindObjectOfType<PoolManager>();
        player = FindObjectOfType<Player>();
    }
}
