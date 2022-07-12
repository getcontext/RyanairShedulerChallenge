using RyanairScheduler.Activity;
using RyanairScheduler.Worker;

namespace RyanairScheduler;

public abstract class AbstractFactory
{
    public const int REST_DURATION_COMPONENT = 2;
    public const int REST_DURATION_MACHINE = 4;
    
    private List<Task[]> restRoom = new List<Task[]>();
    private List<Task> activityTasks = new List<Task>();
    // private Task[] restRoom;
    
    private class RestRoom
    {
        private AbstractWorker worker;
        private int restTimeDuration;
        private int restTimeEnd;
        private bool finished = false;

        public void setWorker(AbstractWorker abstractWorker)
        {
            worker = abstractWorker;
        }

        public void setRestTimeDuration(int hours)
        {
            restTimeDuration = hours;
        }

        public async Task<AbstractWorker> start()
        {
            Console.WriteLine("worker "+worker.getName()+" is started resting");
            await Task.Delay(3600 * restTimeDuration);
            Console.WriteLine("worker "+worker.getName()+" has finished resting");
            finished = true;
            return worker;
        }

        public bool isFinished()
        {
            return finished;
        }
    }

    private class ActivityHelper
    {
        public static bool isStartOverlapping(ActivityInterface activityInterface, DateTime dateTime)
        {
            bool rs = false;

            DateTime start = activityInterface.getStart();
            DateTime stop = activityInterface.getStop();
            int compareStart = compareDateTime(dateTime, start);
            int compareStop = compareDateTime(dateTime, stop);

            if (compareStart == 0 || compareStop == 0)
            {
                rs = true;
            }
            else if (compareStart > 0 && compareStop < 0)
            {
                rs = true;
            }

            return rs;
        }

        public static bool isStopOverlapping(ActivityInterface activityInterface, DateTime dateTime)
        {
            bool rs = false;

            DateTime start = activityInterface.getStart();
            DateTime stop = activityInterface.getStop();
            int compareStart = compareDateTime(dateTime, start);
            int compareStop = compareDateTime(dateTime, stop);

            if (compareStart == 0 || compareStop == 0)
            {
                rs = true;
            }
            else if (compareStart > 0 && compareStop < 0)
            {
                rs = true;
            }

            return rs;
        }

        static int compareDateTime(DateTime date1, DateTime date2)
        {
            int result = DateTime.Compare(date1, date2);
            string relationship;

            if (result < 0)
                relationship = "is earlier than";
            else if (result == 0)
                relationship = "is the same time as";
            else
                relationship = "is later than";

            return result;
        }
    }

    private HashSet<ActivityInterface> activities = new HashSet<ActivityInterface>();
    private List<Task> activitiesTask = new List<Task>();

    public void AddActivity(ActivityInterface activity)
    {
        bool valid = true;
        if (!isActivityStartValid(activity.getStart()))
        {
            Console.WriteLine($"activity start date {activity.getStart()} invalid/overlapping");
            valid = false;
        }
        
        if (!isActivityStopValid(activity.getStop()))
        {
            Console.WriteLine($"activity stop date {activity.getStop()} invalid/overlapping");
            valid = false;
        }

        foreach (ActivityInterface activityInterface in activities)
        {
            HashSet<AbstractWorker> localWorkers =  activityInterface.GetWorkers();
            foreach (AbstractWorker worker in activity.GetWorkers())
            {
                foreach (AbstractWorker localWorker in localWorkers)
                {
                    if (localWorker.getName() == worker.getName()) //can't be >2 same workers
                    {
                        valid = false;
                        Console.WriteLine("worker " + worker.getName() + " already exists in activity");
                    }
                }
            }
        }

        if (valid)
        {
            this.activities.Add(activity);  
            activity.setId(activities.Count-1);
            Console.WriteLine("activity added, activity ID="+ (activities.Count-1));
        }
        else
        {
            Console.WriteLine("activity has invalid param");
        }
    }

    public ActivityInterface GetActivity(int index)
    {
        if (index > activities.Count)
        {
            Console.WriteLine("Activity index out of bounds");
            return null;
        }
        ActivityInterface activity = activities.ElementAt(index);
        return activity;
    }

