using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TCC.Domain.Dtos;
using TCC.Domain.DTOs.Agendamento;
using TCC.Domain.DTOs.Categoria;
using TCC.Domain.DTOs.Login;
using TCC.Domain.DTOs.Produto;
using TCC.Domain.DTOs.Usuario;
using TCC.Domain.Entities;
using TCC.Infra.Data.Context; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ana Paula estética API",
        Version = "v1",
        Description = "API para gerenciamento de agendamentos para um salão de estética"
    });

    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"<b>JWT Autorização</b> <br/> 
                          Digite 'Bearer' [espaço] e em seguida colar seu token na caixa de texto abaixo.
                          <br/> <br/>
                          <b>Exemplo:</b> 'bearer 123456abcdefg...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
});

builder.Services.AddDbContext<MiraBeautyContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "ana.estetica",
        ValidAudience = "ana.estetica",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("{49ff3b22-3c25-4919-8b5f-2a79dd7088a6}"))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddCors();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseCors(x => x
.AllowAnyOrigin()
.AllowAnyMethod() // Get, Post , Put, Delete, etc...
.AllowAnyHeader());

#region Endpoints Categoria

app.MapPost("categoria/adicionar", (MiraBeautyContext context, CategoriaAdicionarDto categoriaDto) =>
{
    var resultado = new CategoriaAdicionarDtoValidator().Validate(categoriaDto);

    if (!resultado.IsValid)
    {
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));
    }

    var categoria = new Categoria
    {
        Id = Guid.NewGuid(),
        Descricao = categoriaDto.Descricao,
        CategoriaImagens = categoriaDto.Imagens.Select(i => new CategoriaImagem
        {
            Id = Guid.NewGuid(),
            Imagem = i.Imagem
        }).ToList()
    };

    context.CategoriaSet.Add(categoria);
    context.SaveChanges();

    return Results.Created("Created", "Categoria registrada com sucesso");
}).RequireAuthorization().
WithTags("Categoria");

app.MapGet("categoria/listar", (MiraBeautyContext context) =>
{
    var listaCategoriaDto = context.CategoriaSet.Select(cat => new CategoriaListarDto
    {
        Id = cat.Id,
        Descricao = cat.Descricao,
    }).ToList();

    return Results.Ok(listaCategoriaDto);
}).RequireAuthorization().
    WithTags("Categoria");

app.MapPut("categoria/atualizar", (MiraBeautyContext context, CategoriaAtualizarDto categoriaDto) =>
{
    var resultado = new CategoriaAtualizarDtoValidator().Validate(categoriaDto);

    if (!resultado.IsValid)
    {
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));
    }

    var categoria = context.CategoriaSet.Find(categoriaDto.Id);
    if (categoria == null)
    {
        return Results.NotFound();
    }
    categoria.Descricao = categoriaDto.Descricao;
    context.SaveChanges();
    return Results.Ok("Categoria atualizada com sucesso");
}).RequireAuthorization().
    WithTags("Categoria");

app.MapDelete("categoria/deletar/{id:guid}", (MiraBeautyContext context, Guid id) =>
{
    var categoria = context.CategoriaSet.Find(id);
    if (categoria == null)
    {
        return Results.NotFound();
    }
    context.CategoriaSet.Remove(categoria);
    context.SaveChanges();
    return Results.Ok("Categoria deletada com sucesso");
}).RequireAuthorization().
    WithTags("Categoria");

#endregion

#region Endpoints Agendamento
app.MapPost("agendamento/adicionar", (MiraBeautyContext context, AgendamentoProdutoAdicionarDto agendamentoDto) =>
    {
        var agendamento = new Agendamento
        {
            Id = Guid.NewGuid()
        };

        context.AgendamentoSet.Add(agendamento);
        context.SaveChanges();
        return Results.Created("Created", "Agendamento registrado com sucesso");
    }).RequireAuthorization().
    WithTags("Agendamento");

