using System.Diagnostics;

namespace LiesOfPractice.Interfaces
{
    public interface IMemoryIoService
    {
        public bool IsAttached { get; }
        public Process? TargetProcess { get; }
        public nint ProcessHandle { get; }
        public nint BaseAddress { get; }
        public nint FollowPointers(nint baseAddress, int[] offsets, bool readFinalPtr);
        void AllocateAndExecute(byte[] shellcode);
        void AllocCodeCave();
        void Dispose();
        bool IsBitSet(nint addr, byte flagMask);
        byte[] ReadBytes(nint addr, int size);
        double ReadDouble(nint addr);
        float ReadFloat(nint addr);
        int ReadInt32(nint addr);
        long ReadInt64(nint addr);
        string ReadString(nint addr, int maxLength = 32);
        bool ReadTest(nint addr);
        void ReadTestFull(nint addr);
        uint ReadUInt32(nint addr);
        ulong ReadUInt64(nint addr);
        byte ReadUInt8(nint addr);
        uint RunThread(nint address, uint timeout = uint.MaxValue);
        bool RunThreadAndWaitForCompletion(nint address, uint timeout = uint.MaxValue);
        void SetBit32(nint addr, int bitPosition, bool setValue);
        void SetBitValue(nint addr, byte flagMask, bool setValue);
        void StartAutoAttach();
        void StopAutoAttach();
        void WriteByte(nint addr, int value);
        void WriteBytes(nint addr, byte[] val);
        void WriteDouble(nint addr, double val);
        void WriteFloat(nint addr, float val);
        void WriteInt32(nint addr, int val);
        void WriteString(nint addr, string value, int maxLength = 32);
        void WriteUInt8(nint addr, byte val);
    }
}