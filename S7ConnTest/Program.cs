// See https://aka.ms/new-console-template for more information.

using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run<S7ConnTest>();
Console.WriteLine("测试完毕,总耗时:" + summary.TotalTime);
Console.ReadLine();