app.MapGet("agendamento/listar", (MiraBeautyContext context) =>
{
    var listaAgendamentoDto = context.AgendamentoSet.Select(agen => new AgendamentoListarDto
    {
        Id = agen.Id,
        DataHoraFinal = agen.DataHoraFinal,
        DataHoraInicial = agen.DataHoraInicial,
        Status = agen.Status,
    }).ToList();
    return Results.Ok(listaAgendamentoDto);
}).RequireAuthorization().
    WithTags("Agendamento");

app.MapPut("agendamento/atualizar", (MiraBeautyContext context, AgendamentoAtualizarDto agendamentoDto) =>
{
    var agendamento = context.AgendamentoSet.Find(agendamentoDto.Id);
    if (agendamento == null)
    {
        return Results.NotFound();
    }
    agendamento.DataHoraFinal = agendamentoDto.DataHoraFinal;
    agendamento.DataHoraInicial = agendamentoDto.DataHoraInicial;
    agendamento.Status = agendamentoDto.Status;
    context.SaveChanges();
    return Results.Ok("Agendamento atualizado com sucesso");
}).RequireAuthorization().
    WithTags("Agendamento");

app.MapDelete("agendamento/deletar/{id:guid}", (MiraBeautyContext context, Guid id) =>
{
    var agendamento = context.AgendamentoSet.Find(id);
    if (agendamento == null)
    {
        return Results.NotFound();
    }
    context.AgendamentoSet.Remove(agendamento);
    context.SaveChanges();
    return Results.Ok("Agendamento deletado com sucesso");
}).RequireAuthorization().
    WithTags("Agendamento");

#endregion

#region Endpoints Produto

app.MapPost("produto/adicionar", (MiraBeautyContext context, ProdutoAdicionarDto produtoDto) =>
{
    var resultado = new ProdutoAdicionarDtoValidator().Validate(produtoDto);

    if (!resultado.IsValid)
    {
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));
    }

    var produto = new Produto
    {
        Id = Guid.NewGuid(),
        Descricao = produtoDto.Descricao,
        Tempo = produtoDto.Tempo,
        Preco = produtoDto.Preco,
        ProdutoImagens = produtoDto.Imagens.Select(i => new ProdutoImagem
        {
            Id = Guid.NewGuid(),
            Imagem = i.Imagem
        }).ToList(),
        CategoriaId = produtoDto.CategoriaId,
    };
    context.ProdutoSet.Add(produto);
    context.SaveChanges();
    return Results.Created("Created", "Produto registrado com sucesso");
}).RequireAuthorization().
    WithTags("Produto");

app.MapGet("produto/listar", (MiraBeautyContext context) =>
{
    var listaProdutoDto = context.ProdutoSet.Select(pro => new ProdutoListarDto
    {
        Id = pro.Id,
        Descricao = pro.Descricao,
        CategoriaId = pro.CategoriaId,
        Preco = pro.Preco,
        Tempo = pro.Tempo,
        ProdutoImagens = pro.ProdutoImagens.Select(i => new ProdutoImagem
        {
            Id = i.Id,
            Imagem = i.Imagem
        }).ToList()
    }).ToList();
    return Results.Ok(listaProdutoDto);
}).RequireAuthorization().
    WithTags("Produto");

app.MapPut("produto/atualizar", (MiraBeautyContext context, ProdutoAtualizarDto produtoAtualizarDto) =>
{
    var resultado = new ProdutoAtualizarDtoValidator().Validate(produtoAtualizarDto);

    if (!resultado.IsValid)
    {
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));
    }

    var produto = context.ProdutoSet.Find(produtoAtualizarDto.Id);
    if (produto == null)
    {
        return Results.NotFound();
    }
    produto.Descricao = produtoAtualizarDto.Descricao;
    produto.Tempo = produtoAtualizarDto.Tempo;
    produto.Preco = produtoAtualizarDto.Preco;
    produto.ProdutoImagens = produtoAtualizarDto.Imagens.Select(i => new ProdutoImagem
    {
        Id = Guid.NewGuid(),
        Imagem = i.Imagem
    }).ToList();
    context.SaveChanges();
    return Results.Ok("Produto atualizado com sucesso");
}).RequireAuthorization().
    WithTags("Produto");

