public abstract class State
{
    protected EnemyController controller;

    public State(EnemyController controller)
    {
        this.controller = controller;
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
    }
}