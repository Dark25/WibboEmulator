namespace WibboEmulator.Games.Users.Permissions;

public sealed class PermissionComponent : IDisposable
{
    private readonly List<string> _permissions;
    private readonly List<string> _commands;

    public PermissionComponent()
    {
        this._permissions = new List<string>();
        this._commands = new List<string>();
    }

    public bool Init()
    {
        this._permissions.Clear();
        this._commands.Clear();

        return true;
    }

    public bool HasRight(string right) => this._permissions.Contains(right);

    public bool HasCommand(string command) => this._commands.Contains(command);

    public void Dispose()
    {
        this._permissions.Clear();
        GC.SuppressFinalize(this);
    }
}
