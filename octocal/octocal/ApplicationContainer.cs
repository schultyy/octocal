using Caliburn.Micro;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using octocal.Domain;
using octocal.UI.Services;

namespace octocal
{
    public class ApplicationContainer : WindsorContainer
    {
        public ApplicationContainer()
        {
            Register(
                Component.For<IWindowManager>().ImplementedBy<WindowManager>().LifeStyle.Is(LifestyleType.Singleton),
                Component.For<IEventAggregator>().ImplementedBy<EventAggregator>().LifeStyle.Is(LifestyleType.Singleton),
                Component.For<IMessageBoxService>().ImplementedBy<MessageBoxService>().LifeStyle.Is(LifestyleType.Singleton),
                Component.For<IAppointmentService>().ImplementedBy<AppointmentService>().LifeStyle.Is(LifestyleType.Singleton)
                );

            //RegisterViewModels();
        }

        private void RegisterViewModels()
        {
            Register(AllTypes.FromAssembly(GetType().Assembly)
                         .Where(x => x.Name.EndsWith("ViewModel"))
                         .Configure(x => x.LifeStyle.Is(LifestyleType.Transient)));
        }
    }
}