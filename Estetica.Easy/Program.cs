using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Estetica.Easy.Domain.DTOs.Agendamento;
using Estetica.Easy.Domain.DTOs.AlterarSenha;
using Estetica.Easy.Domain.DTOs.Base;
using Estetica.Easy.Domain.DTOs.Categoria;
using Estetica.Easy.Domain.DTOs.Login;
using Estetica.Easy.Domain.DTOs.Produto;
using Estetica.Easy.Domain.DTOs.ResetSenha;
using Estetica.Easy.Domain.DTOs.Usuario;
using Estetica.Easy.Domain.Entities;
using Estetica.Easy.Domain.Enumerators;
using Estetica.Easy.Domain.Extensions;
using Estetica.Easy.Infra.Data.Context;
using Estetica.Easy.Infra.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Estética Easy API",
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

builder.Services.AddDbContext<EsteticaEasyContext>();

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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("Usuario", policy => policy.RequireRole("Usuario", "Administrador"));
});

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

app.MapPost("categoria/adicionar", (EsteticaEasyContext context, CategoriaAdicionarDto categoriaDto) =>
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
        CategoriaImagem = categoriaDto.CategoriaImagem,
    };

    context.CategoriaSet.Add(categoria);
    context.SaveChanges();

    return Results.Created("Created", "Categoria registrada com sucesso");
}).RequireAuthorization("Administrador").WithTags("Categoria");

app.MapGet("categoria/listar", (EsteticaEasyContext context) =>
{
    var listaCategoriaDto = context.CategoriaSet.Select(cat => new CategoriaListarDto
    {
        Id = cat.Id,
        Descricao = cat.Descricao,
        CategoriaImagem = cat.CategoriaImagem,
    }).ToList();

    return Results.Ok(listaCategoriaDto);
}).WithTags("Categoria");

app.MapPut("categoria/atualizar", (EsteticaEasyContext context, CategoriaAtualizarDto categoriaDto) =>
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
    categoria.CategoriaImagem = categoriaDto.CategoriaImagem;

    context.SaveChanges();
    return Results.Ok("Categoria atualizada com sucesso");
}).RequireAuthorization("Administrador").WithTags("Categoria");

app.MapDelete("categoria/deletar/{id:guid}", (EsteticaEasyContext context, Guid id) =>
{
    var categoria = context.CategoriaSet.Find(id);
    if (categoria == null)
    {
        return Results.NotFound();
    }
    context.CategoriaSet.Remove(categoria);
    context.SaveChanges();
    return Results.Ok("Categoria deletada com sucesso");
}).RequireAuthorization("Administrador").WithTags("Categoria");

#endregion

#region Endpoints Agendamento

