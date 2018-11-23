using Autofac;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace WMS.PlantFilter.Web
{
    public class AutofacControllerFactory : DefaultControllerFactory
    {
        private IContainer AutofacContainer
        {
            get
            {
                return DIFactory.GetContainer();
            }
        }
        public override void ReleaseController(IController controller)
        {
            base.ReleaseController(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (null == controllerType)
            {
                return null;
            }
            IController controller = (IController)this.AutofacContainer.Resolve(controllerType);
            return controller;
        }
    }
}