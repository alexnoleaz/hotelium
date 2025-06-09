namespace Hotelium.Shared.Entities.Auditing;

public static class EntityAuditingHelper
{
    public static void SetCreatedTimestamp(object entityAsObj)
    {
        if (entityAsObj is not IHasCreationTime entity)
            return;

        if (entity.CreationTime == default)
            entity.CreationTime = DateTime.UtcNow;
    }

    public static void SetModifiedTimestamp(object entityAsObj)
    {
        if (entityAsObj is not IHasModificationTime entity)
            return;

        entity.LastModificationTime = DateTime.UtcNow;
    }

    public static void SetDeletedFlagAndTimestamp(object entityAsObj)
    {
        if (entityAsObj is not IHasDeletionTime entity)
            return;

        if (entity.IsDeleted)
            return;

        entity.IsDeleted = true;
        entity.DeletionTime = DateTime.UtcNow;
    }
}