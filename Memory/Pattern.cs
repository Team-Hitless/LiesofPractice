namespace LiesOfPractice.Memory;

public class Pattern
{
    public byte[] Bytes { get; }
    public string Mask { get; }
    public int InstructionOffset { get; }
    public AddressingMode AddressingMode { get; }
    public int OffsetLocation { get; }
    public int InstructionLength { get; }

    public Pattern(byte[] bytes, string mask, int instructionOffset, AddressingMode addressingMode,
        int offsetLocation = 0, int instructionLength = 0)
    {
        Bytes = bytes;
        Mask = mask;
        InstructionOffset = instructionOffset;
        AddressingMode = addressingMode;
        OffsetLocation = offsetLocation;
        InstructionLength = instructionLength;
    }
}

public enum AddressingMode
{
    Absolute,
    Relative,
    Direct32
}

public static class Patterns
{
    public static readonly Pattern GiveErgoEntity = new(
        [0x48, 0x8D, 0x15, 0x00, 0x00, 0x00, 0x00, 0x44, 0x0F, 0xB6, 0xCF],
        "xxx????xxxx",
        0,
        AddressingMode.Relative,
        3,
        7
    );

    public static readonly Pattern ActivateAllTeleports = new(
        [0x48, 0x8B, 0x05, 0x00, 0x00, 0x00, 0x00, 0x83, 0x38, 0x00, 0x7F, 0xD7],
        "xxx????xxxxx",
        0,
        AddressingMode.Relative,
        3,
        7
    );

    public static readonly Pattern DebugFlagsBaseA = new Pattern(
        [0x44, 0x38, 0x25, 0x00, 0x00, 0x00, 0x00, 0x74, 0x58, 0x45],
        "xxx????xxx",
        0,
        AddressingMode.Relative,
        3,
        7
    );


    //Patch


    //Hooks


    //Funcs

    public static readonly Pattern GiveErgo = new(
        [0x48, 0x83, 0xEC, 0x28, 0x4C, 0x8B, 0xC9, 0x85, 0xD2, 0x78, 0x73],
        "xxx????xxxxx",
        0,
        AddressingMode.Absolute
    );
}