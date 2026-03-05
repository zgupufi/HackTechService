using HackTech_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace HackTech_Service.DAL
{
    public class AirportDbContext : DbContext
    {
        public AirportDbContext(DbContextOptions<AirportDbContext> options) : base(options)
        {

        }

        public DbSet<Node> Nodes { get; set; }
        public DbSet<Edge> Edges { get; set; }
        public DbSet<QRLocation> QRLocations { get; set; }
        public DbSet<PointOfInterest> PointOfInterests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Mapare nume tabele (pentru a evita problemele de litere mari/mici în Postgres)
            modelBuilder.Entity<Node>().ToTable("Nodes");
            modelBuilder.Entity<Edge>().ToTable("Edges");
            modelBuilder.Entity<QRLocation>().ToTable("QRLocations");
            modelBuilder.Entity<PointOfInterest>().ToTable("PointsOfInterest");

            // 2. Configurare relație dublă în tabelul Edges
            // Legătura pentru StartNode
            modelBuilder.Entity<Edge>()
                .HasOne(e => e.StartNode)
                .WithMany()
                .HasForeignKey(e => e.StartNodeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Legătura pentru EndNode
            modelBuilder.Entity<Edge>()
                .HasOne(e => e.EndNode)
                .WithMany()
                .HasForeignKey(e => e.EndNodeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}