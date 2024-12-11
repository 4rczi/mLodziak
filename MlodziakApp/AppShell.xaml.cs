using MlodziakApp.Views;

namespace MlodziakApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ExplorationPage), typeof(ExplorationPage));
            Routing.RegisterRoute(nameof(InvitationPage), typeof(InvitationPage));
        }

    }
}
