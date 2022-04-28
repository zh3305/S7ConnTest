# Sharp7与S7NetPlus 性能测试

### 介绍

​		Sharp7和都S7NetPlus是纯C#实现的基于以太网与S7系列的西门子PLC通讯的开源库.都支持.net core 跨平台可以部署在linxu, docker,windwos 中. 

### 测试环境

	CPU 12th Gen Intel(R) Core(TM) i9-12900K
	内存 32.0 GB
	PLC 西门子 Smart200 
	通讯方式 局域网 TCP/IP
	.net Runtime .net Core 6

### 测试结果

``` ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
12th Gen Intel Core i9-12900K, 1 CPU, 24 logical and 16 physical cores
.NET SDK=6.0.202
  [Host]     : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT
  DefaultJob : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT
```
| Method                      |    平均值 |      误差 |      方差 | 分配的内存 |
| --------------------------- | --------: | --------: | --------: | ---------: |
| Sharp7Readshort             | 15.526 ms | 0.0466 ms | 0.0435 ms |       37 B |
| s7netplusReadshort          |  2.792 ms | 0.0544 ms | 0.0509 ms |    2,531 B |
| s7netplusReadshortAsync     |  2.770 ms | 0.0362 ms | 0.0339 ms |    3,371 B |
| Sharp7Readbyte20            | 15.482 ms | 0.0375 ms | 0.0351 ms |       58 B |
| s7netplusReadbyte20         |  2.789 ms | 0.0351 ms | 0.0328 ms |    2,275 B |
| s7netplusReadbyte20Async    |  2.807 ms | 0.0337 ms | 0.0315 ms |    2,811 B |
| Sharp7WriteReadbyte         | 30.972 ms | 0.0858 ms | 0.0760 ms |       86 B |
| s7netplusWriteReadbyte      |  5.772 ms | 0.0847 ms | 0.0792 ms |    4,627 B |
| s7netplusWriteReadbyteAsync |  5.724 ms | 0.0855 ms | 0.0800 ms |    5,440 B |


### 总结

​	相对来说使用Sharp7 的内存占用最小,s7netplus 读取速度更快.

​	测试代码地址https://github.com/zh3305/S7ConnTest

   博客园:https://www.cnblogs.com/hongshao/p/16203746.html



