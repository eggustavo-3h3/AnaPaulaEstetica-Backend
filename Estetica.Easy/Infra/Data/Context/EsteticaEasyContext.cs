﻿using Estetica.Easy.Domain.Entities;
using Estetica.Easy.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Estetica.Easy.Infra.Data.Context
{
    public class EsteticaEasyContext : DbContext
    {
        public DbSet<Produto> ProdutoSet { get; set; }
        public DbSet<Usuario> UsuarioSet { get; set; }
        public DbSet<Configuracao> ConfiguracaoSet { get; set; }
        public DbSet<Categoria> CategoriaSet { get; set; }
        public DbSet<AgendamentoHorario> AgendamentoProdutoSet { get; set; }
        public DbSet<Agendamento> AgendamentoSet { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
            modelBuilder.ApplyConfiguration(new ConfiguracaoConfiguration());
            modelBuilder.ApplyConfiguration(new AgendamentoConfiguration());
            modelBuilder.ApplyConfiguration(new AgendamentoHorarioConfiguration());

            base.OnModelCreating(modelBuilder);
            // Configurações adicionais de mapeamento podem ser feitas aqui
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string conexao = "server=mysql.tccnapratica.com.br;database=tccnapratica12;port=3306;uid=tccnapratica12;password=Etec3h3";
            optionsBuilder.UseMySql(conexao, ServerVersion.AutoDetect(conexao));                                                                       

            base.OnConfiguring(optionsBuilder);
        }

    }
}