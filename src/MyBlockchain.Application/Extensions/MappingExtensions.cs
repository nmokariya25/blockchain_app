using System.Reflection;

namespace MyBlockchain.Application.Extensions
{
    public static class MappingExtensions
    {
        public static TEntity ToEntity<TEntity, TDto>(this TDto dto)
        where TEntity : class, new()
        where TDto : class
        {
            if (dto == null) return null;

            TEntity entity = new TEntity();
            var dtoProperties = typeof(TDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var entityProperties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var dtoProp in dtoProperties)
            {
                var entityProp = entityProperties.FirstOrDefault(p => p.Name == dtoProp.Name &&
                                                                      p.PropertyType == dtoProp.PropertyType);
                if (entityProp != null && entityProp.CanWrite)
                {
                    entityProp.SetValue(entity, dtoProp.GetValue(dto));
                }
            }

            return entity;
        }
    }
}
