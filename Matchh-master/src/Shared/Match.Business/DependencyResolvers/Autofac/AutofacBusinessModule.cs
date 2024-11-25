using Autofac;
using Match.Business.Concrete;
using Match.Business.Services;
using Match.DataAccess.Abstract;
using Match.DataAccess.Concrete.EntityFreamwork.ReportDal;
using Match.DataAccess.Concrete.EntityFreamwork.UserDal;


namespace Match.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<ReportManager>().As<IReportService>();
            builder.RegisterType<EfReportDal>().As<IReportDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();

        }
    }
}
