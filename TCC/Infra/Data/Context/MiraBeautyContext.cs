using Microsoft.EntityFrameworkCore;
using TCC.Configurations;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.Context
{
    public class MiraBeautyContext : DbContext
    {
        public DbSet<Produto> ProdutoSet { get; set; }
        public DbSet<ProdutoImagem> ImagemProdutoSet { get; set; }
        public DbSet<Usuario> UsuarioSet { get; set; }
        public DbSet<Configuracao> ConfiguracaoSet { get; set; }
        public DbSet<Categoria> CategoriaSet { get; set; }
        public DbSet<AgendamentoProduto> AgendamentoProdutoSet { get; set; }
        public DbSet<Agendamento> AgendamentoSet { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
            modelBuilder.ApplyConfiguration(new ConfiguracaoConfiguration());
            modelBuilder.ApplyConfiguration(new AgendamentoConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutoImagemConfiguration());
            modelBuilder.ApplyConfiguration(new AgendamentoProdutoConfiguration());

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