namespace RyanairScheduler;

public interface ActivityInterface
{
    public enum ActivityType
    {
        COMPONENT,
        MACHINE
    }
    
    public void AddWorker(AbstractWorker worker);
    public AbstractWorker? GetWorker(string name);
    public void DeleteWorker(string name);
    
    public HashSet<AbstractWorker> GetWorkers();

    public ActivityInterface.ActivityType getActivityType();

    public void setActivityType(ActivityInterface.ActivityType val);

    public DateTime getStart();
    public void setStart(DateTime val);


    public DateTime getStop();

    public void setStop(DateTime val);

    public void setId(int id);

    public int getId();

    public bool restart();
}