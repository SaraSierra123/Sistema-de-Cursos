using System;
using Castle.DynamicProxy;

namespace SistemaCursosOnline.Aspects
{
    // ══════════════════════════════════════════════════════════
    // PARADIGMA DE ASPECTOS — Interceptor 2: ErrorHandlingInterceptor
    // Preocupación transversal: captura centralizada de excepciones.
    // Evita que los servicios manejen errores individualmente;
    // el aspecto los intercepta, los muestra y los re-lanza.
    // ══════════════════════════════════════════════════════════
    public class ErrorHandlingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                // Intenta ejecutar el método normalmente
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                // Captura centralizada: muestra el error con contexto del método que lo originó
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR] En {invocation.TargetType.Name}.{invocation.Method.Name}():");
                Console.WriteLine($"        {ex.Message}");
                Console.ResetColor();

                // Re-lanza para que el llamador pueda reaccionar si es necesario
                throw;
            }
        }
    }
}
