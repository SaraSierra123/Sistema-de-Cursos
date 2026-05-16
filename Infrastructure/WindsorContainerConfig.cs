using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.DynamicProxy;
using SistemaCursosOnline.Aspects;
using SistemaCursosOnline.Interfaces;
using SistemaCursosOnline.Services;

namespace SistemaCursosOnline.Infrastructure
{
    // ══════════════════════════════════════════════════════════
    // PARADIGMA DE ASPECTOS — Contenedor Windsor
    // Registra los servicios bajo sus interfaces y aplica
    // los interceptores automáticamente a cada resolución.
    // ══════════════════════════════════════════════════════════
    public static class WindsorContainerConfig
    {
        public static IWindsorContainer Build()
        {
            var container = new WindsorContainer();

            // Registra los interceptores como componentes del contenedor
            container.Register(Component.For<LoggingInterceptor>().LifestyleSingleton());
            container.Register(Component.For<ErrorHandlingInterceptor>().LifestyleSingleton());

            // Registra cada servicio bajo su interfaz con los dos interceptores activos.
            // Castle DynamicProxy generará un proxy que intercepta cada llamada.
            container.Register(
                Component.For<IEstudianteService>()
                    .ImplementedBy<EstudianteService>()
                    .Interceptors<LoggingInterceptor, ErrorHandlingInterceptor>()
                    .LifestyleSingleton()
            );

            container.Register(
                Component.For<IInstructorService>()
                    .ImplementedBy<InstructorService>()
                    .Interceptors<LoggingInterceptor, ErrorHandlingInterceptor>()
                    .LifestyleSingleton()
            );

            container.Register(
                Component.For<ICursoService>()
                    .ImplementedBy<CursoService>()
                    .Interceptors<LoggingInterceptor, ErrorHandlingInterceptor>()
                    .LifestyleSingleton()
            );

            // InscripcionService depende de ICursoService e IEstudianteService;
            // Windsor los inyecta automáticamente al resolver IInscripcionService.
            container.Register(
                Component.For<IInscripcionService>()
                    .ImplementedBy<InscripcionService>()
                    .Interceptors<LoggingInterceptor, ErrorHandlingInterceptor>()
                    .LifestyleSingleton()
            );

            return container;
        }
    }
}
