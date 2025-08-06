using LiesOfPractice.Interfaces;
using LiesOfPractice.Memory;
using System.Diagnostics;
using System.Text;

namespace LiesOfPractice.Services;

public class MemoryIoService : IDisposable, IMemoryIoService
{
    private const int ProcessVmRead = 0x0010;
    private const int ProcessVmWrite = 0x0020;
    private const int ProcessVmOperation = 0x0008;
    private const int ProcessQueryInformation = 0x0400;
    private const string ProcessName = "lop-win64-shipping";

    private bool _disposed;
    private CancellationTokenSource? _cts;

    #region Public Properties
    public bool IsAttached { get; private set; }
    public Process? TargetProcess { get; private set; }
    public nint ProcessHandle { get; private set; } = nint.Zero;
    public nint BaseAddress { get; private set; }
    #endregion

    #region Public Methods
    public void StartAutoAttach()
    {
        _cts = new CancellationTokenSource();
        _ = RunAutoAttachLoop(_cts.Token);

        TryAttachToProcess(); // immidiate first try
    }

    public void StopAutoAttach()
    {
        _cts?.Cancel();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            if (_cts != null)
            {
                _cts.Dispose();
                _cts = null;
            }

            if (ProcessHandle != nint.Zero)
            {
                Kernel32.CloseHandle(ProcessHandle);
                ProcessHandle = nint.Zero;
                TargetProcess = null;
                IsAttached = false;
            }
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
    public bool ReadTest(nint addr)
    {
        var array = new byte[1];
        var lpNumberOfBytesRead = 1;
        return Kernel32.ReadProcessMemory(ProcessHandle, addr, array, 1, ref lpNumberOfBytesRead) && lpNumberOfBytesRead == 1;
    }

    public void ReadTestFull(nint addr)
    {
        Console.WriteLine($"Testing Address: 0x{addr.ToInt64():X}");

        bool available = ReadTest(addr);
        Console.WriteLine($"Availability: {available}");

        if (!available)
        {
            Console.WriteLine("Memory is not readable at this address.");
            return;
        }

        try
        {
            Console.WriteLine($"Int32: {ReadInt32(addr)}");
            Console.WriteLine($"Int64: {ReadInt64(addr)}");
            Console.WriteLine($"UInt8: {ReadUInt8(addr)}");
            Console.WriteLine($"UInt32: {ReadUInt32(addr)}");
            Console.WriteLine($"UInt64: {ReadUInt64(addr)}");
            Console.WriteLine($"Float: {ReadFloat(addr)}");
            Console.WriteLine($"Double: {ReadDouble(addr)}");
            Console.WriteLine($"String: {ReadString(addr)}");

            byte[] bytes = ReadBytes(addr, 16);
            Console.WriteLine("Bytes: " + BitConverter.ToString(bytes));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading memory: " + ex.Message);
        }
    }

    public uint RunThread(nint address, uint timeout = 0xFFFFFFFF)
    {
        nint thread = Kernel32.CreateRemoteThread(ProcessHandle, nint.Zero, 0, address, nint.Zero, 0, nint.Zero);
        var ret = Kernel32.WaitForSingleObject(thread, timeout);
        Kernel32.CloseHandle(thread);
        return ret;
    }

    public bool RunThreadAndWaitForCompletion(nint address, uint timeout = 0xFFFFFFFF)
    {
        nint thread = Kernel32.CreateRemoteThread(ProcessHandle, nint.Zero, 0, address, nint.Zero, 0, nint.Zero);

        if (thread == nint.Zero)
        {
            return false;
        }

        uint waitResult = Kernel32.WaitForSingleObject(thread, timeout);
        Kernel32.CloseHandle(thread);

        return waitResult == 0;
    }

    public void AllocateAndExecute(byte[] shellcode)
    {
        nint allocatedMemory = Kernel32.VirtualAllocEx(ProcessHandle, nint.Zero, (uint)shellcode.Length);

        if (allocatedMemory == nint.Zero) return;

        WriteBytes(allocatedMemory, shellcode);
        bool executionSuccess = RunThreadAndWaitForCompletion(allocatedMemory);

        if (!executionSuccess) return;

        Kernel32.VirtualFreeEx(ProcessHandle, allocatedMemory, 0, 0x8000);
    }

    public int ReadInt32(nint addr)
    {
        var bytes = ReadBytes(addr, 4);
        return BitConverter.ToInt32(bytes, 0);
    }

    public long ReadInt64(nint addr)
    {
        var bytes = ReadBytes(addr, 8);
        return BitConverter.ToInt64(bytes, 0);
    }

    public byte ReadUInt8(nint addr)
    {
        var bytes = ReadBytes(addr, 1);
        return bytes[0];
    }

    public uint ReadUInt32(nint addr)
    {
        var bytes = ReadBytes(addr, 4);
        return BitConverter.ToUInt32(bytes, 0);
    }

    public ulong ReadUInt64(nint addr)
    {
        var bytes = ReadBytes(addr, 8);
        return BitConverter.ToUInt64(bytes, 0);
    }

    public float ReadFloat(nint addr)
    {
        var bytes = ReadBytes(addr, 4);
        return BitConverter.ToSingle(bytes, 0);
    }

    public double ReadDouble(nint addr)
    {
        var bytes = ReadBytes(addr, 8);
        return BitConverter.ToDouble(bytes, 0);
    }

    public byte[] ReadBytes(nint addr, int size)
    {
        var array = new byte[size];
        var lpNumberOfBytesRead = 1;
        Kernel32.ReadProcessMemory(ProcessHandle, addr, array, size, ref lpNumberOfBytesRead);
        return array;
    }

    public string ReadString(nint addr, int maxLength = 32)
    {
        var bytes = ReadBytes(addr, maxLength * 2);

        int stringLength = 0;
        for (int i = 0; i < bytes.Length - 1; i += 2)
        {
            if (bytes[i] == 0 && bytes[i + 1] == 0)
            {
                stringLength = i;
                break;
            }
        }

        if (stringLength == 0)
        {
            stringLength = bytes.Length - bytes.Length % 2;
        }

        return Encoding.Unicode.GetString(bytes, 0, stringLength);
    }

    public void WriteInt32(nint addr, int val)
    {
        WriteBytes(addr, BitConverter.GetBytes(val));
    }

    public void WriteFloat(nint addr, float val)
    {
        WriteBytes(addr, BitConverter.GetBytes(val));
    }

    public void WriteDouble(nint addr, double val)
    {
        WriteBytes(addr, BitConverter.GetBytes(val));
    }

    public void WriteUInt8(nint addr, byte val)
    {
        var bytes = new byte[] { val };
        WriteBytes(addr, bytes);
    }

    public void WriteByte(nint addr, int value)
    {
        Kernel32.WriteProcessMemory(ProcessHandle, addr, new byte[] { (byte)value }, 1, 0);
    }

    public void WriteBytes(nint addr, byte[] val)
    {
        Kernel32.WriteProcessMemory(ProcessHandle, addr, val, val.Length, 0);
    }

    public void WriteString(nint addr, string value, int maxLength = 32)
    {
        var bytes = new byte[maxLength];
        var stringBytes = Encoding.Unicode.GetBytes(value);
        Array.Copy(stringBytes, bytes, Math.Min(stringBytes.Length, maxLength));
        WriteBytes(addr, bytes);
    }

    public nint FollowPointers(nint baseAddress, int[] offsets, bool readFinalPtr)
    {
        ulong ptr = ReadUInt64(baseAddress);

        for (int i = 0; i < offsets.Length - 1; i++)
        {
            ptr = ReadUInt64((nint)ptr + offsets[i]);
        }

        nint finalAddress = (nint)ptr + offsets[offsets.Length - 1];

        if (readFinalPtr)
            return (nint)ReadUInt64(finalAddress);


        return finalAddress;
    }

    public void SetBitValue(nint addr, byte flagMask, bool setValue)
    {
        byte currentByte = ReadUInt8(addr);
        byte modifiedByte;

        if (setValue)
            modifiedByte = (byte)(currentByte | flagMask);
        else
            modifiedByte = (byte)(currentByte & ~flagMask);
        WriteUInt8(addr, modifiedByte);
    }

    public bool IsBitSet(nint addr, byte flagMask)
    {
        byte currentByte = ReadUInt8(addr);

        return (currentByte & flagMask) != 0;
    }

    public void SetBit32(nint addr, int bitPosition, bool setValue)
    {
        nint wordAddr = nint.Add(addr, bitPosition / 32 * 4);

        int bitPos = bitPosition % 32;

        uint currentValue = ReadUInt32(wordAddr);

        uint bitMask = 1u << bitPos;

        uint newValue = setValue
            ? currentValue | bitMask
            : currentValue & ~bitMask;

        WriteInt32(wordAddr, (int)newValue);
    }

    public void AllocCodeCave()
    {
        nint searchRangeStart = BaseAddress - 0x40000000;
        nint searchRangeEnd = BaseAddress - 0x30000;
        uint codeCaveSize = 0x2000;
        nint allocatedMemory;

        for (nint addr = searchRangeEnd; addr.ToInt64() > searchRangeStart.ToInt64(); addr -= 0x10000)
        {
            allocatedMemory = Kernel32.VirtualAllocEx(ProcessHandle, addr, codeCaveSize);

            if (allocatedMemory != nint.Zero)
            {
                CodeCaveOffsets.Base = allocatedMemory;
                break;
            }
        }
    }
    #endregion

    #region Private Methods
    private async Task RunAutoAttachLoop(CancellationToken token)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(4));