app.MapPost("agendamento/adicionar", (EsteticaEasyContext context, ClaimsPrincipal claims, AgendamentoAdcionarDto agendamentoAdicionarDto) =>
    {
        var produto = context.ProdutoSet.Find(agendamentoAdicionarDto.ProdutoId);
        if (produto is null)
            return Results.BadRequest("Produto não encontrado.");

        var usuarioIdClaim = claims.FindFirst("Id")?.Value;
        if (usuarioIdClaim == null)
            return Results.Unauthorized();

        var usuarioId = Guid.Parse(usuarioIdClaim);

        var agendatamnetoId = Guid.NewGuid();
        List<AgendamentoHorario> horarios = [];

        switch (produto.Tempo)
        {
            case EnumTempo.MeiaHora:
                horarios.Add(new AgendamentoHorario
                {
                    Id = Guid.NewGuid(),
                    AgendamentoId = agendatamnetoId,
                    Data = agendamentoAdicionarDto.Data,
                    Hora = agendamentoAdicionarDto.Hora
                });
                break;
            case EnumTempo.UmaHora:
                horarios.Add(new AgendamentoHorario
                {
                    Id = Guid.NewGuid(),
                    AgendamentoId = agendatamnetoId,
                    Data = agendamentoAdicionarDto.Data,
                    Hora = agendamentoAdicionarDto.Hora
                });

                horarios.Add(new AgendamentoHorario
                {
                    Id = Guid.NewGuid(),
                    AgendamentoId = agendatamnetoId,
                    Data = agendamentoAdicionarDto.Data,
                    Hora = agendamentoAdicionarDto.Hora.AddMinutes(30)
                });

                break;
            case EnumTempo.UmaHoraMeia:
                horarios.Add(new AgendamentoHorario
                {
                    Id = Guid.NewGuid(),
                    AgendamentoId = agendatamnetoId,
                    Data = agendamentoAdicionarDto.Data,
                    Hora = agendamentoAdicionarDto.Hora
                });

                horarios.Add(new AgendamentoHorario
                {
                    Id = Guid.NewGuid(),
                    AgendamentoId = agendatamnetoId,
                    Data = agendamentoAdicionarDto.Data,
                    Hora = agendamentoAdicionarDto.Hora.AddMinutes(30)
                });

                horarios.Add(new AgendamentoHorario
                {
                    Id = Guid.NewGuid(),
                    AgendamentoId = agendatamnetoId,
                    Data = agendamentoAdicionarDto.Data,
                    Hora = agendamentoAdicionarDto.Hora.AddMinutes(60)
                });

                break;
            case EnumTempo.DuasHoras:
                horarios.Add(new AgendamentoHorario
                {
                    Id = Guid.NewGuid(),
                    AgendamentoId = agendatamnetoId,
                    Data = agendamentoAdicionarDto.Data,
                    Hora = agendamentoAdicionarDto.Hora
                });

                horarios.Add(new AgendamentoHorario
                {
                    Id = Guid.NewGuid(),
                    AgendamentoId = agendatamnetoId,
                    Data = agendamentoAdicionarDto.Data,
                    Hora = agendamentoAdicionarDto.Hora.AddMinutes(30)
                });

                horarios.Add(new AgendamentoHorario
                {
                    Id = Guid.NewGuid(),
                    AgendamentoId = agendatamnetoId,
                    Data = agendamentoAdicionarDto.Data,
                    Hora = agendamentoAdicionarDto.Hora.AddMinutes(60)
                });

                horarios.Add(new AgendamentoHorario
                {
                    Id = Guid.NewGuid(),
                    AgendamentoId = agendatamnetoId,
                    Data = agendamentoAdicionarDto.Data,
                    Hora = agendamentoAdicionarDto.Hora.AddMinutes(90)
                });

                break;
            default:
                return Results.BadRequest("Tempo Inválido.");
        }

        var agendamento = new Agendamento
        {
            Id = agendatamnetoId,
            UsuarioId = usuarioId,
            ProdutoId = agendamentoAdicionarDto.ProdutoId,
            Horarios = horarios
        };

        context.AgendamentoSet.Add(agendamento);
        context.SaveChanges();
        return Results.Created("Created", "Agendamento registrado com sucesso");
    }).RequireAuthorization().WithTags("Agendamento");

app.MapGet("agendamento/listar-todos", (EsteticaEasyContext context) =>
{
    var agendamentos = context.AgendamentoSet
        .Include(p => p.Horarios)
        .Include(p => p.Usuario)
        .Include(p => p.Produto)
        .ToList()
        .Select(agendamento =>
        {
            var horariosOrdenados = agendamento.Horarios.OrderBy(h => h.Hora).ToList();

            return new AgendamentoListarDto
            {
                Id = agendamento.Id,
                Data = horariosOrdenados.First().Data,
                HoraInicial = horariosOrdenados.First().Hora,
                HoraFinal = horariosOrdenados.Last().Hora.AddMinutes(30),
                Status = agendamento.Status,
                NomeUsuario = agendamento.Usuario.Nome,
                NomeProduto = agendamento.Produto.Descricao
            };
        })
        .OrderBy(dto => dto.HoraInicial)
        .ToList();

    return Results.Ok(agendamentos);
}).RequireAuthorization("Administrador").WithTags("Agendamento");

app.MapGet("agendamento/listar", (EsteticaEasyContext context, ClaimsPrincipal claims) =>
{
    var usuarioIdClaim = claims.FindFirst("Id")?.Value;
    if (usuarioIdClaim == null)
        return Results.Unauthorized();

    var usuarioId = Guid.Parse(usuarioIdClaim);

    var agendamentos = context.AgendamentoSet
        .Include(p => p.Horarios)
        .Include(p => p.Usuario)
        .Include(p => p.Produto)
        .Where(p => p.UsuarioId == usuarioId)
        .ToList()
        .Select(agendamento =>
        {
            var horariosOrdenados = agendamento.Horarios.OrderBy(h => h.Hora).ToList();

            return new AgendamentoListarDto
            {
                Id = agendamento.Id,
                Data = horariosOrdenados.First().Data,
                HoraInicial = horariosOrdenados.First().Hora,
                HoraFinal = horariosOrdenados.Last().Hora,
                Status = agendamento.Status,
                NomeUsuario = agendamento.Usuario.Nome,
                NomeProduto = agendamento.Produto.Descricao
            };
        })
        .OrderBy(dto => dto.HoraInicial) // aqui você ordena pela hora inicial
        .ToList();

    return Results.Ok(agendamentos);
}).RequireAuthorization().WithTags("Agendamento");

