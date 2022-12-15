using Blogs.DataAccess.EntityFramework;

namespace Blogs.CMS.Services
{
    public class BaseService : IAsyncDisposable
    {
        protected readonly BlogsDbContext DbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected BaseService(IHttpContextAccessor httpContextAccessor, BlogsDbContextFactory dbContextFactory)
        {
            DbContext = dbContextFactory.CreateDbContext();
            _httpContextAccessor = httpContextAccessor;
        }

        //private int? GetCurrentUser()
        //{
        //    int? userId = null;
        //    if (_httpContextAccessor.HttpContext != null)
        //        userId = _httpContextAccessor.HttpContext.User.GetCurrentUserId();
        //    return userId;
        //}

        //private string GetUserAgent()
        //{
        //    string userAgent = null!;
        //    if (_httpContextAccessor.HttpContext != null)
        //        userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
        //    return userAgent;
        //}

        //private string GetIpAddress()
        //{
        //    string ipAddress = null!;
        //    if (_httpContextAccessor.HttpContext != null)
        //        ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
        //    return ipAddress;
        //}

        public ValueTask DisposeAsync()
        {
            return DbContext.DisposeAsync();
        }

        //private string GetCurrentUserName()
        //{
        //    var user = DbContext.Users.Find(CurrentUserId);

        //    return user?.UserName;
        //}
    }
}
