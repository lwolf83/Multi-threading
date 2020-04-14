using System;
using System.Collections.Generic;
using System.Threading;

class Course
{
    // Create a new Mutex. The creating thread does not own the mutex.
    private static Mutex mut = new Mutex();
    private const int numIterations = 1;
    private const int numThreads = 3;
    public static int Winner { get; set; } = 0;

    static void Main()
    {
        Course course = new Course();
        course.StartThreads();
    }

    private void StartThreads()
    {
        List<Thread> peloton = new List<Thread>();

        for (int i = 0; i < numThreads; i++)
        {
            Thread nouvelCourse = new Thread(new ThreadStart(CoursChevalCours));
            nouvelCourse.Name = String.Format("Thread{0}", i + 1);
            peloton.Add(nouvelCourse);
            nouvelCourse.Start();
        }

        peloton.ForEach(t => t.Join());
        Console.WriteLine("And the Winner is : {0}", Winner);

    }

    private static void CoursChevalCours()
    {
        Random random = new Random();
        int timeTOSleep = random.Next(10000);
        Console.WriteLine("Horse {0} start.", Thread.CurrentThread.ManagedThreadId);
        Thread.Sleep(timeTOSleep);
        mut.WaitOne();
        if(Winner == 0)
        {
            Winner = Thread.CurrentThread.ManagedThreadId;
        }
        mut.ReleaseMutex();
        Console.WriteLine("Horse {0} chrono = {1}", Thread.CurrentThread.ManagedThreadId, timeTOSleep);
    }


    ~Course()
    {
        mut.Dispose();
    }
}
