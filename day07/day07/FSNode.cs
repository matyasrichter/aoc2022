namespace day07;

public record FsNode
{

    public record Dir : FsNode
    {
        public Dictionary<string, FsNode> Items { get; set; } = new();
    }

    public record File(int Size) : FsNode;
};