using System;
using System.Collections.Generic;
using System.Linq;
using SistemaCursosOnline.Models;

namespace SistemaCursosOnline.Functional
{
    // ══════════════════════════════════════════════════════════
    // PARADIGMA FUNCIONAL
    // ══════════════════════════════════════════════════════════

    // TIPO INMUTABLE: record en C# — una vez creado no se puede modificar.
    // Útil para snapshots o reportes de solo lectura.
    public record ResumenCurso(
        int CursoId,
        string NombreCurso,
        int TotalModulos,
        int TotalHoras,
        int TotalInscritos
    );

    // Colección de funciones puras — no modifican estado externo, solo transforman datos.
    public static class CursoQueryFunctions
    {
        // ── LINQ: Where ──────────────────────────────────────────
        // Filtra cursos usando un predicado pasado como Func<> (función de alto orden)
        public static List<Curso> FiltrarCursos(List<Curso> cursos, Func<Curso, bool> predicado) =>
            cursos.Where(predicado).ToList();

        // ── LINQ: Select ─────────────────────────────────────────
        // Proyecta cada curso a su resumen inmutable (record ResumenCurso)
        public static List<ResumenCurso> ProyectarResumenes(
            List<Curso> cursos,
            List<Inscripcion> inscripciones) =>
            cursos.Select(c => new ResumenCurso(
                CursoId: c.Id,
                NombreCurso: c.Nombre,
                TotalModulos: c.ListaDeModulos.Count,
                TotalHoras: c.DuracionTotalHoras,
                TotalInscritos: inscripciones.Count(i => i.CursoId == c.Id && i.Estado == "Activa")
            )).ToList();

        // ── LINQ: Aggregate ──────────────────────────────────────
        // Acumula el total de horas de todos los cursos — función pura de reducción
        public static int TotalHorasSistema(List<Curso> cursos) =>
            cursos.Aggregate(0, (acum, c) => acum + c.DuracionTotalHoras);

        // ── Función de alto orden con Action<> ───────────────────
        // Aplica una acción sobre cada resumen; el "qué hacer" se inyecta como parámetro
        public static void AplicarSobreResumenes(
            List<ResumenCurso> resumenes,
            Action<ResumenCurso> accion) =>
            resumenes.ForEach(accion);

        // ── Función pura de ordenamiento ─────────────────────────
        // Devuelve una nueva lista ordenada sin mutar la original
        public static List<ResumenCurso> OrdenarPorHorasDesc(List<ResumenCurso> resumenes) =>
            resumenes.OrderByDescending(r => r.TotalHoras).ToList();

        // ── Composición de filtros con Func<> ────────────────────
        // Recibe dos predicados y devuelve la intersección (AND lógico) sin efectos secundarios
        public static Func<Curso, bool> CombinarFiltros(
            Func<Curso, bool> filtro1,
            Func<Curso, bool> filtro2) =>
            curso => filtro1(curso) && filtro2(curso);
    }
}
