using RyanairScheduler.Worker;

namespace RyanairScheduler.UnitTest;

public abstract class TestBase
{
    protected const string WORKER_A = "A";
    protected const string WORKER_B = "B";

    protected AndroidWorker GetAndroidWorker()
    {
        AndroidWorker worker = new RyanairScheduler.Worker.AndroidWorker();

        worker.setName(WORKER_A);

        // worker.setActivity(RyanairScheduler.ActivityInterface.ActivityType.COMPONENT);
        //
        // DateTime timeStart = new DateTime(2008, 3, 1, 7, 0, 0);
        // worker.setStart(timeStart);
        //
        // DateTime timeStop = new DateTime(2022, 12, 1, 7, 0, 0);
        // worker.setStop(timeStop);
        return worker;
    }
    
    protected AndroidWorker GetAndroidWorker2()
    {
        AndroidWorker worker = new RyanairScheduler.Worker.AndroidWorker();

        worker.setName(WORKER_B);
        
        return worker;
    }

    protected RyanairScheduler.Activity.FactoryActivity GetComponentActivityWithWorker()
    {
        RyanairScheduler.Activity.FactoryActivity activity = new RyanairScheduler.Activity.FactoryActivity();
        activity.setActivityType(RyanairScheduler.ActivityInterface.ActivityType.COMPONENT);

        DateTime timeStart = new DateTime(2018, 3, 1, 7, 0, 0);
        activity.setStart(timeStart);

        DateTime timeStop = new DateTime(2022, 12, 1, 7, 0, 0);
        activity.setStop(timeStop);
        activity.AddWorker(GetAndroidWorker());
        return activity;
    }
    
    protected RyanairScheduler.Activity.FactoryActivity GetMachineActivityWithWorker()
    {
        RyanairScheduler.Activity.FactoryActivity activity = new RyanairScheduler.Activity.FactoryActivity();
        activity.setActivityType(RyanairScheduler.ActivityInterface.ActivityType.MACHINE);

        DateTime timeStart = new DateTime(2018, 3, 1, 7, 0, 0);
        activity.setStart(timeStart);

        DateTime timeStop = new DateTime(2022, 12, 1, 7, 0, 0);
        activity.setStop(timeStop);
        activity.AddWorker(GetAndroidWorker());
        return activity;
    }
}