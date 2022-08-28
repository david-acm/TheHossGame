// 🃏 The HossGame 🃏
// <copyright file="SeedData.cs" company="Reactive">
// Copyright (c) Reactive. All rights reserved.
// </copyright>
// 🃏 The HossGame 🃏

namespace TheHossGame.Web;

using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using TheHossGame.Core.ProjectAggregate;
using TheHossGame.Infrastructure.Data;

public static class SeedData
{
   public static readonly Project TestProject1 = new ("Test Project", PriorityStatus.Backlog);
   public static readonly ToDoItem ToDoItem1 = new ()
   {
      Title = "Get Sample Working",
      Description = "Try to get the sample to build.",
   };

   public static readonly ToDoItem ToDoItem2 = new ()
   {
      Title = "Review Solution",
      Description = "Review the different projects in the solution and how they relate to one another.",
   };

   public static readonly ToDoItem ToDoItem3 = new ()
   {
      Title = "Run and Review Tests",
      Description = "Make sure all the tests run and review what they are doing.",
   };

   public static void Initialize(IServiceProvider serviceProvider)
   {
      using var dbContext = new AppDbContext(
          serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), default!);

      // Look for any TO DO items.
      if (dbContext.ToDoItems.Any())
      {
         return;   // DB has been seeded
      }

      PopulateTestData(dbContext);
   }

   public static void PopulateTestData(AppDbContext dbContext)
   {
      Guard.Against.Null(dbContext);
      foreach (var item in dbContext.Projects)
      {
         dbContext.Remove(item);
      }

      foreach (var item in dbContext.ToDoItems)
      {
         dbContext.Remove(item);
      }

      dbContext.SaveChanges();

      TestProject1.AddItem(ToDoItem1);
      TestProject1.AddItem(ToDoItem2);
      TestProject1.AddItem(ToDoItem3);
      dbContext.Projects.Add(TestProject1);

      dbContext.SaveChanges();
   }
}