        try
        {
            while (await timer.WaitForNextTickAsync(token))
            {
                TryAttachToProcess();
            }
        }
        catch (OperationCanceledException)
        {
            // Timer has stopped
        }
    }

    private void TryAttachToProcess()
    {
        if (ProcessHandle != nint.Zero)
        {
            if (TargetProcess == null || TargetProcess.HasExited)
            {
                Kernel32.CloseHandle(ProcessHandle);
                ProcessHandle = nint.Zero;
                TargetProcess = null;
                IsAttached = false;
            }
            return;
        }

        var processes = Process.GetProcessesByName(ProcessName);
        if (processes.Length > 0 && !processes[0].HasExited)
        {
            TargetProcess = processes[0];
            ProcessHandle = Kernel32.OpenProcess(
                ProcessVmRead | ProcessVmWrite | ProcessVmOperation | ProcessQueryInformation,
                false,
                TargetProcess.Id);

            if (ProcessHandle == nint.Zero)
            {
                TargetProcess = null;
                IsAttached = false;
            }
            else
            {
                if (TargetProcess.MainModule != null)
                {
                    BaseAddress = TargetProcess.MainModule.BaseAddress;
                }
                IsAttached = true;
            }
        }
    }

    #endregion

    ~MemoryIoService()
    {
        Dispose();
    }



    // public bool IsGameLoaded()
    // {
    //     return (IntPtr) ReadUInt64((IntPtr)ReadUInt64(Offsets.WorldChrMan.Base) + Offsets.WorldChrMan.PlayerIns)!= IntPtr.Zero;
    // }


    //
    // public IntPtr GetModuleStart(IntPtr address)
    // {
    //     return Kernel32.QueryMemory(ProcessHandle, address).AllocationBase;
    // }
}
