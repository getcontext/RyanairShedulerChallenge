using RyanairScheduler.Activity;
using RyanairScheduler.Worker;

namespace RyanairScheduler.Factory;

public sealed class TycoonFactory : AbstractFactory
{
    public void sayHello()
    {
        Console.WriteLine("Hello everybody !");
    }

    public override void Process()
    {
        while (true)
        {
            string input = Console.ReadLine();

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            
            if (input.Equals("hello", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Hello, how are you ?");
            } 
            else if (input.Contains("addActivity", StringComparison.OrdinalIgnoreCase))
            {
                string[] param = input.Split(' ');
                
                if (param.Length < 4)
                {
                    Console.WriteLine("activity must have at least type,start,end params sepcified");
                    continue;
                }

                string type = param[1];
                string start = param[2];
                string stop = param[3];

                if (type.Length <= 0)
                {
                    Console.WriteLine("activity type must be specified");
                }

                if (start.Length <= 0)
                {
                    Console.WriteLine("activity start time must be specified");
                }

                if (stop.Length <= 0)
                {
                    Console.WriteLine("activity end time must be specified");
                }

                if (param.Length < 5)
                {
                    Console.WriteLine("activity must have at least 1 worker specified");
                }

                FactoryActivity activity = new FactoryActivity();
                activity.setActivityType(type == "component"
                    ? ActivityInterface.ActivityType.COMPONENT
                    : ActivityInterface.ActivityType.MACHINE);
                activity.setStart(DateTime.Parse(start));
                activity.setStop(DateTime.Parse(stop));

                for (int i = 4; i < param.Length; i++)
                {
                    AndroidWorker worker = new AndroidWorker();
                    worker.setName(param[i]);
                    activity.AddWorker(worker);
                }
                
                AddActivity(activity);
            }
            else if (input.Contains("deleteActivity", StringComparison.OrdinalIgnoreCase))
            {
                string[] param = input.Split(' ');

                if (param.Length < 2)
                {
                    Console.WriteLine("deleting activity must have activityID param");
                    continue;
                }

                int index = -1;
                int.TryParse(param[1], out index);

                if (index < 0)
                {
                    Console.WriteLine("can't parse index= " +index+" ");
                }
                else if (DeleteActivity(index))
                {
                    Console.WriteLine("activity " +index+" deleted");
                }
            } 
            else if (input.Contains("changeActivityDate", StringComparison.OrdinalIgnoreCase))
            {
                string[] param = input.Split(' ');

                if (param.Length < 3)
                {
                    Console.WriteLine("changing activity must have at least startDate param - or both start&end dates");
                    continue;
                }

                int activityId = int.Parse(param[1]);
                
                DateTime start = DateTime.Parse(param[2]);
                DateTime stop = DateTime.Parse(param[3]);
                
                GetActivity(activityId).setStart(start);
                GetActivity(activityId).setStop(stop);
                GetActivity(activityId).restart();
                
                startActivity(GetActivity(activityId));
                
                Console.WriteLine("activity start/stop dates changed. activity restarted. ");
                
            }
        }
        
    }
}