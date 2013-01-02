using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Caliburn.Micro;
using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using octocal.UI.Shell.ViewModels;

namespace octocal
{
    // Hooks up Castle Windsor as the container for your Caliburn.Micro application. 
    // Turns on support for delegate factory methods (e.g. passing the factory "Func<XyzEditViewModel>" as a constructor arg)
    // Dependencies: In addition to Caliburn.Micro you will need to reference Castle.Core and Castle.Windsor
    //Taken from: https://gist.github.com/1127914
    public class CastleBootstrapper<TRootViewModel> : Bootstrapper<TRootViewModel>
    {
        private ApplicationContainer _container;

        protected override void Configure()
        {
            _container = new ApplicationContainer();
            _container.AddFacility<TypedFactoryFacility>();
            _container.Register(AllTypes.FromAssembly(typeof(ShellViewModel).Assembly)
                .Where(x => x.Name.EndsWith("ViewModel") || x.Name.EndsWith("View"))
                .Configure(x => x.LifeStyle.Is(LifestyleType.Transient)));
            _container.Register(Component.For<IWindsorContainer>().Instance(_container));

            base.Configure();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[]
                {
                Assembly.GetExecutingAssembly(),
                typeof(ShellViewModel).Assembly
                };
        }

        protected override object GetInstance(Type service, string key)
        {
            return string.IsNullOrWhiteSpace(key)
                       ? _container.Resolve(service)
                       : _container.Resolve(key, service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return (IEnumerable<object>)_container.ResolveAll(service);
        }

        protected override void BuildUp(object instance)
        {
            instance.GetType().GetProperties()
                .Where(property => property.CanWrite && property.PropertyType.IsPublic)
                .Where(property => _container.Kernel.HasComponent(property.PropertyType))
                .ForEach(property => property.SetValue(instance, _container.Resolve(property.PropertyType), null));
        }
    }

    // If you don't already have a ForEach extension method in your project here you go:
}
