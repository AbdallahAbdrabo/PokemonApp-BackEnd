namespace PokemonApp.data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> option)
        :base(option) 
    {
    }

    public DbSet<Category> Ctegories { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Pokemon> Pokemons { get; set; }
    public DbSet<PokemonCategory> PokemonCategories { get; set; }
    public DbSet<PokemonOwner> PokemonOwners { get; set;}
    public DbSet<Reviewer> Reviewers { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();


        var connectionString =
         config.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PokemonCategory>()
            .HasKey(pc => 
            new { pc.CategoryId, pc.PokemonId });
        modelBuilder.Entity<PokemonCategory>()
            .HasOne(p => p.Pokemon)
            .WithMany(pc => pc.PokemonCategories)
            .HasForeignKey(p => p.PokemonId);
        modelBuilder.Entity<PokemonCategory>()
            .HasOne(po => po.category)
            .WithMany(c => c.pokemonCategories)
            .HasForeignKey(pc => pc.CategoryId);

        modelBuilder.Entity<PokemonOwner>().HasKey(po=> 
        new { po.OwnerId, po.PokemonId });
        modelBuilder.Entity<PokemonOwner>()
           .HasOne(po => po.Owner)
           .WithMany(o => o.PokemonOwners)
           .HasForeignKey(pc => pc.OwnerId);
        modelBuilder.Entity<PokemonOwner>()
            .HasOne(Pc =>Pc.Pokemon)
            .WithMany(p=>p.PokemonOwners)
            .HasForeignKey(pc => pc.PokemonId);


    }
}
