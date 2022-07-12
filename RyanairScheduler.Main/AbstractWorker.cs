namespace RyanairScheduler;

public abstract class AbstractWorker
{
    protected string name;

    public string Name
    {
        get => name;
        set => name = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public string getName()
    {
        return name;
    }
    public void setName(string val)
    { 
        name = val;
    }
    
}