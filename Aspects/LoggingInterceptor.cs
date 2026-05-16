using System;
using Castle.DynamicProxy;

namespace SistemaCursosOnline.Aspects
{
    // ══════════════════════════════════════════════════════════
    // PARADIGMA DE ASPECTOS — Interceptor 1: LoggingInterceptor
    // Preocupación transversal: registra entrada, salida y tiempo
    // de ejecución de TODOS los métodos de servicio automáticamente.
    // Castle llama a Intercept() antes y después de cada método.
    // ══════════════════════════════════════════════════════════
    public class LoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            // --- PRE-INVOCACIÓN: registra entrada al método ---
            var hora = DateTime.Now.ToString("HH:mm:ss.fff");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"[LOG {hora}] → {invocation.TargetType.Name}.{invocation.Method.Name}() INICIO");

            // Si el método recibe argumentos, los mostramos para trazabilidad
            if (invocation.Arguments.Length > 0)
            {
                Console.Write($"             Args: ");
                foreach (var arg in invocation.Arguments)
                    Console.Write(arg != null ? $"{arg} " : "null ");
                Console.WriteLine();
            }

            Console.ResetColor();

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // --- INVOCACIÓN REAL del método del servicio ---
            invocation.Proceed();

            stopwatch.Stop();

            // --- POST-INVOCACIÓN: registra salida y duración ---
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"[LOG {DateTime.Now:HH:mm:ss.fff}] ← {invocation.TargetType.Name}.{invocation.Method.Name}() FIN ({stopwatch.ElapsedMilliseconds}ms)");
            Console.ResetColor();
        }
    }
}