app.MapGet("agendamento/horarios-disponiveis", (EsteticaEasyContext context, [FromQuery] DateOnly dataBase) =>
{
    var horariosDisponiveis = new List<AgendamentoHorarioDisponivel>();

    var horariosOcupados = context.AgendamentoSet
        .Include(a => a.Horarios)
        .Where(a => a.Horarios.Any(h => h.Data == dataBase))
        .SelectMany(a => a.Horarios.Where(h => h.Data == dataBase))
        .Select(h => h.Hora)
        .ToList();

    for (var hora = new TimeOnly(8, 0); hora < new TimeOnly(19, 0); hora = hora.AddMinutes(30))
        horariosDisponiveis.Add(new AgendamentoHorarioDisponivel(hora, horariosOcupados.Contains(hora)));

    return Results.Ok(horariosDisponiveis);
}).WithTags("Agendamento");

app.MapDelete("agendamento/deletar/{id:guid}", (EsteticaEasyContext context, Guid id) =>
{
    var agendamento = context.AgendamentoSet.Find(id);
    if (agendamento is null)
        return Results.NotFound("Agendamento não encontrado.");

    context.AgendamentoSet.Remove(agendamento);
    context.SaveChanges();

    return Results.Ok("O agendamento foi cancelado com sucesso.");
}).RequireAuthorization("Usuario").WithTags("Agendamento");

#endregion

#region Endpoints Produto

app.MapPost("produto/adicionar", (EsteticaEasyContext context, ProdutoAdicionarDto produtoDto) =>
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
        Imagem = produtoDto.Imagem, 
        CategoriaId = produtoDto.CategoriaId,
    };
    context.ProdutoSet.Add(produto);
    context.SaveChanges();
    return Results.Created("Created", "Produto registrado com sucesso");
}).RequireAuthorization("Administrador").
    WithTags("Produto");

app.MapGet("produto/listar", (EsteticaEasyContext context) =>
{
    var listaProdutoDto = context.ProdutoSet.Include(p => p.Categoria).Select(pro => new ProdutoListarDto
    {
        Id = pro.Id,
        Descricao = pro.Descricao,
        CategoriaId = pro.CategoriaId,
        CategoriaDescricao = pro.Categoria.Descricao,
        Preco = pro.Preco,
        Tempo = pro.Tempo,
        Imagem = pro.Imagem
    }).ToList();
    return Results.Ok(listaProdutoDto);
}).WithTags("Produto");

app.MapGet("produto/listar-por-categoria/{categoriaId:guid}", (EsteticaEasyContext context, Guid categoriaId) =>
    {
        var listaProdutoDto = context.ProdutoSet.Include(p => p.Categoria).Where(p => p.CategoriaId == categoriaId).Select(pro => new ProdutoListarDto
        {
            Id = pro.Id,
            Descricao = pro.Descricao,
            CategoriaId = pro.CategoriaId,
            CategoriaDescricao = pro.Categoria.Descricao,
            Preco = pro.Preco,
            Tempo = pro.Tempo,
            Imagem = pro.Imagem
        }).ToList();
        return Results.Ok(listaProdutoDto);
    }).WithTags("Produto");

app.MapPut("produto/atualizar", (EsteticaEasyContext context, ProdutoAtualizarDto produtoAtualizarDto) =>
{
    if (produtoAtualizarDto == null)
    {
        return Results.BadRequest("Dados inválidos para atualização do produto.");
    }

    var resultado = new ProdutoAtualizarDtoValidator().Validate(produtoAtualizarDto);

    if (!resultado.IsValid)
    {
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));
    }

    var produto = context.ProdutoSet.Find(produtoAtualizarDto.Id);
    if (produto == null)
    {
        return Results.NotFound("Produto não encontrado.");
    }

    produto.Descricao = produtoAtualizarDto.Descricao;
    produto.Tempo = produtoAtualizarDto.Tempo;
    produto.Preco = produtoAtualizarDto.Preco;
    produto.Imagem = produtoAtualizarDto.Imagem;
    context.Update(produto);
    context.SaveChanges();

    return Results.Ok("Produto atualizado com sucesso.");
})
.RequireAuthorization()
.WithTags("Produto");


