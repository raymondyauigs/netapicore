namespace dotapiBase.Common
{

    public interface IUser
    {
        string UserId { get; }
        string Name { get; }
        string Email { get; }
    }

    public interface ITokenService
    {
        string CreateToken(IUser user);
        
    }
}
