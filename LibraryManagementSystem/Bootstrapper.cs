using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using LibraryManagementSystem.Logic;
using LibraryManagementSystem.Logic.Interfaces;
using AutoMapper;
using LibraryManagementSystem.Data.Mapping;
using LibraryManagementSystem.Data.Models;

namespace LibraryManagementSystem
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<ILibraryAssetLogic, LibraryAssetLogic>();
            container.RegisterType<ICheckoutLogic, CheckoutLogic>();
            container.RegisterType<IHoldLogic, HoldLogic>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<EntityMappingProfile>());

            container.RegisterInstance<IMapper>(config.CreateMapper());
            container.RegisterType<IUserLogic, UserLogic>();
            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}