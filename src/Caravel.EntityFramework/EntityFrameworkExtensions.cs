using Caravel.ApplicationContext;
using Caravel.Entities;
using Caravel.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using JsonSerializer = Caravel.Http.JsonSerializer;


namespace Caravel.EntityFramework;

public static class EntityFrameworkExtensions
{
    public static void ApplyUtcDateConverter(this ModelBuilder modelBuilder)
    {
        var utcDateTimeConverter = new ValueConverter<DateTime, DateTime>(
            d => d,
            d => DateTime.SpecifyKind(d, DateTimeKind.Utc));

        var nullableUtcDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            d => d,
            d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType
                .GetProperties()
                .Where(p => p.Name.EndsWith("Utc"));

            foreach (var p in properties)
            {
                if (p.ClrType == typeof(DateTimeOffset))
                {
                    p.SetValueConverter(utcDateTimeConverter);
                }

                if (p.ClrType == typeof(DateTimeOffset?))
                {
                    p.SetValueConverter(nullableUtcDateTimeConverter);
                }
            }
        }
    }
    
    public static void AuditEntities(this DbContext dbContext, IApplicationContextAccessor contextAccessor)
    {
        var userId = contextAccessor.Context.UserId;

        var entries = dbContext.ChangeTracker
            .Entries()
            .Where(e => e.Entity is IAuditable &&
                        e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

        foreach (var entityEntry in entries)
        {
            var entity = (IEntity) entityEntry.Entity;

            switch (entityEntry.State)
            {
                case EntityState.Added:
                {
                    entity.Created = DateTimeOffset.UtcNow;

                    if (userId.HasValue)
                    {
                        entity.CreatedBy = userId.Value;
                    }

                    break;
                }
                case EntityState.Modified:
                case EntityState.Deleted:
                {
                    entity.Updated = DateTimeOffset.UtcNow;

                    if (userId.HasValue)
                    {
                        entity.UpdatedBy = userId.Value;
                    }

                    break;
                }
            }
        }
    }

    public static void CreateAndSaveDomainEvents<T>(this DbContext dbContext, DbSet<EntityEvent> events) where T : IAggregateRoot
    {
        var entries = dbContext.ChangeTracker
            .Entries()
            .Where(e => e.Entity is T && e.State is
                EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        foreach (var entityEntry in entries)
        {
            if (entityEntry.Entity is not T entity)
                continue;

            switch (entityEntry.State)
            {
                case EntityState.Deleted:
                    entity.AddEvent(new DeletedEntityEvent(entity));
                    break;
                case EntityState.Modified:
                    entity.AddEvent(new UpdatedEntityEvent((T)entityEntry.OriginalValues.ToObject(), entity));
                    break;
                case EntityState.Added:
                    entity.AddEvent(new CreatedEntityEvent(entity));
                    break;
            }
        }

        // Save all events
        foreach (var entry in entries)
        {
            if (entry.Entity is not T entity)
                continue;

            foreach (var domainEvent in entity.Events)
            {
                events.Add(new EntityEvent(domainEvent.Name, JsonSerializer.Serialize(domainEvent,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })));
            }

            entity.ClearEvents();
        }
    }
    
    
    public static void SaveDomainEvents<T>(this DbContext dbContext, DbSet<EntityEvent> events) where T : IAggregateRoot
    {
        var entries = dbContext.ChangeTracker
            .Entries()
            .Where(e => e.Entity is T && e.State is
                EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();
        
        // Save all events
        foreach (var entry in entries)
        {
            if (entry.Entity is not T entity)
                continue;

            foreach (var domainEvent in entity.Events)
            {
                events.Add(new EntityEvent(domainEvent.Name, JsonSerializer.Serialize(domainEvent,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })));
            }

            entity.ClearEvents();
        }
    }
    
    public static void SaveDomainEvents(this DbContext dbContext, DbSet<EntityEvent> events)
    {
        var entries = dbContext.ChangeTracker
            .Entries()
            .Where(e => e.Entity is IAggregateRoot && e.State is
                EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();
        
        // Save all events
        foreach (var entry in entries)
        {
            if (entry.Entity is not IAggregateRoot entity)
                continue;

            foreach (var domainEvent in entity.Events)
            {
                events.Add(new EntityEvent(domainEvent.Name, JsonSerializer.Serialize(domainEvent,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })));
            }

            entity.ClearEvents();
        }
    }
}