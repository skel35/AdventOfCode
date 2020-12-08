using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Math;

namespace AdventOfCode._2019
{
    public class Day23 : Solution
    {
        public Day23() : base(23, 2019) { }
        protected override void Solve()
        {
            var text = ReadText();
            var pc = new Amplifier[50];
            var network = new Queue<(long, long)>[50];
            (long x, long y) nat = (0, 0);
            bool natReceived = false;
            (long x, long y) natPrev = (-123, -123);
            for (var i = 0; i < 50; i++)
            {
                pc[i] = new Amplifier(text);
                pc[i].Input.Enqueue(i);
                network[i] = new Queue<(long, long)>();
            }

            var emptyNetworkTimes = 0;

            while (true)
            {
                for (var i = 0; i < 50; i++)
                {
                    pc[i].Run();
                    if (network[i].Count > 0)
                    {
                        var (x, y) = network[i].Dequeue();
                        pc[i].Input.Enqueue(x);
                        pc[i].Input.Enqueue(y);
                    }
                    else
                    {
                        pc[i].Input.Enqueue(-1);
                    }

                    while (pc[i].Output.Count >= 3)
                    {
                        var address = pc[i].Output.Dequeue();
                        var x = pc[i].Output.Dequeue();
                        var y = pc[i].Output.Dequeue();
                        if (address == 255)
                        {
                            // part 1:
                            // Console.WriteLine(y);
                            // return;

                            // part 2:
                            natReceived = true;
                            nat.x = x;
                            nat.y = y;

                        }
                        else
                        {
                            network[address].Enqueue((x, y));
                        }
                    }
                }

                if (network.All(q => q.Count == 0))
                {
                    emptyNetworkTimes++;
                }
                else
                {
                    emptyNetworkTimes = 0;
                }

                if (natReceived && emptyNetworkTimes >= 50)
                {
                    // idle
                    network[0].Enqueue(nat);
                    if (natPrev.y == nat.y)
                    {
                        Console.WriteLine(natPrev.y);
                        return;
                    }

                    natPrev = nat;
                    emptyNetworkTimes = 0;
                }
            }
            // var tasks = new Task[50];
            // for (var i = 0; i < 50; i++)
            // {
            //     var ind = i;
            //     // var res = pc[ind].Run();
            //     // tasks[i] = new Task(ind => pc[(int)ind].Run(), i);
            //     tasks[i] = Task.Run(() => pc[ind].Run());
            // }
            //
            // var combinedTask = Task.WhenAny(tasks);
            // combinedTask.Wait();
        }
    }
}
