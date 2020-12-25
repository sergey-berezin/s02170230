using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace UserInterfaceRecognition
{
    public class Context : DbContext
    {
        public DbSet<RecognitionModel> DataBaseInfo { get; set; }
        public DbSet<ClassInfo> ClassLabelsInfo { get; set; }
        public Context()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=library.db");
        }
    }
}