using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.ApiEndpoints;

public static class CategoriasEndpoints
{
    public static void MapCategoriasEndpoints(this WebApplication app)
    {
        //endpoints para categorias
        app.MapGet("/", () => "Catálogo de Produtos").ExcludeFromDescription();

        app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
        {
            db.Categorias.Add(categoria);
            await db.SaveChangesAsync();
            return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
        });

        app.MapGet("/categorias", async (AppDbContext db) => await db.Categorias.ToListAsync()).WithTags("Categorias")
            .RequireAuthorization();

        app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
        {
            var categoria = await db.Categorias.FindAsync(id);
            return categoria is null ? Results.NotFound() : Results.Ok(categoria);
        });

        app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, AppDbContext db) =>
        {
            if (id != categoria.CategoriaId)
                return Results.BadRequest();

            var categoriaDb = await db.Categorias.FindAsync(id);

            if (categoriaDb is null)
                return Results.NotFound();

            categoriaDb.Nome = categoria.Nome;
            categoriaDb.Descricao = categoria.Descricao;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
        {
            var categoria = await db.Categorias.FindAsync(id);

            if (categoria is null)
                return Results.NotFound();

            db.Categorias.Remove(categoria);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}