namespace RyanairScheduler.Activity;

public class FactoryActivity : ActivityInterface
{
    protected DateTime start;
    protected DateTime stop;
    protected ActivityInterface.ActivityType activityType;
    protected int id;
    
    private HashSet<AbstractWorker> workers = new HashSet<AbstractWorker>();
    public void AddWorker(AbstractWorker worker)
    {
        if(getActivityType() == ActivityInterface.ActivityType.COMPONENT) {}
        foreach (AbstractWorker worker2 in workers)
        {
            if (worker.getName() == worker2.getName()) //can't be >2 same workers
            {
                return;
            }
        }
        workers.Add(worker);
    }

    public AbstractWorker? GetWorker(string name)
    {
        foreach (AbstractWorker worker in workers)
        {
            if (worker.getName() == name)
            {
                return worker;
            }
        }

        return null;
    }

    public void DeleteWorker(string name)
    {
        foreach (AbstractWorker worker in workers)
        {
            if (worker.getName() == name)
            {
                workers.Remove(worker);
            }
        }
    }

    public HashSet<AbstractWorker> GetWorkers()
    {
        return workers;
    }
    
    public ActivityInterface.ActivityType ActivityType
    {
        get => activityType;
        set => activityType = value;
    }

    public DateTime Start
    {
        get => start;
        set => start = value;
    }

    public DateTime Stop
    {
        get => stop;
        set => stop = value;
    }

    public ActivityInterface.ActivityType getActivityType()
    {
        return activityType;
    }
    public void setActivityType(ActivityInterface.ActivityType val)
    { 
        activityType = val;
    }
    
    
    public DateTime getStart()
    {
        return start;
    }
    public void setStart(DateTime val)
    { 
        start = val;
    }

    public DateTime getStop()
    {
        return stop;
    }
    public void setStop(DateTime val)
    { 
        stop = val;
    }
    
    public async Task<ActivityInterface> startActivity()
    {
        Console.WriteLine("activity " +id+" started");
        TimeSpan ts = getStop() - getStart();
        await Task.Delay(ts.Milliseconds);
        Console.WriteLine("activity "+id+" has finished");
        return this;
    }

    public void setId(int id)
    {
        this.id = id;
    }

    public int getId()
    {
        return id;
    }

    public bool restart() //restart Activity when start or stop date changed
    {
        return true;
    }
    
    
}