namespace LiesOfPractice.Memory;

public class Pattern(byte[] bytes, string mask, int instructionOffset, AddressingMode addressingMode,
    int offsetLocation = 0, int instructionLength = 0)
{
    public byte[] Bytes { get; } = bytes;
    public string Mask { get; } = mask;
    public int InstructionOffset { get; } = instructionOffset;
    public AddressingMode AddressingMode { get; } = addressingMode;
    public int OffsetLocation { get; } = offsetLocation;
    public int InstructionLength { get; } = instructionLength;
}

public enum AddressingMode
{
    Absolute,
    Relative,
    Direct32
}

public static class Patterns
{
    public static readonly Pattern PlayerBaseA = new(
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

    public static readonly Pattern InfiniteGoodsFlag = new(
        [0x80, 0x3D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x75, 0x0E, 0x45],
        "xx????xxxx",
        0,
        AddressingMode.Relative,
        2,
        7
    );


    //Patch

    public static readonly Pattern NoErgoLoss = new Pattern(
        [0x89, 0x99, 0xA4, 0x00, 0x00, 0x00, 0x4C],
        "xxxxxxx",
        0,
        AddressingMode.Absolute
    );


    //Hooks


    //Funcs

    public static readonly Pattern GiveErgo = new(
        [0x48, 0x83, 0xEC, 0x28, 0x4C, 0x8B, 0xC9, 0x85, 0xD2, 0x78, 0x73],
        "xxx????xxxxx",
        0,
        AddressingMode.Absolute
    );

    public static readonly Pattern Rest = new Pattern(
        [0xE8, 0x00, 0x00, 0x00, 0x00, 0x4C, 0x8B, 0x83, 0xD8, 0x08],
        "x????xxxxx",
        0,
        AddressingMode.Relative,
        1,
        5
    );

    public static readonly Pattern GiveItem = new Pattern(
        [0xE8, 0x00, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x5D, 0xF7, 0x44, 0x8B, 0xE8],
        "x????xxxxxxx",
        0,
        AddressingMode.Relative,
        1,
        5
    );

    public static readonly Pattern GiveWeapon = new Pattern(
        [0xE8, 0x00, 0x00, 0x00, 0x00, 0x84, 0xC0, 0x74, 0x03, 0x41, 0xFF, 0xC5, 0x49],
        "x????xxxxxxxx",
        0,
        AddressingMode.Relative,
        1,
        5
    );


    public static readonly Pattern GetGiveItemEntity = new Pattern(
        [
            0xE8, 0x00, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x88, 0x88, 0x01, 0x00, 0x00, 0x48, 0x85, 0xC9, 0x0F, 0x84, 0x2E
        ],
        "x????xxxxxxxxxxxxx",
        0,
        AddressingMode.Relative,
        1,
        5
    );
    
}