using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImageSharingWithUpload.Startup))]
namespace ImageSharingWithUpload
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