app.MapDelete("produto/deletar/{id:guid}", (MiraBeautyContext context, Guid id) =>
{
    var produto = context.ProdutoSet.Find(id);
    if (produto == null)
    {
        return Results.NotFound();
    }
    context.ProdutoSet.Remove(produto);
    context.SaveChanges();
    return Results.Ok("Produto deletado com sucesso");
}).RequireAuthorization().
    WithTags("Produto");

#endregion

#region Endpoints Usuario

app.MapPost("usuario/adicionar", (MiraBeautyContext context, UsuarioAdicionarDto usuarioDto) =>
{
    var resultado = new UsuarioAdicionarDtoValidator().Validate(usuarioDto);

    if (!resultado.IsValid)
    {
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));
    }

    var usuario = new Usuario
    {
        Id = Guid.NewGuid(),
        Nome = usuarioDto.Nome,
        Email = usuarioDto.Email,
        Senha = usuarioDto.Senha,
        ConfirmacaoSenha = usuarioDto.ConfirmarSenha
    };
    context.UsuarioSet.Add(usuario);
    context.SaveChanges();
    return Results.Created("Created", "Usu�rio registrado com sucesso");
}).RequireAuthorization().
    WithTags("Usuário");

app.MapGet("usuario/listar", (MiraBeautyContext context) =>
{
    var listaUsuarioDto = context.UsuarioSet.Select(user => new UsuarioListarDto
    {
        Id = user.Id,
        Nome = user.Nome,
        Email = user.Email,
        Perfil = user.Perfil
    }).ToList();
    return Results.Ok(listaUsuarioDto);
}).RequireAuthorization().
    WithTags("Usuário");

app.MapPut("usuario/atualizar", (MiraBeautyContext context, UsuarioAtualizarDto usuarioDto) =>
{
    var resultado = new UsuarioAtualizarDtoValidator().Validate(usuarioDto);

    if (!resultado.IsValid)
    {
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));
    }

    var usuario = context.UsuarioSet.Find(usuarioDto.Id);
    if (usuario == null)
    {
        return Results.NotFound();
    }
    usuario.Nome = usuarioDto.Nome;
    usuario.Email = usuarioDto.Email;
    context.SaveChanges();
    return Results.Ok("Usuário atualizado com sucesso");
}).RequireAuthorization().
    WithTags("Usuário");

app.MapDelete("usuario/deletar/{id:guid}", (MiraBeautyContext context, Guid id) =>
{
    var usuario = context.UsuarioSet.Find(id);
    if (usuario == null)
    {
        return Results.NotFound();
    }
    context.UsuarioSet.Remove(usuario);
    context.SaveChanges();
    return Results.Ok("Usuário deletado com sucesso");
}).RequireAuthorization().
    WithTags("Usuário");

#endregion

app.MapPost("autenticar", (MiraBeautyContext context, LoginDto loginDto) =>
{
    var usuario = context.UsuarioSet.FirstOrDefault(p => p.Email == loginDto.Login && p.Senha == loginDto.Senha);
    if (usuario is null)
        return Results.BadRequest("Usuário ou Senha Inválidos!");

    var claims = new[]
    {
        new Claim("Nome", usuario.Nome),
        new Claim("Login", usuario.Email),
        new Claim("Perfil", usuario.Perfil.ToString()),
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("" + "{49ff3b22-3c25-4919-8b5f-2a79dd7088a6}"));

    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: "ana.estetica",
        audience: "ana.estetica",
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: creds);

    return Results.Ok(
    new JwtSecurityTokenHandler()
    .WriteToken(token));
}).WithTags("Autorização");

app.Run();