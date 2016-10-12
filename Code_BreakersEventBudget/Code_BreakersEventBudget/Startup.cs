using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Code_BreakersEventBudget.Startup))]
namespace Code_BreakersEventBudget
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
