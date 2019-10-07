using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Web.Models;
using Gemstar.BSPMS.HotelScanOrder.Web.Models.Pay;

namespace Gemstar.BSPMS.HotelScanOrder.Web.App_Start
{
    public class AutofacConfig
    {
        public static void Config(MvcApplication mvcApplication)
        {
            /*
             说明：创建服务接口和接口实现后，都需要在此处进行注册，然后在需要使用的地方调用baseController中的GetService<Interface>来获取对应接口的实现实例
             注册时为了防止每次合并都出现冲突，请按模块进行注册，下方使用注释的形式列出了相关模块区域
             */
            var builder = new ContainerBuilder();
            builder.Register(c => { return DependencyResolver.Current; }).As<IDependencyResolver>();
            //通用的mvc组件注册
            //builder.RegisterType<CurrentInfoSession>().As<ICurrentInfo>().InstancePerRequest();
            builder.Register(c =>
            {
                var currentInfo = new CurrentInfo();
                currentInfo.LoadValues();
                return currentInfo;
            }).As<ICurrentInfo>().InstancePerRequest();
            builder.RegisterType<MvcApplication>().As<ISettingProvider>();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule<AutofacWebTypesModule>();

            builder.RegisterType<PayActionXmlBuilder>().AsSelf();
            builder.RegisterType<PayActionXmlBase>().AsSelf();
            builder.RegisterType<PayActionXmlHouse>().AsSelf();
            builder.RegisterType<PayActionXmlMbrcard>().AsSelf();
            builder.RegisterType<PayActionXmlMbrLargess>().AsSelf();
            builder.RegisterType<PayActionXmlMbrCardAndLargess>().AsSelf();
            builder.RegisterType<PayActionXmlCorp>().AsSelf();
            builder.RegisterType<PayActionXmlOnlineBarCode>().AsSelf();


            //注册到mvc中
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}