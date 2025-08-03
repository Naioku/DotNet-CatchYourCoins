using System.Collections.Generic;
using Domain;
using Domain.Dashboard.Entities;

namespace Application.Tests.Factories.Entity;

public abstract class TestFactoryEntityBase<TEntity> where TEntity : IEntity
{
   public abstract TEntity CreateEntity(CurrentUser currentUser, int id = 1);

   public List<TEntity> CreateEntities(CurrentUser currentUser, int quantity)
   {
      List<TEntity> result = [];
      for (int i = 0; i < quantity; i++)
      {
         result.Add(CreateEntity(currentUser, i + 1));
      }
        
      return result;
   }
}