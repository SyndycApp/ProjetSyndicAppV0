using Refit;
using System.Threading.Tasks;

public interface IPasswordApi
{
    [Post("/api/Password/forgot")] Task<object> Forgot([Body] object dto);
    [Post("/api/Password/reset")] Task<object> Reset([Body] object dto);
}
