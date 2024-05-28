using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.ApiEndpoints;

public static class ProdutosEndpoints
{
    public static void MapProdutosEndpoints(this WebApplication app)
    {
        //endpoints para produtos
        app.MapPost("/produtos", async (Produto produto, AppDbContext db) =>
        {
            db.Produtos.Add(produto);
            await db.SaveChangesAsync();
            return Results.Created($"/produtos/{produto.ProdutoId}", produto);
        });

        app.MapGet("/produtos", async (AppDbContext db) => await db.Produtos.ToListAsync()).WithTags("Produtos")
            .RequireAuthorization();

        app.MapGet("/produtos/{id:int}", async (int id, AppDbContext db) =>
        {
            var produto = await db.Produtos.FindAsync(id);
            return produto is null ? Results.NotFound() : Results.Ok(produto);
        });

        app.MapPut("/produtos/{id:int}", async (int id, Produto produto, AppDbContext db) =>
        {
            if (id != produto.ProdutoId)
                return Results.BadRequest();

            var produtoDb = await db.Produtos.FindAsync(id);

            if (produtoDb is null)
                return Results.NotFound();

            produtoDb.Nome = produto.Nome;
            produtoDb.Descricao = produto.Descricao;
            produtoDb.Imagem = produto.Imagem;
            produtoDb.Preco = produto.Preco;
            produtoDb.CategoriaId = produto.CategoriaId;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        app.MapDelete("/produtos/{id:int}", async (int id, AppDbContext db) =>
        {
            var produto = await db.Produtos.FindAsync(id);

            if (produto is null)
                return Results.NotFound();

            db.Produtos.Remove(produto);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}