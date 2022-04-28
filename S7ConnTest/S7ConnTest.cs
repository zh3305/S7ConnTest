using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using S7.Net;
using Sharp7;


[RPlotExporter]
[MemoryDiagnoser]
// [SimpleJob(launchCount: 3, warmupCount: 10, targetCount: 30)]
public class S7ConnTest
{

    const string plcIp = "192.168.1.60";

    readonly Plc s7netClient;
    int  WriteStart = 200;


    readonly S7Client _sharp7Client;
    byte[] testWriteBytes;
    Random randomEngine;

    public S7ConnTest()
    {
        s7netClient = new Plc(CpuType.S7200Smart, plcIp, 0, 1);

        s7netClient.Open();


        _sharp7Client = new S7Client();

        //设置为基本连接
        _sharp7Client.SetConnectionType(S7Client.CONNTYPE_BASIC);

        _sharp7Client.ConnectTo(plcIp, 0, 1);
        randomEngine = new Random();
        testWriteBytes = new byte[33];

    }




    [Benchmark]
    public void s7netplusReadshort()
    {
        s7netClient.Read("DB1.DBW0");
    }


    [Benchmark]
    public void s7netplusReadbyte20()
    {
        s7netClient.ReadBytes(DataType.DataBlock, 1, 0, 20);
    }
    [Benchmark]
    public async Task s7netplusReadbyte20Async()
    {
        await s7netClient.ReadBytesAsync(DataType.DataBlock, 1, 0, 20);
    }

    [Benchmark]
    public async Task s7netplusReadshortAsync()
    {
        await s7netClient.ReadAsync("DB1.DBW0");
    }

    [Benchmark]
    public void Sharp7Readshort()
    {
        byte[] db1Buffer = new byte[2];
        _sharp7Client.DBRead(1, 0, 2, db1Buffer);
        db1Buffer.GetUIntAt(0);
    }

    [Benchmark]
    public void Sharp7Readbyte20()
    {
        byte[] db1Buffer = new byte[20];
        _sharp7Client.DBRead(1, 0, 20, db1Buffer);

    }
    [Benchmark]
    public void Sharp7WriteReadbyte()
    {
        randomEngine.NextBytes(testWriteBytes);
        _sharp7Client.DBWrite(1, WriteStart, testWriteBytes.Length, testWriteBytes);
        byte[] db1Buffer = new byte[testWriteBytes.Length];
        _sharp7Client.DBRead(1, WriteStart, testWriteBytes.Length, db1Buffer);

        CompareBytes(testWriteBytes, db1Buffer);

    }
    [Benchmark]
    public async Task s7netplusWriteReadbyteAsync()
    {
        randomEngine.NextBytes(testWriteBytes);
        await s7netClient.WriteBytesAsync(DataType.DataBlock, 1, WriteStart, testWriteBytes);
        var readBytesAsync = await s7netClient.ReadBytesAsync(DataType.DataBlock, 1, WriteStart, testWriteBytes.Length);

        CompareBytes(testWriteBytes, readBytesAsync);
    }
    [Benchmark]
    public void s7netplusWriteReadbyte()
    {
        randomEngine.NextBytes(testWriteBytes);
        s7netClient.WriteBytes(DataType.DataBlock, 1, WriteStart, testWriteBytes);
        var readBytesAsync  =  s7netClient.ReadBytes(DataType.DataBlock, 1, WriteStart, testWriteBytes.Length);
        CompareBytes(testWriteBytes, readBytesAsync);
    }

    private void CompareBytes(byte[] t1, byte[] t2)
    {
        if (!t1.SequenceEqual(t2))
        {
            throw new Exception("bytes not equal " + "\r\n" + BitConverter.ToString(t1) + "\r\n" + BitConverter.ToString(t2));
        }

    }
}