using Microsoft.AspNetCore.Http.HttpResults;

namespace dotapiBase.Core.Model
{
    public class FakeUser: IUser
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set;} = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
