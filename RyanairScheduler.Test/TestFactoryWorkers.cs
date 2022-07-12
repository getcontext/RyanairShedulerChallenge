using RyanairScheduler.Activity;
using RyanairScheduler.Worker;
using Xunit;

namespace RyanairScheduler.UnitTest;

public class TestFactoryWorkers : TestBase
{
    [Fact]
        public void TestCreateEmptyWorker()
        {
            RyanairScheduler.Worker.AndroidWorker worker = new RyanairScheduler.Worker.AndroidWorker();

            Assert.True(worker is RyanairScheduler.AbstractWorker, "Worker object should be created");
        }

        [Fact]
        public void TestCreateConcreteWorker()
        {
            AndroidWorker worker = GetAndroidWorker();
            Assert.True(worker.getName() == WORKER_A); //extract constant A for 1st worker
            Assert.True(worker is RyanairScheduler.AbstractWorker, "Worker object should be created");
        }

        [Fact]
        public void TestCreateEmptyActivity()
        {
            RyanairScheduler.Activity.FactoryActivity activity = new RyanairScheduler.Activity.FactoryActivity();

            Assert.True(activity is RyanairScheduler.ActivityInterface, "Sheduler object should be created");
        }


        [Fact]
        public void TestCreateConcreteActivity()
        {
            RyanairScheduler.Activity.FactoryActivity activity = GetComponentActivityWithWorker();
            Assert.True(activity.GetWorkers().Count == 1);
            Assert.True(activity is RyanairScheduler.ActivityInterface, "Sheduler object should be created");
        }

        [Fact]
        public void TestCreateEmptyFactory()
        {
            RyanairScheduler.Factory.TycoonFactory factory = new RyanairScheduler.Factory.TycoonFactory();

            Assert.True(factory is RyanairScheduler.AbstractFactory, "Factory object should be created");
        }

        [Fact]
        public void TestCreateConcreteFactoryWithComponent()
        {
            RyanairScheduler.Factory.TycoonFactory factory = new RyanairScheduler.Factory.TycoonFactory();
            factory.AddActivity(GetComponentActivityWithWorker()); //add sheduler to factory, with worker already instantiated 
            //only 1 sheduler per factory is allowed
            factory.Process();
            // Console.WriteLine("factory should not add 2nd");

            Assert.True(factory is RyanairScheduler.AbstractFactory, "Factory object should be created");
        }

        // [Fact]
        // public void TestCreateConcreteFactoryWithComponentAndWorkersTooMuch()
        // {
        //     RyanairScheduler.Factory.TycoonFactory factory = new RyanairScheduler.Factory.TycoonFactory();
        //     factory.AddActivity(GetComponentActivityWithWorker()); //add sheduler to factory, with worker already instantiated 
        //     //only 1 sheduler per factory is allowed
        //     factory.AddWorker(GetAndroidWorker2(), 0); //add single worker directly to factory without sheduler
        //     //there can not be 2 same workers...
        //     factory.Process();
        //     // Console.WriteLine("factory should not add 2nd");
        //
        //     Assert.True(factory is RyanairScheduler.AbstractFactory, "Factory object should be created");
        // }        
        [Fact]
        public void TestCreateConcreteFactoryWithComponentAndManyWorkers()
        {
            RyanairScheduler.Factory.TycoonFactory factory = new RyanairScheduler.Factory.TycoonFactory();
            factory.AddActivity(GetComponentActivityWithWorker()); //add activity to factory, with worker already instantiated 
            //only 1 activity per factory is allowed 
            factory.AddWorker(GetAndroidWorker2(), 0); //add single worker directly to factory without activity
            //there can not be 2 same workers...
            factory.Process();
            // Console.WriteLine("factory should not add 2nd");

            Assert.True(factory is RyanairScheduler.AbstractFactory, "Factory object should be created");
        } 
        
        [Fact]
        public void TestCreateConcreteFactoryWithMachine()
        {
            RyanairScheduler.Factory.TycoonFactory factory = new RyanairScheduler.Factory.TycoonFactory();
            factory.AddActivity(GetMachineActivityWithWorker()); //add activity to factory, with worker already instantiated 
            //should be added
            factory.AddWorker(GetAndroidWorker2(), 0); //add single worker directly to factory without activity
            //there can not be 2 same workers...
            factory.Process();
            // Console.WriteLine("factory should not add 2nd");

            Assert.True(factory is RyanairScheduler.AbstractFactory, "Factory object should be created");
        }       
        
        [Fact]
        public void TestCreateConcreteFactoryWithMachineSameWorkerName()
        {
            RyanairScheduler.Factory.TycoonFactory factory = new RyanairScheduler.Factory.TycoonFactory();
            factory.AddActivity(GetMachineActivityWithWorker()); //add activity to factory, with worker already instantiated 
            //should be added
            factory.AddWorker(GetAndroidWorker2(), 0); //add single worker directly to factory without activity
            //same worker name not allowed
            factory.AddWorker(GetAndroidWorker(), 0); //add single worker directly to factory without activity
            //there can not be 2 same workers...
            factory.Process();
            // Console.WriteLine("factory should not add 2nd");

            Assert.True(factory is RyanairScheduler.AbstractFactory, "Factory object should be created");
        }         

        [Fact]
        public void TestCreateConcreteFactoryWithActivityOverlapping()
        {
            RyanairScheduler.Factory.TycoonFactory factory = new RyanairScheduler.Factory.TycoonFactory();
            factory.AddActivity(GetMachineActivityWithWorker()); //add activity to factory, with worker already instantiated 
            factory.AddActivity(GetMachineActivityWithWorker()); //add activity to factory, with worker already instantiated 
            factory.Process();
            // Console.WriteLine("factory should not add 2nd");

            Assert.True(factory is RyanairScheduler.AbstractFactory, "Factory object should be created");
        }   
        [Fact]
        public void TestAddWorkerToActivity()
        {
            AndroidWorker worker = GetAndroidWorker();
            RyanairScheduler.Activity.FactoryActivity activity = new FactoryActivity();
            activity.AddWorker(worker);

            Assert.True(activity.GetWorkers().Count == 1, "Worker object should be created and added to activity");
        }
}
