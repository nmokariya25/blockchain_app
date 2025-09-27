using Microsoft.EntityFrameworkCore;
using MyBlockchain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Infrastructure.Data
{
    public class BlockCypherDbContext : DbContext
    {
        public BlockCypherDbContext(DbContextOptions<BlockCypherDbContext> options) : base(options)
        {

        }

        public DbSet<EthBlock> EthBlocks { get; set; }
        public DbSet<DashBlock> DashBlocks { get; set; }
        public DbSet<BtcBlock> BtcBlocks { get; set; }
        public DbSet<LtcBlock> LtcBlocks { get; set; }
        public DbSet<BitCoinBlock> BitCoinBlocks { get; set; }
        public DbSet<ApiAuditLog> ApiAudits { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EthBlock>().ToTable("EthBlocks");
            modelBuilder.Entity<DashBlock>().ToTable("DashBlocks");
            modelBuilder.Entity<BtcBlock>().ToTable("BtcBlocks");
            modelBuilder.Entity<BitCoinBlock>().ToTable("BitCoinBlocks");
            modelBuilder.Entity<ApiAuditLog>().ToTable("ApiAudits");
        }
    }
}
