using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq.Expressions;

namespace OAuth2Example.DAL
{
    internal static class DbModelBuilderExtensions
    {
        public static void UniqueIndexFor<TModel>(this DbModelBuilder builder, Expression<Func<TModel, string>> propExpr)
            where TModel : class
        {
            builder.Entity<TModel>()
                .Property(propExpr)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute { IsUnique = true }));
        }
    }
}
