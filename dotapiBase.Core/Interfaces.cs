namespace dotapiBase.Core
{

    public interface IUser
    {
        string UserId { get; }
        string Name { get; }
        string Email { get; }
    }
}
