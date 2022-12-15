namespace Blogs.CMS.Configurations
{
    public class AppSettings
    {
        public SwaggerSettings Swagger { get; set; } = null!;

        public IdentityServerSettings IdentityServer { get; set; } = null!;
        public JwtSettings Jwt { get; set; } = null!;
        public EmailSettings Email { get; set; } = null!;

        public MinioSettings Minio { get; set; } = null!;

        public CloudSettings Cloud { get; set; } = null!;

        public ClientUploadSettings ClientUpload { get; set; } = null!;

        public FeatureFlag FeatureFlag { get; set; } = null!;
        public ClientUrl ClientUrl { get; set; } = null!;
        public BasicOauth BasicOauth { get; set; } = null!;
    }

    public class SwaggerSettings
    {
        public string ClientId { get; set; } = null!;

        public string Title { get; set; } = null!;

        public Dictionary<string, string> Scopes { get; set; } = new();
    }

    public class IdentityServerSettings
    {
        public string BaseUrl { get; set; } = null!;

        public string Audience { get; set; } = null!;

        public bool RequireHttps { get; set; }
    }

    public class JwtSettings
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;

        public double AccessTokenExpiryDuration { get; set; }

        public double RefreshTokenExpiryDuration { get; set; }
    }

    public class RabbitMQSettings
    {
        public string Uri { get; set; } = null!;
        public string VirtualHost { get; set; } = null!;
        public bool SSLEnable { get; set; } = false;
        public string Queue { get; set; } = null!;
        public string RoutingKey { get; set; } = null!;
        public string ExchangeName { get; set; } = null!;
    }

    public class EmailSettings
    {
        public string MailServer { get; set; } = null!;
        public string MailPort { get; set; } = null!;
        public string SenderName { get; set; } = null!;
        public string Sender { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class MinioSettings
    {
        public string AwsEndpoint { get; set; } = null!;
        public string AwsKey { get; set; } = null!;
        public string AwsSecret { get; set; } = null!;
        public string AwsRegion { get; set; } = null!;
        public string AwsBucket { get; set; } = null!;
    }

    public class CloudSettings
    {
        public string TempDir { get; set; } = null!;
        public string RootDir { get; set; } = null!;
        public string Domain { get; set; } = null!;
    }

    public class ClientUploadSettings
    {
        public string Folder { get; set; } = null!;
    }

    public class FeatureFlag
    {
        public bool EnableSendMailChangeRadioStatus { get; set; }
    }

    public class ClientUrl
    {
        public string ResetPassword { get; set; } = null!;
        public string VerifyUser { get; set; } = null!;
        public string ActiveStaffAccount { get; set; } = null!;
    }

    public class BasicOauth
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string ClientDomain { get; set; } = null!;
    }
}
