using System.Reflection;
using Core.Domain;
using Core.SeedWork;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public class Context : DbContext
{
    private readonly string _connectionString;
    private readonly bool _useConsoleLogger;
    private readonly EventDispatcher _eventDispatcher;

    public Context(string connectionString, bool useConsoleLogger, EventDispatcher eventDispatcher)
    {
        _connectionString = connectionString;
        _useConsoleLogger = useConsoleLogger;
        _eventDispatcher = eventDispatcher;
    }

    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<Training> Trainings { get; set; }
    public DbSet<TrainerEnrollment> TrainerEnrollments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ILoggerFactory? loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                .AddConsole();
        });

        optionsBuilder
            .UseSqlServer(_connectionString)
            .UseLazyLoadingProxies();
        if (_useConsoleLogger)
            optionsBuilder.UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ApplyDateTimeConverters();
    }

    public override int SaveChanges()
    {
        AddDateTimeToEntities();
        RemoveEnumerationsFromTracker();
        DispatchEventFromEntities();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new CancellationToken())
    {
        AddDateTimeToEntities();
        RemoveEnumerationsFromTracker();
        DispatchEventFromEntities();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void RemoveEnumerationsFromTracker()
    {
        ChangeTracker
            .Entries<Enumeration>()
            .ToList()
            .ForEach(ee => ee.State = EntityState.Detached);
    }

    private void AddDateTimeToEntities()
    {
        var entries = ChangeTracker
            .Entries<Entity>()
            .Where(entry =>  entry.State == EntityState.Added
                         || entry.State == EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            entityEntry.Entity.LastModifiedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Entity.CreatedAt = DateTime.UtcNow;
            }
        }
    }

    private void DispatchEventFromEntities()
    {
        var entries = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Any());
        foreach (var entity in entries)
        {
            _eventDispatcher.Dispatch(entity.DomainEvents);
            entity.ClearDomainEvents();
        }
    }
}