app.MapDelete("produto/deletar/{id:guid}", (EsteticaEasyContext context, Guid id) =>
{
    var produto = context.ProdutoSet.Find(id);
    if (produto == null)
    {
        return Results.NotFound();
    }
    context.ProdutoSet.Remove(produto);
    context.SaveChanges();
    return Results.Ok("Produto deletado com sucesso");
}).RequireAuthorization("Administrador").
    WithTags("Produto");

#endregion

#region Endpoints Usuario

app.MapPost("usuario/adicionar", (EsteticaEasyContext context, UsuarioAdicionarDto usuarioDto) =>
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
        Perfil = EnumPerfil.Usuario,
        Senha = usuarioDto.Senha.EncryptPassword()
    };
    context.UsuarioSet.Add(usuario);
    context.SaveChanges();
    return Results.Created("Created", "Usuário registrado com sucesso");
}).WithTags("Usuário");

app.MapGet("usuario/listar", (EsteticaEasyContext context) =>
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

app.MapPut("usuario/atualizar", (EsteticaEasyContext context, UsuarioAtualizarDto usuarioDto) =>
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

app.MapDelete("usuario/deletar/{id:guid}", (EsteticaEasyContext context, Guid id) =>
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

#region Endpoints Segurança

app.MapPost("autenticar", (EsteticaEasyContext context, LoginDto loginDto) =>
{
    var usuario = context.UsuarioSet.FirstOrDefault(p => p.Email == loginDto.Login && p.Senha == loginDto.Senha.EncryptPassword());
    if (usuario is null)
        return Results.BadRequest("Usuário ou Senha Inválidos!");

    var claims = new[]
    {
        new Claim("Id", usuario.Id.ToString()),
        new Claim("Nome", usuario.Nome),
        new Claim("Login", usuario.Email),
        new Claim(ClaimTypes.Role, usuario.Perfil.ToString()),
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
}).WithTags("Segurança");

app.MapPost("gerar-chave-reset-senha", (EsteticaEasyContext context, GerarResetSenhaDto gerarResetSenhaDto) =>
{
    var resultado = new GerarResetSenhaDtoValidator().Validate(gerarResetSenhaDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var usuario = context.UsuarioSet.FirstOrDefault(p => p.Email == gerarResetSenhaDto.Email);

    if (usuario is not null)
    {
        usuario.ChaveResetSenha = Guid.NewGuid();
        context.UsuarioSet.Update(usuario);
        context.SaveChanges();

        var emailService = new EmailService();
        var enviarEmailResponse = emailService.EnviarEmail(gerarResetSenhaDto.Email, "Reset de Senha", $"https://anapaulaestetica.tccnapratica.com.br/reset-senha/{usuario.ChaveResetSenha}", true);
        if (!enviarEmailResponse.Sucesso)
            return Results.BadRequest(new BaseResponse("Erro ao enviar o e-mail: " + enviarEmailResponse.Mensagem));
    }

    return Results.Ok(new BaseResponse("Se o e-mail informado estiver correto, você receberá as instruções por e-mail."));
}).WithTags("Segurança");

app.MapPut("resetar-senha", (EsteticaEasyContext context, ResetSenhaDto resetSenhaDto) =>
{
    var resultado = new ResetSenhaDtoValidator().Validate(resetSenhaDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var usuario = context.UsuarioSet.FirstOrDefault(p => p.ChaveResetSenha == resetSenhaDto.ChaveResetSenha);

    if (usuario is null)
        return Results.BadRequest(new BaseResponse("Chave de reset de senha inválida."));

    usuario.Senha = resetSenhaDto.NovaSenha.EncryptPassword();
    usuario.ChaveResetSenha = null;
    context.UsuarioSet.Update(usuario);
    context.SaveChanges();

    return Results.Ok(new BaseResponse("Senha alterada com sucesso."));
}).WithTags("Segurança");

app.MapPut("alterar-senha", (EsteticaEasyContext context, ClaimsPrincipal claims, AlterarSenhaDto alterarSenhaDto) =>
{
    var resultado = new AlterarSenhaDtoValidator().Validate(alterarSenhaDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var userIdClaim = claims.FindFirst("Id")?.Value;
    if (userIdClaim == null)
        return Results.Unauthorized();

    var userId = Guid.Parse(userIdClaim);
    var startup = context.UsuarioSet.FirstOrDefault(p => p.Id == userId);
    if (startup == null)
        return Results.NotFound(new BaseResponse("Usuário não encontrado."));

    startup.Senha = alterarSenhaDto.NovaSenha.EncryptPassword();
    context.UsuarioSet.Update(startup);
    context.SaveChanges();

    return Results.Ok(new BaseResponse("Senha alterada com sucesso."));
}).WithTags("Segurança");

#endregion

app.Run();