    public bool DeleteActivity(int index)
    {
        ActivityInterface activity = GetActivity(index);
        if (activity == null)
        {
            return false;
        }
        return activities.Remove(activity);
    }

    public void AddWorker(AbstractWorker worker, int activityIndex)
    {
        AddWorker(worker, activities.ElementAt(activityIndex));
    }

    public void AddWorker(AbstractWorker worker, ActivityInterface activity)
    {
        if (activity.getActivityType() == ActivityInterface.ActivityType.COMPONENT
            && activity.GetWorkers().Count == 1)
        {
            Console.WriteLine("activity of type: component , has already 1 worker");
            return; //can not add more than 1 worker for component activity
        }

        foreach (AbstractWorker worker2 in activity.GetWorkers())
        {
            if (worker.getName() == worker2.getName()) //can't be >2 same workers
            {
                Console.WriteLine("worker " + worker.getName() + " already exists in activity");
                return;
            }
        }

        activity.AddWorker(worker);
        Console.WriteLine("worker " + worker.getName() + " added to activity");

    }

    public void SetActivityStart(int activityIndex, DateTime dateTime)
    {
        SetActivityStart(activities.ElementAt(activityIndex), dateTime);
    }

    public void SetActivityStart(ActivityInterface activity, DateTime dateTime)
    {
        activity.setStart(dateTime);
    }

    public void SetActivityStop(int activityIndex, DateTime dateTime)
    {
        SetActivityStop(activities.ElementAt(activityIndex), dateTime);
    }

    public void SetActivityStop(ActivityInterface activity, DateTime dateTime)
    {
        activity.setStop(dateTime);
    }

    protected bool isActivityStartValid(DateTime dateTime)
    {
        bool rs = true;
        foreach (ActivityInterface activity in activities)
        {
            if (ActivityHelper.isStartOverlapping(activity, dateTime)) //is overlapping in one of activities
            {
                rs = false;
            }
        }

        return rs;
    }

    protected bool isActivityStopValid(DateTime dateTime)
    {
        bool rs = true;
        foreach (ActivityInterface activity in activities)
        {
            if (ActivityHelper.isStopOverlapping(activity, dateTime)) //is overlapping in one of activities
            {
                rs = false;
            }
        }

        return rs;
    }

    protected void onActivityEnd(ActivityInterface activity)
    {
        HashSet<AbstractWorker> workers =  activity.GetWorkers();
        int i = 0;
        Task[] taskArray = new Task[workers.Count];
        
        foreach (AbstractWorker worker in workers)
        {
            RestRoom r = new RestRoom();
            r.setWorker(worker);
            r.setRestTimeDuration(activity.getActivityType() == ActivityInterface.ActivityType.COMPONENT ? REST_DURATION_COMPONENT : REST_DURATION_MACHINE);

            taskArray[i] = Task.Factory.StartNew( (Object obj ) => {
                RestRoom data = obj as RestRoom;
                data.start();
            },
                r 
            );
            i++;
            // restRoom.Add(t);
            // await 
            workers.Remove(worker);
        }
        
        // foreach (var task in taskArray) {
        //     var data = task.AsyncState as RestRoom;
        //     // if (data != null)
        //     //     Console.WriteLine("Task #{0} created at {1}, ran on thread #{2}.",
        //     //         data.Name, data.CreationTime, data.ThreadNum);
        // }
        restRoom.Add(taskArray);
        Task.WaitAll(taskArray);
        activities.Remove(activity);
    }

    protected void startActivity(ActivityInterface activity)
    {

        bool exists = false;
        Task task = null;
        
        foreach (Task activityTask in activityTasks)
        {
            var data = activityTask.AsyncState as FactoryActivity;
            if (data.getId() == activity.getId())
            {
                
                exists = true;
                data.startActivity();
                task = activityTask;
            }
        }


        if (!exists)
        {
            task  = Task.Factory.StartNew((Object obj) =>
                {
                    FactoryActivity data = obj as FactoryActivity;
                    data.startActivity();
                },
                activity
            );            
            activityTasks.Add(task); //@todo check for an old activity
        }

        
        Task.WaitAll(task);
    }

    public abstract void Process();
}