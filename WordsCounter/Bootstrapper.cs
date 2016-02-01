namespace WordsCounter
{
    using System.ComponentModel.Composition.Hosting;
    using System.Windows;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Mef;

    public class Bootstrapper : MefBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<MainWindow>();
        }

        //protected override void InitializeShell()
        //{
        //    App.Current.MainWindow.Show();
        //}

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Bootstrapper).Assembly));
        }
    }
}
