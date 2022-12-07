namespace day07;

public record Cmd
{
    public record Ls(IEnumerable<string> Items) : Cmd;

    public record Cd(string Target) : Cmd;
    private Cmd(){}
}