using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace OrderManagement.Infrastructure.Extensions
{
    public static class ModelExtensiosn
    {
        /*Riepilogo:
            Il metodo scorre tutte le entità del modello e verifica se implementano l'interfaccia TInterface.
            Se l'entità implementa l'interfaccia, crea una nuova versione del filtro (una Expression<Func<TEntity, bool>>) che può essere applicata a quell'entità concreta, adattando l'espressione iniziale.
            Applica questo filtro globale usando HasQueryFilter su ciascuna entità che implementa l'interfaccia.
            */

        public static void ApplyGlobalFiltersByInterface<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> expression, string indexPropertyName = "IsDeleted")
        {

            foreach( var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetInterface(typeof(TInterface).Name) != null)
                {
                    // Aggiungi il filtro globale
                    ParameterExpression newParam = Expression.Parameter(entityType.ClrType);
                    Expression newBody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(newBody, newParam));

                    // Aggiungi l'indice sulla proprietà specificata (di default "IsDeleted")
                    var foundProperty = entityType.FindProperty(indexPropertyName);
                    if (foundProperty != null)
                    {
                        modelBuilder.Entity(entityType.ClrType)
                            .HasIndex(foundProperty.Name); // Aggiungi indice sulla proprietà trovata
                    }

                }
            }

        }

        /*Riepilogo:
        Il metodo itera su tutte le entità del modello di Entity Framework.
        Per ogni entità, cerca una proprietà con il nome specificato (propertyName).
        Se la proprietà esiste e ha il tipo corretto (T), crea un filtro che confronta quella proprietà con il valore (value) passato come parametro.
        Il filtro viene applicato a tutte le query per quell'entità tramite HasQueryFilter.       
         */

        public static void ApplyGlobalFiltersByPropertyName<T>(this ModelBuilder modelBuilder, string propertyName , T value, string indexPropertyName = "IsDeleted")
        {

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var foundProperty = entityType.FindProperty(propertyName);

                if (foundProperty != null && foundProperty.ClrType == typeof(T))
                {
                    var newParam = Expression.Parameter(entityType.ClrType);
                    var filter = Expression.Lambda(Expression.Equal(Expression.Property(newParam, propertyName), Expression.Constant(value)), newParam);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }

                // Aggiungi l'indice sulla proprietà specificata (di default "IsDeleted")
                var foundIndexProperty = entityType.FindProperty(indexPropertyName);
                if (foundIndexProperty != null)
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasIndex(foundIndexProperty.Name); // Crea l'indice sulla proprietà trovata
                }


            }

        }

    }
}
