using System.Collections.Generic;
using Domain.Dashboard.Entities;

namespace Application.Tests.Factories;

public abstract class TestFactoryBase<TEntity> where TEntity : class
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