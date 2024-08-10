using chat.Server.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace chat.Server.Data
{
    public class ChatDbContext : IdentityDbContext<User>
    {
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Message> Messages{ get; set; }

        public ChatDbContext()
        {

        }
        public ChatDbContext(DbContextOptions<ChatDbContext> dbContextOptions):base(dbContextOptions) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Chat>()
                .HasMany<User>(c => c.Users)
                .WithMany(u => u.Chats);

            modelBuilder.Entity<Chat>()
                .HasMany<Message>(c => c.Messages);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.User);

        }
    }
}
