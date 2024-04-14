
using System.Diagnostics;
using System.Text;

static bool TestEmpty(okPriorityQueue fpq)
{
    StringBuilder sb = new StringBuilder();

    int prev = fpq.Dequeue();
    sb.Append(prev);
    while(fpq.Count > 0)
    {
        int curr = fpq.Dequeue();

        //fpq.Debug();

        sb.Append($" -> {curr}");
        if (curr <= prev)
        {
            Console.WriteLine(sb.ToString());
            fpq.Clear();
            return false;
        }
        prev = curr;
    }

    Console.WriteLine(sb.ToString());
    return true;
}

static void Test(okPriorityQueue fpq)
{
    if (fpq.IsValid())
    {
        Console.WriteLine($"valid");
    }
    else
    {
        Console.WriteLine($"invalid");
    }

    if (TestEmpty(fpq))
    {
        Console.WriteLine($"valid");
    }
    else
    {
        Console.WriteLine($"invalid");
    }
}

var fpq = new okPriorityQueue(15);

fpq.Clear();

// add out of order
for (int i = 10; i > 0; --i)
    fpq.Enqueue(i, (float)i);

Test(fpq);

#if true

// add in order
for (int i = 1; i <= 10; ++i)
    fpq.Enqueue(i, (float)i);

Test(fpq);

// add every other
for (int i = 1; i <= 5; ++i)
{
    fpq.Enqueue(i, (float)i);
    fpq.Enqueue(i+5, (float)(i+5));
}

Test(fpq);

// add every 3rd
for (int i = 1; i <= 3; ++i)
{
    fpq.Enqueue(i, (float)i);
    fpq.Enqueue(i + 3, (float)(i + 3));
    fpq.Enqueue(i + 6, (float)(i + 6));
}

Test(fpq);

#endif

